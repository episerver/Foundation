(function ($) {
    var _utilsSvc = epi.EPiServer.Forms.Utils;
    $(".EPiServerForms").on("formsReset formsNavigationPrevStep", function (event) {
        resetRecaptchaElements(event.target);
    });

    // reset reCAPTCH elements in target form
    function resetRecaptchaElements(target) {

        var reCaptchaElements = $(".FormRecaptcha", target);
        $.each(reCaptchaElements, function (index, element) {
            var widgetId = $(element).data("epiforms-recaptcha-widgetid");
            if (widgetId != undefined && grecaptcha) {
                grecaptcha.reset(widgetId);
            }
        });
    };

})($$epiforms || $);

function initRecaptchaElements() {
    (function ($) {

        //This is the callback function executed after Google authenticates recaptcha values.
        function onVerify($element) {
            return function () {
                return function (response) {
                    if (!response || response.length == 0) {
                        return;
                    };

                    $element.find(".Form__Element__ValidationError").hide();
                }
            }($element);
        };

        $(".Form__Element.FormRecaptcha").each(function (index, element) {
            var $element = $(element),
                $widgetContainer = $(".g-recaptcha", $element),
                siteKey = $element.data("epiforms-sitekey");

            if ($widgetContainer.length == 1 && siteKey) {
                var widgetId = grecaptcha.render($widgetContainer[0], { sitekey: siteKey, callback: onVerify($element) });
                $element.data("epiforms-recaptcha-widgetid", widgetId);
            }
        });

    })($$epiforms || $);
}