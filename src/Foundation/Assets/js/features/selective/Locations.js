class Locations {
    constructor() {
        this.locationMap = {};
        this.locationInfobox = {};
        this.locationInfo = "";
        this.markers = [];
        this.tempvals = [-20, 40];
        this.originalVal;
    }

    loadMapScenario() {
        this.locationMap = new Microsoft.Maps.Map('#locationMap', {
            credentials: "Agf8opFWW3n3881904l3l0MtQNID1EaBrr7WppVZ4v38Blx9l8A8x86aLVZNRv2I",
            disableScrollWheelZoom: true
        });
        this.locationInfobox = new Microsoft.Maps.Infobox(this.locationMap.getCenter(), { visible: false });
        this.locationInfobox.setMap(this.locationMap);
        this.showLocations();
        this.initializeFilters();
        this.setFilterSelection();
    }

    showLocations() {
        let locations = [];
        let instance = this;
        $('#locations .locationArticle').each(function () {
            locations.push({
                name: $(this).attr('data-mapname'),
                lat: $(this).attr('data-maplat'),
                lon: $(this).attr('data-maplon'),
                url: $(this).attr('data-mapurl')
            });
        });

        for (let i = 0; i < locations.length; i++) {
            const loc = locations[i];
            var locationCoordinates = new Microsoft.Maps.Location(loc.lat, loc.lon);
            let pushpin = new Microsoft.Maps.Pushpin(locationCoordinates, {});
            Microsoft.Maps.Events.addHandler(pushpin, 'click', (e) => {
                instance.locationInfobox.setOptions({
                    location: e.target.getLocation(),
                    maxHeight: 300,
                    maxWidth: 280,
                    description: loc.name,
                    visible: true
                });
            });
            this.locationMap.entities.push(pushpin);
            this.markers.push(locationCoordinates);
            this.locationMap.setView({
                bounds: new Microsoft.Maps.LocationRect.fromLocations(this.markers)
            });
        }
    }

    initializeFilters() {
        let instance = this;
        $('#slider-range').slider({ min: -20, max: 40, value: [-20, 40] });

        $(document).on('slideStop', '#slider-range', () => {
            var newVal = $('#slider-range').data('slider').getValue();
            instance.tempvals = newVal;
            instance.DoAjaxCallback($('.filterblock'));
        });

        $(document).off('change', '.filterblock input[type=checkbox].select-all');
        $(document).off('change', '.filterblock input[type=checkbox].select-some');
        $(document).off('change', '.filterblock input[type=checkbox]');

        $(document).on('change', ".filterblock input[type=checkbox].select-all", function (event) {
            if ($(event.target).prop('checked')) {
                $(event.target).closest('.filterblock').find('input[type=checkbox].select-some').prop('checked', false);
            }
        });

        $(document).on('change', ".filterblock input[type=checkbox].select-some", function (event) {
            if ($(event.target).prop('checked')) {
                $(event.target).closest('.filterblock').find('input[type=checkbox].select-all').prop('checked', false);
            }
            else {
                if ($(event.target).closest('.filterblock').find("input[type=checkbox]:checked").length === 0) {
                    $(event.target).closest('.filterblock').find('input[type=checkbox].select-all').prop('checked', true);
                }
            }
        });

        $(document).on('change', ".filterblock input[type=checkbox]", function () {
            var triggerFilterId = $(this).closest('.filterblock').attr('id');
            var filtersToUpdate = $('.filterblock:not([id=' + triggerFilterId + '])');
            if ($(this).is('.select-all')) {
                filtersToUpdate = $('.filterblock');
            }
            instance.DoAjaxCallback(filtersToUpdate);
        });
    }

    setFilterSelection() {
        $('.filterblock').each(function () {
            if ($(this).find('input[type = checkbox]').attr('checked')) {
                $(this).addClass('selected');
            } else {
                $(this).removeClass('selected');
            }
        });
    }

    getFilterUrl() {
        var uri = new Uri(location.pathname);
        let instance = this;
        $('.filterblock').each(function (i, e) {
            var filterName = $(e).attr('data-filtertype');
            var value = '';
            $(e).find('input[type=checkbox].select-some:checked').each(function (j, k) {
                if (value !== '') {
                    value += ',';
                }
                value += $(k).val();
            });
            uri.replaceQueryParam(filterName, value);
        });
        uri.replaceQueryParam("t", this.tempvals[0] + "," + this.tempvals[1]);
        return uri;
    }

    DoAjaxCallback(filtersToUpdate) {
        let instance = this;
        $('.loading-box').show();
        axios.get(instance.getFilterUrl())
            .then(function (result) {
                var fetched = $(result.data);
                $('#locations').html(fetched.find('#locations').html());
                instance.loadMapScenario();
                filtersToUpdate.each(function () {
                    $(this).html(fetched.find('#' + $(this).attr('id')).html());
                });
            })
            .catch(function (error) {
                notification.Error(error);
            })
            .finally(function () {
                $('.loading-box').hide();
            });
    }

}
