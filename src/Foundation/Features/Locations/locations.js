import Uri from "jsuri";
require("bootstrap-slider");

export default class Locations {
    constructor() {
        this.locationMap = {};
        this.locationInfobox = {};
        this.locationInfo = "";
        this.markers = [];
        this.tempvals = [-20, 40];
        this.originalVal;
    }

    init() {
        if (document.querySelector("#locationMap") === null) {
            return;
        }

        let instance = this;
        instance.loadScript("https://www.bing.com/api/maps/mapcontrol?&callback=getMap");
        window.getMap = () => {
            instance.loadMapScenario();
        }
    }

    loadScript(url) {
        let script = document.createElement("script");
        script.type = "text/javascript";
        script.async = true;
        script.defer = true;
        script.src = url;
        document.getElementsByTagName("head")[0].appendChild(script);
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
        document.querySelectorAll('#locations .locationArticle').forEach(function (el,i) {
            locations.push({
                name: el.getAttribute('data-mapname'),
                lat: el.getAttribute('data-maplat'),
                lon: el.getAttribute('data-maplon'),
                url: el.getAttribute('data-mapurl')
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

        $('#slider-range').bootstrapSlider(
            { min: -20, max: 40, value: [-20, 40] }
        );

        $(document).on('slideStop', '#slider-range', () => {
            var newVal = document.querySelector('#slider-range').value.split(",");
            instance.tempvals = newVal;
            instance.doAjaxCallback(document.querySelectorAll('.filterblock'));
        });

        document.querySelector(".filterblock input[type=checkbox].select-all").removeEventListener("change", instance.removeFilters, false);
        document.querySelector(".filterblock input[type=checkbox].select-some").removeEventListener("change", instance.removeFilters, false);
        document.querySelector(".filterblock input[type=checkbox]").removeEventListener("change", instance.removeFilters, false);
        //$(document).off('change', '.filterblock input[type=checkbox].select-all');
        //$(document).off('change', '.filterblock input[type=checkbox].select-some');
        //$(document).off('change', '.filterblock input[type=checkbox]');

        instance.delegateSelector('.location-filter', "change", ".filterblock input[type=checkbox].select-all", function (e) {
            if (e.target.checked) {
                //console.log("inside first if")
                e.target.closest('.filterblock').querySelector('input[type=checkbox].select-some').checked = false;
            }
        });

        //$(document).on('change', ".filterblock input[type=checkbox].select-all", function (event) {
        //    //console.log(event.target);
        //    if ($(event.target).prop('checked')) {
        //        $(event.target).closest('.filterblock').find('input[type=checkbox].select-some').prop('checked', false);
        //    }
        //});

        instance.delegateSelector('.location-filter', "change", ".filterblock input[type=checkbox].select-some", function (e) {
            //console.log(e.target);
            //console.log(e.target.checked);
            if (e.target.checked) {
                //console.log("inside second if")
                e.target.closest('.filterblock').querySelector('input[type=checkbox].select-all').checked = false;
            }
            else {
                //console.log("inside second else")
                if (e.target.closest('.filterblock').querySelector("input[type=checkbox]:checked").length === 0) {
                    e.target.closest('.filterblock').querySelector('input[type=checkbox].select-all').checked = true;
                }
            }
        });
        //$(document).on('change', ".filterblock input[type=checkbox].select-some", function (event) {
        //    if ($(event.target).prop('checked')) {
        //        $(event.target).closest('.filterblock').find('input[type=checkbox].select-all').prop('checked', false);
        //    }
        //    else {
        //        if ($(event.target).closest('.filterblock').find("input[type=checkbox]:checked").length === 0) {
        //            $(event.target).closest('.filterblock').find('input[type=checkbox].select-all').prop('checked', true);
        //        }
        //    }
        //});
        //'.location-filter'
        instance.delegateSelector('.location-filter', "change", ".filterblock input[type=checkbox]", function (e) {
            //console.log(e.target);
            //console.log(e.target.checked);
            var triggerFilterId = e.target.closest('.filterblock').getAttribute('id');
            var filtersToUpdate = document.querySelectorAll('.filterblock:not([id=' + triggerFilterId + '])');
            if (e.target.classList.contains('select-all')) {
                //console.log("inside third if")
                filtersToUpdate = document.querySelectorAll('.filterblock');
            }
            instance.doAjaxCallback(filtersToUpdate); 
        });
        //$(document).on('change', ".filterblock input[type=checkbox]", function () {
        //    var triggerFilterId = $(this).closest('.filterblock').attr('id');
        //    var filtersToUpdate = document.querySelectorAll('.filterblock:not([id=' + triggerFilterId + '])');
        //    if ($(this).is('.select-all')) {
        //        filtersToUpdate = document.querySelectorAll('.filterblock');
        //    }
        //    instance.doAjaxCallback(filtersToUpdate);
        //});
    }

    setFilterSelection() {
        document.querySelectorAll('.filterblock').forEach(function (el, i) {
            if (el.querySelector('input[type = checkbox]').getAttribute('checked')) {
                el.classList.add('selected');
            } else {
                el.classList.remove('selected');
            }
        });
    }

    getFilterUrl() {

        var uri = new Uri(location.pathname);
        document.querySelectorAll('.filterblock').forEach(function (el, i) {
            var filterName = el.getAttribute('data-filtertype');
            var value = '';
            el.querySelectorAll('input[type=checkbox].select-some:checked').forEach(function (k, j) {
                if (value !== '') {
                    value += ',';
                }
                value += k.value;
            });
            uri.replaceQueryParam(filterName, value);
        });
        uri.replaceQueryParam("t", this.tempvals[0] + "," + this.tempvals[1]);
        return uri;
    }

    doAjaxCallback(filtersToUpdate) {
        let instance = this;
        document.querySelector('.loading-box').style.display = "block";

        axios.get(instance.getFilterUrl())
            .then(function (result) {
                var newFetchedElement = document.createElement("div");
                newFetchedElement.innerHTML = result.data;
                var fetched = newFetchedElement;
                document.querySelector('#locations').innerHTML = fetched.querySelector('#locations').innerHTML;
                instance.loadMapScenario();
                filtersToUpdate.forEach(function (m, n) {
                    m.innerHTML = fetched.querySelector('#' + m.getAttribute('id')).innerHTML;
                });
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {
                setTimeout(instance.hideLoadingBox(), 300);
            });


        
    }

    hideLoadingBox() {
        document.querySelector('.loading-box').style.display = "none";
    }

    delegateSelector(selector, event, childSelector, handler) {
        //console.log(selector);
        let inst = this;
        var is = function (el, selector) {
            return (el.matches || el.matchesSelector || el.msMatchesSelector || el.mozMatchesSelector || el.webkitMatchesSelector || el.oMatchesSelector).call(el, selector);
        };

        var elements = document.querySelectorAll(selector);
        [].forEach.call(elements, function (el, i) {
            el.addEventListener(event, function (e) {
                if (is(e.target, childSelector)) {
                    handler(e);
                }
            });
        });
    }

    removeFilters() {
        return false;
    }
}
