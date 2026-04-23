import feather from "feather-icons";

export default class Checkout {
  addPaymentClick() {
    let inst = this;
      if (document.querySelector(".jsAddPayment") == null) return;
      Array.from(document.querySelectorAll(".jsAddPayment")).forEach(function (el, i) {
          el.addEventListener("click", function () {
              if (document.getElementById("SelectedCreditCardId") != null && document.getElementById("SelectedCreditCardId").innerText == "Select credit card" && document.querySelector("#availableCreditType").checked) {
                  notification.error("You have to select Credit card");
                  return;
              }
              document.querySelector(".loading-box").style.display = 'block'; // show

              let url = el.getAttribute("url");
              //console.log(url);

              let methodId = "";
              let keyword = "";

              Array.from(document.querySelectorAll(".jsChangePayment")).forEach(function (element, ind) {
                  if (element.checked) {
                      methodId = element.getAttribute('methodId');
                      keyword = element.getAttribute('keyword');

                  }
              });

              let additionVal = {
                  PaymentMethodId: methodId,
                  SystemKeyword: keyword
              };

              let form = document.querySelector('.jsCheckoutForm');

              // Get all field data from the form 
              let data = new FormData(form);
              data.append("PaymentMethodId", methodId);
              data.append("SystemKeyword", keyword);

              axios.post(url, data)
                  .then(function (result) {
                      if (result.status != 200) {
                          notification.error(result);
                      } else {
                          document.querySelector("#paymentBlock").innerHTML = result.data;
                          feather.replace();
                          inst.initPayment();
                      }
                  })
                  .catch(function (error) {
                      if (error.response.status == 400) {
                          document.querySelector("#giftcard-alert").innerHTML = error.response.statusText;
                          document.querySelector("#giftcard-alert").classList.remove("alert-info");
                          document.querySelector("#giftcard-alert").classList.add("alert-danger");
                      } else {
                          notification.error(error);
                      }
                  })
                  .finally(function () {
                      document.querySelector(".loading-box").style.display = "none";
                  });
          });
      });
      }
  

