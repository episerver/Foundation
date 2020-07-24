import * as $ from "jquery";
import * as axios from "axios";

export default class HeroBlockTracking {
    init() {
        var inst = this;
        $('.heroblock').click(function (e) {
            inst.trackingHeroBlock(e);
        });
    }

    trackingHeroBlock(e) {
        let data = {
            blockId: $(e.currentTarget).children('div').attr('blockId'),
            blockName: $(e.currentTarget).children('div').attr('name'),
            pageName: $('title').text().replace(' - NOT FOR COMMERCIAL USE', ''),
            __RequestVerificationToken: token
        };

        axios.post('/publicapi/TrackHeroBlock', convertFormData(data))
            .then(function (result) {
                console.log("Hero Block clicked: '" + $(e.currentTarget).children('div').attr('name') + "' on page - '" + $('title').text().replace(' - NOT FOR COMMERCIAL USE', '') + "'");
            }).catch(function (error) {
                notification.Error(error);
            });
    }
}