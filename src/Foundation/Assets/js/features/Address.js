class Address {
    CountryClick() {
        $('.jsCountrySelectionContainer').change(function () {
            var countryCode = $(this).find('input[type=radio]:checked').val();
            var region = $(this).find('input[type=radio]:checked').val();
            var inputName = $(this).closest('form').find('.jsRegionName').val();
            var element = $(this);
            axios.get("/addressbook/GetRegions?countryCode=" + countryCode + "&region=" + region + "&inputName=" + inputName)
                .then(function (r) {
                    var region = $(element).closest('form').find('.jsCountryRegionContainer').first();
                    region.html(r.data);

                    feather.replace();
                    var dropdown = new Dropdown(region);
                    dropdown.Init();
                })
                .catch(function (e) {
                    notification.Error(e);
                })
        })
    }

    Init() {
        this.CountryClick();
    }
}