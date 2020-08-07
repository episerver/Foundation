import axios from "axios";
import feather from "feather-icons";

export default class ProductRecommendations {
    init() {
        let widget = $(".recommendation-widget-block");
        let url = '/WidgetBlock/GetRecommendations';
        let data = {
            widgetType: widget.data("widget-type"),
            numberOfRecs: widget.data("recs-number"),
            name: widget.data("name"),
            value: widget.data("value")
        };
        axios.post(url, data)
            .then((result) => {
                $('.recommendation-widget-block').html(result.data).ready(() => {
                    feather.replace();
                });
            })
            .catch((error) => {
                notification.error(error);
            })
            .finally(() => {
                $('body>.loading-box').hide();
            });
    }
}