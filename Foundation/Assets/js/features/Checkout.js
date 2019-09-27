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
                    $('#oldShippingAddressForm').hide();
                    $('#newShippingAddressForm').show();
                } else {
                    $('#oldShippingAddressForm').show();
                    $('#newShippingAddressForm').hide();
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
}