$(document).ready(function () {

    // convert json to formdata and append __RequestVerificationToken key for CORS
    window.convertFormData = function (data) {
        var formData = new FormData();
        for (var key in data) {
            formData.append(key, data[key]);
        }
        if ($('input[name=__RequestVerificationToken]').length > 0) {
            formData.append("__RequestVerificationToken", $('input[name=__RequestVerificationToken]').val());
        }

        return formData;
    };

    var cms = new FoundationCms();
    cms.init();

    //var commerce = new FoundationCommerce();
    //commerce.init();

    //var cmsPersonalization = new FoundationCmsPersonalization();
    //cmsPersonalization.init();
});