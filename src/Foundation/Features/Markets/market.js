export default class Market {
    init() {
        this.setMarket();
        this.setLanguage();
        this.setCurrency();
    }

    setMarket() {
        $('.jsMarketSelector').each(function (i, e) {
            $(e).click(function () {
                let form = $(this).closest('form');
                let url = form.attr('action');
                let method = form.attr('method');
                let bodyFormData = new FormData();
                bodyFormData.set('__RequestVerificationToken', $("input[name=__RequestVerificationToken]", form).val());
                bodyFormData.set('MarketId', $("input[name=MarketId]", e).val());

                axios({
                    url: url,
                    method: method,
                    data: bodyFormData
                }).then(function (result) {
                    window.location = result.data.returnUrl;
                }).catch(function (e) {
                    notification.error(e);
                });
            });
        });
    }

    setLanguage() {
        $('.jsLanguageSelector').each(function (i, e) {
            $(e).click(function () {
                let form = $(this).closest('form');
                let url = form.attr('action');
                let method = form.attr('method');
                let bodyFormData = new FormData();
                bodyFormData.set('__RequestVerificationToken', $("input[name=__RequestVerificationToken]", form).val());
                bodyFormData.set('Language', $("input[name=Language]", e).val());

                axios({
                    url: url,
                    method: method,
                    data: bodyFormData
                }).then(function (result) {
                    window.location = result.data.returnUrl;
                }).catch(function (e) {
                    notification.error(e);
                });
            });
        });
    }

    setCurrency() {
        $('.jsCurrencySelector').each(function (i, e) {
            $(e).click(function () {
                let form = $(this).closest('form');
                let url = form.attr('action');
                let method = form.attr('method');
                let bodyFormData = new FormData();
                bodyFormData.set('__RequestVerificationToken', $("input[name=__RequestVerificationToken]", form).val());
                bodyFormData.set('CurrencyCode', $("input[name=CurrencyCode]", e).val());

                axios({
                    url: url,
                    method: method,
                    data: bodyFormData
                }).then(function (result) {
                    window.location = result.data.returnUrl;
                }).catch(function (e) {
                    notification.error(e);
                });
            });
        });
    }
}