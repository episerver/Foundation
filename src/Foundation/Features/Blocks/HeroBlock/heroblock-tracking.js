import * as axios from "axios";

export default class HeroBlockTracking {
    init() {
        $('.heroblock').click((e) => {
            let data = {
                blockId: $(e.currentTarget).children('div').attr('blockId'),
                blockName: $(e.currentTarget).children('div').attr('name'),
                pageName: $('title').text().replace(' - NOT FOR COMMERCIAL USE', ''),
            };

            axios.post('/publicapi/TrackHeroBlock', data)
                .then((result) => {
                    console.log("Hero Block clicked: '" + $(e.currentTarget).children('div').attr('name') + "' on page - '" + $('title').text().replace(' - NOT FOR COMMERCIAL USE', '') + "'");
                }).catch((error) => {
                    notification.error(error);
                });
        });
    }
}