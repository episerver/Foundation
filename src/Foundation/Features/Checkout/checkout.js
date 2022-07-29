import feather from "feather-icons";

export default class Checkout {
  addPaymentClick() {
    let inst = this;
    //$('.jsAddPayment').click(function () {
      alert(" addPaymentClick ");
      if (document.querySelector(".jsAddPayment") == null) return;
      document.querySelector(".jsAddPayment").addEventListener("click", function (e) {
          alert(" 123 ");
          if (document.getElementById("SelectedCreditCardId") != null && document.getElementById("SelectedCreditCardId").innerText == "Select credit card") {
              notification.error("You have to select Credit card");
              return;
          }

          //$('.loading-box').show();
          document.querySelector(".loading-box").style.display = 'block'; // show

          //let url = urlParam;
          let url = this.attr('url');
          //let checked = $('.jsChangePayment:checked');
          let checked = document.querySelector('.jsChangePayment:checked');
          let methodId = checked.attr('methodId');
          let keyword = checked.attr('keyword');

          let additionVal = {
              PaymentMethodId: methodId,
              SystemKeyword: keyword
          };

          //let data = $('.jsCheckoutForm').serialize() + '&' + $.param(additionVal);

          let form = document.querySelector('.jsCheckoutForm');

          // Get all field data from the form
          let data = new FormData(form);

          // Convert to an object
          let formObj = serialize(data) + '&' + $.param(additionVal);

          axios.post(url, formObj)
              .then(function (result) {
                  if (result.status != 200) {
                      notification.error(result);
                  } else {
                      //$('#paymentBlock').html(result.data);
                      document.querySelector('#paymentBlock').innerHTML = result.data;
                      feather.replace();
                      inst.initPayment();
                  }
              })
              .catch(function (error) {
                  if (error.response.status == 400) {
                      document.querySelector("#giftcard-alert").innerHTML = error.response.statusText;
                      //$("#giftcard-alert").html(error.response.statusText);
                      //$("#giftcard-alert").removeClass("alert-info");
                      document.querySelector("#giftcard-alert").removeClass("alert-info");
                      //$("#giftcard-alert").addClass("alert-danger");
                      document.querySelector("#giftcard-alert").addClass("alert-danger");
                  } else {
                      notification.error(error);
                  }
              })
              .finally(function () {
                  //$('.loading-box').hide();
                  document.querySelector(".loading-box").style.display = "none";
              });
          });
      }
  

    removePayment(element) {
        alert(" removePayment ");
    let inst = this;
    //$('.loading-box').show();
      document.querySelector(".loading-box").style.display = "block";
    //let url = $(element).data('url');
    let url = document.querySelector(element).data.url;
    //let methodId = $(element).data('method-id');
    let methodId = document.querySelector(element).data.methodId;
    //let keyword = $(element).data('keyword');
    let keyword = document.querySelector(element).data.keyword;
    //let paymentTotal = $(element).data('amount');
    let paymentTotal = document.querySelector(element).data.paymentTotal;
    let data = {
      PaymentMethodId: methodId,
      SystemKeyword: keyword,
      'OrderSummary.PaymentTotal': paymentTotal
    };

    axios.post(url, data)
      .then(function (result) {
        //$('#paymentBlock').html(result.data);
          document.querySelector("#paymentBlock").innerHTML = result.data;
        feather.replace();
        inst.initPayment();
      })
      .catch(function (error) {
        notification.error(error);
      })
      .finally(function () {
        //$('.loading-box').hide();
          document.querySelector(".loading-box").style.display = "none";
      });
  }

  removePaymentClick() {
      let inst = this;
      alert(" removePaymentClick ");
    //$('.jsRemovePayment').each(function (i, e) {
    //  console.log(i, e);
    //  $(e).click(function () {
    //    console.log(i, e);
    //    inst.removePayment(e);
    //  });
    //});
      if (document.querySelector(".jsRemovePayment") == null) return;
      Array.from(document.querySelector(".jsRemovePayment")).forEach(function (el, i) {
        el.addEventListener('click', inst.removePayment(el));
    });
    
  }

