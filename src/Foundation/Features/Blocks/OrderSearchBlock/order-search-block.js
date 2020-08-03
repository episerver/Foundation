import * as $ from "jquery";
import * as axios from "axios";

export default class OrderSearchBlock {
    init() {
        this.showHideAdvancedFilter();
        this.filterClick();
    }

    showHideAdvancedFilter() {
        $('.jsAdvancedBtn').each(function (i, e) {
            $(e).click(function () {
                let container = $(e).parents('.jsOrderSearchFilterContainer').first();
                let advanceBox = container.find('.jsAdvancedFilterBox');
                if (advanceBox.is(":visible")) {
                    advanceBox.slideUp();
                } else {
                    advanceBox.slideDown();
                }
            })
        })
    }

    filterClick() {
        let inst = this;
        $('.jsFilterOrderSearchBtn').each(function (i, e) {
            $(e).click(function () {
                $('.loading-box').show();
                let container = $(e).parents('.jsOrderSearchFilterContainer').first();
                let filterBox = container.find('.jsAdvancedFilterBox').first();
                let formData = new FormData();
                let valid = true;

                if (filterBox.is(":visible")) {
                    valid = inst.validate(filterBox);
                    let data = serializeObject(container);
                    formData = convertFormData(data);
                } else {
                    formData.append("Keyword", container.find('input[name=Keyword]').first().val());
                    formData.append("CurrentBlockId", container.find('input[name=CurrentBlockId]').first().val());
                }

                let url = container[0].action;
                if (valid) {
                    axios.post(url, formData)
                        .then(function (r) {
                            let table = container.siblings('.jsOrderSearchTable').first();
                            table.find('.jsOrderSearchTableBody').html(r.data);
                        })
                        .catch(function (e) {
                            notification.error(e);
                        })
                        .finally(function () {
                            $('.loading-box').hide();
                        })
                } else {
                    $('.loading-box').hide();
                }
            })
        })
    }

    validate(e) {
        let valid = true;
        let message = "";
        let dateFrom = $(e).find('input[name=DateFrom]').first().val();
        let dateTo = $(e).find('input[name=DateTo]').first().val();
        let priceFrom = $(e).find('input[name=PriceFrom]').first().val();
        let priceTo = $(e).find('input[name=PriceTo]').first().val();

        if (dateFrom != "" || dateTo != "") {
            let dateFromParse = Date.parse(dateFrom);
            let dateToParse = Date.parse(dateTo);
            if (dateFrom != "" && dateFromParse > Date.now()) {
                valid = false;
                message += "<p>DateFrom is invalid</p>";
            }

            if (dateTo != "" && dateToParse > Date.now()) {
                valid = false;
                message += "<p>DateTo is invalid</p>";
            }

            if (dateFrom != "" && dateTo != "" && dateFromParse > dateToParse) {
                valid = false;
                message += "<p>DateFrom can not greater than DateTo</p>";
            }
        }

        if (priceFrom != "" || priceTo != "") {
            let priceFromParse = parseInt(priceFrom);
            let priceToParse = parseInt(priceTo);

            if (priceFromParse < 0) {
                valid = false;
                message += "<p>PriceFrom is invalid</p>";
            }

            if (priceToParse < 0) {
                valid = false;
                message += "<p>PriceTo is invalid</p>";
            }

            if (priceFromParse > 0 && priceToParse > 0 && priceFromParse > priceToParse) {
                valid = false;
                message += "<p>PriceFrom can not greater than PriceTo</p>";
            }
        }

        if (valid) {
            $(e).find('.jsOrderSearchError').html("");
            $(e).find('.jsOrderSearchError').removeClass("error");
        } else {
            $(e).find('.jsOrderSearchError').html(message);
            $(e).find('.jsOrderSearchError').addClass("error");
        }

        return valid;
    }
}