(function ($) {
    $.extend(true, epi.EPiServer.Forms, {
        // this dateFormats is for using in Samples only
        Samples: {
            // pickerFormat will be used for jQuery datepicker validation,
            // while friendlyFormat will show in validation message for user.
            DateFormats: {
                "en-us": {
                    "pickerFormat": "mm/dd/yy",
                    "friendlyFormat": "MM/dd/yyyy"
                },
                "sv-se": {
                    "pickerFormat": "yy-mm-dd",
                    "friendlyFormat": "yyyy-MM-dd"
                },
                "nb-no": {
                    "pickerFormat": "dd.mm.yy",
                    "friendlyFormat": "dd.MM.yyyy"
                },
                "da-dk": {
                    "pickerFormat": "dd-mm-yy",
                    "friendlyFormat": "dd-MM-yyyy"
                },
                "de-de": {
                    "pickerFormat": "dd.mm.yy",
                    "friendlyFormat": "dd.MM.yyyy"
                },
                "nl-nl": {
                    "pickerFormat": "d-m-yy",
                    "friendlyFormat": "d-M-yyyy"
                },
                "fi-fi": {
                    "pickerFormat": "d.m.yy",
                    "friendlyFormat": "d.M.yyyy"
                },

                "fr-fr": {
                    "pickerFormat": "dd/mm/yy",
                    "friendlyFormat": "dd/MM/yyyy"
                }
            }
        }
    });

    var _utilsSvc = epi.EPiServer.Forms.Utils,
        originalGetCustomElementValue = epi.EPiServer.Forms.Extension.getCustomElementValue,
        originalBindCustomElementValue = epi.EPiServer.Forms.Extension.bindCustomElementValue,
        dateTimePickerTypes = {
            datePicker: "datepicker",
            timePicker: "timepicker",
            dateTimePicker: "datetimepicker"
        },

        language = navigator.language || navigator.userLanguage, // on iOS naviagtor.language is in lower case (ex: en-us)
        dateFormatSettings = epi.EPiServer.Forms.Samples.DateFormats[language.toLowerCase()] || epi.EPiServer.Forms.Samples.DateFormats["en-us"],
        dateFormat = dateFormatSettings.pickerFormat,


        dateTimeValidate = function (fieldName, fieldValue, validatorMetaData) {
            // this function is used to validate visitor's data value in ViewMode

            // the fieldValue return by get getCustomElementValue already contains validate information,
            // just check fieldValue.isValid and return along with error message

            if (typeof fieldValue === 'object' && fieldValue.isValid == false) {

                var friendlyFormat = dateFormatSettings.friendlyFormat,
                    message = _utilsSvc.stringFormat(validatorMetaData.model.message, [friendlyFormat]);

                return { isValid: false, message: message };
            }

            return { isValid: true };
        },

        dateTimeRangeValidate = function (fieldName, fieldValue, validatorMetaData) {
            // this function is used to validate visitor's data value in ViewMode

            // the fieldValue return by get getCustomElementValue already contains validate information,
            // just check fieldValue.isValid and return along with error message
            if (typeof fieldValue === 'object' && fieldValue.isValid == false) {
                return { isValid: false, message: validatorMetaData.model.message };
            }
            // if value is empty, then let the required validator take the decision
            if (fieldValue == "") {
                return { isValid: true };
            }

            var values = fieldValue.split('|');
            if (!values || values.length < 2) {
                return { isValid: false };
            }

            var startDateTimeString = values[0],
                endDateTimeString = values[1];
            var startDate = Date.parse(startDateTimeString);
            var endDate = Date.parse(endDateTimeString);
            if (!startDate || !endDate || startDate >= endDate) {
                return { isValid: false, message: validatorMetaData.model.message };
            }

            return { isValid: true };
        };

    addressesValidate = function validateAddress(fieldName, fieldValue, validatorMetaData ) {
        var validateEnpoint = '/ExternalValidate/ValidateAddress';
        var validateResult = { isValid: false };
        $.ajax({
            url: validateEnpoint,
            type: "POST",
            async: false,
            data: JSON.parse(fieldValue),
            dataType: "json",
            success: function (valid) {
                validateResult.isValid = valid;
                if (!validateResult.isValid) validateResult.message = validatorMetaData.model.message;
            },
            error: function () {
                validateResult.isValid = false;
            }
        });
        return validateResult;
    }


    // extend the EpiForm JavaScript API in ViewMode
    $.extend(true, epi.EPiServer.Forms, {
        /// extend the Validator to validate Visitor's value in Clientside.
        /// Serverside's Fullname of the Validator instance is used as object key (Case-sensitve) to lookup the Clientside validate function.        
        Validators: {
            "EPiServer.Forms.Samples.Implementation.Validation.AddressValidator": addressesValidate,
            "EPiServer.Forms.Samples.Implementation.Validation.DateTimeRangeValidator": dateTimeRangeValidate,
            "EPiServer.Forms.Samples.Implementation.Validation.DateTimeValidator": dateTimeValidate,
            "EPiServer.Forms.Samples.Implementation.Validation.DateValidator": dateTimeValidate,
            "EPiServer.Forms.Samples.Implementation.Validation.TimeValidator": dateTimeValidate,
            "EPiServer.Forms.Samples.Implementation.Validation.RecaptchaValidator": function (fieldName, fieldValue, validatorMetaData) {
                // validate recaptcha element
                if (fieldValue) {
                    return { isValid: true };
                }

                return { isValid: false, message: validatorMetaData.model.message };
            }
        },

        /// BETA
        /// value store in localStorage and value displaying for Visitor can be different. This function return the value for display to Visitor.
        CustomBindingElements: {
            "EPiServer.Forms.Samples.Implementation.Elements.DateTimeElementBlock": function (elementInfo, val) {

                // Datetime with format YYYY-MM-DDTHH:mmTZD(ISO-8601) "2018-08-07T00:00+07:00"
                if (!val || $.type(val) != "string" || !Date.parse(val)) {
                    return;
                }

                var picker = { settings: { dateFormat: dateFormat } },
                    dateTime = new Date(val),
                    dateString = $.datetimepicker.formatDate(picker.settings.dateFormat, dateTime),
                    //we only support time with AM/PM -> 'en-US' is used.
                    timeString = dateTime.toLocaleString('en-US', { hour: '2-digit', minute: '2-digit', hour12: true }),  
                    timeStringWithLeadingZero = ("0" + timeString).slice(-8); // with format "hh:mm tt", toLocaleString return hour without leading zero


                switch (elementInfo.pickerType) {
                    case dateTimePickerTypes.datePicker:
                        return dateString;
                    case dateTimePickerTypes.dateTimePicker:
                        return dateString + " " + timeStringWithLeadingZero;
                    case dateTimePickerTypes.timePicker:
                        return timeStringWithLeadingZero;
                }
            },

            "EPiServer.Forms.Samples.Implementation.Elements.DateTimeRangeElementBlock": function (elementInfo, val) {
                if (!val || $.type(val) != "string") {
                    return;
                }
                var dateTimes = val.split("|");
                if (!dateTimes || dateTimes.length != 2) {
                    return;
                }
                var startDate = dateTimes[0],
                    endDate = dateTimes[1];
                var startDateString = this["EPiServer.Forms.Samples.Implementation.Elements.DateTimeElementBlock"](elementInfo, startDate);
                var endDateString = this["EPiServer.Forms.Samples.Implementation.Elements.DateTimeElementBlock"](elementInfo, endDate);
                return startDateString + ' : ' + endDateString;
            },

            "EPiServer.Forms.Samples.Implementation.Elements.AddressesElementBlock": function (elementInfo, val) {
                if (!val) {
                    return;
                }
                var locationObj = JSON.parse(val);
                var locationString = locationObj.address;
                if (locationObj.street) {
                    locationString += ', ' + locationObj.street;
                }
                if (locationObj.city) {
                    locationString += ', ' + locationObj.city;
                }
                if (locationObj.state) {
                    locationString += ', ' + locationObj.state;
                }
                if (locationObj.postalCode) {
                    locationString += ', ' + locationObj.postalCode;
                }
                if (locationObj.country) {
                    locationString += ', ' + locationObj.country;
                }
                return locationString;
            }
        },

        Extension: {

            // OVERRIDE, this function to show jQuery Dialog as Summarized Dialog before submission
            showSummarizedText: function (data, workingFormInfo, ignoreFields, shouldBeHiddenKeys) {
                // workingFormInfo.ElementsInfo: contains friendlyName for each field code
                // ignoreFields: are (System) fields should be ignored
                // shouldBeHiddenKeys: are fields which should not be shown (hidden field, tracking code, ...)
                // to determine field should be display or not, check $("[name=" + key + "]", workingFormInfo.$workingForm).hasClass("FormHideInSummarized"))

                var $deferred = $.Deferred(),
                    summarizedText = this.getSummarizedText(workingFormInfo, data, true);
                // empty text, no need to show the confirm box
                if (!summarizedText || summarizedText.trim() === "") {
                    $deferred.resolve(true);

                    return $deferred.promise();
                }

                var confirmationResources = epi.EPiServer.Forms.LocalizedResources.samples.confirmationdialog;

                $("<div class=\"Form__ConfirmationDialog\"></div>").html(summarizedText).appendTo(workingFormInfo.$workingForm);

                $(".Form__ConfirmationDialog", workingFormInfo.$workingForm).dialog({
                    title: confirmationResources.title,
                    resizable: false,
                    modal: true,
                    buttons: [
                        {
                            text: confirmationResources.buttons.ok,
                            click: function () {
                                $(this).dialog("close");
                                $deferred.resolve(true);
                            }
                        },
                        {
                            text: confirmationResources.buttons.cancel,
                            click: function () {
                                $(this).dialog("close");
                                $deferred.resolve(false);
                            }
                        }
                    ]
                });

                return $deferred.promise();
            },

            // OVERRIDE, process to get value from datetime picker element
            getCustomElementValue: function ($element) {

                // for datetime picker we always return result in format "2015-12-25 10:30 AM",
                // and depend on picker type (date/time/datetime) we will parse its value later.
                if ($element.hasClass("FormDateTime")) {
                    var $input = $('.FormDateTime__Input', $element);
                    return this.getCustomDateTimeElementValue($input);
                } else if ($element.hasClass("FormRecaptcha")) {
                    // for recaptcha element
                    var widgetId = $element.data("epiforms-recaptcha-widgetid");
                    if (widgetId != undefined && grecaptcha) {
                        return grecaptcha.getResponse(widgetId);
                    } else {
                        return null;
                    }
                }
                else if ($element.hasClass("FormDateTimeRange")) {
                    var $startInput = $('.FormDateTimeRange__Start', $element),
                        $endInput = $('.FormDateTimeRange__End', $element);
                    var startDateTime = this.getCustomDateTimeElementValue($startInput);
                    var endDateTime = this.getCustomDateTimeElementValue($endInput);
                    if (startDateTime.isValid == false || endDateTime.isValid == false) {
                        return { isValid: false };
                    }

                    // if only startDate or endDate exists, then return invalid
                    if (!((startDateTime && endDateTime) || (!startDateTime && !endDateTime))) {
                        return { isValid: false };
                    }
                    if (!startDateTime && !endDateTime) {
                        return '';
                    }
                    return startDateTime + '|' + endDateTime;
                }

                else if ($element.hasClass("FormAddressElement")) {

                    var address = $('.FormAddressElement__Address', $element).first().val(),
                        country = $('.FormAddressElement__Country', $element).first().val(),
                        state = $('.FormAddressElement__State', $element).first().val(),
                        city = $('.FormAddressElement__Locality', $element).first().val(),
                        postalCode = $('.FormAddressElement__ZipCode', $element).first().val(),
                        street = $('.FormAddressElement__Route', $element).first().val();

                    return JSON.stringify({
                        address: address,
                        street: street,
                        city: city,
                        state: state,
                        postalCode: postalCode,
                        country: country
                    });
                }

                // if current element is not our job, let others process
                return originalGetCustomElementValue.apply(this, [$element]);
            },

            getCustomDateTimeElementValue: function ($element) {
                var dateTimeString = $.trim($element.val()),
                        picker = $.datetimepicker._getInst($element[0]),
                        timeRegex = "^(0?[1-9]|1[012])(:[0-5]\\d) [APap][mM]$";

                if (!dateTimeString) {
                    return dateTimeString;
                }

                switch (picker.settings.type) {

                    case dateTimePickerTypes.datePicker:
                    case dateTimePickerTypes.dateTimePicker:
                        var dateTime;
                        try {
                            // try to parse date with format
                            dateTime = $.datetimepicker.parseDate(picker.settings.dateFormat, dateTimeString);
                        } catch (err) {
                            return { isValid: false };
                        }
                        var result = _utilsSvc.stringFormat("{0}-{1}-{2}", [dateTime.getFullYear(), this.addLeadingZero(dateTime.getMonth() + 1), this.addLeadingZero(dateTime.getDate())]);
                        if (picker.settings.type == dateTimePickerTypes.dateTimePicker) {
                            var dateTimeSegments = dateTimeString.split(" ");

                            if (dateTimeSegments.length < 3) {
                                return { isValid: false };
                            }

                            var timeString = dateTimeSegments[1] + " " + dateTimeSegments[2];
                            if (!_utilsSvc.isMatchedReg(timeString, timeRegex)) {
                                return { isValid: false };
                            }

                            result += " " + timeString;
                        } else {
                            result += " 12:00 AM"; // add fake time string into return results, this will be ignored when rebind data
                        }
                        return this.toISODateTimeString(result);

                    case dateTimePickerTypes.timePicker:
                        if (!_utilsSvc.isMatchedReg(dateTimeString, timeRegex)) {
                            return { isValid: false };
                        }
                        var yearString = new Date().getFullYear();
                        return this.toISODateTimeString(yearString + "-01-01 " + dateTimeString); // add fake date string into return results, this will be ignored when rebind data
                }

                return null;
            },

            // convert datetime format YYYY-MM-DD hh:mm tt to format YYYY-MM-DDTHH:mmTZD(ISO-8601)
            // https://www.w3.org/TR/NOTE-datetime
            toISODateTimeString: function (dateTimeString) {

                if (!dateTimeString) {
                    return null;
                }

                var dateTimeSegments = dateTimeString.split(" ");
                if (dateTimeSegments.length < 3) {
                    return null;
                }

                var dateRegex = "^\\d{4}-(0?[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])";
                var dateString = dateTimeSegments[0];
                if (!_utilsSvc.isMatchedReg(dateString, dateRegex)) {
                    return null;
                }

                var timeRegex = "^(0?[1-9]|1[012])(:[0-5]\\d) [APap][mM]$";
                var timeString = dateTimeSegments[1] + " " + dateTimeSegments[2];
                if (!_utilsSvc.isMatchedReg(timeString, timeRegex)) {
                    return null;
                }

                return dateString + "T" + this.convertTo24Hour(timeString) + this.getTimeZoneDesignator(dateString);
            },

            // convert 12h base to 24h base
            convertTo24Hour: function (time) {
                time = time.toLowerCase();
                var hours = parseInt(time.substr(0, 2));
                if (time.indexOf("am") !== -1 && hours === 12) {
                    time = time.replace("12", "00");
                }
                if (time.indexOf("pm") !== -1 && hours < 12) {
                    time = time.replace(this.addLeadingZero(hours), (hours + 12));
                }
                return time.replace(/(am|pm)/, "").trim();
            },

            // get time zone designator TZD based on TimezoneOffset
            // Firefox and IE don't understand YYYY-MM-DD hh:mm tt format, we should use dateString only (ex:2018-01-01)
            getTimeZoneDesignator: function (dateString) {
                // note: on Chrome getTimeZoneDesignator return different result for different years (2018,1970,1900)
                // ref: https://stackoverflow.com/questions/50609860/browsers-time-zones-chrome-67-error
                var timeZoneOffset = -(new Date(dateString)).getTimezoneOffset(),
                    dif = timeZoneOffset >= 0 ? "+" : "-";

                timeZoneOffset = Math.abs(timeZoneOffset);

                var offsetMinutes = timeZoneOffset % 60;
                var offsetHours = (timeZoneOffset - offsetMinutes) / 60;

                return dif + this.addLeadingZero(offsetHours) + ":" + this.addLeadingZero(offsetMinutes);
            },

            // add leading zero to number < 10 (Ex: 9 -> 09, 7-> 07)
            addLeadingZero: function (number) {
                if (number < 10) {
                    return "0" + number;
                }
                return number;
            },

            // OVERRIDE, custom binding data for date/time/datetime picker and date-time-range picker
            bindCustomElementValue: function ($element, val) {
                if ($element.hasClass('FormDateTimeRange')) {
                    // skip binding value for DateTimeRange when it has invalid value
                    if (typeof val != "string") {
                        return;
                    }
                    var values = val.split('|');
                    if (values && values.length < 2) {
                        return;
                    }

                    var $startEl = $element.find(".FormDateTimeRange__Start");
                    var $endEl = $element.find(".FormDateTimeRange__End");
                    var startPicker = $.datetimepicker._getInst($startEl[0]),
                        startBindingValue = epi.EPiServer.Forms.CustomBindingElements["EPiServer.Forms.Samples.Implementation.Elements.DateTimeElementBlock"]({ pickerType: startPicker.settings.type }, values[0]);
                    var endPicker = $.datetimepicker._getInst($endEl[0]),
                        endBindingValue = epi.EPiServer.Forms.CustomBindingElements["EPiServer.Forms.Samples.Implementation.Elements.DateTimeElementBlock"]({ pickerType: endPicker.settings.type }, values[1]);
                    $startEl.val(startBindingValue);
                    $endEl.val(endBindingValue);
                    return;
                }
                else if ($element.hasClass('FormDateTime')) {
                    var $item = $element.find(".Form__CustomInput");
                    if ($item.hasClass("FormDateTime__Input")) {

                        var picker = $.datetimepicker._getInst($item[0]),
                            bindingValue = epi.EPiServer.Forms.CustomBindingElements["EPiServer.Forms.Samples.Implementation.Elements.DateTimeElementBlock"]({ pickerType: picker.settings.type }, val);
                        $item.val(bindingValue);
                        return;
                    }
                }

                else if ($element.hasClass('FormAddressElement')) {
                    var $addressEl = $element.find(".FormAddressElement__Address");
                    var $countryEl = $element.find(".FormAddressElement__Country");
                    var $stateEl = $element.find(".FormAddressElement__State");
                    var $cityEl = $element.find(".FormAddressElement__Locality");
                    var $routeEl = $element.find(".FormAddressElement__Route");
                    var $zipEl = $element.find(".FormAddressElement__ZipCode");
                    var addressInfo = JSON.parse(val);
                    $countryEl.val(addressInfo.country);
                    $zipEl.val(addressInfo.postalCode);
                    $stateEl.val(addressInfo.state);
                    $cityEl.val(addressInfo.city);
                    $routeEl.val(addressInfo.street);
                    $addressEl.val(addressInfo.address);
                    return;
                }

                // if current element is not our job, let others process
                return originalBindCustomElementValue.apply(this, [$element, val]);
            }
        }
    });

})($$epiforms || $);