  paymentMethodChange() {
      let inst = this;
      alert(" paymentMethodChange ");
    //$('.jsChangePayment').each(function (i, e) {
    //  $(e).change(function () {
    //    $('.jsPaymentMethod').siblings('.loading-box').first().show();
    //    let url = $(e).attr('url');
    //    let methodId = $(e).attr('methodid');
    //    let keyword = $(e).attr('keyword');
    //    let data = {
    //      PaymentMethodId: methodId,
    //      SystemKeyword: keyword
    //    };

    //    axios.post(url, data)
    //      .then(function (result) {
    //        $('.jsPaymentMethod').html(result.data);
    //        feather.replace();
    //        inst.creditCardChange();
    //      })
    //      .catch(function (error) {
    //        notification.error(error);
    //      })
    //      .finally(function () {
    //        $('.loading-box').hide();
    //      });
    //  });
    //});

      if (document.querySelector(".jsChangePayment") == null) return;
      Array.from(document.querySelector(".jsChangePayment")).forEach(function (el, i) {
        el.addEventListener("change", function () {
            document.querySelector(".jsPaymentMethod").nextSibling(".loading").first.style.display = "block";
            let url = el.attr('url');
            let methodId = el.attr('methodid');
            let keyword = el.attr('keyword');
            let data = {
              PaymentMethodId: methodId,
              SystemKeyword: keyword
            };

            axios.post(url, data)
              .then(function (result) {
                  document.querySelector(".jsPaymentMethod").innerHTML = result.data;
                feather.replace();
                inst.creditCardChange();
              })
              .catch(function (error) {
                notification.error(error);
              })
              .finally(function () {
                  document.querySelector(".loading-box").style.display = "none";
              });
        });
    });
  }

    creditCardChange() {

        alert(" creditCardChange ");
    //$('.jsSelectCreditCard').each(function (i, e) {
    //  $(e).change(function () {
    //    $('.selectCreditCardType').hide();
    //    let targetId = $(e).val();
    //    $(targetId).show();
    //  });
    //});

        if (document.querySelector(".jsSelectCreditCard") == null) return;
        Array.from(document.querySelector(".jsSelectCreditCard")).forEach(function (el, i) {
        el.addEventListener("change", function () {
            document.querySelector(".selectCreditCardType").style.display = "none";
            let targetId = el.value;
            targetId.style.display = "block";
        });
    });
  }

  initPayment() {
    let inst = this;
    inst.addPaymentClick();
    inst.removePaymentClick();
    inst.paymentMethodChange();
    inst.creditCardChange();
  }

  ///

  // Shipping Address
    formShippingAddressChange() {

        alert(" formShippingAddressChange ");
    //$('.jsSingleAddress').each(function (i, e) {
    //  $(e).change(function () {
    //    let shippingRow = $(e).parents('.jsShippingAddressRow').first();
    //    let value = $(this).val();
    //    $('#AddressType').val(value);
    //    if (value == 0) {
    //      shippingRow.find('.jsOldShippingAddressForm').hide();
    //      shippingRow.find('.jsNewShippingAddressForm').show();
    //    } else {
    //      shippingRow.find('.jsOldShippingAddressForm').show();
    //      shippingRow.find('.jsNewShippingAddressForm').hide();
    //    }

    //  });
    //});

    ////Array.prototype.forEach.call(document.querySelector(".jsSingleAddress"), function (el, index, array) {
    ////    el.addEventListener("change", function () {
    ////        let shippingRow = el.parents.querySelector('.jsShippingAddressRow').first();
    ////        let value = this.value
    ////        document.querySelector('#AddressType').value = value;
    ////        if (value == 0) {
    ////          shippingRow.find('.jsOldShippingAddressForm').hide();
    ////          shippingRow.find('.jsNewShippingAddressForm').show();
    ////        } else {
    ////          shippingRow.find('.jsOldShippingAddressForm').show();
    ////          shippingRow.find('.jsNewShippingAddressForm').hide();
    ////        }

    ////    });

    ////});
  }

    formBillingAddressChange() {

        alert(" formBillingAddressChange ");
    //$('.jsBillingAddress').each(function (i, e) {
    //  $(e).click(function () {
    //    let value = $(e).val();
    //    $('#AddressType').val(value);
    //    if (value == 0) {
    //      $('#oldBillingAddressForm').hide();
    //      $('#newBillingAddressForm').show();
    //    } else if (value == 1) {
    //      $('#oldBillingAddressForm').show();
    //      $('#newBillingAddressForm').hide();
    //    } else if (value == 2) {
    //      $('#oldBillingAddressForm').hide();
    //      $('#newBillingAddressForm').hide();
    //    }
    //  });
    //});
        if (document.querySelector(".jsBillingAddress") == null) return;
        Array.from(document.querySelector(".jsBillingAddress")).forEach(function (el, i) {
        el.addEventListener("click", function () {
            let value = el.value;
            document.querySelector('#AddressType').value = value;
            ////if (value == 0) {
            ////    document.querySelector('#oldBillingAddressForm').style.display = "none";
            ////    document.querySelector('#newBillingAddressForm').style.display = "block";
            ////} else if (value == 1) {
            ////    document.querySelector('#oldBillingAddressForm').style.display = "block";
            ////    document.querySelector('#newBillingAddressForm').style.display = "none";
            ////} else if (value == 2) {
            ////    document.querySelector(('#oldBillingAddressForm').style.display = "none";
            ////    document.querySelector(('#newBillingAddressForm').style.display = "none";
            ////}
        });

    });
  }

