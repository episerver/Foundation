(function ($) {
    var _utilsSvc = epi.EPiServer.Forms.Utils,
        language = navigator.language || navigator.userLanguage, // on iOS naviagtor.language is in lower case (ex: en-us)
        dateFormatSettings = epi.EPiServer.Forms.Samples.DateFormats[language.toLowerCase()] || epi.EPiServer.Forms.Samples.DateFormats["en-us"],
        dateFormat = dateFormatSettings.pickerFormat;

    // init all DateTimeFormElement on the ViewMode with our modified jQuery DatePicker
    if (typeof __SamplesDateTimeElements != "undefined") {
        $.each(__SamplesDateTimeElements || [], function (i, dateTimeElementInfo) {

            var element = $("#" + dateTimeElementInfo.guid);

            // setup picker
            element.datetimepicker({
                type: dateTimeElementInfo.pickerType,
                constrainInput: false,
                dateFormat: dateFormat
            });

            // format datetime value to match with current culture 
            formatDateTimeValue(element, dateTimeElementInfo.pickerType);
        });

        function formatDateTimeValue(element, pickerType) {
            // reformat auto fill value
            var elementValue = element.val();
            if (elementValue) {
                switch (pickerType) {
                    case "datepicker":
                        element.datetimepicker('setDate', new Date(elementValue)); // this will set date only
                        break;
                    case "datetimepicker":
                        var dateTimeSegments = elementValue.split(" ");

                        if (dateTimeSegments.length === 3) {
                            element.datetimepicker('setDate', new Date(dateTimeSegments[0])); // this will set date only
                            element.val(element.val() + " " + dateTimeSegments[1] + " " + dateTimeSegments[2]);
                        }
                        break;
                }
            }
        }

        $('.EPiServerForms .Form__CustomInput.FormDateTime__Input, .EPiServerForms .Form__CustomInput.FormDateTimeRange__End, .EPiServerForms .Form__CustomInput.FormDateTimeRange__Start').on('keydown', function _onKeyDown(e) {
            return _utilsSvc.showNextStepOnEnterKeyDown(e);
        });
    }

})($$epiforms || $);