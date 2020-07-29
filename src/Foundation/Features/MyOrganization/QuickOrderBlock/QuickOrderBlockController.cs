using EPiServer;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Foundation.Cms;
using Foundation.Cms.Extensions;
using Foundation.Cms.Settings;
using Foundation.Commerce.Customer.Services;
using Foundation.Features.CatalogContent.Services;
using Foundation.Features.Checkout.Services;
using Foundation.Features.MyOrganization.QuickOrderPage;
using Foundation.Features.Search;
using Foundation.Features.Settings;
using Foundation.Infrastructure;
using Mediachase.Commerce.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.MyOrganization.QuickOrderBlock
{
    [TemplateDescriptor(Default = true)]
    public class QuickOrderBlockController : BlockController<QuickOrderBlock>
    {
        private readonly IQuickOrderService _quickOrderService;
        private readonly ICartService _cartService;
        private readonly IFileHelperService _fileHelperService;
        private ICart _cart;
        private readonly IOrderRepository _orderRepository;
        private readonly ReferenceConverter _referenceConverter;
        private readonly ISearchService __searchService;
        private readonly ICustomerService _customerService;
        private readonly IContentLoader _contentLoader;
        private readonly ContentLocator _contentLocator;
        private readonly ISettingsService _settingsService;

        public QuickOrderBlockController(
            IQuickOrderService quickOrderService,
            ICartService cartService,
            IFileHelperService fileHelperService,
            IOrderRepository orderRepository,
            ReferenceConverter referenceConverter,
            ISearchService _searchService,
            ICustomerService customerService,
            IContentLoader contentLoader,
            ContentLocator contentLocator,
            ISettingsService settingsService)
        {
            _quickOrderService = quickOrderService;
            _cartService = cartService;
            _fileHelperService = fileHelperService;
            _orderRepository = orderRepository;
            _referenceConverter = referenceConverter;
            __searchService = _searchService;
            _customerService = customerService;
            _contentLoader = contentLoader;
            _contentLocator = contentLocator;
            _settingsService = settingsService;
        }
        public override ActionResult Index(QuickOrderBlock currentBlock)
        {
            currentBlock.ReturnedMessages = TempData["messages"] as List<string>;
            currentBlock.ProductsList = TempData["products"] as List<QuickOrderProductViewModel>;
            return PartialView(currentBlock);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import(QuickOrderProductViewModel[] ProductsList)
        {
            var returnedMessages = new List<string>();

            ModelState.Clear();

            if (Cart == null)
            {
                _cart = _cartService.LoadOrCreateCart(_cartService.DefaultCartName);
            }

            foreach (var product in ProductsList)
            {
                if (!product.ProductName.Equals("removed"))
                {
                    var variationReference = _referenceConverter.GetContentLink(product.Sku);
                    var currentQuantity = GetCurrentItemQuantity(product.Sku);
                    product.Quantity += (int)currentQuantity;
                    var responseMessage = _quickOrderService.ValidateProduct(variationReference, Convert.ToDecimal(product.Quantity), product.Sku);
                    if (responseMessage.IsNullOrEmpty())
                    {
                        var result = _cartService.AddToCart(Cart, product.Sku, 1, "delivery", "");
                        if (!result.EntriesAddedToCart)
                        {
                            continue;
                        }
                        _cartService.ChangeCartItem(Cart, 0, product.Sku, product.Quantity, "", "");
                        _orderRepository.Save(Cart);
                    }
                    else
                    {
                        returnedMessages.Add(responseMessage);
                    }
                }
            }
            if (returnedMessages.Count == 0)
            {
                returnedMessages.Add("All items were added to cart.");
            }
            TempData["messages"] = returnedMessages;

            return Json(new { Message = returnedMessages, TotalItem = Cart.GetAllLineItems().Sum(x => x.Quantity) });
        }

        private decimal GetCurrentItemQuantity(string variantCode)
        {
            if (Cart == null)
            {
                return 0;
            }

            var lineItem = Cart.GetAllLineItems().Where(x => x.Code == variantCode).FirstOrDefault();
            if (lineItem != null)
            {
                return lineItem.Quantity;
            }

            return 0;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFromFile()
        {
            var fileContent = Request.Files[0];
            if (fileContent != null && fileContent.ContentLength > 0)
            {
                var uploadedFile = fileContent.InputStream;
                var fileName = fileContent.FileName;
                var productsList = new List<QuickOrderProductViewModel>();

                //validation for csv
                if (!fileName.Contains(".csv"))
                {
                    TempData["messages"] = new List<string>() { "The uploaded file is not valid!" };
                    return Json(new { Message = TempData["messages"] });
                }

                var fileData = _fileHelperService.GetImportData<QuickOrderData>(uploadedFile);
                foreach (var record in fileData)
                {
                    //find the product
                    var variationReference = _referenceConverter.GetContentLink(record.Sku);
                    var product = _quickOrderService.GetProductByCode(variationReference);

                    product.Quantity = record.Quantity;
                    product.TotalPrice = product.Quantity * product.UnitPrice;

                    productsList.Add(product);
                }

                return Json(new { Status = "OK", Message = "Import .csv file successfully", Products = productsList });
            }
            else
            {
                return Json(new { Message = "The uploaded file is not valid!" });
            }
        }

        public JsonResult GetSku(string query)
        {
            var data = __searchService.SearchSkus(query);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        private ICart Cart => _cart ?? (_cart = _cartService.LoadCart(_cartService.DefaultCartName, true)?.Cart);

        private QuickOrderPage.QuickOrderPage GetQuickOrderPage() => _contentLoader.FindPagesRecursively<QuickOrderPage.QuickOrderPage>(ContentReference.StartPage).FirstOrDefault();

        [HttpPost]

        public ActionResult RequestQuote(QuickOrderProductViewModel[] ProductsList)
        {
            var returnedMessages = new List<string>();
            ModelState.Clear();

            var currentCustomer = _customerService.GetCurrentContact();
            var referencePages = _settingsService.GetSiteSettings<ReferencePageSettings>();
            var quoteCart = _cartService.LoadOrCreateCart("QuickOrderToQuote");

            if (quoteCart != null)
            {
                foreach (var product in ProductsList)
                {
                    if (!product.ProductName.Equals("removed"))
                    {
                        var variationReference = _referenceConverter.GetContentLink(product.Sku);
                        var responseMessage = _quickOrderService.ValidateProduct(variationReference, Convert.ToDecimal(product.Quantity), product.Sku);
                        if (responseMessage.IsNullOrEmpty())
                        {
                            var result = _cartService.AddToCart(quoteCart, product.Sku, 1, "delivery", "");
                            if (!result.EntriesAddedToCart)
                            {
                                continue;
                            }
                            _cartService.ChangeCartItem(quoteCart, 0, product.Sku, product.Quantity, "", "");
                            _orderRepository.Save(quoteCart);
                        }
                        else
                        {
                            returnedMessages.Add(responseMessage);
                        }
                    }
                }

                _cartService.PlaceCartForQuote(quoteCart);
                _cartService.DeleteCart(quoteCart);

                var quickOrderPage = GetQuickOrderPage();
                return Redirect(quickOrderPage?.LinkURL ?? Request.UrlReferrer.AbsoluteUri);
            }

            return RedirectToAction("Index", new { Node = referencePages?.OrderHistoryPage ?? ContentReference.StartPage });
        }
    }
}