    checkoutAsGuestOrRegister() {
        alert(" checkoutAsGuestOrRegister ");
    //$('.jsContinueCheckoutMethod').click(function () {
    //  let type = $('input[name="checkoutMethod"]:checked').val();
    //  if (type == 'register') {
    //    $('#js-profile-popover').css("visibility", "visible");
    //    $('#login-selector-signup-tab').click();
    //    return false;
    //  }
    //});
        if (document.querySelector(".jsContinueCheckoutMethod") == null) return;
    document.querySelector('.jsContinueCheckoutMethod').addEventListener("click" ,function () {
        let type = document.querySelector('input[name="checkoutMethod"]:checked').value;
        if (type == 'register') {
            document.querySelector('#js-profile-popover').style.display = "block";
            document.querySelector('#login-selector-signup-tab').addEventListener("click", function () {
                return false;
            });
        }
    });
  }

  applyCouponCode() {
      let inst = this;
      alert(" applyCouponCode ");
    //$('.jsCouponCode').keypress(function (e) {
    //  if (e.keyCode == 13) {
    //    $('.jsAddCoupon').click();
    //    return false;
    //  }
    //});
      if (document.querySelector(".jsCouponCode") == null) return;
    document.querySelector('.jsCouponCode').addEventListener("keypress",function (e) {
        if (e.keyCode == 13) {
            document.querySelector('.jsAddCoupon').addEventListener("click", function () { 
                return false;
            });
        }
    });

    //$('.jsAddCoupon').click(function () {
    //  let e = this;
    //  let form = $(this).parents('.jsAddCouponContainer').first();
    //  let url = form.attr('action');
    //  let couponCode = form.find('.jsCouponCode').val();
    //  let data = convertFormData({ couponCode: couponCode });

    //  axios.post(url, data)
    //    .then(function (r) {
    //      if (r.status == 200) {
    //        $('.jsCouponLabel').removeClass('hidden');
    //        if ($(e).hasClass('jsInCheckout')) {
    //          $('.jsCouponListing').append(inst.couponTemplate(couponCode, "jsInCheckout"));
    //        } else {
    //          $('.jsCouponListing').append(inst.couponTemplate(couponCode, ""));
    //        }

    //        inst.removeCouponCode($('.jsRemoveCoupon[data-couponcode=' + couponCode + ']'));
    //        $('.jsCouponReplaceHtml').html(r.data);
    //        $('.jsOrderSummary').html($('.jsOrderSummaryInPayment').html());
    //        feather.replace();
    //        if ($(e).hasClass('jsInCheckout')) {
    //          inst.initPayment();
    //        }
    //        form.find('.jsCouponCode').val("");
    //        $('.jsCouponErrorMess').hide();
    //      } else {
    //        $('.jsCouponErrorMess').show();
    //      }
    //    })
    //    .catch(function (e) {
    //      notification.error(e);
    //    });
    //});
      if (document.querySelector(".jsAddCoupon") == null) return;
    document.querySelector('.jsAddCoupon').addEventListener("click", function () {
        let e = this;
        let form = e.parents.querySelector('.jsAddCouponContainer').first();
        let url = form.attr('action');
        let couponCode = form.find('.jsCouponCode').val();
        let data = convertFormData({ couponCode: couponCode });

        axios.post(url, data)
            .then(function (r) {
                if (r.status == 200) {
                    document.querySelector('.jsCouponLabel').removeClass('hidden');
                    if (e.hasClass("jsInCheckout")) {
                        document.querySelector('.jsCouponListing').append(inst.couponTemplate(couponCode, "jsInCheckout"));
                    } else {
                        document.querySelector('.jsCouponListing').append(inst.couponTemplate(couponCode, ""));
                    }

                    ////inst.removeCouponCode($('.jsRemoveCoupon[data-couponcode=' + couponCode + ']'));
                    ////document.querySelector('.jsCouponReplaceHtml').innerHTML = r.data;
                    ////document.querySelector('.jsOrderSummary').innerHTML = (document.querySelector('.jsOrderSummaryInPayment').innerHTML);
                    feather.replace();
                    if (e.hasClass('jsInCheckout')) {
                        inst.initPayment();
                    }
                    form.querySelector('.jsCouponCode').value = "";
                    document.querySelector('.jsCouponErrorMess').style.display = "none";
                } else {
                    document.querySelector('.jsCouponErrorMess').style.display = "block";
                }
            })
            .catch(function (e) {
                notification.error(e);
            });
    });
  }

    removeCouponCode(selector) {
        alert(" removeCouponCode ");
    let inst = this;
    if (selector) {
      inst.removeCoupon(selector);
    } else {
      //$('.jsRemoveCoupon').each(function (i, e) {
      //  inst.removeCoupon(e);
      //});
        if (document.querySelector(".jsRemoveCoupon") == null) return;
        Array.from(document.querySelector(".jsRemoveCoupon")).forEach(function (el, i) {
            inst.removeCoupon(el);
        });
    }
  }

