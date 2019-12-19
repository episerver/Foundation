class OrderSearchBlock {

    ShowHideAdvancedFilter() {
        $('.jsAdvancedBtn').each(function (i, e) {
            $(e).click(function () {
                var container = $(e).parents('.jsOrderSearchFilterContainer').first();
                var advanceBox = container.find('.jsAdvancedFilterBox');
                if (advanceBox.is(":visible")) {
                    advanceBox.slideUp();
                } else {
                    advanceBox.slideDown();
                }
            })
        })
    }

    FilterClick() {
        var inst = this;
        $('.jsFilterOrderSearchBtn').each(function (i, e) {
            $(e).click(function () {
                $('.loading-box').show();
                var container = $(e).parents('.jsOrderSearchFilterContainer').first();
                var filterBox = container.find('.jsAdvancedFilterBox').first();
                var formData = new FormData();
                var valid = true;

                if (filterBox.is(":visible")) {
                    valid = inst.Validate(filterBox);
                    var data = serializeObject(container);
                    formData = convertFormData(data);
                } else {
                    formData.append("Keyword", container.find('input[name=Keyword]').first().val());
                    formData.append("CurrentBlockId", container.find('input[name=CurrentBlockId]').first().val());
                }

                var url = container[0].action;
                if (valid) {
                    axios.post(url, formData)
                        .then(function (r) {
                            var table = container.siblings('.jsOrderSearchTable').first();
                            table.find('.jsOrderSearchTableBody').html(r.data);
                        })
                        .catch(function (e) {
                            notification.Error(e);
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

    Validate(e) {
        var valid = true;
        var message = "";
        var dateFrom = $(e).find('input[name=DateFrom]').first().val();
        var dateTo = $(e).find('input[name=DateTo]').first().val();
        var priceFrom = $(e).find('input[name=PriceFrom]').first().val();
        var priceTo = $(e).find('input[name=PriceTo]').first().val();

        if (dateFrom != "" || dateTo != "") {
            var dateFromParse = Date.parse(dateFrom);
            var dateToParse = Date.parse(dateTo);
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
            var priceFromParse = parseInt(priceFrom);
            var priceToParse = parseInt(priceTo);

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

    Init() {
        this.ShowHideAdvancedFilter();
        this.FilterClick();
    }
}