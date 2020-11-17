export default class Product {
  constructor(divId) {
    if (divId) {
      this.divContainerId = divId;
    } else {
      this.divContainerId = document;
    }
  }

  init() {
    this.addToWishlistClick();
    this.addToSharedCartClick();
    this.addToCartClick();
    this.addAllToCartClick();
    this.deleteWishlistClick();
  }

  addToCart(data, url, callback, isAddToCart) {
    $('body > .loading-box').show();
    data.requestFrom = "axios";
    axios.post(url, data)
      .then(function (result) {
        if (result.data.StatusCode == 0) {
          notification.warning(result.data.Message);
        }
        if (result.data.StatusCode == 1) {
          let checkoutLink = "";
          let cartLink = "";
          if ($('#checkoutBtnId')) {
            checkoutLink = $('#checkoutBtnId').attr('href');
          }

          if ($('#cartBtnId')) {
            cartLink = $('#cartBtnId').attr('href');
          }

          let message = result.data.Message;
          if (isAddToCart) {
            let bottomNotification = `\n<div style='display: flex; justify-content: space-between; margin-top: 15px;'>
                            <a href='`+ cartLink + `' class='btn-notification'>View Cart</a>
                            <a href='`+ checkoutLink + `' class='btn-notification'>Checkout</a>
                        </div>`;
            message += bottomNotification;
          }

          notification.success(message, false);

          if (callback) callback(result.data.CountItems);
        }
      })
      .catch(function (error) {
        notification.error("Can not add the product to the cart.\n" + error.response.statusText);
      })
      .finally(function () {
        $('body>.loading-box').hide();
      });

    return false;
  }

  // use in Wishlist Page
  removeItem(data, url, message, callback) {
    $('body>.loading-box').show();
    axios.post(url, data)
      .then(function (result) {
        if (result.status == 200) {
          notification.success(message);
          $('#my-wishlist').html(result.data);
          feather.replace();

          let product = new Product('#my-wishlist');
          product.init();
          let count = $('#countWishListInPage').val();
          if (callback) callback(count);
        }
        if (result.status == 204) {
          notification.error(result.statusText);
        }
      })
      .catch(function (error) {
        notification.error(error);
      })
      .finally(function () {
        $('body>.loading-box').hide();
      });
  }

  callbackAddToCart(selector, count) {
    if (selector == ".jsCartBtn") { cartHelper.setCartReload(count); }
    else if (selector == ".jsSharedCartBtn") { cartHelper.setSharedCartReload(count) }
    else cartHelper.setWishlistReload(count);
  }

  addToSharedCartClick() {
    let inst = this;
    $(this.divContainerId).find('.addToSharedCart').each(function (i, e) {
      $(e).click(function () {
        let code = $(this).attr('data');

        let callback = (count) => {
          inst.callbackAddToCart('.jsSharedCartBtn', count);
        };

        inst.addToCart({ Code: code }, '/SharedCart/AddToCart', callback);
      });
    });
  }

  addToWishlistClick() {
    let inst = this;

    $(this.divContainerId).find('.addToWishlist').each(function (i, e) {
      $(e).click(function () {
        let code = $(this).attr('data');

        let callback = (count) => {
          inst.callbackAddToCart('#js-wishlist', count);
        };

        inst.addToCart({ Code: code }, '/Wishlist/AddToCart', callback);
      });
    });

  }

  addToCartClick() {
    let inst = this;

    $(this.divContainerId).find('.addToCart').each(function (i, e) {
      $(e).attr("href", "javascript:void(0);")
      $(e).click(function () {
        let code = $(this).attr('data');
        let data = {
          Code: code
        };

        if ($(this).attr('qty')) data.Quantity = $(this).attr('qty');
        if ($(this).attr('store')) data.Store = $(this).attr('store');
        if ($(this).attr('selectedStore')) data.SelectedStore = $(this).attr('selectedStore');
        //if ($(this).attr('dynamicCodes')) data.DynamicCodes = $(this).attr('dynamicCodes');
        if ($('.jsDynamicOptions').length > 0 || $('.jsDynamicOptionsInSubgroup').length > 0) {
          data.DynamicCodes = [];
          $('.jsDynamicOptions:checked').each(function (j, dynamicOption) {
            data.DynamicCodes.push(dynamicOption.value);
          })
          $('.jsDynamicOptionsInSubgroup:checked').each(function (j, dynamicOption) {
            if ($(dynamicOption).closest('.tab-pane').hasClass('active'))
              data.DynamicCodes.push(dynamicOption.value);
          })    
        }

        let callback = (count) => {
          inst.callbackAddToCart('.jsCartBtn', count);
        };

        inst.addToCart(data, '/DefaultCart/AddToCart', callback, true);
      });
    });
  }

  deleteWishlistClick() {
    let inst = this;

    $(this.divContainerId).find('.deleteLineItemWishlist').each(function (i, e) {
      $(e).click(function () {
        if (confirm("Are you sure?")) {
          let code = $(e).attr('data');
          let data = { Code: code, Quantity: 0, RequestFrom: "axios" };
          let callback = (count) => {
            inst.callbackAddToCart("#js-wishlist", count);
          };
          inst.removeItem(data, '/Wishlist/ChangeCartItem', "Removed " + code + " from wishlist", callback);
        }
      });
    });
  }

  addAllToCartClick() {
    $(this.divContainerId).find('.jsAddAllToCart').each(function (i, e) {
      $(e).click(function () {
        $('.loading-box').show();
        let url = $(this).attr('url');
        axios.post(url)
          .then(function (result) {
            notification.success(result.data.Message);
            cartHelper.setCartReload(result.data.CountItems);
          })
          .catch(function (error) {
            notification.error(error);
          })
          .finally(function () {
            $('.loading-box').hide();
          });

      });
    });

  }
}