  //removeCoupon(e) {
  //  let inst = this;
  //  $(e).click(function () {
  //    let element = $(this);
  //    let url = $('#jsRenoveCouponUrl').val();
  //    let couponCode = $(this).data('couponcode');
  //    let data = convertFormData({ couponCode: couponCode });

  //    axios.post(url, data)
  //      .then(function (r) {
  //        element.remove();
  //        let coupons = $('.jsCouponListing').find('.jsRemoveCoupon');
  //        if (coupons.length == 0) {
  //          $('.jsCouponLabel').addClass('hidden');
  //        }
  //        $('.jsCouponReplaceHtml').html(r.data);
  //        $('.jsOrderSummary').html($('.jsOrderSummaryInPayment').html());
  //        if ($(e).hasClass('jsInCheckout')) {
  //          feather.replace();
  //          inst.initPayment();
  //        }

  //        $('.jsCouponErrorMess').hide();
  //      })
  //      .catch(function (e) {
  //        notification.error(e);
  //      });
  //  });
  //}

    removeCoupon(e) {
        alert(" removeCoupon ");
        let inst = this;
        if (e == null) return;
        e.addEventListener("click", function () {
        let url = document.querySelector('#jsRenoveCouponUrl').value;
        let couponCode = this.data.couponCode;
        let data = convertFormData({ couponCode: couponCode });

        axios.post(url, data)
            .then(function (r) {
                element.remove();
                let coupons = document.querySelector(".jsCouponListing").querySelectorAll(".jsRemoveCoupon");
                if (coupons.length == 0) {
                    document.querySelector('.jsCouponLabel').addClass('hidden');
                }
                ////document.querySelector('.jsCouponReplaceHtml').innerHTML =  r.data;
                ////document.querySelector(('.jsOrderSummary').innerHTML = document.querySelector('.jsOrderSummaryInPayment').innerHTML;
                ////if (document.querySelector(e).hasClass('jsInCheckout')) {
                ////    feather.replace();
                ////    inst.initPayment();
                ////}

                document.querySelector('.jsCouponErrorMess').style.display = "none";
            })
            .catch(function (e) {
                notification.error(e);
            });
    });
}

  couponTemplate(couponCode, jsInCheckout) {
    return `<label class="filters-tag jsRemoveCoupon ${jsInCheckout}" data-couponcode="${couponCode}">
                    <span>${couponCode}</span>
                    <span class="filters-tag__remove"><i class="cursor-pointer" data-feather="x" width="12"></i></span>
                </label>`;
  }

//  changeShippingMethod() {
//    let inst = this;
//    $('.jsShippingMethodContainer').each(function (i, e) {
//      $(e).change(function () {
//        let isInstorePickup = $(e).find('.jsChangeShipment:checked').attr('instorepickup');
//        if (isInstorePickup == "True") {
//          $(e).parents('.jsShipmentRow').find('.jsShippingAddressRow').hide();
//        } else {
//          $(e).parents('.jsShipmentRow').find('.jsShippingAddressRow').show();
//        }

//        let url = $(e).attr('url');
//        let data = $('.jsCheckoutForm').serialize();
//        $('.loading-box').show();

//        axios.post(url, data)
//          .then(function (r) {
//            $('.jsCouponReplaceHtml').html(r.data);
//            $('.jsOrderSummary').html($('.jsOrderSummaryInPayment').html());
//            feather.replace();
//            inst.initPayment();
//          })
//          .catch(function (e) {
//            notification.error(e);
//          })
//          .finally(function () {
//            $('.loading-box').hide();
//          });
//      });
//    });
//}

    changeShippingMethod() {
        alert(" changeShippingMethod ");
        let inst = this;
        if (document.querySelector(".jsShippingMethodContainer") == null) return;
        Array.from(document.querySelector(".jsShippingMethodContainer")).forEach(function (el, i) {
        document.querySelector(el).addEventListener("change", function () {
            let isInstorePickup = document.querySelector(el).find('.jsChangeShipment:checked').attr('instorepickup');
            if (isInstorePickup == "True") {
                document.querySelector( el).parents.querySelector('.jsShipmentRow').find('.jsShippingAddressRow').style.display = "none";
            } else {
                document.querySelector(el).parents.querySelector('.jsShipmentRow').find('.jsShippingAddressRow').style.display = "block";
            }

            let url = document.querySelector(el).attr('url');
            let data = document.querySelector('.jsCheckoutForm').serialize();
            document.querySelector('.loading-box').style.display = "block";

            axios.post(url, data)
                .then(function (r) {
                    document.querySelector('.jsCouponReplaceHtml').innerHTML = r.data;
                    document.querySelector('.jsOrderSummary').innerHTML = (document.querySelector('.jsOrderSummaryInPayment').innerHTML);
                    feather.replace();
                    inst.initPayment();
                })
                .catch(function (e) {
                    notification.error(e);
                })
                .finally(function () {
                    document.querySelector('.loading-box').style.display = "none";
                });
        });
    });
}

