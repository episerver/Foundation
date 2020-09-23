import Popper from "popper.js";

export default class SearchBox {
    constructor() {
        this.apiUrl = "https://eastus.api.cognitive.microsoft.com/vision/v1.0/analyze?visualFeatures=Description";
        this.authKey = "38192ad9dc5647d1b4d6328d420ac505";
        this.imageSizeLimit = 5;
    }

    init() {
        const inst = this;
        this.btn = document.getElementById("js-searchbutton");
        this.box = document.getElementById("js-searchbox");
        this.boxInput = document.getElementsByClassName("js-searchbox-input");
        if (this.box) {
            this.box.style.width = "80px";
            this.box.style.visibility = "hidden";
        }
        let typingTimer;


        $("#js-searchbutton").click(function () {
            inst.expandSearchBox();
        });
        $("#js-searchbox-close").click(function () {
            inst.collapseSearchBox();
        });
        $(".jsSearchText").each(function (i, e) {

            inst.boxContent = $($(e).data('result-container'))[0];
            if ($("#searchOption").val() != "QuickSearch") {
                inst.AutoSearch(e);
                $(e).on("keyup", function () {
                    clearTimeout(typingTimer);
                    const val = $(this).val();
                    if(val != ""){
                        typingTimer = setTimeout(function () {
                            let e = $.Event("keypress", { which: 13 });
                            $('.js-searchbox-input').trigger(e);
                        }, 5000);
                    }
                });
            } else {
                $(e).on("keyup", function () {
                    clearTimeout(typingTimer);
                    const val = $(this).val();
                    const container = $(this).data('result-container');
                    const divParent = "#" + $(this).parent().attr('id');
                    if(val != ""){
                        typingTimer = setTimeout(function () {
                            inst.Search(val, divParent, container);
                        }, 1000);
                    }
                });
            }

            $(e).on('keypress',
                function (e) {
                    if (e.which == 13) {
                        const searchUrl = $(this).data('search');
                        const val = $(this).val();
                        if(val != ""){
                            let url = `${searchUrl}?search=${val}`;
                            if ($(this).attr('id') == 'js-searchbox-input') {
                                let confidence = $('#searchConfidence').val();
                                url += "&Confidence=" + confidence;
                            }
                            location.href = url;
                        }
                    }
                });
        });

        document.addEventListener("click", function (e) {
            if (inst.box && inst.boxContent && inst.btn) {
                if (inst.box.contains(e.target) || inst.btn.contains(e.target) || inst.boxContent.contains(e.target)) {
                    return;
                }

                inst.hidePopover();
                inst.collapseSearchBox();
            }
        });

        inst.ProcessImage();
    }

    expandSearchBox() {
        this.btn.style.display = "none";
        this.box.style.width = "324px";
        this.box.style.visibility = "visible";
        // this.boxInput.focus();
    }

    collapseSearchBox() {
        this.btn.style.display = "flex";
        this.box.style.width = "80px";
        const inst = this;
        setTimeout(
            function () {
                inst.box.style.visibility = "hidden";
                inst.hidePopover();
            },
            200
        );
    }

    Search(val, divInputElement, containerPopover) {
        let waitTimer;
        clearTimeout(waitTimer);
        waitTimer = setTimeout(function () {
            $(containerPopover + ' .loading-cart').show();
        }, 500);
        const inst = this;
        if (val) {
            if (!this.desktop && containerPopover === '#jsResultSearch') {
                const reference = $(divInputElement);
                const popover = $(containerPopover);
                this.desktop = new Popper(reference, popover);
            } else if (!this.mobile && containerPopover === '#jsResultSearchMobile') {
                const reference = $(divInputElement);
                const popover = $(containerPopover);
                this.mobile = new Popper(reference, popover, {
                    modifiers: {
                        preventOverflow: {
                            padding: 0
                        }
                    }
                });
            } else if (this.desktop) {
                this.desktop.update();
            } else if (this.mobile) {
                this.mobile.update();
            }

            if (inst.searching) {
                inst.cancel();
            }

            inst.searching = true;
            const CancelToken = axios.CancelToken;

            axios.post(
                "/Search/QuickSearch",
                {
                    search: val
                },
                {
                    cancelToken: new CancelToken(function (c) {
                        inst.cancel = c;
                    })
                })
                .then(function ({ data }) {
                    inst.searching = false;
                    $(containerPopover).find('.js-searchbox-content').first().html(data);
                    clearTimeout(waitTimer);
                    $(containerPopover + ' .loading-cart').hide();
                })
                .catch(function (response) {
                    if (!axios.isCancel(response)) {
                        inst.searching = false;
                        clearTimeout(waitTimer);
                        $(containerPopover + ' .loading-cart').hide();
                    }
                });

            this.showPopover(containerPopover);
        } else {
            this.hidePopover();
        }
    }
   
