export default class Market {
    init() {
        this.setMarket();
        this.setLanguage();
        this.setCurrency();
    }

    setMarket() {
        Array.from(document.querySelectorAll(".jsMarketSelector")).forEach(function (el, i) {
            el.addEventListener("click", function () {
                let form = el.closest('form');
                let url = form.getAttribute('action');
                let method = form.getAttribute('method');
                let bodyFormData = new FormData();
                bodyFormData.append('__RequestVerificationToken', document.querySelector("input[name=__RequestVerificationToken]").value);
                bodyFormData.append('MarketId', el.querySelector("input[name=MarketId]").value);
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
        Array.from(document.querySelectorAll(".jsLanguageSelector")).forEach(function (el, i) {
            el.addEventListener("click", function () {
                let form = el.closest('form');
                let url = form.getAttribute('action');
                let method = form.getAttribute('method');
                let bodyFormData = new FormData();
                bodyFormData.append('__RequestVerificationToken', document.querySelector("input[name=__RequestVerificationToken]").value);
                bodyFormData.append('Language', el.querySelector("input[name=Language]").value);
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

        Array.from(document.querySelectorAll(".jsCurrencySelector")).forEach(function (el, i) {
            el.addEventListener("click", function () {
                let form = el.closest('form');
                let url = form.getAttribute('action');
                let method = form.getAttribute('method');
                let bodyFormData = new FormData();
                bodyFormData.append('__RequestVerificationToken', document.querySelector("input[name=__RequestVerificationToken]").value);
                bodyFormData.append('CurrencyCode', el.querySelector("input[name=CurrencyCode]").value);
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