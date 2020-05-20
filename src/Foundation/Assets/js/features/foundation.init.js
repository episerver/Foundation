$(document).ready(function () {

    // convert json to formdata and append __RequestVerificationToken key for CORS
    window.convertFormData = function (data, containerToken) {
        var formData = new FormData();
        var isAddedToken = false;
        for (var key in data) {
            if (key == "__RequestVerificationToken") {
                isAddedToken = true;
            }
            formData.append(key, data[key]);
        }

        if (!isAddedToken) {
            if (containerToken) {
                formData.append("__RequestVerificationToken", $(containerToken + ' input[name=__RequestVerificationToken]').val());
            } else {
                formData.append("__RequestVerificationToken", $('input[name=__RequestVerificationToken]').val());
            }
        }

        return formData;
    };

    window.serializeObject = function (form) {
        var datas = $(form).serializeArray();
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