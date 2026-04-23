import { ProductSearch, NewProductsSearch, SalesSearch } from "../../../Features/Search/search";
import ProductDetail from "Features/CatalogContent/product-detail";
import Product from "Features/CatalogContent/product";
import Review from "./review";
import MyProfile from "Features/MyAccount/ProfilePage/my-profile";
import { Cart, CartHelper } from "Features/NamedCarts/cart";
import Checkout from "Features/Checkout/checkout";
import OrderDetails from "Features/MyAccount/OrderDetails/order-details";
import OrderPadsComponent from "Features/NamedCarts/OrderPadsPage/order-pads";
import Address from "Features/Checkout/address";
import OrderSearchBlock from "Features/Blocks/OrderSearchBlock/order-search-block";
import ProductRecommendations from "Features/Recommendations/WidgetBlock/product-recommendations";
import B2bOrder from "Features/MyOrganization/Orders/b2b-order";
import B2bBudget from "Features/MyOrganization/Budgeting/b2b-budget";
import B2bOrganization from "Features/MyOrganization/b2b-organization";
import B2bUsersOrganization from "Features/MyOrganization/Users/b2b-users-organization";
import Stores from "Features/Stores/stores";
import People from "Features/People/people";
import Market from "Features/Markets/market";
import QuickOrderBlock from "Features/MyOrganization/QuickOrderBlock/quick-order-block";

export default class FoundationCommerce {
  init() {
    window.cartHelper = new CartHelper();

    let market = new Market();
    market.init();

    let productDetail = new ProductDetail('.product-detail');
    productDetail.initProductDetail();

    let quickView = new ProductDetail('#quickView');
    quickView.initQuickView();

    let search = new ProductSearch();
    search.init();

    let newProductsSearch = new NewProductsSearch();
    newProductsSearch.init();

    let salesSearch = new SalesSearch();
    salesSearch.init();

    let product = new Product();
    product.init();

    let review = new Review();
    review.ratingHover();
    review.ratingClick();
    review.submitReview();

    let myProfile = new MyProfile();
    myProfile.editProfileClick();
    myProfile.saveProfileClick();

    let address = new Address();
    address.init();

    let cart = new Cart();
    cart.initLoadCarts();
    cart.initRemoveItem();
    cart.initClearCart();
    cart.initMoveToWishtlist();
    cart.initChangeQuantityItem();
    cart.initChangeVariant();

    let checkout = new Checkout();
    checkout.init();

    let orderDetails = new OrderDetails();
    orderDetails.initNote();
    orderDetails.initReturnOrder();

    let firstTable = new OrderPadsComponent('#firstTable');

    // Quick Order Block
    $('.jsQuickOrderBlockForm').each(function (i, e) {
      let newBlockId = 'jsQuickOrderBlockForm' + i;
      $(e).attr('id', newBlockId);
      let quickOrderBlock = new QuickOrderBlock('#' + newBlockId);
      quickOrderBlock.init();
    })

    let orderSearchBlock = new OrderSearchBlock();
    orderSearchBlock.init();

    let productRecommendations = new ProductRecommendations();
    productRecommendations.init();

    let b2bBudget = new B2bBudget();
    b2bBudget.saveNewBudget();

    let b2bOrganization = new B2bOrganization();
    b2bOrganization.init();

    let b2bOrder = new B2bOrder();
    b2bOrder.init();

    //let b2bUsers = new B2bUsersOrganization();
    //b2bUsers.init();

    let stores = new Stores();
    stores.init();

    let people = new People();
    people.init();
  }
}