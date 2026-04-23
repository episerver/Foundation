export class Cart {
    changeInfoCart(result) {
        document.querySelector('.largecart-Subtotal').innerHTML = "$" + result.data.subTotal.amount;
        if (document.querySelector('.largecart-TotalDiscount') != null)
        document.querySelector('.largecart-TotalDiscount').innerHTML = "$" + result.data.totalDiscount.amount;
        document.querySelector('.largecart-TaxTotal').innerHTML = "$" + result.data.taxTotal.amount;
        document.querySelector('.largecart-ShippingTotal').innerHTML = "$" + result.data.shippingTotal.amount;
        document.querySelector('.largecart-Total').innerHTML = "$" + result.data.total.amount;
        cartHelper.setCartReload(result.data.countItems);
    }

    removeItem(url, elementClick, typeCart) {
        var inst = this;
        var data = {
            Code: elementClick.getAttribute('code'),
            ShipmentId: elementClick.getAttribute('shipmentId'),
            RequestFrom: elementClick.getAttribute('type')
        };

        axios.post(url, data)
            .then(function (result) {
                if (result.data.statusCode == 0) {
                    notification.error(result.data.message);
                }
                else if (result.data.statusCode == 1) {
                    if (typeCart == 'cart') {
                        Array.from(document.querySelectorAll(".countItemCartHeader")).forEach(function (el, i) {
                            el.innerHTML = result.data.CountItems;
                        });
                        Array.from(document.querySelectorAll(".amountCartHeader")).forEach(function (el, i) {
                            el.innerHTML = "$" + result.data.subTotal.amount;
                        });
                    }

                    if (typeCart !== 'large-cart' && typeCart !== "shared-cart-large") {
                        elementClick.closest('.cart__row').remove();
                        if (typeCart == "cart") cartHelper.setCartReload(result.data.countItems);
                        else if (typeCart == "shared-cart") cartHelper.setSharedCartReload(result.data.countItems);
                        else cartHelper.setWishlistReload(result.data.countItems);

                    } else { // if large cart, large shared 
                        if (typeCart == "shared-cart-large") {
                            elementClick.closest('tr').remove();
                            cartHelper.setSharedCartReload(result.data.countItems);
                        } else {
                            elementClick.closest('.product-tile-list__item').remove();
                            inst.changeInfoCart(result);
                        }
                    }
                }
                else {
                    notification.error(result.data.message);
                }
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {

            });
    }

    moveToWishlist(element) {
        var inst = this;
        document.querySelector(".loading-box").style.display = 'block'; // show
        var url = element.getAttribute('url');
        var code = element.getAttribute('code');
        axios.post(url, { Code: code })
            .then(function (result) {
                if (result.data.statusCode === 1) {
                    inst.changeInfoCart(result);
                    element.closest('.product-tile-list__item').remove();

                    cartHelper.AddWishlist();
                    notification.success(result.data.message);
                } else {
                    notification.warning(result.data.message);
                }
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {
                document.querySelector(".loading-box").style.display = 'none';
            });
    }

    changeVariant(url, data, elementChange) {
        var inst = this;
        axios.post(url, data)
            .then(function (result) {
                var container = elementChange.closest('.product-tile-list__item');
                container.innerHTML = result.data;
                inst.initCartItems(container);
                feather.replace();
                var dropdown = new Dropdown(container);
                dropdown.init();
                notification.success("Success");
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {

            });
    }

    changeQuantity(element) {
        var inst = this;
        var code = element.getAttribute('code');
        var shipmentId = element.getAttribute('shipmentId');
        var qty = element.value;
        var url = element.getAttribute('url');
        var data = {
            Code: code,
            ShipmentId: shipmentId,
            Quantity: qty
        };
        element.setAttribute('disabled', 'disabled');
        axios.post(url, data)
            .then(function (result) {
                switch (result.data.statusCode) {
                    case 0:
                        for (let sibling of element.parentNode.children) {
                            if (sibling.classList.contains("required")) {
                                sibling.innerHTML = result.data.message;
                            }
                        }
                        notification.warning(result.data.message);
                        break;
                    case -1:
                        notification.error(result.data.message);
                        break;
                    default:
                        notification.success(result.data.message);
                        inst.changeInfoCart(result);
                        var subtotal = parseFloat(element.getAttribute('unitPrice')) * qty;
                        document.querySelector('.subtotal-' + code).innerHTML = (element.getAttribute('currency') + subtotal);
                        element.closest('.product-tile-list__item').querySelector('.currentVariantInfo').getAttribute('quantity', qty);
                        break;
                }
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {
                element.removeAttribute('disabled');
            });
    }


    initClearCart() {
        let element = document.querySelector('#clearCart');
        if (element == null) return;
        element.addEventListener("click", function () {
            if (confirm("Are you sure?")) {
                document.querySelector(".loading-box").style.display = 'block';
                var url = element.getAttribute('url');
                axios.post(url)
                    .then(function (result) {
                        notification.success("Delete cart successfully.");
                        setTimeout(function () {
                            window.location.reload();
                        }, 1000);
                    })
                    .catch(function (error) {
                        notification.error(error);
                    })
                    .finally(function () {
                        document.querySelector(".loading-box").style.display = 'none';
                    });
            }
        });
    }

    loadMiniCartClick(urlLoadCart, clickSelector, reloadSelector) {
        var inst = this;
        
        clickSelector.addEventListener("click", function () {
            var isNeedReload = clickSelector.getAttribute('reload');
            if (isNeedReload == 1) { // reload mini cart
                document.querySelector(reloadSelector + " .loading-cart").style.display = "block";
                axios.get(urlLoadCart, null)
                    .then(function (result) {
                        document.querySelector(reloadSelector + " .cart-item-listing").innerHTML = result.data;
                        inst.initRemoveItem(reloadSelector);
                        document.querySelector(reloadSelector).setAttribute('reload', 0);
                    })
                    .catch(function (error) {
                        notification.error(error);
                    })
                    .finally(function () {
                        document.querySelector(reloadSelector + " .loading-cart").style.display = "none";
                    });
            }
        });
    }

    initLoadCarts() {
        var inst = this;
        Array.from(document.querySelectorAll(".jsCartBtn")).forEach(function (e, i) {
            var url = e.getAttribute("data-reloadurl");
            var container = e.getAttribute("data-cartcontainer");
            inst.loadMiniCartClick(url, e, container);
        });
        Array.from(document.querySelectorAll(".jsWishlistBtn")).forEach(function (e, i) {
            var url = e.getAttribute("data-reloadurl");
            var container = e.getAttribute("data-cartcontainer");
            inst.loadMiniCartClick(url, e, container);
        });
        Array.from(document.querySelectorAll(".jsSharedCartBtn")).forEach(function (e, i) {
            var url = e.getAttribute("data-reloadurl");
            var container = e.getAttribute("data-cartcontainer");
            inst.loadMiniCartClick(url, e, container);
        });
    }

    initChangeVariant(selector) {
        var inst = this;
        if (selector == undefined) {
            selector = document;
        }
        Array.from(document.querySelectorAll(".jsChangeSizeVariantLargeCart")).forEach(function (el, i) {
            el.addEventListener("change", function () {
                var parent = el.closest('.product-tile-list__item');
                var variantInfo = parent.closest('.currentVariantInfo');
                var data = {
                    Code: variantInfo.value,
                    Size: variantInfo.getAttribute('size'),
                    Quantity: variantInfo.getAttribute('quantity'),
                    NewSize: el.value,
                    ShipmentId: variantInfo.getAttribute('shipmentId'),
                    RequestFrom: "changeSizeItem"
                };
                var url = variantInfo.getAttribute('url');

                inst.changeVariant(url, data, el);
            });
        });
    }

    initMoveToWishtlist(selector) {
        var inst = this;
        if (selector == undefined) {
            selector = document;
        }
        Array.from(document.querySelectorAll(".jsMoveToWishlist")).forEach(function (el, i) {
            el.addEventListener("click", function () {
                inst.moveToWishlist(el);
            });
        });
    }

    initChangeQuantityItem(selector) {
        var inst = this;
        if (selector == undefined) {
            selector = document;
        }
        Array.from(document.querySelectorAll(".jsChangeQuantityCartItem")).forEach(function (el, i) {
            el.addEventListener("change", function () {
                var valueInt = parseInt(el.value);
                if (!isNaN(valueInt) && Number.isInteger(valueInt)) {
                    for (let sibling of el.parentNode.children) {
                        if (sibling.classList.contains("required")) {
                            sibling.innerHTML = "";
                        }
                    }
                    if (valueInt > 0)
                        inst.changeQuantity(el);
                    else {
                        if (confirm("Are you sure delete this item?")) {
                            var elementDelete = el.closest('.product-tile-list__item').querySelector('.jsRemoveCartItem');
                            inst.removeItem('/defaultcart/RemoveCartItem', elementDelete, "large-cart");
                        }
                    }
                }
                else {
                    for (let sibling of el.parentNode.children) {
                        if (sibling.classList.contains("required")) {
                            sibling.innerHTML = "This field must be a number.";
                        }
                    }
                }
            });
        });
    }


    initRemoveItem(selector) {
        var inst = this;
        if (selector == undefined) {
            selector = document;
        }
        Array.from(document.querySelectorAll(".jsRemoveCartItem")).forEach(function (el, i) {
            el.addEventListener("click", function () {
                if (confirm("Are you sure?")) {
                    var type = el.getAttribute('type');
                    var url = "/defaultcart/RemoveCartItem";
                    //var typeCart = "#js-cart";
                    if (type === "wishlist") {
                        url = "/wishlist/RemoveWishlistItem";
                        //typeCart = "#js-wishlist";
                    }

                    if (type === "large-cart") {
                        url = "/defaultcart/RemoveCartItem";
                        //typeCart = "#cartItemsId";
                    }

                    if (type === "shared-cart") {
                        url = "/sharedcart/RemoveCartItem";
                        //typeCart = "#jsSharedCartContainer";
                    }

                    if (type === "shared-cart-large") {
                        url = "/sharedcart/RemoveCartItem";
                    }

                    inst.removeItem(url, el, type);
                }
            });
        });
    }

    initCartItems(selector) {
        var inst = this;
        inst.initRemoveItem(selector);
        inst.initChangeQuantityItem(selector);
        inst.initMoveToWishtlist(selector);
        inst.initChangeVariant(selector);
    }
}

export class CartHelper {
    setCartReload(totalItem) {
        if (totalItem != undefined) {
            Array.from(document.querySelectorAll(".jsCartBtn")).forEach(function (el, i) {
                el.querySelector('.icon-menu__badge').innerHTML = totalItem;
                el.setAttribute('reload', 1);
            });
        }
    }

    setWishlistReload(totalItem) {
        if (totalItem != undefined) {
            Array.from(document.querySelectorAll(".jsWishlistBtn")).forEach(function (el, i) {
                el.querySelector('.icon-menu__badge').innerHTML = totalItem;
                el.setAttribute('reload', 1);
            });
        }
    }

    setSharedCartReload(totalItem) {
        if (totalItem != undefined) {
            Array.from(document.querySelectorAll(".jsSharedCartBtn")).forEach(function (el, i) {
                el.querySelector('.icon-menu__badge').innerHTML = totalItem;;
                el.setAttribute('reload', 1);
            });
        }
    }

    addWishlist() {
        var wishlistHeader = document.querySelector('#js-wishlist').getElementsByClassName('icon-menu__badge')[0];

        var newQty = parseInt(wishlistHeader.innerHTML) + 1;
        this.SetWishlistReload(newQty);
    }

    subtractWishlist() {
        var wishlistHeader = document.querySelector('#js-wishlist').children('.icon-menu__badge').first();

        var newQty = parseInt(wishlistHeader.innerHTML) + 1;
        this.SetWishlistReload(newQty);
    }
}