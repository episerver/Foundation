import * as $ from "jquery";
import * as axios from "axios";

export default class Address {
    init() {
        this.countryClick();
        this.loadAddressInRegisterUser();
    }

    countryClick(selector) {
        if (selector == undefined || selector == "") {
            selector = ".jsCountrySelectionContainer";
        }

        $(selector).change(function () {
            let countryCode = $(this).find('option:selected').val();
            let region = $(this).find('option:selected').val();
            let inputName = $(this).closest('form').find('.jsRegionName').val();
            let element = $(this);
            axios.get("/addressbook/GetRegions?countryCode=" + countryCode + "&region=" + region + "&inputName=" + inputName)
                .then(function (r) {
                    if ($(element).parents('form').length > 0) {
                        let region = $(element).closest('form').find('.jsCountryRegionContainer').first();
                        region.html(r.data);
                    } else {
                        let region = $(element).parent().siblings('.jsCountryRegionContainer').first();
                        region.html(r.data);
                    }

                    feather.replace();
                    let dropdown = new Dropdown(region);
                    dropdown.Init();
                })
                .catch(function (e) {
                    notification.error(e);
                })
        })
    }

    loadAddressInRegisterUser() {
        let inst = this;
        $('.jsCountrySelectionRegisterUser').click(function () {
            let element = this;
            let data = $(this).find('.jsCountryOptionName').val();
            if ($(this).find('option').length == 0) {
                axios.get('/header/getcountryoptions?inputName=' + data)
                    .then(function (r) {
                        let html = $(r.data).html();
                        $(element).html(html);
                        feather.replace();
                        let dropdown = new Dropdown(element);
                        dropdown.Init();
                        inst.countryClick(element);

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
}