  //changeCartItem() {
  //  let inst = this;
  //  $('.jsChangeQuantityItemCheckout').each(function (i, e) {
  //    $(e).change(function () {
  //      $('.loading-box').show();
  //      let quantity = $(e).val();
  //      let code = $(e).data('code');
  //      let url = $(e).data('url');
  //      let shipmentId = $(e).data('shipmentid');
  //      let data = {
  //        code: code,
  //        quantity: quantity,
  //        shipmentId: shipmentId
  //      };

  //      axios.post(url, data)
  //        .then(function (r) {
  //          if (quantity == 0) {
  //            let parent = $(e).parents('.jsShipmentRow');
  //            $(e).parents('.jsCartItem').first().remove();

  //            if (parent.find('.jsCartItem').length == 0) {
  //              parent.remove();
  //              window.location.href = window.location.href;
  //            }
  //          }

  //          if (quantity > 1) {
  //            let btn = $(e).parents('.jsCartItem').find('.jsSeparateHint');
  //            btn.parent('div').removeClass('hidden');
  //            btn.addClass('jsSeparateBtn');
  //            inst.separateClick(btn);
  //          } else {
  //            let btn = $(e).parents('.jsCartItem').find('.jsSeparateHint');
  //            btn.parent('div').addClass('hidden');
  //            btn.removeClass('jsSeparateBtn');
  //          }

  //          $('.jsCouponReplaceHtml').html(r.data);
  //          $('.jsOrderSummary').html($('.jsOrderSummaryInPayment').html());
  //          cartHelper.setCartReload($('.jsTotalQuantityCheckout').val());
  //          feather.replace();
  //          inst.initPayment();
  //        })
  //        .catch(function (e) {
  //          notification.error(e);
  //        })
  //        .finally(function () {
  //          $('.loading-box').hide();
  //        });
  //    });
  //  });
  //}

    changeCartItem() {
        alert(" changeCartItem ");
        let inst = this;
        if (document.querySelector(".jsChangeQuantityItemCheckout") == null) return;
        Array.from(document.querySelector(".jsChangeQuantityItemCheckout")).forEach(function (e, i) {
        document.querySelector(e).change(function () {
            document.querySelector('.loading-box').style.display = "block";
            let quantity = document.querySelector(e).value;
            let code = document.querySelector(e).data.code
            let url = document.querySelector(e).data.url;
            let shipmentId = document.querySelector(e).data.shipmentId;
            let data = {
                code: code,
                quantity: quantity,
                shipmentId: shipmentId
            };

            axios.post(url, data)
                .then(function (r) {
                    if (quantity == 0) {
                        let parent = document.querySelector(e).parents.querySelector('.jsShipmentRow');
                        document.querySelector(e).parent().parents('.jsCartItem').first().remove();

                        if (parent.querySelectorAll('.jsCartItem').length == 0) {
                            parent.remove();
                            window.location.href = window.location.href;
                        }
                    }

                    if (quantity > 1) {
                        let btn = document.querySelector(e).parents('.jsCartItem').find('.jsSeparateHint');
                        btn.parent('div').removeClass('hidden');
                        btn.addClass('jsSeparateBtn');
                        inst.separateClick(btn);
                    }
                    else {
                        let btn = document.querySelector((e).parents('.jsCartItem').querySelectorAll('.jsSeparateHint'));
                        btn.parent('div').addClass('hidden');
                        btn.removeClass('jsSeparateBtn');
                    }

                    document.querySelector('.jsCouponReplaceHtml').innerHTML = r.data;
                    document.querySelector('.jsOrderSummary').innerHTML = (document.querySelector('.jsOrderSummaryInPayment').innerHTML);
                    cartHelper.setCartReload(document.querySelector('.jsTotalQuantityCheckout').value);
                    feather.replace();
                    inst.initPayment();
                })
                .catch(function (e) {
                    notification.error(e);
                })
                .finally(function () {
                    document.querySelector('.loading-box').style.display = "none";
                });
        });
    });
}
  //separateClick(selector) {
  //  if (selector) {
  //    $(selector).click(function () {
  //      $('.jsSelectShipment').each(function (j, s) {
  //        $(s).show();
  //      });
  //      let code = $(selector).data('code');
  //      let shipmentid = $(selector).data('shipmentid');
  //      let qty = $(selector).parents('.jsCartItem').find('.jsChangeQuantityItemCheckout').val();
  //      let delivery = $(selector).data('delivery');
  //      let selectedstore = $(selector).data('selectedstore');
  //      $('#lineItemInfomation').data("code", code);
  //      $('#lineItemInfomation').data("shipmentid", shipmentid);
  //      $('#lineItemInfomation').data("qty", qty);
  //      $('#lineItemInfomation').data("delivery", delivery);
  //      $('#lineItemInfomation').data("selectedstore", selectedstore);

