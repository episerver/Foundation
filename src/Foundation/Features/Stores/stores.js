import axios from "axios";
require('webpack-jquery-ui');

export default class Stores {
    constructor() {
        this.storeMap = {};
        this.searchManager = null;
        this.storeInfobox = {};
        this.storeInfo = "";
        this.markers = [];
        this.searched = false;
    }

    init() {
        if ($("#storeMap").length === 0) {
            return;
        }

        let instance = this;
        instance.loadScript("https://www.bing.com/api/maps/mapcontrol?&callback=getMap");
        window.getMap = () => {
            instance.loadMapScenario();
        }

        $(document).on('keyup', '#searchMapInput', (e) => {
            if (e.keyCode === 13) {
                e.preventDefault();
                this.search();
            }
        });

        $(document).on('click', '.use-current-location', this.useCurrentLocation);

        $(document).ready(() => {
            $("#searchMapInput").autocomplete({
                source: (request, response) => {
                    axios.get("http://dev.virtualearth.net/REST/v1/Locations", {
                        params: {
                            key: "Agf8opFWW3n3881904l3l0MtQNID1EaBrr7WppVZ4v38Blx9l8A8x86aLVZNRv2I",
                            q: request.term
                        }
                    })
                        .then(({ data }) => {
                            let result = data.resourceSets[0];
                            if (result) {
                                if (result.estimatedTotal > 0) {
                                    response($.map(result.resources, (item) => {
                                        $("#searchMapInput").autocomplete('option', 'autoFocus', true);
                                        return {
                                            data: item,
                                            label: item.name + ' (' + item.address.countryRegion + ')',
                                            value: item.name
                                        };
                                    }));
                                }
                            }
                        })
                        .catch((error) => {
                            console.log(error);
                        });
                },
                minLength: 1,
                select: (event, ui) => {
                    if (instance.searched) {
                        instance.storeMap.entities.pop();
                        instance.searched = false;
                    }
                    instance.addSearchedLocationMarker(new Microsoft.Maps.Location(ui.item.data.point.coordinates[0], ui.item.data.point.coordinates[1]));
                }
            });
        });
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
        this.storeMap = new Microsoft.Maps.Map('#storeMap', {
            credentials: "Agf8opFWW3n3881904l3l0MtQNID1EaBrr7WppVZ4v38Blx9l8A8x86aLVZNRv2I"
        });
        this.storeInfobox = new Microsoft.Maps.Infobox(this.storeMap.getCenter(), { visible: false });
        this.storeInfobox.setMap(this.storeMap);
        this.showStoreLocation();
        this.setDefaultStore();
    }

    showStoreLocation() {
        let locations = [];
        $('.store-detail__store-locator').each((index, element) => {
            locations.push({
                address: $(element).attr('address'),
                html: $(element).closest('.store-detail').clone()
            });
        });

        for (let i = 0; i < locations.length; i++) {
            const loc = locations[i];
            $(loc.html).removeClass("row").find("div").removeClass("col").removeClass("col-auto");
            this.getStoreLocation(loc.address, $(loc.html).prop('outerHTML'));
        }
    }

    getStoreLocation(address, html) {
        let instance = this;
        let searchRequest;
        if (!this.searchManager) {
            Microsoft.Maps.loadModule('Microsoft.Maps.Search', () => {
                this.searchManager = new Microsoft.Maps.Search.SearchManager(this.storeMap);
                this.getStoreLocation(address, html);
            });
        } else {
            searchRequest = {
                where: address,
                callback: (r) => {
                    if (r && r.results && r.results.length > 0) {
                        let pushpin = new Microsoft.Maps.Pushpin(r.results[0].location, {});
                        Microsoft.Maps.Events.addHandler(pushpin, 'click', (e) => {
                            this.storeMap.setView({
                                center: e.target.getLocation(),
                                zoom: 15
                            });
                            this.storeInfobox.setOptions({
                                location: e.target.getLocation(),
                                maxHeight: 300,
                                maxWidth: 280,
                                description: html,
                                visible: true
                            });
                            instance.setDefaultStore();
                        });
                        this.storeMap.entities.push(pushpin);
                        this.markers.push(r.results[0].location);
                        this.storeMap.setView({
                            bounds: new Microsoft.Maps.LocationRect.fromLocations(this.markers)
                        });
                    }
                },
                errorCallback: (e) => {
                    alert("No results found");
                }
            };
            this.searchManager.geocode(searchRequest);
        }
    }

    search() {
        if (!this.searchManager) {
            Microsoft.Maps.loadModule('Microsoft.Maps.Search', () => {
                this.searchManager = new Microsoft.Maps.Search.SearchManager(this.storeMap);
                this.search();
            });
        }
        else {
            if (this.searched) {
                this.storeMap.entities.pop();
                this.searched = false;
            }
            let address = $('#searchMapInput').val();
            let searchRequest = {
                where: address,
                callback: (r) => {
                    if (r && r.results && r.results.length > 0) {
                        this.addSearchedLocationMarker(r.results[0].location);
                    }
                },
                errorCallback: (e) => {
                    alert("No results found");
                }
            };
            this.searchManager.geocode(searchRequest);
        }
    }

    addSearchedLocationMarker(location) {
        let pushpin = new Microsoft.Maps.Pushpin(location, {
            icon: window.location.origin + '/Assets/icons/gfx/bingmap-position.png'
        });
        Microsoft.Maps.Events.addHandler(pushpin, 'click', (e) => {
            this.storeMap.setView({
                center: e.target.getLocation(),
                zoom: 15
            });
        });

        this.storeMap.entities.push(pushpin);
        this.markers.push(location);

        this.storeMap.setView({
            bounds: Microsoft.Maps.LocationRect.fromLocations(this.markers)
        });

        this.storeInfobox.setOptions({ visible: false });
        this.searched = true;
    }

    useCurrentLocation() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition((position) => {
                let location = new Microsoft.Maps.Location(position.coords.latitude, position.coords.longitude);
                let pushpin = new Microsoft.Maps.Pushpin(location, {
                    color: 'blue'
                });
                Microsoft.Maps.Events.addHandler(pushpin, 'click', (e) => {
                    this.storeMap.setView({
                        center: e.target.getLocation(),
                        zoom: 15
                    });
                });

                this.storeMap.entities.push(pushpin);
                this.markers.push(r.results[0].location);

                this.storeMap.setView({
                    bounds: Microsoft.Maps.LocationRect.fromLocations(this.markers)
                });

                this.storeInfobox.setOptions({ visible: false });
            }, (error) => {
                alert(error.message + " This feature is available in HTTPS.");
            });
        } else {
            x.innerHTML = "Geolocation is not supported by this browser.";
        }
    }

    setDefaultStore() {
        let instance = this;
        $('.set-default-store').each((index, element) => {
            $(element).click((e) => {
                axios.post("/StorePage/SetDefaultStore", { storeCode: e.target.dataset.code })
                    .then((response) => {
                        $("#storeName").text(e.target.dataset.name);
                        instance.storeInfobox.setOptions({ visible: false });
                    })
                    .catch((error) => {
                        console.log(error);
                    });
            });
        });
    }
}