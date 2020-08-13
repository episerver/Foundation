using EPiServer;
using EPiServer.ContentApi.Core.Serialization;
using EPiServer.ContentApi.Core.Serialization.Models;
using EPiServer.Core;
using EPiServer.Data.Entity;
using EPiServer.DataAbstraction;
using EPiServer.Shell.Navigation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Cms
{
    public class BulkUpdateController : Controller
    {
        private readonly IContentModelMapper _contentModelMapper;
        private readonly IContentTypeRepository _contentTypeRepository;
        private readonly IContentRepository _contentRepository;
        private readonly ILanguageBranchRepository _languageBranchRepository;
        private readonly IContentLoader _contentLoader;
        private const int InformationBitCount = 30;
        public const string CatalogProviderKey = "CatalogContent";
        private readonly Dictionary<string, Type> _contentTypes = new Dictionary<string, Type>()
        {
            { "Page",  typeof(PageData) },
            { "Block",  typeof(BlockData) },
            { "Media",  typeof(MediaData) },
            { "Node",  Type.GetType("EPiServer.Commerce.Catalog.ContentTypes.NodeContent, EPiServer.Business.Commerce", false) },
            { "Entry",  Type.GetType("EPiServer.Commerce.Catalog.ContentTypes.EntryContentBase, EPiServer.Business.Commerce", false) },
            { "Campaign",  Type.GetType("EPiServer.Commerce.Marketing.SalesCampaign, EPiServer.Business.Commerce", false) },
            { "Discount",  Type.GetType("EPiServer.Commerce.Marketing.PromotionData, EPiServer.Business.Commerce", false) }
        };

        public BulkUpdateController(IContentModelMapper contentModelMapper,
            IContentTypeRepository contentTypeRepository,
            IContentRepository contentRepository,
            ILanguageBranchRepository languageBranchRepository,
            IContentLoader contentLoader)
        {
            _contentModelMapper = contentModelMapper;
            _contentTypeRepository = contentTypeRepository;
            _contentRepository = contentRepository;
            _languageBranchRepository = languageBranchRepository;
            _contentLoader = contentLoader;
        }

        [HttpGet]
        [MenuItem("/global/foundation/bulkupdate", Text = "Bulk Update", TextResourceKey = "Bulk Update", SortIndex = 100)]
        public ActionResult Index() => View();

        [HttpGet]
        public ActionResult GetContentTypes(string type)
        {
            var contentTypes = _contentTypeRepository.List().Where(o => o.Name != "SysRoot" && o.Name != "SysRecycleBin" && IsValidType(type, o.ModelType))
                .Select(o => new
                {
                    o.ID,
                    o.GUID,
                    o.Name,
                    o.DisplayName,
                })
                .OrderBy(o => o.Name);
            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(contentTypes),
                ContentType = "application/json",
            };
        }

        [HttpGet]
        public ActionResult GetProperties(int id)
        {
            var contentType = _contentTypeRepository.Load(id);
            var properties = contentType.PropertyDefinitions
                    .Where(o => o.Type.DataType == PropertyDataType.LongString && o.Type.DefinitionType.Name == typeof(PropertyLongString).Name
                        || o.Type.DataType == PropertyDataType.String && o.Type.DefinitionType.Name == typeof(PropertyString).Name
                        || o.Type.DataType == PropertyDataType.Number
                        || o.Type.DataType == PropertyDataType.FloatNumber
                        || o.Type.DataType == PropertyDataType.Boolean
                        || o.Type.DataType == PropertyDataType.Date)
                    .Select(o => new
                    {
                        o.ID,
                        o.Name,
                    });
            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(properties),
                ContentType = "application/json",
            };
        }

        [HttpGet]
        public ActionResult GetLanguages()
        {
            var languages = _languageBranchRepository.ListEnabled().Select(o => new
            {
                o.ID,
                o.LanguageID,
                o.Name,
            });
            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(languages),
                ContentType = "application/json",
            };
        }

        [HttpGet]
        public ActionResult Get(int contentTypeId, string language, string properties, string keyword = "")
        {
            var contentType = _contentTypeRepository.Load(contentTypeId);
            var catalogContent = Type.GetType("EPiServer.Commerce.Catalog.ContentTypes.CatalogContentBase, EPiServer.Business.Commerce", false)?.IsAssignableFrom(contentType.ModelType) ?? false;
            var contentReferences = _contentLoader.GetDescendents(!catalogContent ? ContentReference.RootPage : GetContentLink(1, 1, 0));
            var contents = GetItemsWithFallback(contentReferences, language);
            contents = contents.Where(o => o.ContentTypeID == contentTypeId).ToList();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                contents = contents.Where(o => o.Name.IndexOf(keyword, StringComparison.CurrentCultureIgnoreCase) >= 0);
            }
            var models = contents.Select(o => _contentModelMapper.TransformContent(o, false, properties));
            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(models),
                ContentType = "application/json",
            };
        }

        [HttpPost]
        public ActionResult UpdateContent(UpdateContentModel updateContentModel)
        {
            var props = updateContentModel.Properties.Split(',');
            var message = "";
            try
            {
                foreach (var updateContent in updateContentModel.Contents)
                {
                    var content = _contentRepository.Get<IContent>(updateContent.ContentLink.GuidValue.Value);
                    if (!(((IReadOnly)content)?.CreateWritableClone() is IContent clone))
                    {
                        message = "No IReadonly implementation!";
                    }
                    else
                    {
                        foreach (var prop in props)
                        {
                            var propData = clone.Property.FirstOrDefault(o => o.Name == prop);
                            propData.Value = updateContent.Properties[prop];
                            clone.Property.Set(prop, propData);
                        }
                        clone.Name = updateContent.Name;
                        _contentRepository.Save(clone, EPiServer.DataAccess.SaveAction.Publish);
                    }
                }
                message = "Save Successfully!";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return new ContentResult
            {
                Content = message
            };
        }

        public class UpdateContentModel
        {
            public IEnumerable<ContentApiModel> Contents { get; set; }
            public string Properties { get; set; }
        }

        private IEnumerable<IContent> GetItemsWithFallback(IEnumerable<ContentReference> contentReferences, string language)
        {
            if (contentReferences == null || !contentReferences.Any())
            {
                return Enumerable.Empty<IContent>();
            }

            var fallbackLanguageSelector = string.IsNullOrWhiteSpace(language) ? LanguageSelector.MasterLanguage() : LanguageSelector.Fallback(language, false); ;
            return _contentLoader.GetItems(contentReferences, fallbackLanguageSelector);
        }

        private bool IsValidType(string type, Type inputType)
        {
            if (string.IsNullOrEmpty(type))
            {
                return false;
            }

            var contentType = _contentTypes[type];
            if (contentType == null)
            {
                return false;
            }

            return contentType.IsAssignableFrom(inputType);
        }

        private ContentReference GetContentLink(int objectId, int contentType, int versionId)
        {
            var contentId = objectId | 1 - (contentType << InformationBitCount);
            return new ContentReference(contentId, versionId, CatalogProviderKey);
        }
    }
}
