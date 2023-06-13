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
      document.querySelector(".loading-box").style.display = 'block'; // show
    data.requestFrom = "axios";
    axios.post(url, data)
        .then(function (result) {
        if (result.data.statusCode == 0) {
          notification.warning(result.data.message);
        }
        if (result.data.statusCode == 1) {
          let checkoutLink = "";
          let cartLink = "";
          if (document.querySelector('#checkoutBtnId')) {
              checkoutLink = document.querySelector('#checkoutBtnId').getAttribute('href');
          }

            if (document.querySelector('#cartBtnId')) {
                cartLink = document.querySelector('#cartBtnId').getAttribute('href');
          }

          let message = result.data.message;
          if (isAddToCart) {
            let bottomNotification = `\n<div style='display: flex; justify-content: space-between; margin-top: 15px;'>
                            <a href='`+ cartLink + `' class='btn-notification'>View Cart</a>
                            <a href='`+ checkoutLink + `' class='btn-notification'>Checkout</a>
                        </div>`;
            message += bottomNotification;
          }

          notification.success(message, false);

          if (callback) callback(result.data.countItems);
        }
      })
      .catch(function (error) {
        notification.error("Can not add the product to the cart.\n" + error.response.statusText);
      })
      .finally(function () {
          document.querySelector(".loading-box").style.display = 'none'; // show
      });

    return false;
  }

  // use in Wishlist Page
  removeItem(data, url, message, callback) {
      document.querySelector(".loading-box").style.display = 'block'; // show
    axios.post(url, data)
      .then(function (result) {
        if (result.status == 200) {
          notification.success(message);
          document.querySelector('#my-wishlist').innerHTML = result.data;
          feather.replace();

          let product = new Product('#my-wishlist');
          product.init();
            let count = document.querySelector('#countWishListInPage').value;
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
          document.querySelector(".loading-box").style.display = 'none'; // hide
      });
  }

  callbackAddToCart(selector, count) {
    if (selector == ".jsCartBtn") { cartHelper.setCartReload(count); }
    else if (selector == ".jsSharedCartBtn") { cartHelper.setSharedCartReload(count) }
    else cartHelper.setWishlistReload(count);
  }

  addToSharedCartClick() {
      let inst = this;
      Array.from(document.querySelectorAll(".addToSharedCart")).forEach(function (el, i) {
      el.addEventListener("click", function () {
        let code = el.getAttribute('data');

        let callback = (count) => {
          inst.callbackAddToCart('.jsSharedCartBtn', count);
        };

        inst.addToCart({ Code: code }, '/SharedCart/AddToCart', callback);
      });
    });
  }

  addToWishlistClick() {
    let inst = this;
      Array.from(document.querySelectorAll(".addToWishlist")).forEach(function (el, i) {
      el.addEventListener("click", function () {
        let code = el.getAttribute('data');

        let callback = (count) => {
          inst.callbackAddToCart('#js-wishlist', count);
        };

        inst.addToCart({ Code: code }, '/Wishlist/AddToCart', callback);
      });
    });

  }

  addToCartClick() {
    let inst = this;
      Array.from(document.querySelectorAll(".addToCart")).forEach(function (el, i) {
      el.setAttribute("href", "javascript:void(0);")
          el.addEventListener("click", function () {
        let code = el.getAttribute('data');
        let data = {
          Code: code
        };

          if (el.getAttribute('qty') != null) data.Quantity = el.getAttribute('qty');
          if (el.getAttribute('store') != null) data.Store = el.getAttribute('store');
          if (el.getAttribute('selectedStore') != null) data.SelectedStore = el.getAttribute('selectedStore');
          if (document.querySelector('.jsDynamicOptions') != null || document.querySelector('.jsDynamicOptionsInSubgroup') != null) {
          data.DynamicCodes = [];

              Array.from(document.querySelectorAll('.jsDynamicOptions')).forEach(function (el, i) {
                  if (el.checked)
                      data.DynamicCodes.push(el.value);
              });

              Array.from(document.querySelectorAll('.jsDynamicOptionsInSubgroup')).forEach(function (el, i) {
                  if (el.checked) {
                      if (el.closest('.tab-pane').classList.contains('active'))
                                  data.DynamicCodes.push(dynamicOption.value);
                  }
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
      Array.from(document.querySelectorAll(".deleteLineItemWishlist")).forEach(function (el, i) {
      el.addEventListener("click", function () {
        if (confirm("Are you sure?")) {
          let code = el.getAttribute('data');
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
        Array.from(document.querySelectorAll(".jsAddAllToCart")).forEach(function (el, i) {
            el.addEventListener("click", function () {
        document.querySelector(".loading-box").style.display = 'block'; // show
        let url = el.getAttribute('url');
        axios.post(url)
          .then(function (result) {
            notification.success(result.data.message);
            cartHelper.setCartReload(result.data.countItems);
          })
          .catch(function (error) {
            notification.error(error);
          })
          .finally(function () {
              document.querySelector(".loading-box").style.display = 'none';
          });

      });
    });

  }
}