import * as axios from "axios";

export default class VideoBlockTracking {
    init() {
        $('.video-block').on('ended', (e) => {
            let data = {
                blockId: $(e.currentTarget).attr('blockId'),
                blockName: $(e.currentTarget).attr('name'),
                pageName: $('title').text().replace(' - NOT FOR COMMERCIAL USE', ''),
            };

            axios.post('/publicapi/TrackVideoBlock', data)
                .then((result) => {
                    console.log("Video Block viewed: '" + $(e.currentTarget).attr('name') + "' on page - '" + $('title').text().replace(' - NOT FOR COMMERCIAL USE', '') + "'");
                }).catch((error) => {
                    notification.error(error);
                });
        });
    }
}