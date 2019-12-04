﻿class Checkout {

    // Payment
    AddPaymentClick() {
        var inst = this;
        $('.jsAddPayment').click(function () {
            $('.loading-box').show();
            var url = $(this).attr('url');
            var checked = $('.jsChangePayment:checked');
            var methodId = checked.attr('methodId');
            var keyword = checked.attr('keyword');
            //var paymentTotal = $('input[name="OrderSummary.PaymentTotal"]').val();

            var additionVal = {
                PaymentMethodId: methodId,
                SystemKeyword: keyword
                //'OrderSummary.PaymentTotal': paymentTotal
            };

            var data = $('.jsCheckoutForm').serialize() + '&' + $.param(additionVal);

            axios.post(url, data)
                .then(function (result) {
                    $('#paymentBlock').html(result.data);
                    feather.replace();
                    inst.InitPayment();
                })
                .catch(function (error) {
                    notification.Error(error);
                })
                .finally(function () {
                    $('.loading-box').hide();
                });
        });
    }

    removePayment(element) {
        var inst = this;
        $('.loading-box').show();
        var url = $(element).data('url');
        var methodId = $(element).data('method-id');
        var keyword = $(element).data('keyword');
        var paymentTotal = $(element).data('amount');
        var data = {
            PaymentMethodId: methodId,
            SystemKeyword: keyword,
            'OrderSummary.PaymentTotal': paymentTotal
        };

        axios.post(url, data)
            .then(function (result) {
                $('#paymentBlock').html(result.data);
                feather.replace();
                inst.InitPayment();
            })
            .catch(function (error) {
                notification.Error(error);
            })
            .finally(function () {
                $('.loading-box').hide();
            });
    }

    RemovePaymentClick() {
        var inst = this;
        $('.jsRemovePayment').each(function (i, e) {
            console.log(i, e);
            $(e).click(function () {
                console.log(i, e);
                inst.removePayment(e);
            });
        });
    }

    PaymentMethodChange() {
        var inst = this;
        $('.jsChangePayment').each(function (i, e) {
            $(e).change(function () {
                $('.jsPaymentMethod').siblings('.loading-box').first().show();
                var url = $(e).attr('url');
                var methodId = $(e).attr('methodid');
                var keyword = $(e).attr('keyword');
                var data = {
                    PaymentMethodId: methodId,
                    SystemKeyword: keyword
                };

                axios.post(url, data)
                    .then(function (result) {
                        $('.jsPaymentMethod').html(result.data);
                        feather.replace();
                        inst.CreditCardChange();
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

    CreditCardChange() {
        $('.jsSelectCreditCard').each(function (i, e) {
            $(e).change(function () {
                $('.selectCreditCardType').hide();
                var targetId = $(e).val();
                $(targetId).show();
            });
        });
    }

    InitPayment() {
        var inst = this;
        inst.AddPaymentClick();
        inst.RemovePaymentClick();
        inst.PaymentMethodChange();
        inst.CreditCardChange();
    }

    ///

    // Shipping Address
    FormShippingAddressChange() {
        $('.jsSingleAddress').each(function (i, e) {
            $(e).change(function () {
                var shippingRow = $(e).parents('.jsShippingAddressRow').first();
                var value = $(this).val();
                $('#AddressType').val(value);
                if (value == 0) {
                    shippingRow.find('.jsOldShippingAddressForm').hide();
                    shippingRow.find('.jsNewShippingAddressForm').show();
                } else {
                    shippingRow.find('.jsOldShippingAddressForm').show();
                    shippingRow.find('.jsNewShippingAddressForm').hide();
                }
            })
        })
    }
    ///////////////////


    // Billing Address
    FormBillingAddressChange() {
        $('.jsBillingAddress').each(function (i, e) {
            $(e).click(function () {
                var value = $(e).val();
                $('#AddressType').val(value);
                if (value == 0) {
                    $('#oldBillingAddressForm').hide();
                    $('#newBillingAddressForm').show();
                } else {
                    $('#oldBillingAddressForm').show();
                    $('#newBillingAddressForm').hide();
                }
            });
        });
    }
    //////////////////


    CheckoutAsGuestOrRegister() {
        $('.jsContinueCheckoutMethod').click(function () {
            var type = $('input[name="checkoutMethod"]:checked').val();
            if (type == 'register') {
                $('#js-profile-popover').css("visibility", "visible");
                $('#login-selector-signup-tab').click();
                return false;
            }
        });
    }

    // Coupon code handle 
    ApplyCouponCode() {
        var inst = this;
        $('.jsCouponCode').keypress(function (e) {
            if (e.keyCode == 13) {
                $('.jsAddCoupon').click();
                return false;
            }
        })

        $('.jsAddCoupon').click(function () {
            var e = this;
            var form = $(this).parents('.jsAddCouponContainer').first();
            var url = form.attr('action');
            var couponCode = form.find('.jsCouponCode').val();
            var data = convertFormData({ couponCode: couponCode });
            axios.post(url, data)
                .then(function (r) {
                    if (r.status == 200) {
                        $('.jsCouponLabel').removeClass('hidden');
                        if ($(e).hasClass('jsInCheckout')) {
                            $('.jsCouponListing').append(inst.couponTemplate(couponCode, "jsInCheckout"));
                        } else {
                            $('.jsCouponListing').append(inst.couponTemplate(couponCode, ""));
                        }

                        inst.RemoveCouponCode($('.jsRemoveCoupon[data-couponcode=' + couponCode + ']'));
                        $('.jsCouponReplaceHtml').html(r.data); 
                        $('.jsOrderSummary').html($('.jsOrderSummaryInPayment').html()); 
                        feather.replace();
                        if ($(e).hasClass('jsInCheckout')) {
                            inst.InitPayment();
                        }
                        form.find('.jsCouponCode').val("");
                        $('.jsCouponErrorMess').hide();
                    } else {
                        $('.jsCouponErrorMess').show();
                    }
                })
                .catch(function (e) {
                    notification.Error(e);
                })
        })
    }

    RemoveCouponCode(selector) {
        var inst = this;
        if (selector) {
            inst.removeCoupon(selector);
        } else {
            $('.jsRemoveCoupon').each(function (i, e) {
                inst.removeCoupon(e);
            })
        }
    }

    removeCoupon(e) {
        var inst = this;
        $(e).click(function () {
            var element = $(this);
            var url = $('#jsRenoveCouponUrl').val();
            var couponCode = $(this).data('couponcode');
            var data = convertFormData({ couponCode: couponCode });
            axios.post(url, data)
                .then(function (r) {
                    element.remove();
                    var coupons = $('.jsCouponListing').find('.jsRemoveCoupon');
                    if (coupons.length == 0) {
                        $('.jsCouponLabel').addClass('hidden');
                    }
                    $('.jsCouponReplaceHtml').html(r.data);
                    $('.jsOrderSummary').html($('.jsOrderSummaryInPayment').html()); 
                    if ($(e).hasClass('jsInCheckout')) {
                        feather.replace();
                        inst.InitPayment();
                    }

                    $('.jsCouponErrorMess').hide();
                })
                .catch(function (e) {
                    notification.Error(e);
                })
        })
    }

    couponTemplate(couponCode, jsInCheckout) {
        return `<label class="filters-tag jsRemoveCoupon ${jsInCheckout}" data-couponcode="${couponCode}">
                    <span>${couponCode}</span>
                    <span class="filters-tag__remove"><i class="cursor-pointer" data-feather="x" width="12"></i></span>
                </label>`;
    }
    //////////////////

    // Change shipping method
    ChangeShippingMethod() {
        var inst = this;
        $('.jsShippingMethodContainer').each(function (i, e) {
            $(e).change(function () {
                var isInstorePickup = $(e).find('.jsChangeShipment:checked').attr('instorepickup');
                if (isInstorePickup == "True") {
                    $(e).parents('.jsShipmentRow').find('.jsShippingAddressRow').hide();
                } else {
                    $(e).parents('.jsShipmentRow').find('.jsShippingAddressRow').show();
                }

                var url = $(e).attr('url');
                var data = $('.jsCheckoutForm').serialize();
                $('.loading-box').show();
                axios.post(url, data)
                    .then(function (r) {
                        $('.jsCouponReplaceHtml').html(r.data);
                        $('.jsOrderSummary').html($('.jsOrderSummaryInPayment').html());
                        feather.replace();
                        inst.InitPayment();
                    })
                    .catch(function (e) {
                        notification.Error(e)
                    })
                    .finally(function () {
                        $('.loading-box').hide();
                    })
            })
        });
    }
    //////////////////

    // Change cart item
    ChangeCartItem() {
        var inst = this;
        $('.jsChangeQuantityItemCheckout').each(function (i, e) {
            $(e).change(function () {
                $('.loading-box').show();
                var quantity = $(e).val();
                var code = $(e).data('code');
                var url = $(e).data('url');
                var shipmentId = $(e).data('shipmentid');
                var data = {
                    code: code,
                    quantity: quantity,
                    shipmentId: shipmentId
                }
                axios.post(url, data)
                    .then(function (r) {
                        if (quantity == 0) {
                            var parent = $(e).parents('.jsShipmentRow');
                            $(e).parents('.jsCartItem').first().remove();
                            
                            if (parent.find('.jsCartItem').length == 0) {
                                parent.remove();
                            }

                            if ($('.jsShipmentRow').length == 0) {
                                window.location.href = window.location.href;
                            }
                        }

                        if (quantity > 1) {
                            var btn = $(e).parents('.jsCartItem').find('.jsSeparateHint');
                            btn.parent('div').removeClass('hidden');
                            btn.addClass('jsSelectShipment');
                            inst.SeparateClick(btn);
                        } else {
                            var btn = $(e).parents('.jsCartItem').find('.jsSeparateHint');
                            btn.parent('div').addClass('hidden');
                            btn.removeClass('jsSelectShipment');
                        }

                        $('.jsCouponReplaceHtml').html(r.data);
                        $('.jsOrderSummary').html($('.jsOrderSummaryInPayment').html());
                        feather.replace();
                        inst.InitPayment();
                    })
                    .catch(function (e) {
                        notification.Error(e);
                    })
                    .finally(function () {
                        $('.loading-box').hide();
                    })
            })
        })
    }
    //////////////////

    // Use billing address as shipping address
    UseBillingAsShipping() {
        var defaultValue = $('#UseBillingAddressForShipment').is(':checked');
        if (defaultValue == true) {
            $('.jsShippingAddressSelection').find('input').each(function (i, e) {
                $(e).attr('disabled', 'disabled');
            })

            $('.jsOldShippingAddressForm').find('input').each(function (i, e) {
                $(e).attr('disabled', 'disabled');
            })

            $('.jsOldShippingAddressForm').find('.dropdown').each(function (i, e) {
                $(e).css('pointer-events', 'none');
            })

            $('.jsNewShippingAddressForm').find('input').each(function (i, e) {
                $(e).attr('disabled', 'disabled');
            })

            $('.jsNewShippingAddressForm').find('.dropdown').each(function (i, e) {
                $(e).css('pointer-events', 'none');
            })
        }

        $('#UseBillingAddressForShipment').change(function () {
            var value = $(this).is(':checked');
            if (value == true) {
                $('.jsShippingAddressSelection').find('input').each(function (i, e) {
                    $(e).attr('disabled', 'disabled');
                })

                $('.jsOldShippingAddressForm').find('input').each(function (i, e) {
                    $(e).attr('disabled', 'disabled');
                })

                $('.jsOldShippingAddressForm').find('.dropdown').each(function (i, e) {
                    $(e).css('pointer-events', 'none');
                })

                $('.jsNewShippingAddressForm').find('input').each(function (i, e) {
                    $(e).attr('disabled', 'disabled');
                })

                $('.jsNewShippingAddressForm').find('.dropdown').each(function (i, e) {
                    $(e).css('pointer-events', 'none');
                })
            } else {
                $('.jsShippingAddressSelection').find('input').each(function (i, e) {
                    $(e).removeAttr('disabled');
                })

                $('.jsOldShippingAddressForm').find('input').each(function (i, e) {
                    $(e).removeAttr('disabled');
                })

                $('.jsOldShippingAddressForm').find('.dropdown').each(function (i, e) {
                    $(e).css('pointer-events', 'auto');
                })

                $('.jsNewShippingAddressForm').find('input').each(function (i, e) {
                    $(e).removeAttr('disabled');
                })

                $('.jsNewShippingAddressForm').find('.dropdown').each(function (i, e) {
                    $(e).css('pointer-events', 'auto');
                })
            }
        })
    }
    //////////////////


    // Separate line item
    SeparateClick(selector) {
        if (selector) {
            $(selector).click(function () {
                $('.jsSelectShipment').each(function (j, s) {
                    $(s).show();
                })
                var code = $(selector).data('code');
                var shipmentid = $(selector).data('shipmentid');
                var qty = $(selector).parents('.jsCartItem').find('.jsChangeQuantityItemCheckout').val();
                var delivery = $(selector).data('delivery');
                var selectedstore = $(selector).data('selectedstore');
                $('#lineItemInfomation').data("code", code);
                $('#lineItemInfomation').data("shipmentid", shipmentid);
                $('#lineItemInfomation').data("qty", qty);
                $('#lineItemInfomation').data("delivery", delivery);
                $('#lineItemInfomation').data("selectedstore", selectedstore);

                $('.jsSelectShipment[data-shipmentid=' + shipmentid + ']').hide();
            })
        } else {
            $('.jsSeparateBtn').each(function (i, e) {
                $(e).click(function () {
                    $('.jsSelectShipment').each(function (j, s) {
                        $(s).show();
                    })

                    var code = $(e).data('code');
                    var shipmentid = $(e).data('shipmentid');
                    var qty = $(e).parents('.jsCartItem').find('.jsChangeQuantityItemCheckout').val();
                    var delivery = $(e).data('delivery');
                    var selectedstore = $(e).data('selectedstore');
                    $('#lineItemInfomation').data("code", code);
                    $('#lineItemInfomation').data("shipmentid", shipmentid);
                    $('#lineItemInfomation').data("qty", qty);
                    $('#lineItemInfomation').data("delivery", delivery);
                    $('#lineItemInfomation').data("selectedstore", selectedstore);

                    $('.jsSelectShipment[data-shipmentid=' + shipmentid + ']').hide();
                })
            })
        }
    }

    ConfirmSeparateItemClick() {
        $('.jsSelectShipment').each(function (i, e) {
            $(e).click(function () {
                $('.loading-box').show();
                var url = $('#lineItemInfomation').data('url');
                var code = $('#lineItemInfomation').data('code');
                var shipmentid = $('#lineItemInfomation').data('shipmentid');
                var qty = $('#lineItemInfomation').data('qty');
                var delivery = $('#lineItemInfomation').data('delivery');
                var selectedstore = $('#lineItemInfomation').data('selectedstore');
                var toShipmentId = $(e).data('shipmentid');
                var data = {
                    Code: code,
                    Quantity: qty,
                    ShipmentId: shipmentid,
                    ToShipmentId: toShipmentId,
                    DeliveryMethodId: delivery,
                    SelectedStore: selectedstore
                }

                axios.post(url, data)
                    .then(function (r) {
                        if (r.data.Status == true) {
                            window.location.href = r.data.RedirectUrl;
                        } else {
                            notification.Error(r.data.Message);
                        }
                    })
                    .catch(function (e) {
                        notification.Error(e);
                    })
                    .finally(function () {
                        $('.loading-box').hide();
                    })
            })
        })
    }

    SeparateInit() {
        this.SeparateClick();
        this.ConfirmSeparateItemClick();
    }
    /////////////////////
}