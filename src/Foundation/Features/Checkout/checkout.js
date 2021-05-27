import feather from "feather-icons";

export default class Checkout {
  addPaymentClick() {
    let inst = this;
    $('.jsAddPayment').click(function () {
      if ($("#SelectedCreditCardId option:selected").text() === "Select credit card") {
        notification.error("You have to select Credit card");
        return;
      }

      $('.loading-box').show();
      let url = $(this).attr('url');
      let checked = $('.jsChangePayment:checked');
      let methodId = checked.attr('methodId');
      let keyword = checked.attr('keyword');

      let additionVal = {
        PaymentMethodId: methodId,
        SystemKeyword: keyword
      };

      let data = $('.jsCheckoutForm').serialize() + '&' + $.param(additionVal);

      axios.post(url, data)
        .then(function (result) {
          if (result.status != 200) {
            notification.error(result);
          } else {
            $('#paymentBlock').html(result.data);
            feather.replace();
            inst.initPayment();
          }
        })
        .catch(function (error) {
          if (error.response.status == 400) {
            $("#giftcard-alert").html(error.response.statusText);
            $("#giftcard-alert").removeClass("alert-info");
            $("#giftcard-alert").addClass("alert-danger");
          } else {
            notification.error(error);
          }
        })
        .finally(function () {
          $('.loading-box').hide();
        });
    });
  }

  removePayment(element) {
    let inst = this;
    $('.loading-box').show();
    let url = $(element).data('url');
    let methodId = $(element).data('method-id');
    let keyword = $(element).data('keyword');
    let paymentTotal = $(element).data('amount');
    let data = {
      PaymentMethodId: methodId,
      SystemKeyword: keyword,
      'OrderSummary.PaymentTotal': paymentTotal
    };

    axios.post(url, data)
      .then(function (result) {
        $('#paymentBlock').html(result.data);
        feather.replace();
        inst.initPayment();
      })
      .catch(function (error) {
        notification.error(error);
      })
      .finally(function () {
        $('.loading-box').hide();
      });
  }

  removePaymentClick() {
    let inst = this;
    $('.jsRemovePayment').each(function (i, e) {
      console.log(i, e);
      $(e).click(function () {
        console.log(i, e);
        inst.removePayment(e);
      });
    });
  }

