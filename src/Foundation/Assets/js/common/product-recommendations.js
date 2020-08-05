class ProductRecs {
    getRecommendations(widget, numberOfRecs, name, value) {
        let url = '/WidgetBlock/GetRecommendations';
        let data = {
            widgetType: widget,
            numberOfRecs: numberOfRecs,
            name: name,
            value: value
        };
        axios.post(url, data)
            .then(function (result) {
                $('.recommendationwidgetblock').html(result.data).ready(function () {
                    feather.replace();
                });
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {
                $('body>.loading-box').hide();
            });
    }
}