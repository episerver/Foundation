export default class Market {
    init() {
        this.setMarket();
    }

    setMarket() {
        $('.jsMarketSelector').each(function (i, e) {
            $(e).click(function () {
                let form = $(this).closest('form');
                let url = form.attr('action');
                let method = form.attr('method');
                let bodyFormData = new FormData();
                bodyFormData.set('__RequestVerificationToken', $("input[name=__RequestVerificationToken]", form).val());

                axios({
                    url: url,
                    method: method,
                    data: bodyFormData
                })
                    .then(function (result) {
                        window.location = result.data.returnUrl;
                    }).catch(function (e) {
                        notification.error(e);
                    });

                return false;
            });
        });
    }
}