  paymentMethodChange() {
    let inst = this;
    $('.jsChangePayment').each(function (i, e) {
      $(e).change(function () {
        $('.jsPaymentMethod').siblings('.loading-box').first().show();
        let url = $(e).attr('url');
        let methodId = $(e).attr('methodid');
        let keyword = $(e).attr('keyword');
        let data = {
          PaymentMethodId: methodId,
          SystemKeyword: keyword
        };

        axios.post(url, data)
          .then(function (result) {
            $('.jsPaymentMethod').html(result.data);
            feather.replace();
            inst.creditCardChange();
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

  creditCardChange() {
    $('.jsSelectCreditCard').each(function (i, e) {
      $(e).change(function () {
        $('.selectCreditCardType').hide();
        let targetId = $(e).val();
        $(targetId).show();
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
    $('.jsSingleAddress').each(function (i, e) {
      $(e).change(function () {
        let shippingRow = $(e).parents('.jsShippingAddressRow').first();
        let value = $(this).val();
        $('#AddressType').val(value);
        if (value == 0) {
          shippingRow.find('.jsOldShippingAddressForm').hide();
          shippingRow.find('.jsNewShippingAddressForm').show();
        } else {
          shippingRow.find('.jsOldShippingAddressForm').show();
          shippingRow.find('.jsNewShippingAddressForm').hide();
        }

      });
    });
  }

  formBillingAddressChange() {
    $('.jsBillingAddress').each(function (i, e) {
      $(e).click(function () {
        let value = $(e).val();
        $('#AddressType').val(value);
        if (value == 0) {
          $('#oldBillingAddressForm').hide();
          $('#newBillingAddressForm').show();
        } else if (value == 1) {
          $('#oldBillingAddressForm').show();
          $('#newBillingAddressForm').hide();
        } else if (value == 2) {
          $('#oldBillingAddressForm').hide();
          $('#newBillingAddressForm').hide();
        }
      });
    });
  }

  checkoutAsGuestOrRegister() {
    $('.jsContinueCheckoutMethod').click(function () {
      let type = $('input[name="checkoutMethod"]:checked').val();
      if (type == 'register') {
        $('#js-profile-popover').css("visibility", "visible");
        $('#login-selector-signup-tab').click();
        return false;
      }
    });
  }

  applyCouponCode() {
    let inst = this;
    $('.jsCouponCode').keypress(function (e) {
      if (e.keyCode == 13) {
        $('.jsAddCoupon').click();
        return false;
      }
    });

    $('.jsAddCoupon').click(function () {
      let e = this;
      let form = $(this).parents('.jsAddCouponContainer').first();
      let url = form.attr('action');
      let couponCode = form.find('.jsCouponCode').val();
      let data = convertFormData({ couponCode: couponCode });

      axios.post(url, data)
        .then(function (r) {
          if (r.status == 200) {
            $('.jsCouponLabel').removeClass('hidden');
            if ($(e).hasClass('jsInCheckout')) {
              $('.jsCouponListing').append(inst.couponTemplate(couponCode, "jsInCheckout"));
            } else {
              $('.jsCouponListing').append(inst.couponTemplate(couponCode, ""));
            }

            inst.removeCouponCode($('.jsRemoveCoupon[data-couponcode=' + couponCode + ']'));
            $('.jsCouponReplaceHtml').html(r.data);
            $('.jsOrderSummary').html($('.jsOrderSummaryInPayment').html());
            feather.replace();
            if ($(e).hasClass('jsInCheckout')) {
              inst.initPayment();
            }
            form.find('.jsCouponCode').val("");
            $('.jsCouponErrorMess').hide();
          } else {
            $('.jsCouponErrorMess').show();
          }
        })
        .catch(function (e) {
          notification.error(e);
        });
    });
  }

  removeCouponCode(selector) {
    let inst = this;
    if (selector) {
      inst.removeCoupon(selector);
    } else {
      $('.jsRemoveCoupon').each(function (i, e) {
        inst.removeCoupon(e);
      });
    }
  }

  removeCoupon(e) {
    let inst = this;
    $(e).click(function () {
      let element = $(this);
      let url = $('#jsRenoveCouponUrl').val();
      let couponCode = $(this).data('couponcode');
      let data = convertFormData({ couponCode: couponCode });

      axios.post(url, data)
        .then(function (r) {
          element.remove();
          let coupons = $('.jsCouponListing').find('.jsRemoveCoupon');
          if (coupons.length == 0) {
            $('.jsCouponLabel').addClass('hidden');
          }
          $('.jsCouponReplaceHtml').html(r.data);
          $('.jsOrderSummary').html($('.jsOrderSummaryInPayment').html());
          if ($(e).hasClass('jsInCheckout')) {
            feather.replace();
            inst.initPayment();
          }

          $('.jsCouponErrorMess').hide();
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

  changeShippingMethod() {
    let inst = this;
    $('.jsShippingMethodContainer').each(function (i, e) {
      $(e).change(function () {
        let isInstorePickup = $(e).find('.jsChangeShipment:checked').attr('instorepickup');
        if (isInstorePickup == "True") {
          $(e).parents('.jsShipmentRow').find('.jsShippingAddressRow').hide();
        } else {
          $(e).parents('.jsShipmentRow').find('.jsShippingAddressRow').show();
        }

        let url = $(e).attr('url');
        let data = $('.jsCheckoutForm').serialize();
        $('.loading-box').show();

        axios.post(url, data)
          .then(function (r) {
            $('.jsCouponReplaceHtml').html(r.data);
            $('.jsOrderSummary').html($('.jsOrderSummaryInPayment').html());
            feather.replace();
            inst.initPayment();
          })
          .catch(function (e) {
            notification.error(e);
          })
          .finally(function () {
            $('.loading-box').hide();
          });
      });
    });
  }

  changeCartItem() {
    let inst = this;
    $('.jsChangeQuantityItemCheckout').each(function (i, e) {
      $(e).change(function () {
        $('.loading-box').show();
        let quantity = $(e).val();
        let code = $(e).data('code');
        let url = $(e).data('url');
        let shipmentId = $(e).data('shipmentid');
        let data = {
          code: code,
          quantity: quantity,
          shipmentId: shipmentId
        };

        axios.post(url, data)
          .then(function (r) {
            if (quantity == 0) {
              let parent = $(e).parents('.jsShipmentRow');
              $(e).parents('.jsCartItem').first().remove();

              if (parent.find('.jsCartItem').length == 0) {
                parent.remove();
                window.location.href = window.location.href;
              }
            }

            if (quantity > 1) {
              let btn = $(e).parents('.jsCartItem').find('.jsSeparateHint');
              btn.parent('div').removeClass('hidden');
              btn.addClass('jsSeparateBtn');
              inst.separateClick(btn);
            } else {
              let btn = $(e).parents('.jsCartItem').find('.jsSeparateHint');
              btn.parent('div').addClass('hidden');
              btn.removeClass('jsSeparateBtn');
            }

            $('.jsCouponReplaceHtml').html(r.data);
            $('.jsOrderSummary').html($('.jsOrderSummaryInPayment').html());
            cartHelper.setCartReload($('.jsTotalQuantityCheckout').val());
            feather.replace();
            inst.initPayment();
          })
          .catch(function (e) {
            notification.error(e);
          })
          .finally(function () {
            $('.loading-box').hide();
          });
      });
    });
  }

  separateClick(selector) {
    if (selector) {
      $(selector).click(function () {
        $('.jsSelectShipment').each(function (j, s) {
          $(s).show();
        });
        let code = $(selector).data('code');
        let shipmentid = $(selector).data('shipmentid');
        let qty = $(selector).parents('.jsCartItem').find('.jsChangeQuantityItemCheckout').val();
        let delivery = $(selector).data('delivery');
        let selectedstore = $(selector).data('selectedstore');
        $('#lineItemInfomation').data("code", code);
        $('#lineItemInfomation').data("shipmentid", shipmentid);
        $('#lineItemInfomation').data("qty", qty);
        $('#lineItemInfomation').data("delivery", delivery);
        $('#lineItemInfomation').data("selectedstore", selectedstore);

        $('.jsSelectShipment[data-shipmentid=' + shipmentid + ']').hide();
      });
    } else {
      $('.jsSeparateBtn').each(function (i, e) {
        $(e).click(function () {
          $('.jsSelectShipment').each(function (j, s) {
            $(s).show();
          });

          let code = $(e).data('code');
          let shipmentid = $(e).data('shipmentid');
          let qty = $(e).parents('.jsCartItem').find('.jsChangeQuantityItemCheckout').val();
          let delivery = $(e).data('delivery');
          let selectedstore = $(e).data('selectedstore');
          $('#lineItemInfomation').data("code", code);
          $('#lineItemInfomation').data("shipmentid", shipmentid);
          $('#lineItemInfomation').data("qty", qty);
          $('#lineItemInfomation').data("delivery", delivery);
          $('#lineItemInfomation').data("selectedstore", selectedstore);

          $('.jsSelectShipment[data-shipmentid=' + shipmentid + ']').hide();
        });
      });
    }
  }

  confirmSeparateItemClick() {
    $('.jsSelectShipment').each(function (i, e) {
      $(e).click(function () {
        $('.loading-box').show();
        let url = $('#lineItemInfomation').data('url');
        let code = $('#lineItemInfomation').data('code');
        let shipmentid = $('#lineItemInfomation').data('shipmentid');
        let qty = $('#lineItemInfomation').data('qty');
        let delivery = $('#lineItemInfomation').data('delivery');
        let selectedstore = $('#lineItemInfomation').data('selectedstore');
        let toShipmentId = $(e).data('shipmentid');
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
            $('.loading-box').hide();
          });
      });
    });
  }

  separateInit() {
    this.separateClick();
    this.confirmSeparateItemClick();
  }

  changeAddressClick() {
    $('.jsChangeAddress').each(function (i, e) {
      $(e).change(function () {
        $('.loading-box').show();
        let shipmentIndex = "";
        let type = $(e).data('addresstype');
        if (type == "Billing") {

        } else {
          shipmentIndex = $(e).data('shipmentindex');
        }
        let addressId = $(e).find('input[type=radio]:checked').val();
        let useBillingAddressForShipmentInput = $('#UseBillingAddressForShipment');
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
        let url = $(e).parents('.jsChangeAddressCard').data('urlchangeaddress');

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
            $('.loading-box').hide();
          });
      });
    });
  }

  addNewAddress() {
    $('.jsSaveAddress').each(function (i, e) {
      $(e).click(function () {
        $('.loading-box').show();
        let form = $(e).parents('.jsFormNewAddress').first();
        let data = serializeObject(form);
        let formData = convertFormData(data);
        let url = form[0].action;
        let returnUrl = form.find('.jsAddressReturnUrl').val();
        formData.append("returnUrl", returnUrl);

        axios.post(url, formData)
          .then(function (r) {
            if (r.data.Status == false) {
              form.find('.jsAddressError').html(r.data.Message);
              form.find('.jsAddressError').addClass('error');
            } else {
              window.location.href = r.data.RedirectUrl;
            }
          })
          .catch(function (e) {
            notification.error(e);
            form.find('.jsAddressError').html(e);
            form.find('.jsAddressError').addClass('error');
          })
          .finally(function () {
            $('.loading-box').hide();
          });
      });
    });
  }

  showHideSubscription() {
    $('#IsUsePaymentPlan').change(function () {
      if ($(this).is(':checked')) {
        $('.jsSubscription').slideDown();
      } else {
        $('.jsSubscription').slideUp();
      }
    });
  }

  onSubmitClick() {
    let inst = this;
    $('#jsCheckoutForm').submit(function () {
      let blocksRequired = $('.jsFormInputRequired:visible');
      let isValid = true;
      blocksRequired.each((j, b) => {
        let fields = $(b).find('.jsRequired');

        fields.each((i, e) => {
          let tE = $(e);
          if (tE.html() == "") {
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