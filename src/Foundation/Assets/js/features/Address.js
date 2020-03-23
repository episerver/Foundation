class Address {
    CountryClick(selector) {
        if (selector == undefined || selector == "") {
            selector = ".jsCountrySelectionContainer";
        }

        $(selector).change(function () {
            var countryCode = $(this).find('option:selected').val();
            var region = $(this).find('option:selected').val();
            var inputName = $(this).closest('form').find('.jsRegionName').val();
            var element = $(this);
            axios.get("/addressbook/GetRegions?countryCode=" + countryCode + "&region=" + region + "&inputName=" + inputName)
                .then(function (r) {
                    if ($(element).parents('form').length > 0) {
                        var region = $(element).closest('form').find('.jsCountryRegionContainer').first();
                        region.html(r.data);
                    } else {
                        var region = $(element).parent().siblings('.jsCountryRegionContainer').first();
                        region.html(r.data);
                    }

                    feather.replace();
                    var dropdown = new Dropdown(region);
                    dropdown.Init();
                })
                .catch(function (e) {
                    notification.Error(e);
                })
        })
    }

    LoadAddressInRegisterUser() {
        var inst = this;
        $('.jsCountrySelectionRegisterUser').click(function () {
            var element = this;
            var data = $(this).find('.jsCountryOptionName').val();
            if ($(this).find('option').length == 0) {
                axios.get('/header/getcountryoptions?inputName=' + data)
                    .then(function (r) {
                        var html = $(r.data).html();
                        $(element).html(html);
                        feather.replace();
                        var dropdown = new Dropdown(element);
                        dropdown.Init();
                        inst.CountryClick(element);

                        $('#login-selector-signup').click(function (e) {
                            if (!($(e.target).parents('.dropdown').children('.dropdown__selected').length > 0 || $(e.target).hasClass('.dropdown'))) {
                                $('.dropdown__group').hide();
                            }
                        });

                        $(element).find('.dropdown__selected').first().click();
                    })
                    .catch(function (e) {

                    })
                    .finally(function () {

                    })
            }
        })
    }

    Init() {
        this.CountryClick();
        this.LoadAddressInRegisterUser();
    }
}