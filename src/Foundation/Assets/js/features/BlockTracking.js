class BlockTracking  {
    init() {
        var inst = this;
        $('.heroblock').click(function (e) {
            inst.trackingHeroBlock(e);
        });

        $('.video-block').on('ended', function (e) {
            inst.trackingVideoBlock(e);
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
                notification.Error(error);
            });
    }
}