    removePayment(element) {
    let inst = this;
        document.querySelector(".loading-box").style.display = "block";
        let url = element.getAttribute("data-url");
        let methodId = element.getAttribute("data-method-id");
    let keyword = element.getAttribute("data-keyword");
        let paymentTotal = element.getAttribute("data-amount");

    let data = {
      PaymentMethodId: methodId,
      SystemKeyword: keyword,
      'OrderSummary.PaymentTotal': paymentTotal
        };

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
          document.querySelector(".loading-box").style.display = "none";
      });
  }

  removePaymentClick() {
      let inst = this;
      if (document.querySelector(".jsRemovePayment") == null) return;
      document.querySelector(".jsRemovePayment").addEventListener("click", function () {
          inst.removePayment(document.querySelector(".jsRemovePayment"))
      });
  }

  paymentMethodChange() {
      let inst = this;

      if (document.querySelector(".jsChangePayment") == null) return;

      Array.from(document.querySelectorAll(".jsChangePayment")).forEach(function (el, i) {
          el.addEventListener("change", function () {
            document.querySelector(".loading-box").style.display = 'block'; // show
            let url = el.getAttribute('url');
            let methodId = el.getAttribute('methodid');
            let keyword = el.getAttribute('keyword');
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
        if (document.querySelector(".jsSelectCreditCard") == null) return;
        var inputs = document.querySelectorAll('.jsSelectCreditCard');
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

  // Shipping Address
    formShippingAddressChange() {
        var inputs = document.querySelectorAll('.jsSingleAddress');
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
        var inputs = document.querySelectorAll('.jsBillingAddress');
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
        if (document.querySelector(".jsContinueCheckoutMethod") == null) return;
    document.getElementsByClassName('jsContinueCheckoutMethod')[0].addEventListener("click" ,function () {
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
      if (document.querySelector(".jsCouponCode") == null) return;
      document.querySelector('.jsCouponCode').addEventListener("keypress", function (e) {
        if (e.keyCode == 13) {
            document.getElementsByClassName('jsAddCoupon')[0].addEventListener("click", function () {
                return false;
            });
        }
    });

      if (document.querySelector(".jsAddCoupon") == null) return;
      document.querySelector('.jsAddCoupon').addEventListener("click", function () {
        let form = document.getElementsByClassName[0]("jsAddCouponContainer");
          let couponButton = document.querySelector(".jsAddCoupon");
          let couponContainer = document.querySelector(".jsAddCouponContainer");
          let url = couponContainer.getAttribute("action");
          let couponCode = document.querySelector('.jsCouponCode').value;
        let data = convertFormData({ couponCode: couponCode });

        axios.post(url, data)
            .then(function (r) {
                if (r.status == 200) {
                     document.querySelector('.jsCouponLabel').classList.remove('hidden');
                    if (couponButton.classList.contains("jsInCheckout") ) {
                        document.querySelector('.jsCouponListing').append(inst.couponTemplate(couponCode, "jsInCheckout"));
                    } else {
                        document.querySelector('.jsCouponListing').append(inst.couponTemplate(couponCode, ""));
                    }
                    document.querySelector('.jsRemoveCoupon').addEventListener("click", function () {
                        inst.removeCouponCode(document.querySelector('.jsRemoveCoupon'));
                    });
                    inst.removeCouponCode(document.querySelector('.jsRemoveCoupon[data-couponcode=' + couponCode + ']'));
                    document.querySelector('.jsCouponReplaceHtml').innerHTML = r.data;
                    document.querySelector('.jsOrderSummary').innerHTML = document.querySelector('.jsOrderSummaryInPayment').innerHTML;
                    feather.replace();
                    if (couponButton.classList.contains("jsInCheckout") ) {
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
    let inst = this;
    if (selector) {
      inst.removeCoupon(selector);
    } else {
        if (document.querySelector(".jsRemoveCoupon") == null) return;
        Array.from(document.querySelectorAll(".jsRemoveCoupon")).forEach(function (el, i) {
            inst.removeCoupon(el);
        });
    }
  }

    removeCoupon(e) {
        let inst = this;

        if (e == null) return;
        let url = "/Checkout/RemoveCouponCode";
            let couponCode = e.getAttribute("data-couponcode");

        let data = convertFormData({ couponCode: couponCode });

        axios.post(url, data)
            .then(function (r) {
                let coupons = document.querySelector(".jsCouponListing");
                if (coupons == null) {
                    document.querySelector('.jsCouponLabel').classList.add('hidden');
                }
                //coupons.removeChild(document.querySelector(".jsRemoveCoupon"));
                document.querySelector('.jsCouponReplaceHtml').innerHTML =  r.data;
                document.querySelector('.jsOrderSummary').innerHTML = document.querySelector('.jsOrderSummaryInPayment').innerHTML;
                if (e.classList.contains('jsInCheckout')) {
                    feather.replace();
                    inst.initPayment();
                }
                document.querySelector('.jsCouponErrorMess').style.display = "none";
            })
            .catch(function (e) {
                notification.error(e);
            });
    //});
}

  couponTemplate(couponCode, jsInCheckout) {
    return `<label class="filters-tag jsRemoveCoupon ${jsInCheckout}" data-couponcode="${couponCode}">
                    <span>${couponCode}</span>
                    <span class="filters-tag__remove"><i class="cursor-pointer" data-feather="x" width="12"></i></span>
                </label>`;
  }

    changeShippingMethod() {
        let inst = this;
        if (document.querySelector(".jsShippingMethodContainer") == null) return;
        Array.from(document.querySelectorAll(".jsShippingMethodContainer")).forEach(function (el, i) {
        el.addEventListener("change", function () {
            let isInstorePickup = document.querySelector('.jsChangeShipment:checked').getAttribute('instorepickup');
            if (isInstorePickup == "True") {
                el.closest('.jsShipmentRow').querySelector('.jsShippingAddressRow').style.display = "none";
            } else {
                el.closest('.jsShipmentRow').querySelector('.jsShippingAddressRow').style.display = "block";
            }

            let url = el.getAttribute('url');
            let form = document.querySelector('.jsCheckoutForm');
            // Get all field data from the form
            let data = new FormData(form);

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

    changeCartItem() {
        let inst = this;
        if (document.querySelector(".jsChangeQuantityItemCheckout") == null) return;
        Array.from(document.querySelectorAll(".jsChangeQuantityItemCheckout")).forEach(function (el, i) {
        el.addEventListener("change", function (e) {
            document.querySelector('.loading-box').style.display = "block";
            let quantity = el.value;
            let code = el.getAttribute("data-code");
            let url = el.getAttribute("data-url");
            let shipmentId = el.getAttribute("data-shipmentid");
            let data = {
                code: code,
                quantity: quantity,
                shipmentId: shipmentId
            };
            axios.post(url, data)
                .then(function (r) {
                    if (quantity == 0) {
                        let parent = el.closest('.jsShipmentRow');
                        el.closest('.jsCartItem').remove();

                        if (parent.querySelector('.jsCartItem') != null) {
                            parent.remove();
                            window.location.href = window.location.href;
                        }
                    }
                    

                    if (quantity > 1) {
                        let btn = el.closest(".jsCartItem").querySelector('.jsSeparateHint');
                        btn.parentElement.classList.remove('hidden');
                        btn.classList.add('jsSeparateBtn');
                        inst.separateClick(btn);
                        
                    }
                    else {
                        let btn = el.closest(".jsCartItem").querySelector('.jsSeparateHint');
                        btn.parentElement.classList.add('hidden');
                        btn.classList.remove('jsSeparateBtn');
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
  
    separateClick(selector) {
        
    if (selector) {
        selector.addEventListener("click", function () {
            Array.from(document.querySelectorAll(".jsSelectShipment")).forEach(function (s, i) {
                s.style.display = "block";
            });
            let code = selector.getAttribute("data-code");
            let shipmentid = selector.getAttribute("data-shipmentid");
            let qty = selector.closest("jsCartItem").querySelector('.jsChangeQuantityItemCheckout').value;
            let delivery = selector.getAttribute("data-delivery");
            let selectedstore = selector.getAttribute("data-selectedstore");

            document.querySelector('#lineItemInfomation').setAttribute("data-code", code);
            document.querySelector('#lineItemInfomation').setAttribute("data-shipmentid", shipmentid);
            document.querySelector('#lineItemInfomation').setAttribute("data-qty", qty);
            document.querySelector('#lineItemInfomation').setAttribute("data-delivery", delivery);
            document.querySelector('#lineItemInfomation').setAttribute("data-selectedstore", selectedstore);
            Array.from(document.querySelectorAll(".jsSelectShipment")).forEach(function (e, i) {
                if (e.getAttribute("data-shipmentid") == shipmentid)
                    e.style.display = "none";
            });
        })
    } else {
        if (document.querySelector(".jsSeparateBtn") == null) return;
        Array.from(document.querySelectorAll(".jsSeparateBtn")).forEach(function (e, i) {
            e.addEventListener("click", function () {

                if (document.querySelector(".jsSelectShipment") == null) return;
                Array.from(document.querySelectorAll(".jsSelectShipment")).forEach(function (s, i) {
                    s.style.display = "block";
                });

                let code = e.getAttribute("data-code");
                let shipmentid = e.getAttribute("data-shipmentid");
                let qty = e.closest('.jsCartItem').querySelector('.jsChangeQuantityItemCheckout').value;
                let delivery = e.getAttribute("data-delivery");
                let selectedstore = e.getAttribute("data-selectedstore");

                document.querySelector('#lineItemInfomation').setAttribute("data-code", code);
                document.querySelector('#lineItemInfomation').setAttribute("data-shipmentid", shipmentid);
                document.querySelector('#lineItemInfomation').setAttribute("data-qty", qty);
                document.querySelector('#lineItemInfomation').setAttribute("data-delivery", delivery);
                document.querySelector('#lineItemInfomation').setAttribute("data-selectedstore", selectedstore);
                Array.from(document.querySelectorAll(".jsSelectShipment")).forEach(function (j, i) {
                    if (j.getAttribute("data-shipmentid") == shipmentid)
                        j.style.display = "none";
                });
            });
        });
    }
}

    confirmSeparateItemClick() {
        
        if (document.querySelector(".jsSelectShipment") == null) return;
        Array.from(document.querySelectorAll(".jsSelectShipment")).forEach(function (e, i) {
        e.addEventListener("click", function () {
            document.querySelector('.loading-box').style.display = "block";

            let url = document.querySelector('#lineItemInfomation').getAttribute("data-url");
            let code = document.querySelector('#lineItemInfomation').getAttribute("data-code");
            let shipmentid = document.querySelector('#lineItemInfomation').getAttribute("data-shipmentid");
            let qty = document.querySelector('#lineItemInfomation').getAttribute("data-qty");
            let delivery = document.querySelector('#lineItemInfomation').getAttribute("data-delivery");
            let selectedstore = document.querySelector('#lineItemInfomation').getAttribute("data-selectedstore");
            let toShipmentId = e.getAttribute("data-shipmentid");

            let data = {
                code: code,
                quantity: qty,
                toShipmentId: toShipmentId,
                deliveryMethodId: delivery,
                selectedStore: selectedstore,
                shipmentId: shipmentid
            };

            axios.post(url, data)
                .then(function (r) {
                    if (r.data.status == true) {
                        window.location.href = r.data.redirectUrl;
                    } else {
                        notification.error(r.data.message);
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

    changeAddressClick() {
        if (document.querySelector(".jsChangeAddress") == null) return;

        Array.from(document.querySelectorAll(".jsChangeAddress")).forEach(function (e, i) {
            e.addEventListener("change", function () {
                document.querySelector('.loading-box').style.display = "block";
                let shipmentIndex = "";
                let type = e.getAttribute("data-addresstype");

                if (type == "Billing") {

                } else {
                    shipmentIndex = e.getAttribute("data-shipmentIndex");
                }

                var addressId = "";
                Array.from(e.closest(".jsChangeAddressCard").querySelectorAll('.jsSingleAddress')).forEach(function (el, i) {
                    console.log(el);
                    if (el.checked)
                        addressId = el.value;
                });
            
            
            console.log(addressId);
            let useBillingAddressForShipmentInput = document.querySelector('#UseBillingAddressForShipment');
            let useBillingAddressForShipment = false;
            console.log(useBillingAddressForShipmentInput + " " + useBillingAddressForShipment);
            if (useBillingAddressForShipmentInput != null && useBillingAddressForShipmentInput.checked) {
                useBillingAddressForShipment = useBillingAddressForShipmentInput.checked;
            }

            let data = {
                AddressId: addressId,
                UseBillingAddressForShipment: useBillingAddressForShipment,
                ShippingAddressIndex: shipmentIndex,
                AddressType: type
            };
            let url = e.closest('.jsChangeAddressCard').getAttribute("data-urlChangeAddress");
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

    addNewAddress() {
        
        if (document.querySelector(".jsSaveAddress") == null) return;
        
        Array.from(document.querySelectorAll(".jsSaveAddress")).forEach(function (e, i) {

        e.addEventListener("click", function () {
            document.querySelector('.loading-box').style.display = "block";
            let form = e.closest('.jsFormNewAddress');
                let data = new FormData(form);

                let url = form.getAttribute("action");
                let returnUrl = form.querySelector('.jsAddressReturnUrl').getAttribute("value");

            data.append("returnUrl", returnUrl);

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
    });
}

    showHideSubscription() {
        if (document.querySelector("#IsUsePaymentPlan") == null) return;
    document.querySelector('#IsUsePaymentPlan').addEventListener("change", event => {
        if (event.target.checked) {
            document.querySelector('.jsSubscription').style.display = "block";
        } else {
            document.querySelector('.jsSubscription').style.display = "none";
        }
    });
}

///# todo: parent children rewrite in vanilla js
    onSubmitClick() {
        let inst = this;
        if (document.querySelector("#jsCheckoutForm") == null) return;
        document.querySelector('#btnPlaceOrder').addEventListener("click", function () {
        let isValid = true;
        Array.from(document.querySelectorAll(".jsFormInputRequired")).forEach(function (el, i) {
            if (el.style.display == "block") {
                Array.from(el.querySelectorAll(".jsRequired")).forEach(function (element, index) {
                let tE = element;
                if (tE.innerHTML == "") {
                    isValid = false;
                    let parent = tE.parentElement();
                    if (parent.closest(".field-validation-error") == null) {
                        parent.append(inst.errorMessage());
                    } else {
                        parent.closest(".field-validation-error").innerHTML = inst.errorMessage();
                    }
                }
            });

            }
        });

            //if complete form is valid, submit form
            if (isValid) {
                document.querySelector(".loading-box").style.display = "block";
                let form = document.querySelector('.jsCheckoutForm');
                let url = form.getAttribute("action");
                // Get all field data from the form 
                let data = new FormData(form);
                axios.post(url, data)
                    .then(function (result) {
                        if (result.data.status == false) {
                            notification.error(result.data.message);
                        }
                        else {
                            window.location.href = result.data.redirectUrl;
                        }
                    })
                    .catch(function (error) {
                            notification.error(error);
                    })
                    .finally(function () {
                        document.querySelector(".loading-box").style.display = "none";
                    });
            }
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