    AutoSearch(e) {
        let options = {
            url: function (phrase) {
                return "/find_v2/_autocomplete?prefix=" + phrase;
            },
            requestDelay: 500,
            list: {
                match: {
                    enabled: false
                },
                onChooseEvent: function () {
                    let keyword = $(e).getSelectedItemData().query;
                    $(e).val(keyword);
                    let e = $.Event("keypress", { which: 13 });
                    $(e).trigger(e);
                }
            },
            listLocation: "hits",
            getValue: "query",
            template: {
                type: "custom",
                method: function (value, item) {
                    return value;
                }
            },
            adjustWidth: false
        };
        $(e).easyAutocomplete(options);
    }

    showPopover(containerPopover) {
        $(containerPopover).show();
    }

    hidePopover() {
        $('.searchbox-popover').hide();
    }

    // Search Image
    ProcessImage() {
        let inst = this;
        $('.jsSearchImage').each(function (i, e) {
            let fileId = $(e).data('input');
            $(e).click(function () {
                $(fileId).click();
            });

            $(fileId).change(function () {
                try {
                    $('.loading-box').show();
                    let files = this.files;
                    $(".validateErrorMsg").hide();
                    inst.InputValidation(files);
                } catch (e) {
                    console.log(e);
                }
            });
        });
    }

    InputValidation(files) {
        const inst = this;
        if (files.length == 1) {
            let regexForExtension = /(?:\.([^.]+))?$/;
            let extension = regexForExtension.exec(files[0].name)[1];
            let size = files[0].size / 1024 / 1024;
            if ((size > inst.imageSizeLimit)) {
                errorMessage = "Image Size Should be lesser than " + inst.imageSizeLimit + "MB";
                $(".validateErrorMsg").text(errorMessage).show();
                return false;
            } else if ((extension != "jpg" && extension != "png" && extension != "jpeg")) {
                errorMessage = "Uploaded File Should Be An Image";
                $(".validateErrorMsg").text(errorMessage).show();
                return false;
            }
            let reader = new FileReader();
            reader.onload = function () {
                inst.ProcessImage.imageData = reader.result;
                let arrayBuffer = this.result, array = new Uint8Array(arrayBuffer);
                if (typeof (inst.ProcessImage.imageData) == "undefined") {
                    errorMessage = "Upload File A Vaild Image";
                    $(".validateErrorMsg").text(errorMessage).show();
                }
                inst.imageProcess(inst.ProcessImage.imageData);
            };
            reader.readAsDataURL(files[0]);
        }
    }

    imageProcess(DataURL) {
        const inst = this;
        let request = new XMLHttpRequest();
        request.open('POST', inst.apiUrl);
        request.setRequestHeader('Content-Type', 'application/octet-stream');
        request.setRequestHeader('Ocp-Apim-Subscription-Key', inst.authKey);
        request.onreadystatechange = function () {
            if (this.readyState === 4) {
                let result = JSON.parse(this.response);
                if (result.description) {
                    $('#searchConfidence').val(result.description.captions[0].confidence);
                    $('#js-searchbox-input').val(result.description.captions[0].text);
                    let e = $.Event("keypress", { which: 13 });
                    $('#js-searchbox-input').trigger(e);
                } else {
                    errorMessage = "Uploaded Image has been failed.";
                    $(".validateErrorMsg").text(errorMessage).show();
                    return false;
                }
            }
        };
        let body = {
            'image': DataURL,
            'locale': 'en_US'
        };
        let response = request.send(inst.makeblob(DataURL));
    }

    makeblob(dataURL) {
        let BASE64_MARKER = ';base64,';
        if (dataURL.indexOf(BASE64_MARKER) == -1) {
            let parts = dataURL.split(',');
            let contentType = parts[0].split(':')[1];
            let raw = decodeURIComponent(parts[1]);
            return new Blob([raw], { type: contentType });
        }
        let base64parts = dataURL.split(BASE64_MARKER);
        let base64contentType = base64parts[0].split(':')[1];
        let base64raw = window.atob(base64parts[1]);
        let base64rawLength = base64raw.length;

        let uInt8Array = new Uint8Array(base64rawLength);

        for (let i = 0; i < base64rawLength; ++i) {
            uInt8Array[i] = base64raw.charCodeAt(i);
        }

        return new Blob([uInt8Array], { type: base64contentType });
    }
}