class FoundationCommerce {
    init() {
        window.cartHelper = new CartHelper();

        // Search & Product List
        var search = new ProductSearch();
        search.Init();

        var newProductsSearch = new NewProductsSearch();
        newProductsSearch.Init();

        var salesSearch = new SalesSearch();
        salesSearch.Init();
        // --- End

        // Product
        var product = new Product();
        product.Init();

        var productDetail = new ProductDetail('.product-detail');
        productDetail.InitProductDetail();
        // --- End

        // Quick View
        var quickView = new ProductDetail('#quickView');
        quickView.InitQuickView();
        // --- End

        // Review
        var review = new Review();
        review.RatingHover();
        review.RatingClick();
        review.SubmitReview();
        // --- End

        // Cart
        var cart = new Cart();
        cart.InitLoadCarts();
        cart.InitRemoveItem();
        cart.InitClearCart();
        cart.InitMoveToWishtlist();
        cart.InitChangeQuantityItem();
        cart.InitChangeVariant();
        // --- End

        // My Profile
        var myProfile = new MyProfile();
        myProfile.EditProfileClick();
        myProfile.SaveProfileClick();
        // --- End

        // Checkout
        var checkout = new Checkout();
        checkout.FormShippingAddressChange();
        checkout.FormBillingAddressChange();
        checkout.AddPaymentClick();
        checkout.RemovePaymentClick();
        checkout.PaymentMethodChange();
        checkout.CreditCardChange();
        checkout.CheckoutAsGuestOrRegister();
        checkout.ApplyCouponCode();
        checkout.RemoveCouponCode();
        checkout.ChangeShippingMethod();
        checkout.ChangeCartItem();
        checkout.SeparateInit();
        checkout.ChangeAddressClick();
        checkout.AddNewAddress();
        checkout.ShowHideSubscription();
        // --- End

        // Order Detail
        var orderDetai = new OrderDetail();
        orderDetai.InitNote();
        orderDetai.InitReturnOrder();
        // --- End


        // order pad
        var firstTable = new OrderPadsComponent('#firstTable');

        // B2B Budget
        var budget = new Budget();
        budget.SaveNewBudget();
        // --- End

        // B2B Organization
        var organization = new Organization();
        organization.init();
        // --- End

        // Quick Order Block
        $('.jsQuickOrderBlockForm').each(function (i, e) {
            let newBlockId = 'jsQuickOrderBlockForm' + i;
            $(e).attr('id', newBlockId);
            let quickOrderBlock = new QuickOrderBlock('#' + newBlockId);
            quickOrderBlock.Init();
        })
        // --- End

        // Address
        var address = new Address();
        address.Init();
        // --- End

        // B2B Order
        var b2bOrder = new B2BOrder();
        b2bOrder.Init();
        // --- End

        // Order Search Block
        var orderSearchBlock = new OrderSearchBlock();
        orderSearchBlock.Init();
        // --- End


        // B2B Users
        var b2bUsers = new UsersOrganization();
        b2bUsers.Init();
        // --- End
    }
}