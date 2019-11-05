class Product {

    constructor(divId) {
        if (divId) {
            this.DivContainerID = divId;
        } else {
            this.DivContainerID = document;
        }
    }

    /// Product handler

    addToCart(data, url, callback) {
        $('body>.loading-box').show();
        data.requestFrom = "axios";
        axios.post(url, data)
            .then(function (result) {
                if (result.data.StatusCode == 0) {
                    notification.Warning(result.data.Message);
                }
                if (result.data.StatusCode == 1) {
                    notification.Success(result.data.Message);

                    if (callback) callback(result.data.CountItems);
                }
            })
            .catch(function (error) {
                notification.Error(error.response.statusText);
            })
            .finally(function () {
                $('body>.loading-box').hide();
            });

        return false;
    }

    // use in Wishlist Page
    removeItem(data, url, message, callback) {
        var inst = this;
        $('body>.loading-box').show();
        axios.post(url, data)
            .then(function (result) {
                if (result.status == 200) {
                    notification.Success(message);
                    $('#my-wishlist').html(result.data);
                    var product = new Product('#my-wishlist');
                    product.Init();
                    var count = $('#countWishListInPage').val();
                    if (callback) callback(count);

                    feather.replace();
                }
                if (result.status == 204) {
                    notification.Error(result.statusText);
                }
            })
            .catch(function (error) {
                notification.Error(error);
            })
            .finally(function () {
                $('body>.loading-box').hide();
            });
    }

    callbackAddToCart(selector, count) {
        if (selector == ".jsCartBtn") { cartHelper.SetCartReload(count); }
        else if (selector == ".jsSharedCartBtn") { cartHelper.SetSharedCartReload(count)}
        else cartHelper.SetWishlistReload(count);
    }

    /// --- end


    AddToSharedCartClick() {
        var inst = this;
        $(this.DivContainerID).find('.addToSharedCart').each(function (i, e) {
            $(e).click(function () {
                var code = $(this).attr('data');

                var callback = (count) => {
                    inst.callbackAddToCart('.jsSharedCartBtn', count);
                };

                inst.addToCart({ Code: code }, '/SharedCart/AddToCart', callback);
            });
        });
    }

    AddToWishlistClick() {
        var inst = this;

        $(this.DivContainerID).find('.addToWishlist').each(function (i, e) {
            $(e).click(function () {
                var code = $(this).attr('data');

                var callback = (count) => {
                    inst.callbackAddToCart('#js-wishlist', count);
                };

                inst.addToCart({ Code: code }, '/Wishlist/AddToCart', callback);
            });
        });

    }

    AddToCartClick() {
        var inst = this;

        $(this.DivContainerID).find('.addToCart').each(function (i, e) {
            $(e).attr("href", "javascript:void(0);")
            $(e).click(function () {
                var code = $(this).attr('data');
                var data = {
                    Code: code
                };

                if ($(this).attr('qty')) data.Quantity = $(this).attr('qty');
                if ($(this).attr('store')) data.Store = $(this).attr('store');
                if ($(this).attr('selectedStore')) data.SelectedStore = $(this).attr('selectedStore');

                var callback = (count) => {
                    inst.callbackAddToCart('.jsCartBtn', count);
                };

                inst.addToCart(data, '/DefaultCart/AddToCart', callback);
            });
        });
    }

    DeleteWishlistClick() {
        var inst = this;

        $(this.DivContainerID).find('.deleteLineItemWishlist').each(function (i, e) {
            $(e).click(function () {
                if (confirm("Are you sure?")) {
                    var code = $(e).attr('data');
                    var data = { Code: code, Quantity: 0, RequestFrom: "axios" };
                    var callback = (count) => {
                        inst.callbackAddToCart("#js-wishlist", count);
                    };
                    inst.removeItem(data, '/Wishlist/ChangeCartItem', "Removed " + code + " from wishlist", callback);
                }
            });
        });
    }

    AddAllToCartClick() {
        $(this.DivContainerID).find('.jsAddAllToCart').each(function (i, e) {
            $(e).click(function () {
                $('.loading-box').show();
                var url = $(this).attr('url');
                axios.post(url)
                    .then(function (result) {
                        notification.Success(result.data.Message);
                        cartHelper.SetCartReload(result.data.CountItems);
                    })
                    .catch(function (error) {
                        notification.Error(error);
                    })
                    .finally(function () {
                        $('.loading-box').hide();
                    });

            });
        });
        
    }

    Init() {
        // Init product \
        this.AddToWishlistClick();
        this.AddToSharedCartClick();
        this.AddToCartClick();
        this.AddAllToCartClick();
        this.DeleteWishlistClick();
        //-- end
    }
}