  //      $('.jsSelectShipment[data-shipmentid=' + shipmentid + ']').hide();
  //    });
  //  } else {
  //    $('.jsSeparateBtn').each(function (i, e) {
  //      $(e).click(function () {
  //        $('.jsSelectShipment').each(function (j, s) {
  //          $(s).show();
  //        });

  //        let code = $(e).data('code');
  //        let shipmentid = $(e).data('shipmentid');
  //        let qty = $(e).parents('.jsCartItem').find('.jsChangeQuantityItemCheckout').val();
  //        let delivery = $(e).data('delivery');
  //        let selectedstore = $(e).data('selectedstore');
  //        $('#lineItemInfomation').data("code", code);
  //        $('#lineItemInfomation').data("shipmentid", shipmentid);
  //        $('#lineItemInfomation').data("qty", qty);
  //        $('#lineItemInfomation').data("delivery", delivery);
  //        $('#lineItemInfomation').data("selectedstore", selectedstore);

  //        $('.jsSelectShipment[data-shipmentid=' + shipmentid + ']').hide();
  //      });
  //    });
  //  }
  //}

    separateClick(selector) {
        alert(" separateClick ");
    if (selector) {
        document.querySelector(selector).addEventListener("click",function () {
            document.querySelector('.jsSelectShipment').each(function (j, s) {
                document.querySelector(s).style.display = "block";
            });
            ////let code = document.querySelector(selector).data.code;
            ////let shipmentid = document.querySelector((selector).data.shipmentId;
            ////let qty = document.querySelector(selector).parents('.jsCartItem').querySelector('.jsChangeQuantityItemCheckout').value;
            ////let delivery = $(selector).data.delivery;
            ////let selectedstore = $(selector).data.selectedstore;
            ////document.querySelector('#lineItemInfomation').data.code = code;
            ////document.querySelector('#lineItemInfomation').data.shipmentId = shipmentid;
            ////document.querySelector('#lineItemInfomation').data.qty = qty;
            ////document.querySelector('#lineItemInfomation').data.delivery = delivery;
            ////document.querySelector('#lineItemInfomation').data.selectedstore = selectedstore;

            ////document.querySelector('.jsSelectShipment[data-shipmentid=' + shipmentid + ']').style.display = "none";
        });
    } else {
        if (document.querySelector(".jsSeparateBtn") == null) return;
        Array.from(document.querySelector(".jsSeparateBtn")).forEach(function (e, i) {
            e.addEventListener("click", function () {

                if (document.querySelector(".jsSelectShipment") == null) return;
                Array.from(document.querySelector(".jsSelectShipment")).forEach(function (s, i) {
                    s.style.display = "block";
                });

                let code = e.data.code;
                let shipmentid = e.data.shipmentid;
                let qty = e.parents.querySelector('.jsCartItem').find('.jsChangeQuantityItemCheckout').value;
                let delivery = e.data.delivery;
                let selectedstore = e.data.selectedstore;
                ////document.querySelector('#lineItemInfomation').data.code =  code;
                ////document.querySelector('#lineItemInfomation').data.shipmentid = shipmentid;
                ////document.querySelector('#lineItemInfomation').data.qty =  qty;
                ////document.querySelector(('#lineItemInfomation').data.delivery = delivery;
                ////document.querySelector('#lineItemInfomation').data.selectedstore = selectedstore;

                ////document.querySelector('.jsSelectShipment[data-shipmentid=' + shipmentid + ']').style.display = "none";
            });
        });
    }
}

  //confirmSeparateItemClick() {
  //  $('.jsSelectShipment').each(function (i, e) {
  //    $(e).click(function () {
  //      $('.loading-box').show();
  //      let url = $('#lineItemInfomation').data('url');
  //      let code = $('#lineItemInfomation').data('code');
  //      let shipmentid = $('#lineItemInfomation').data('shipmentid');
  //      let qty = $('#lineItemInfomation').data('qty');
  //      let delivery = $('#lineItemInfomation').data('delivery');
  //      let selectedstore = $('#lineItemInfomation').data('selectedstore');
  //      let toShipmentId = $(e).data('shipmentid');
  //      let data = {
  //        Code: code,
  //        Quantity: qty,
  //        ShipmentId: shipmentid,
  //        ToShipmentId: toShipmentId,
  //        DeliveryMethodId: delivery,
  //        SelectedStore: selectedstore
  //      };

  //      axios.post(url, data)
  //        .then(function (r) {
  //          if (r.data.Status == true) {
  //            window.location.href = r.data.RedirectUrl;
  //          } else {
  //            notification.error(r.data.Message);
  //          }
  //        })
  //        .catch(function (e) {
  //          notification.error(e);
  //        })
  //        .finally(function () {
  //          $('.loading-box').hide();
  //        });
  //    });
  //  });
  //}

