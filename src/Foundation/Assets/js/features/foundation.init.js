$(document).ready(function () {

    // convert json to formdata and append __RequestVerificationToken key for CORS
    window.convertFormData = function (data) {
        var formData = new FormData();
        for (var key in data) {
            if (key == "__RequestVerificationToken") {
                continue;
            }
            formData.append(key, data[key]);
        }

        if ($('input[name=__RequestVerificationToken]').length > 0) {
            formData.append("__RequestVerificationToken", $('input[name=__RequestVerificationToken]').val());
        }

        return formData;
    };

    window.serializeObject = function (form) {
        var datas = form.serializeArray();
        var jsonData = {};
        for (var d in datas) {
            jsonData[datas[d].name] = datas[d].value;
        }

        return jsonData;
    }

    var cms = new FoundationCms();
    cms.init();

    var commerce = new FoundationCommerce();
    commerce.init();

    var cmsPersonalization = new FoundationCmsPersonalization();
    cmsPersonalization.init();
});