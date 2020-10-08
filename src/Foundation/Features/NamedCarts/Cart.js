export class Cart {
  changeInfoCart(result) {
    $('.largecart-Subtotal').html("$" + result.data.SubTotal.Amount);
    $('.largecart-TotalDiscount').html("$" + result.data.TotalDiscount.Amount);
    $('.largecart-TaxTotal').html("$" + result.data.TaxTotal.Amount);
    $('.largecart-ShippingTotal').html("$" + result.data.ShippingTotal.Amount);
    $('.largecart-Total').html("$" + result.data.Total.Amount);

    cartHelper.setCartReload(result.data.CountItems);
  }

  removeItem(url, elementClick, typeCart) {
    var inst = this;
    var data = {
      Code: elementClick.attr('code'),
      ShipmentId: elementClick.attr('shipmentId'),
      RequestFrom: elementClick.attr('type')
    };

    axios.post(url, data)
      .then(function (result) {
        if (result.data.StatusCode == 0) {
          notification.error(result.data.Message);
        }
        else if (result.data.StatusCode == 1) {
          if (typeCart == 'cart') {
            $('.countItemCartHeader').each(function (i, el) {
              $(el).html(result.data.CountItems);
            });
            $('.amountCartHeader').each(function (i, el) {
              $(el).html("$" + result.data.SubTotal.Amount);
            });
          }

          if (typeCart !== 'large-cart' && typeCart !== "shared-cart-large") {
            elementClick.parents('.cart__row').first().remove();
            if (typeCart == "cart") cartHelper.setCartReload(result.data.CountItems);
            else if (typeCart == "shared-cart") cartHelper.setSharedCartReload(result.data.CountItems);
            else cartHelper.setWishlistReload(result.data.CountItems);

          } else { // if large cart, large shared 
            if (typeCart == "shared-cart-large") {
              elementClick.parents('tr').first().remove();
              cartHelper.setSharedCartReload(result.data.CountItems);
            } else {
              elementClick.parents('.product-tile-list__item').first().remove();
              inst.changeInfoCart(result);
            }
          }
        }
        else {
          notification.error(result.data.Message);
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
    $('.loading-box').show();
    var url = $(element).attr('url');
    var code = $(element).attr('code');
    axios.post(url, { Code: code })
      .then(function (result) {
        if (result.data.StatusCode === 1) {
          inst.changeInfoCart(result);
          element.parents('.product-tile-list__item').first().remove();

          cartHelper.AddWishlist();
          notification.success(result.data.Message);
        } else {
          notification.warning(result.data.Message);
        }
      })
      .catch(function (error) {
        notification.error(error);
      })
      .finally(function () {
        $('.loading-box').hide();
      });
  }

  changeVariant(url, data, elementChange) {
    var inst = this;
    axios.post(url, data)
      .then(function (result) {
        var container = $(elementChange).parents('.product-tile-list__item').first();
        $(container).html(result.data);
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
    var code = $(element).attr('code');
    var shipmentId = $(element).attr('shipmentId');
    var qty = $(element).val();
    var url = $(element).attr('url');
    var data = {
      Code: code,
      ShipmentId: shipmentId,
      Quantity: qty
    };
    $(element).attr('disabled', 'disabled');
    axios.post(url, data)
      .then(function (result) {
        switch (result.data.StatusCode) {
          case 0:
            $(element).siblings('.required').html(result.data.Message);
            notification.warning(result.data.Message);
            break;
          case -1:
            notification.error(result.data.Message);
            break;
          default:
            notification.success(result.data.Message);
            inst.changeInfoCart(result);
            var subtotal = parseFloat($(element).attr('unitPrice')) * qty;
            $('.subtotal-' + code).html($(element).attr('currency') + subtotal);
            $(element).parents('.product-tile-list__item').first().find('.currentVariantInfo').attr('quantity', qty);
            break;
        }
      })
      .catch(function (error) {
        notification.error(error);
      })
      .finally(function () {
        $(element).removeAttr('disabled');
      });
  }


  initClearCart() {
    $('#clearCart').click(function () {
      if (confirm("Are you sure?")) {
        $('.loading-box').show();
        var url = $(this).attr('url');
        axios.post(url)
          .then(function (result) {
            notification.success("Delete cart successfully.");
            setTimeout(function () { window.location.href = result.data; }, 1000);
          })
          .catch(function (error) {
            notification.error(error);
          })
          .finally(function () {
            $('.loading-box').hide();
          });
      }
    });
  }

  loadMiniCartClick(urlLoadCart, clickSelector, reloadSelector) {
    var inst = this;
    $(clickSelector).click(function () {
      var isNeedReload = $(this).attr('reload');
      if (isNeedReload == 1) { // reload mini cart
        $(reloadSelector + " .loading-cart").show();
        axios.get(urlLoadCart, null)
          .then(function (result) {
            $(reloadSelector + " .cart-item-listing").html(result.data);
            inst.initRemoveItem(reloadSelector);
            $(clickSelector).attr('reload', 0);
          })
          .catch(function (error) {
            notification.error(error);
          })
          .finally(function () {
            $(reloadSelector + " .loading-cart").hide();
          });
      }
    });
  }

  initLoadCarts() {
    var inst = this;
    $('.jsCartBtn').each(function (i, e) {
      var url = $(e).data("reloadurl");
      var container = $(e).data("cartcontainer");
      inst.loadMiniCartClick(url, e, container);
    });
    $('.jsWishlistBtn').each(function (i, e) {
      var url = $(e).data("reloadurl");
      var container = $(e).data("cartcontainer");
      inst.loadMiniCartClick(url, e, container);
    });
    $('.jsSharedCartBtn').each(function (i, e) {
      var url = $(e).data("reloadurl");
      var container = $(e).data("cartcontainer");
      inst.loadMiniCartClick(url, e, container);
    });
  }

  initChangeVariant(selector) {
    var inst = this;
    if (selector == undefined) {
      selector = document;
    }

    $(selector).find('.jsChangeSizeVariantLargeCart').each(function (i, e) {
      $(e).change(function () {
        var parent = $(e).parents('.product-tile-list__item').first();
        var variantInfo = $(parent).find('.currentVariantInfo').first();
        var data = {
          Code: variantInfo.val(),
          Size: variantInfo.attr('size'),
          Quantity: variantInfo.attr('quantity'),
          NewSize: $(e).val(),
          ShipmentId: variantInfo.attr('shipmentId'),
          RequestFrom: "changeSizeItem"
        };
        var url = variantInfo.attr('url');

        inst.changeVariant(url, data, e);
      });
    });
  }

  initMoveToWishtlist(selector) {
    var inst = this;
    if (selector == undefined) {
      selector = document;
    }
    $(selector).find('.jsMoveToWishlist').each(function (i, e) {
      $(e).click(function () {
        inst.moveToWishlist($(e));
      });
    });
  }

  initChangeQuantityItem(selector) {
    var inst = this;
    if (selector == undefined) {
      selector = document;
    }
    $(selector).find('.jsChangeQuantityCartItem').each(function (i, e) {
      $(e).change(function () {
        var valueInt = parseInt($(e).val());
        if (!isNaN(valueInt) && Number.isInteger(valueInt)) {
          $(e).siblings('.required').html("");
          if (valueInt > 0)
            inst.changeQuantity($(e));
          else {
            if (confirm("Are you sure delete this item?")) {
              var elementDelete = $(e).parents('.product-tile-list__item').first().find('.jsRemoveCartItem').first();
              inst.removeItem('/defaultcart/RemoveCartItem', elementDelete, "large-cart");
            }
          }
        }
        else {
          $(e).siblings('.required').html("This field must be a number.");
        }
      });
    });
  }


  initRemoveItem(selector) {
    var inst = this;
    if (selector == undefined) {
      selector = document;
    }

    $(selector).find('.jsRemoveCartItem').each(function (i, e) {
      $(e).click(function () {
        if (confirm("Are you sure?")) {
          var type = $(this).attr('type');
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

          inst.removeItem(url, $(this), type);
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
      $('.jsCartBtn').each(function (i, e) {
        $(e).find('.icon-menu__badge').first().html(totalItem);
        $(e).attr('reload', 1);
      });
    }
  }

  setWishlistReload(totalItem) {
    if (totalItem != undefined) {
      $('.jsWishlistBtn').each(function (i, e) {
        $(e).find('.icon-menu__badge').first().html(totalItem);
        $(e).attr('reload', 1);
      });
    }
  }

  setSharedCartReload(totalItem) {
    if (totalItem != undefined) {
      $('.jsSharedCartBtn').each(function (i, e) {
        $(e).find('.icon-menu__badge').first().html(totalItem);
        $(e).attr('reload', 1);
      });
    }
  }

  addWishlist() {
    var wishlistHeader = $('#js-wishlist').children('.icon-menu__badge').first();

    var newQty = parseInt(wishlistHeader.html()) + 1;
    this.SetWishlistReload(newQty);
  }

  subtractWishlist() {
    var wishlistHeader = $('#js-wishlist').children('.icon-menu__badge').first();

    var newQty = parseInt(wishlistHeader.html()) + 1;
    this.SetWishlistReload(newQty);
  }
}