    confirmSeparateItemClick() {
        alert(" confirmSeparateItemClick ");
        if (document.querySelector(".jsSelectShipment") == null) return;
        Array.from(document.querySelector(".jsSelectShipment")).forEach(function (e, i) {
        e.addEventListener("click", function () {
            document.querySelector('.loading-box').style.display = "block";
            let url = document.querySelector('#lineItemInfomation').data.url;
            let code = document.querySelector('#lineItemInfomation').data.code;
            let shipmentid = document.querySelector('#lineItemInfomation').data.shipmentid;
            let qty = document.querySelector('#lineItemInfomation').data.qty;
            let delivery = document.querySelector('#lineItemInfomation').data.delivery;
            let selectedstore = document.querySelector('#lineItemInfomation').data.selectedstore;
            let toShipmentId = e.data.shipmentid;
            let data = {
                Code: code,
                Quantity: qty,
                ShipmentId: shipmentid,
                ToShipmentId: toShipmentId,
                DeliveryMethodId: delivery,
                SelectedStore: selectedstore
            };

            axios.post(url, data)
                .then(function (r) {
                    if (r.data.Status == true) {
                        window.location.href = r.data.RedirectUrl;
                    } else {
                        notification.error(r.data.Message);
                    }
                })
                .catch(function (e) {
                    notification.error(e);
                })
                .finally(function () {
                    document.querySelector('.loading-box').style.display = "none";
                });
        });
    });
}

  separateInit() {
    this.separateClick();
    this.confirmSeparateItemClick();
  }

  //changeAddressClick() {
  //  $('.jsChangeAddress').each(function (i, e) {
  //    $(e).change(function () {
  //      $('.loading-box').show();
  //      let shipmentIndex = "";
  //      let type = $(e).data('addresstype');
  //      if (type == "Billing") {

  //      } else {
  //        shipmentIndex = $(e).data('shipmentindex');
  //      }
  //      let addressId = $(e).find('input[type=radio]:checked').val();
  //      let useBillingAddressForShipmentInput = $('#UseBillingAddressForShipment');
  //      let useBillingAddressForShipment = false;
  //      if (useBillingAddressForShipmentInput.length > 0) {
  //        useBillingAddressForShipment = useBillingAddressForShipmentInput.is(':checked');
  //      }
  //      let data = {
  //        AddressId: addressId,
  //        UseBillingAddressForShipment: useBillingAddressForShipment,
  //        ShippingAddressIndex: shipmentIndex,
  //        AddressType: type
  //      };
  //      let url = $(e).parents('.jsChangeAddressCard').data('urlchangeaddress');

  //      axios.post(url, data)
  //        .then(function (r) {
  //          if (r.data.Status == true) {

  //          } else {
  //            notification.error(r.data.Message);
  //          }
  //        })
  //        .catch(function (e) {
  //          notification.error(e);
  //        })
  //        .finally(function () {
  //          $('.loading-box').hide();
  //        });
  //    });
  //  });
  //}

    changeAddressClick() {
        alert(" changeAddressClick ");
        if (document.querySelector(".jsChangeAddress") == null) return;
        Array.from(document.querySelector(".jsChangeAddress")).forEach(function (e, i) {
        e.addEventListener("change", function () {
            document.querySelector('.loading-box').style.display = "block";
            let shipmentIndex = "";
            let type = e.data.AddressType;
            if (type == "Billing") {

            } else {
                shipmentIndex = e.data.shipmentIndex;
            }
            let addressId = e.querySelector('input[type=radio]:checked').value;
            let useBillingAddressForShipmentInput = document.querySelector('#UseBillingAddressForShipment');
            let useBillingAddressForShipment = false;
            if (useBillingAddressForShipmentInput.length > 0) {
                useBillingAddressForShipment = useBillingAddressForShipmentInput.is(':checked');
            }
            let data = {
                AddressId: addressId,
                UseBillingAddressForShipment: useBillingAddressForShipment,
                ShippingAddressIndex: shipmentIndex,
                AddressType: type
            };
            let url = e.parents.querySelector('.jsChangeAddressCard').data.urlchangeaddress;

            axios.post(url, data)
                .then(function (r) {
                    if (r.data.Status == true) {

                    } else {
                        notification.error(r.data.Message);
                    }
                })
                .catch(function (e) {
                    notification.error(e);
                })
                .finally(function () {
                    document.querySelector('.loading-box').style.display = "none";
                });
        });
    });
}

  //addNewAddress() {
  //  $('.jsSaveAddress').each(function (i, e) {
  //    $(e).click(function () {
  //      $('.loading-box').show();
  //      let form = $(e).parents('.jsFormNewAddress').first();
  //      let data = serializeObject(form);
  //      let formData = convertFormData(data);
  //      let url = form[0].action;
  //      let returnUrl = form.find('.jsAddressReturnUrl').val();
  //      formData.append("returnUrl", returnUrl);

