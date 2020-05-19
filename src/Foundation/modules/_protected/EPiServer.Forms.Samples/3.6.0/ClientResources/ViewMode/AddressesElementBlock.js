(function ($) {
    var _utilsSvc = epi.EPiServer.Forms.Utils;
    var addressComponentsMap = {
        street_number: 'short_name',
        route: 'long_name', //street
        locality: 'long_name', // city
        administrative_area_level_1: 'long_name', //state
        country: 'long_name', //country
        postal_code: 'short_name' //zip
    };

    //init address elements in the ViewMode
    if (typeof __SamplesAddressElements != "undefined") {
        $.each(__SamplesAddressElements || [], function (i, addressElementInfo) {
            var addressEl = $("#" + addressElementInfo.guid + '_address');
            if (!addressEl || addressEl.length < 1) {
                return;
            }

            $('.EPiServerForms .FormAddressElement .Form__CustomInput').on('keydown', function _onKeyDown(e) {
                if (e.keyCode == 13) {
                    if ($('.pac-container:visible').length) { // prevent form from submitting when the place options list is being displayed (.pac-container:visible)
                        e.preventDefault();
                    }
                    else {
                        _utilsSvc.showNextStepOnEnterKeyDown(e)
                    }
                }
            });

            if (!google || !google.maps.places) {
                return;
            }
            var options = {
                types: ['establishment']
            };

            var gPlace = new google.maps.places.Autocomplete(addressEl[0], options);
            google.maps.event.addListener(gPlace, 'place_changed', function () {
                var geoComponents = gPlace.getPlace(),
                    addressComponents = geoComponents.address_components,
                    geometry = geoComponents.geometry;

                //clear all address components
                $('input[id^="' + addressElementInfo.guid + '"]').val('');

                // generate map
                generatingMap(addressElementInfo, geometry);

                if (!addressComponents || !addressComponents.length) {
                    return;
                }
                $("#" + addressElementInfo.guid + '_address').val(geoComponents.name);
                for (var i = 0; i < addressComponents.length; i++) {
                    var addressType = addressComponents[i].types[0];
                    if (addressComponentsMap[addressType]) {
                        var el = $("#" + addressElementInfo.guid + '_' + addressType);
                        el.val(addressComponents[i][addressComponentsMap[addressType]]);
                    }
                }
            });
        });
    }

    // display google map for a specific location
    function generatingMap(addressElementInfo, geometry) {
        var $addressEl = $("#" + addressElementInfo.guid + '_map');
        if (!$addressEl || $addressEl.length < 1) {
            return;
        }
        // if location data is invalid, then hide the map
        if (!geometry || !geometry.location) {
            $addressEl.hide();
            return;
        }

        var map,
            markers = [],
            mapOptions = {
                zoom: 16,
            };
        $addressEl.show();
        map = new google.maps.Map($addressEl[0], mapOptions);
        var location = geometry.location;
        if (location) {
            //center map
            map.setCenter(location);
            //set marker
            var marker = new google.maps.Marker({
                position: location,
                map: map,
                title: '',
                draggable: false
            });
            markers.push(marker);
        }
    }

})($$epiforms || $);