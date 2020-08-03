import * as $ from "jquery";
import * as axios from "axios";

export default class VideoBlockTracking {
    init() {
        $('.video-block').on('ended', function (e) {
            inst.trackingVideoBlock(e);
        });
    }

    trackingVideoBlock(e) {
        let data = {
            blockId: $(e.currentTarget).attr('blockId'),
            blockName: $(e.currentTarget).attr('name'),
            pageName: $('title').text().replace(' - NOT FOR COMMERCIAL USE', ''),
            __RequestVerificationToken: token
        };

        axios.post('/publicapi/TrackVideoBlock', convertFormData(data))
            .then(function (result) {
                console.log("Video Block viewed: '" + $(e.currentTarget).attr('name') + "' on page - '" + $('title').text().replace(' - NOT FOR COMMERCIAL USE', '') + "'");
            }).catch(function (error) {
                notification.error(error);
            });
    }
}