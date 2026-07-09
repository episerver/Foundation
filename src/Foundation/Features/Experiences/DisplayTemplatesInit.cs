using EPiServer.DataAbstraction;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using Foundation.Features.Experiences.Elements;

namespace Foundation.Features.Experiences
{
    /// <summary>
    /// Seeds Visual Builder display templates (style definitions) — the code-first
    /// equivalent of opti-astro's *.opti-style.json files. Templates attach style
    /// setting dropdowns to elements/nodes in the Visual Builder editor; the chosen
    /// values are exposed as displaySettings on composition nodes in Optimizely Graph
    /// and mapped to CSS classes by the headless frontend.
    /// </summary>
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class DisplayTemplatesInit : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var templates = context.Locate.Advanced.GetInstance<IDisplayTemplateRepository>();
            var contentTypes = context.Locate.Advanced.GetInstance<IContentTypeRepository>();

            SeedTemplate(templates, new DisplayTemplate
            {
                Key = "DefaultHeading",
                Name = "Heading (default)",
                ContentTypeID = contentTypes.Load(typeof(HeadingElement))?.ID,
                IsDefault = true,
                Settings =
                {
                    Setting("size", "Size", 10, ("h1", "Extra large", 10), ("h2", "Large", 20), ("h3", "Medium", 30), ("h4", "Small", 40)),
                    Setting("align", "Alignment", 20, ("left", "Left", 10), ("center", "Center", 20), ("right", "Right", 30)),
                    Setting("transform", "Text transform", 30, ("keep", "As entered", 10), ("uppercase", "Uppercase", 20), ("capitalize", "Capitalize", 30)),
                },
            });

            SeedTemplate(templates, new DisplayTemplate
            {
                Key = "DefaultButton",
                Name = "Button (default)",
                ContentTypeID = contentTypes.Load(typeof(ButtonElement))?.ID,
                IsDefault = true,
                Settings =
                {
                    Setting("buttonStyle", "Button style", 10, ("primary", "Primary", 10), ("outline", "Outline", 20), ("ghost", "Ghost", 30)),
                    Setting("buttonSize", "Size", 20, ("medium", "Medium", 10), ("small", "Small", 20), ("large", "Large", 30)),
                },
            });

            SeedTemplate(templates, new DisplayTemplate
            {
                Key = "DefaultProductCard",
                Name = "Product card (default)",
                ContentTypeID = contentTypes.Load(typeof(ProductElement))?.ID,
                IsDefault = true,
                Settings =
                {
                    Setting("cardStyle", "Card style", 10, ("standard", "Standard", 10), ("compact", "Compact", 20), ("featured", "Featured", 30)),
                },
            });

            // Node template: applies to section structure nodes (not a content type).
            SeedTemplate(templates, new DisplayTemplate
            {
                Key = "DefaultSection",
                Name = "Section (default)",
                NodeType = "section",
                IsDefault = true,
                Settings =
                {
                    Setting("background", "Background", 10, ("default", "Default", 10), ("muted", "Muted", 20), ("inverted", "Inverted", 30)),
                    Setting("spacing", "Vertical spacing", 20, ("normal", "Normal", 10), ("compact", "Compact", 20), ("spacious", "Spacious", 30)),
                },
            });
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        private static void SeedTemplate(IDisplayTemplateRepository repository, DisplayTemplate template)
        {
            // Content type not yet registered (first startup ordering) — skip; next startup seeds it.
            if (template.NodeType == null && template.ContentTypeID == null)
            {
                return;
            }

            if (repository.Load(template.Key) != null)
            {
                return;
            }

            repository.Save(template);
        }

        private static DisplaySetting Setting(string key, string name, int sortOrder, params (string Key, string Name, int SortOrder)[] choices)
        {
            var setting = new DisplaySetting
            {
                Key = key,
                Name = name,
                Editor = "select",
                SortOrder = sortOrder,
            };
            foreach (var (choiceKey, choiceName, choiceSortOrder) in choices)
            {
                setting.Choices.Add(new DisplaySettingChoice
                {
                    Key = choiceKey,
                    Name = choiceName,
                    SortOrder = choiceSortOrder,
                });
            }

            return setting;
        }
    }
}
