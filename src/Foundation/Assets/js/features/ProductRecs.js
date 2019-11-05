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
                $('.recommendationwidgetblock').html(result.data);
            })
            .catch(function (error) {
                notification.Error(error);
            })
            .finally(function () {
                $('body>.loading-box').hide();
            });
    }
}