class Checkout {

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
                var value = $(this).val();
                $('#AddressType').val(value);
                if (value == 0) {
                    $('#oldShippingAddressForm').hide();
                    $('#newShippingAddressForm').show();
                } else {
                    $('#oldShippingAddressForm').show();
                    $('#newShippingAddressForm').hide();
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
        $('.jsShippingMethodContainer').change(function () {
            var url = $('.jsShippingMethodContainer').attr('url');
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
                var data = {
                    code: code,
                    quantity: quantity
                }
                axios.post(url, data)
                    .then(function (r) {
                        if (quantity == 0) {
                            $(e).parents('.jsCartItem').first().remove();

                            if ($('.jsCartItem').length == 0) {
                                window.location.href = window.location.href;
                            }
                        }

                        $('.jsCouponReplaceHtml').html(r.data);
                        $('.jsOrderSummary').html($('.jsOrderSummaryInPayment').html());
                        feather.replace();
                        inst.InitPayment();
                    })
                    .catch(function (e) {
                        notification.error(e);
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
            $('#shippingAddressSelection').find('input').each(function (i, e) {
                $(e).attr('disabled', 'disabled');
            })

            $('#oldShippingAddressForm').find('input').each(function (i, e) {
                $(e).attr('disabled', 'disabled');
            })

            $('#oldShippingAddressForm').find('.dropdown').each(function (i, e) {
                $(e).css('pointer-events', 'none');
            })

            $('#newShippingAddressForm').find('input').each(function (i, e) {
                $(e).attr('disabled', 'disabled');
            })

            $('#newShippingAddressForm').find('.dropdown').each(function (i, e) {
                $(e).css('pointer-events', 'none');
            })
        }

        $('#UseBillingAddressForShipment').change(function () {
            var value = $(this).is(':checked');
            if (value == true) {
                $('#shippingAddressSelection').find('input').each(function (i, e) {
                    $(e).attr('disabled', 'disabled');
                })

                $('#oldShippingAddressForm').find('input').each(function (i, e) {
                    $(e).attr('disabled', 'disabled');
                })

                $('#oldShippingAddressForm').find('.dropdown').each(function (i, e) {
                    $(e).css('pointer-events', 'none');
                })

                $('#newShippingAddressForm').find('input').each(function (i, e) {
                    $(e).attr('disabled', 'disabled');
                })

                $('#newShippingAddressForm').find('.dropdown').each(function (i, e) {
                    $(e).css('pointer-events', 'none');
                })
            } else {
                $('#shippingAddressSelection').find('input').each(function (i, e) {
                    $(e).removeAttr('disabled');
                })

                $('#oldShippingAddressForm').find('input').each(function (i, e) {
                    $(e).removeAttr('disabled');
                })

                $('#oldShippingAddressForm').find('.dropdown').each(function (i, e) {
                    $(e).css('pointer-events', 'auto');
                })

                $('#newShippingAddressForm').find('input').each(function (i, e) {
                    $(e).removeAttr('disabled');
                })

                $('#newShippingAddressForm').find('.dropdown').each(function (i, e) {
                    $(e).css('pointer-events', 'auto');
                })
            }
        })
    }
    //////////////////
}