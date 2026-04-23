import * as $ from "jquery";
import * as axios from "axios";
import Dropdown from "../../wwwroot/js/common/dropdown";
export default class Address {
    init() {
        this.countryClick();
        this.loadAddressInRegisterUser();
    }

    countryClick(selector) {
        if (selector == undefined || selector == "") {
            selector = ".jsCountrySelectionContainer";
        }
        window.addEventListener('load', function () {
            var inputs = document.querySelectorAll(selector);
            if (inputs == null) return;
            Array.from(inputs).forEach(function (el, i) {
                let element = el;
                var countrySelectBox = el.querySelector(".select-menu");
                countrySelectBox.addEventListener("change", function () {
                    let countryCode = countrySelectBox.options[countrySelectBox.selectedIndex].value;
                    let region = countrySelectBox.options[countrySelectBox.selectedIndex].value;
                    let inputName = document.getElementsByClassName("jsRegionName")[0].getAttribute("value");
                    axios.get("/addressbook/GetRegions?countryCode=" + countryCode + "&region=" + region + "&inputName=" + inputName)
                        .then(function (r) {
                            var region1 = document.querySelectorAll('.jsCountryRegionContainer');
                            if (region1.length > 1) {
                                region1[0].innerHTML = r.data;
                                region1[1].innerHTML = r.data;
                            }
                            else {
                                region1[0].innerHTML = r.data;
                            }
                            feather.replace();
                            let dropdown = new Dropdown(region);
                            dropdown.init();
                        })
                        .catch(function (e) {
                            notification.error(e);
                        });
                });

            });
        });
        
    }

    loadAddressInRegisterUser() {
        let inst = this;
        var inputs = document.querySelectorAll(".jsCountrySelectionRegisterUser");
        if (inputs == null) return;

        Array.from(inputs).forEach(function (el, i) {
            let element = el;
            let inputName = el.querySelector(".jsCountryOptionName").getAttribute("value");
            axios.get('/header/getcountryoptions?inputName=' + inputName)
                .then(function (r) {
                    
                    const htmlElement = document.createElement("div");
                    htmlElement.innerHTML= r.data;
                    el.innerHTML = htmlElement.getElementsByClassName("jsCountrySelectionContainer")[0].innerHTML;
                    feather.replace();
                    let dropdown = new Dropdown(element);
                    dropdown.Init();
                    inst.countryClick(element);
                })
                .catch(function (e) {

                })
                .finally(function () {
                });

        });
 
    }
}