  //      axios.post(url, formData)
  //        .then(function (r) {
  //          if (r.data.Status == false) {
  //            form.find('.jsAddressError').html(r.data.Message);
  //            form.find('.jsAddressError').addClass('error');
  //          } else {
  //            window.location.href = r.data.RedirectUrl;
  //          }
  //        })
  //        .catch(function (e) {
  //          notification.error(e);
  //          form.find('.jsAddressError').html(e);
  //          form.find('.jsAddressError').addClass('error');
  //        })
  //        .finally(function () {
  //          $('.loading-box').hide();
  //        });
  //    });
  //  });
  //}

    addNewAddress() {
        alert(" addNewAddress ");
        if (document.querySelector(".jsSaveAddress") == null) return;
        Array.from(document.querySelector(".jsSaveAddress")).forEach(function (e, i) {
            e.addEventListener("click", function () {
            document.querySelector('.loading-box').style.display = "block";
            let form = e.parents.querySelector('.jsFormNewAddress').first();
            let data = serializeObject(form);
            let formData = convertFormData(data);
            let url = form[0].action;
            let returnUrl = form.find('.jsAddressReturnUrl').value;
            formData.append("returnUrl", returnUrl);

            axios.post(url, formData)
                .then(function (r) {
                    if (r.data.Status == false) {
                        form.querySelector('.jsAddressError').innerHTML = r.data.Message;
                        form.querySelector('.jsAddressError').addClass('error');
                    } else {
                        window.location.href = r.data.RedirectUrl;
                    }
                })
                .catch(function (e) {
                    notification.error(e);
                    form.querySelector('.jsAddressError').innerHTML = e;
                    form.querySelector('.jsAddressError').addClass('error');
                })
                .finally(function () {
                    document.querySelector('.loading-box').style.display = "none";
                });
        });
    });
}

  //showHideSubscription() {
  //  $('#IsUsePaymentPlan').change(function () {
  //    if ($(this).is(':checked')) {
  //      $('.jsSubscription').slideDown();
  //    } else {
  //      $('.jsSubscription').slideUp();
  //    }
  //  });
  //}

///# todo  - rewrite slidedown and slideup in vanilla js
    showHideSubscription() {
    ////    alert(" showHideSubscription ");
    ////    if (document.querySelector("#IsUsePaymentPlan") == null) return;
    ////document.querySelector('#IsUsePaymentPlan').addEventListener("change", event => {
    ////    if (event.target.checked) {
    ////        $('.jsSubscription').slideDown();
    ////    } else {
    ////        $('.jsSubscription').slideUp();
    ////    }
    ////});
}

  //onSubmitClick() {
  //  let inst = this;
  //  $('#jsCheckoutForm').submit(function () {
  //    let blocksRequired = $('.jsFormInputRequired:visible');
  //    let isValid = true;
  //    blocksRequired.each((j, b) => {
  //      let fields = $(b).find('.jsRequired');

  //      fields.each((i, e) => {
  //        let tE = $(e);
  //        if (tE.html() == "") {
  //          isValid = false;
  //          let parent = tE.parent();
  //          if (parent.children(".field-validation-error").length == 0) {
  //            tE.parent().append(inst.errorMessage());
  //          } else {
  //            tE.parent().children(".field-validation-error").html(inst.errorMessage());
  //          }
  //        }
  //      });
  //    });

  //    return isValid;
  //  });
  //}

///# todo: parent children rewrite in vanilla js
    onSubmitClick() {
        alert(" onSubmitClick ");
        let inst = this;
        if (document.querySelector("#jsCheckoutForm") == null) return;
    document.querySelector('#jsCheckoutForm').addEventListener("submit", function () {
        //let blocksRequired = $('.jsFormInputRequired:visible');
        let isValid = true;
        if (document.querySelector(".jsFormInputRequired:visible") == null) return;
        Array.from(document.querySelector(".jsFormInputRequired:visible")).forEach(function (b, i) {
            let fields = (b).document.querySelectorAll('.jsRequired');

            if (document.querySelector(".jsRequired") == null) return;
            Array.from(document.querySelector(".jsRequired")).forEach(function (e, i) {
                let tE = e;
                if (tE.innerHTML == "") {
                    isValid = false;
                    let parent = tE.parent();
                    if (parent.children(".field-validation-error").length == 0) {
                        tE.parent().append(inst.errorMessage());
                    } else {
                        tE.parent().children(".field-validation-error").html(inst.errorMessage());
                    }
                }
            });
        });

        return isValid;
    });
}

  errorMessage() {
    return `<span class="field-validation-error">This field is required.</span>`;
  }

  init() {
    this.formShippingAddressChange();
    this.formBillingAddressChange();
    this.addPaymentClick();
    this.removePaymentClick();
    this.paymentMethodChange();
    this.creditCardChange();
    this.checkoutAsGuestOrRegister();
    this.applyCouponCode();
    this.removeCouponCode();
    this.changeShippingMethod();
    this.changeCartItem();
    this.separateInit();
    this.changeAddressClick();
    this.addNewAddress();
    this.showHideSubscription();
    this.onSubmitClick();
  }
}