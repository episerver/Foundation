import feather from "feather-icons";

export default class Checkout {
  addPaymentClick() {
    let inst = this;
    //$('.jsAddPayment').click(function () {
      
      if (document.querySelector(".jsAddPayment") == null) return;
      document.querySelector(".jsAddPayment").addEventListener("click", function (e) {
          console.log(" addPaymentClick ");
          if (document.getElementById("SelectedCreditCardId") != null && document.getElementById("SelectedCreditCardId").innerText == "Select credit card") {
              notification.error("You have to select Credit card");
              return;
          }

          //$('.loading-box').show();
          document.querySelector(".loading-box").style.display = 'block'; // show

          let url = document.querySelector(".jsAddPayment").getAttribute("url");
          console.log(url);
          //let url = this.attr('url');
          //let checked = $('.jsChangePayment:checked');
          let checked = document.querySelector('.jsChangePayment:checked');
          let methodId = checked.getAttribute('methodId');
          let keyword = checked.getAttribute('keyword');

          console.log(methodId + " - " + keyword);
          let additionVal = {
              PaymentMethodId: methodId,
              SystemKeyword: keyword
          };

          let form = document.querySelector('.jsCheckoutForm');

          // Get all field data from the form
          let data = new FormData(form);
          console.log(data);
          // Convert to an object
          let formObj = data.serializeObject + '&' + $.param(additionVal);
          console.log(formObj);
          axios.post(url, formObj)
              .then(function (result) {
                  if (result.status != 200) {
                      notification.error(result);
                  } else {
                      //$('#paymentBlock').html(result.data);
                      document.getElementById('paymentBlock').innerHTML = result.data;
                      feather.replace();
                      inst.initPayment();
                  }
              })
              .catch(function (error) {
                  if (error.response.status == 400) {
                      document.querySelector("#giftcard-alert").innerHTML = error.response.statusText;
                      //$("#giftcard-alert").html(error.response.statusText);
                      //$("#giftcard-console.log").removeClass("alert-info");
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
        console.log(" removePayment ");
    let inst = this;
    //$('.loading-box').show();
        document.querySelector(".loading-box").style.display = "block";
        console.log(element);
    //let url = $(element).data('url');
        let url = element.getAttribute("data-url");
        
    //let methodId = $(element).data('method-id');
        let methodId = element.getAttribute("data-method-id");
    //////let keyword = $(element).data('keyword');
    let keyword = element.getAttribute("data-keyword");
    //////let paymentTotal = $(element).data('amount');
        let paymentTotal = element.getAttribute("data-amount");

        let additionVal = {
            PaymentMethodId: methodId,
            SystemKeyword: keyword
        };

    let data = {
      PaymentMethodId: methodId,
      SystemKeyword: keyword,
      'OrderSummary.PaymentTotal': paymentTotal
        };
        console.log(data);
        axios.post(url, data)
      .then(function (result) {
          document.getElementById("paymentBlock").innerHTML = result.data;
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
    //$('.jsRemovePayment').each(function (i, e) {
    //  console.log(i, e);
    //  $(e).click(function () {
    //    console.log(i, e);
    //    inst.removePayment(e);
    //  });
    //});
      if (document.querySelector(".jsRemovePayment") == null) return;
      console.log(" removePaymentClick ");
      Array.from(document.getElementsByClassName("jsRemovePayment")).forEach(function (el, i) {
        el.addEventListener('click', inst.removePayment(el));
    });
    
  }

  paymentMethodChange() {
      let inst = this;
      
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
      console.log(" paymentMethodChange ");
      Array.from(document.getElementsByClassName("jsChangePayment")).forEach(function (el, i) {
        el.addEventListener("change", function () {
            document.querySelector(".loading-box").style.display = 'block'; // show
            let url = el.getAttribute('url');
            let methodId = el.getAttribute('methodid');
            let keyword = el.getAttribute('keyword');
            let data = {
              PaymentMethodId: methodId,
              SystemKeyword: keyword
            };
            console.log(data);
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
    //$('.jsSelectCreditCard').each(function (i, e) {
    //  $(e).change(function () {
    //    $('.selectCreditCardType').hide();
    //    let targetId = $(e).val();
    //    $(targetId).show();
    //  });
    //});

        if (document.querySelector(".jsSelectCreditCard") == null) return;
        console.log(" creditCardChange ");
        var inputs = document.getElementsByClassName('jsSelectCreditCard');
        Array.from(inputs).forEach(function (el, index, array) {
            el.addEventListener("change", function () {
                if (index == 1) {
                    document.getElementsByClassName("selectCreditCardType")[0].style.display = "none";
                    document.getElementById("newCreditCard").style.display = "block";
                }
                else {
                    document.getElementsByClassName("selectCreditCardType")[0].style.display = "block";
                    document.getElementById("newCreditCard").style.display = "none";
                }
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

        //console.log(" formShippingAddressChange ");
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
        var inputs = document.getElementsByClassName('jsSingleAddress');
        Array.from(inputs).forEach(function (el, index, array) {
            el.addEventListener("change", function () {
                let shippingRow = el.closest('.jsShippingAddressRow');
                let value = el.value;
                if (value == 0) {
                document.getElementsByClassName('jsOldShippingAddressForm')[0].style.display = "none";
                document.getElementsByClassName('jsNewShippingAddressForm')[0].style.display = "block";
            } else {
                document.getElementsByClassName('jsOldShippingAddressForm')[0].style.display = "block";
                document.getElementsByClassName('jsNewShippingAddressForm')[0].style.display = "none";
            }
        });

            });
  }

    formBillingAddressChange() {

        
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

        console.log(" formBillingAddressChange ");

        var inputs = document.getElementsByClassName('jsBillingAddress');
        Array.from(inputs).forEach(function (el, index, array) {
            el.addEventListener("click", function () {
                let value = el.value;
                if (value == 0) {
                    document.getElementById('oldBillingAddressForm').style.display = "none";
                    document.getElementById('newBillingAddressForm').style.display = "block";
                } else if (value == 1) {
                    document.getElementById('oldBillingAddressForm').style.display = "block";
                    document.getElementById('newBillingAddressForm').style.display = "none";
                } else if (value == 2) {
                    document.getElementById('oldBillingAddressForm').style.display = "none";
                    document.getElementById('newBillingAddressForm').style.display = "none";
                }
            });
        });
  }

    checkoutAsGuestOrRegister() {
        
    //$('.jsContinueCheckoutMethod').click(function () {
    //  let type = $('input[name="checkoutMethod"]:checked').val();
    //  if (type == 'register') {
    //    $('#js-profile-popover').css("visibility", "visible");
    //    $('#login-selector-signup-tab').click();
    //    return false;
    //  }
    //});
        if (document.querySelector(".jsContinueCheckoutMethod") == null) return;
        console.log(" checkoutAsGuestOrRegister ");
    document.getElementsByClassName('jsContinueCheckoutMethod').addEventListener("click" ,function () {
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
    //$('.jsCouponCode').keypress(function (e) {
    //  if (e.keyCode == 13) {
    //    $('.jsAddCoupon').click();
    //    return false;
    //  }
    //});
      if (document.querySelector(".jsCouponCode") == null) return;
      document.querySelector('.jsCouponCode').addEventListener("keypress", function (e) {
          console.log(" applyCouponCode  ");
        if (e.keyCode == 13) {
            document.getElementsByClassName('jsAddCoupon').addEventListener("click", function () {
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
        let form = document.getElementsByClassName("jsAddCouponContainer");
          let couponButton = document.getElementsByClassName("jsAddCoupon");
          console.log(couponButton);
          let couponContainer = document.querySelector(".jsAddCouponContainer");
          console.log(couponContainer);
          let url = couponContainer.getAttribute("action");
          let couponCode = document.getElementsByClassName('jsCouponCode')[0].value;
          console.log(couponCode);
        let data = convertFormData({ couponCode: couponCode });
        console.log(url + " " + data);
        axios.post(url, data)
            .then(function (r) {
                if (r.status == 200) {
                    //document.querySelector('.jsCouponLabel').removeClass('hidden');
                    //console.log(document.querySelector('.jsCouponLabel').classList);
                    document.querySelector('.jsCouponLabel').classList.remove('hidden');
                    if (document.querySelector(".jsInCheckout") != null) {
                        document.querySelector('.jsCouponListing').innerHTML = inst.couponTemplate(couponCode, "jsInCheckout");
                    } else {
                        document.querySelector('.jsCouponListing').innerHTML = inst.couponTemplate(couponCode, "");
                    }
                    ////inst.removeCouponCode($('.jsRemoveCoupon[data-couponcode=' + couponCode + ']'));
                    ////document.querySelector('.jsCouponReplaceHtml').innerHTML = r.data;
                    ////document.querySelector('.jsOrderSummary').innerHTML = (document.querySelector('.jsOrderSummaryInPayment').innerHTML);
                    feather.replace();
                    if (document.querySelector(".jsInCheckout") != null) {
                        inst.initPayment();
                    }
                    document.querySelector('.jsCouponCode').value = "";
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
        console.log("removeCouponCode with selector")
    let inst = this;
    if (selector) {
      inst.removeCoupon(selector);
    } else {
      //$('.jsRemoveCoupon').each(function (i, e) {
      //  inst.removeCoupon(e);
      //});
        if (document.querySelector(".jsRemoveCoupon") == null) return;
        console.log(" removeCouponCode ");
        Array.from(document.querySelectorAll(".jsRemoveCoupon")).forEach(function (el, i) {
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
        
        let inst = this;
        if (e == null) return;
        console.log(" removeCoupon ");
        e.addEventListener("click", function (e,i) {
            let url = document.querySelector('#jsRenoveCouponUrl').value;

            
            let couponCode = document.querySelector('.jsRemoveCoupon').getAttribute("data-couponcode");

            console.log(url + " " + couponCode);
        let data = convertFormData({ couponCode: couponCode });

        axios.post(url, data)
            .then(function (r) {
                //element.remove();
                let coupons = document.querySelector(".jsCouponListing");
                coupons.removeChild(document.querySelector(".jsRemoveCoupon"));
                //console.log("r data " + r.data); 
                //console.log(document.querySelector('.jsCouponLabel').classList);
                    document.querySelector('.jsCouponLabel').classList.add('hidden');
                //document.querySelector('.jsCouponReplaceHtml').innerHTML =  r.data;
                //document.querySelector(('.jsOrderSummary').innerHTML = document.querySelector('.jsOrderSummaryInPayment').innerHTML;
                console.log(e.classList);
                //if (e.classList.value.contains('jsInCheckout')) {
                //    feather.replace();
                //    inst.initPayment();
                //}

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
        
        let inst = this;
        if (document.querySelector(".jsShippingMethodContainer") == null) return;
        console.log(" changeShippingMethod ");
        Array.from(document.querySelectorAll(".jsChangeShipment")).forEach(function (el, i) {
            //console.log(el.checked);
        el.addEventListener("change", function () {
            let isInstorePickup = document.querySelector('.jsChangeShipment:checked').getAttribute('instorepickup');
            if (isInstorePickup == "True") {
                document.querySelector('.jsShippingAddressRow').style.display = "none";
            } else {
                document.querySelector('.jsShippingAddressRow').style.display = "block";
            }

            let url = document.querySelector('.jsShippingMethodContainer').getAttribute('url');
            let form = document.querySelector('.jsCheckoutForm');
            // Get all field data from the form
            let data = new FormData(form);
            console.log(data);
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
        
        let inst = this;
        if (document.querySelector(".jsChangeQuantityItemCheckout") == null) return;
        console.log(" changeCartItem ");
        //Array.from(document.querySelector(".jsChangeQuantityItemCheckout")).forEach(function (e, i) {
        let txtQty = document.querySelector(".jsChangeQuantityItemCheckout");
        txtQty.addEventListener("change", function (e) {
            document.querySelector('.loading-box').style.display = "block";
            let quantity = txtQty.value;
            let code = txtQty.getAttribute("data-code");
            let url = txtQty.getAttribute("data-url");
            let shipmentId = txtQty.getAttribute("data-shipmentid");
            let data = {
                code: code,
                quantity: quantity,
                shipmentId: shipmentId
            };
            console.log(data + " " + url);
            axios.post(url, data)
                .then(function (r) {
                    if (quantity == 0) {
                        let parent = document.querySelector('.jsShipmentRow');
                        document.querySelector("jsCartItem").remove();

                        if (parent.querySelectorAll('.jsCartItem').length == 0) {
                            parent.remove();
                            //window.location.href = window.location.href;
                        }
                    }

                    if (quantity > 1) {
                        console.log("inside > 1")
                        let btn = document.querySelector('.jsSeparateHint');
                        console.log(btn.parentElement + " " + btn.parentElement.getAttribute("class"));
                        btn.parentElement.classList.remove('hidden');
                        btn.classList.add('jsSeparateBtn');
                        inst.separateClick(btn);
                        
                    }
                    else {
                        console.log("inside <= 1")
                        let btn = document.querySelector('.jsSeparateHint');
                        btn.parentElement.classList.add('hidden');
                        btn.classList.remove('jsSeparateBtn');
                    }
                    console.log("i am here 1" + document.querySelector('.jsCouponReplaceHtml'))
                    document.querySelector('.jsCouponReplaceHtml').innerHTML = r.data;
                    console.log("i am here 2")
                    document.querySelector('.jsOrderSummary').innerHTML = (document.querySelector('.jsOrderSummaryInPayment').innerHTML);
                    console.log("i am here 3")
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
    //});
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
        
    if (selector) {
        selector.addEventListener("click", function () {
            console.log(" separateClick ");
            Array.from(document.querySelector(".jsSelectShipment")).forEach(function (s, i) {
                s.style.display = "block";
            });
            let code = selector.getAttribute("data-code");
            let shipmentid = selector.getAttribute("data-shipmentid");
            let qty = document.querySelector('.jsChangeQuantityItemCheckout').value;
            let delivery = selector.getAttribute("data-delivery");
            let selectedstore = selector.getAttribute("data-selectedstore");

            console.log(code + " " + shipmentid + " " + qty + " " + delivery + " " + selectedstore);
            document.querySelector('#lineItemInfomation').setAttribute("data-code", code);
            document.querySelector('#lineItemInfomation').setAttribute("data-shipmentid", shipmentid);
            document.querySelector('#lineItemInfomation').setAttribute("data-qty", qty);
            document.querySelector('#lineItemInfomation').setAttribute("data-delivery", delivery);
            document.querySelector('#lineItemInfomation').setAttribute("data-selectedstore", selectedstore);
            Array.from(document.querySelector(".jsSelectShipment")).forEach(function (e, i) {
                if (e.getAttribute("data-shipmentid") == shipmentid)
                    e.style.display = "none";
            });
        })
    } else {
        if (document.querySelector(".jsSeparateBtn") == null) return;
        Array.from(document.querySelector(".jsSeparateBtn")).forEach(function (e, i) {
            e.addEventListener("click", function () {

                if (document.querySelector(".jsSelectShipment") == null) return;
                Array.from(document.querySelector(".jsSelectShipment")).forEach(function (s, i) {
                    s.style.display = "block";
                });

                let code = e.getAttribute("data-code");
                let shipmentid = e.getAttribute("data-shipmentid");
                let qty = document.querySelector('.jsChangeQuantityItemCheckout').value;
                let delivery = e.getAttribute("data-delivery");
                let selectedstore = e.getAttribute("data-selectedstore");

                console.log("inside else statement " + code + " " + shipmentid + " " + qty + " " + delivery + " " + selectedstore);
                document.querySelector('#lineItemInfomation').setAttribute("data-code", code);
                document.querySelector('#lineItemInfomation').setAttribute("data-shipmentid", shipmentid);
                document.querySelector('#lineItemInfomation').setAttribute("data-qty", qty);
                document.querySelector('#lineItemInfomation').setAttribute("data-delivery", delivery);
                document.querySelector('#lineItemInfomation').setAttribute("data-selectedstore", selectedstore);
                Array.from(document.querySelector(".jsSelectShipment")).forEach(function (j, i) {
                    if (j.getAttribute("data-shipmentid") == shipmentid)
                        j.style.display = "none";
                });
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
        
        if (document.querySelector(".jsSelectShipment") == null) return;
        console.log(" confirmSeparateItemClick ");
        Array.from(document.querySelectorAll(".jsSelectShipment")).forEach(function (e, i) {
        e.addEventListener("click", function () {
            document.querySelector('.loading-box').style.display = "block";

            document.querySelector('#lineItemInfomation').setAttribute("data-code","SKU-39850363");
            let url = document.querySelector('#lineItemInfomation').getAttribute("data-url");
            let code = document.querySelector('#lineItemInfomation').getAttribute("data-code");
            let shipmentid = document.querySelector('#lineItemInfomation').getAttribute("data-shipmentid");
            let qty = document.querySelector('#lineItemInfomation').getAttribute("data-qty");
            let delivery = document.querySelector('#lineItemInfomation').getAttribute("data-delivery");
            let selectedstore = document.querySelector('#lineItemInfomation').getAttribute("data-selectedstore");
            let toShipmentId = e.getAttribute("data-shipmentid");
            console.log("confirmSeparateItemClick " + code + " " + shipmentid + " " + qty + " " + delivery + " " + selectedstore + " " +  url);
            let data = {
                code: code,
                quantity: qty,
                toShipmentId: toShipmentId,
                deliveryMethodId: delivery,
                selectedStore: selectedstore,
                shipmentId: shipmentid
            };
            console.log(data);
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
        
        if (document.querySelector(".jsChangeAddress") == null) return;
        console.log(" changeAddressClick ");
        Array.from(document.querySelectorAll(".jsChangeAddress")).forEach(function (e, i) {
        e.addEventListener("change", function () {
            document.querySelector('.loading-box').style.display = "block";
            let shipmentIndex = "";
            let type = e.getAttribute("data-addresstype");
            console.log(type);
            if (type == "Billing") {

            } else {
                shipmentIndex = e.getAttribute("data-shipmentIndex");
            }
            

            let addressId = document.querySelector('.jsSingleAddress').value;
            
            console.log(addressId);
            let useBillingAddressForShipmentInput = document.querySelector('#UseBillingAddressForShipment');
            let useBillingAddressForShipment = false;
            console.log(useBillingAddressForShipmentInput + " " + useBillingAddressForShipment);
            if (useBillingAddressForShipmentInput!= null && useBillingAddressForShipmentInput.length > 0) {
                useBillingAddressForShipment = useBillingAddressForShipmentInput.is(':checked');
            }
            console.log(type + " " + shipmentIndex + " " + addressId + " " + useBillingAddressForShipment);
            let data = {
                AddressId: addressId,
                UseBillingAddressForShipment: useBillingAddressForShipment,
                ShippingAddressIndex: shipmentIndex,
                AddressType: type
            };
            let url = document.querySelector('.jsChangeAddressCard').getAttribute("data-urlChangeAddress");
            console.log(url);
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
        
        if (document.querySelector(".jsSaveAddress") == null) return;
        
        //Array.from(document.querySelector(".jsSaveAddress")).forEach(function (e, i) {
            console.log("addNewAddress ");
        document.querySelector(".jsSaveAddress").addEventListener("click", function () {
            document.querySelector('.loading-box').style.display = "block";
            let form = document.querySelector('.jsFormNewAddress');
                let data = new FormData(form);
            //let formData = convertFormData(data);
                let url = form.getAttribute("action");
                let returnUrl = document.querySelector('.jsAddressReturnUrl').getAttribute("value");
                let formObj = data + '&' + returnUrl;
                console.log(url + " " + formObj);
            axios.post(url, data)
                .then(function (r) {
                    if (r.data.Status == false) {
                        form.querySelector('.jsAddressError').innerHTML = r.data.Message;
                        form.querySelector('.jsAddressError').classList.add('error');
                    } else {
                        window.location.href = returnUrl;//r.data.RedirectUrl;
                    }
                })
                .catch(function (e) {
                    notification.error(e);
                    form.querySelector('.jsAddressError').innerHTML = e;
                    form.querySelector('.jsAddressError').classList.add('error');
                })
                .finally(function () {
                    document.querySelector('.loading-box').style.display = "none";
                });
        });
    //});
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
        if (document.querySelector("#IsUsePaymentPlan") == null) return;
    document.querySelector('#IsUsePaymentPlan').addEventListener("change", event => {
        if (event.target.checked) {
            console.log(" showHideSubscription ");
            document.querySelector('.jsSubscription').style.display = "block";
        } else {
            document.querySelector('.jsSubscription').style.display = "none";
        }
    });
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
        let inst = this;
        if (document.querySelector("#jsCheckoutForm") == null) return;
    document.querySelector('#jsCheckoutForm').addEventListener("submit", function () {
        //let blocksRequired = $('.jsFormInputRequired:visible');
        console.log(" onSubmitClick ");
        let isValid = true;
        //if (document.querySelector(".jsFormInputRequired").classList.contains("visible")) return;
        Array.from(document.querySelector(".jsFormInputRequired:visible")).forEach(function (b, i) {
            //let fields = (b).querySelectorAll('.jsRequired');
            console.log(" 1 ");
            if (document.querySelector(".jsRequired") == null) return;
            Array.from(document.querySelector(".jsRequired")).forEach(function (e, i) {
                console.log(" 2 ");
                let tE = e;
                if (tE.innerHTML == "") {
                    console.log(" 3 ");
                    isValid = false;
                    let parent = tE.closest();
                    if (parent(".field-validation-error").length == 0) {
                       parent.innerHTML = inst.errorMessage();
                    } else {
                        //tE.parent().children(".field-validation-error").html(inst.errorMessage());
                        parent.innerHTML = inst.errorMessage();
                    }
                    console.log(" 4 ");
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