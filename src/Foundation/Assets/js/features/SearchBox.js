class SearchBox {
    constructor() {
        this.ApiUrl = "https://eastus.api.cognitive.microsoft.com/vision/v1.0/analyze?visualFeatures=Description";
        this.AuthKey = "38192ad9dc5647d1b4d6328d420ac505";
        this.ImageSizeLimit = 5;
    }

    Init() {
        const inst = this;
        this.btn = document.getElementById("js-searchbutton");
        this.box = document.getElementById("js-searchbox");
        this.boxInput = document.getElementById("js-searchbox-input");
        if (this.box) {
            this.box.style.width = "80px";
            this.box.style.visibility = "hidden";
        }
        var typingTimer;

        $("#js-searchbutton").click(function () {
            inst.ExpandSearchBox();
        });
        $("#js-searchbox-close").click(function () {
            inst.CollapseSearchBox();
        });
        $(".jsSearchText").each(function (i, e) {
            inst.boxContent = $($(e).data('result-container'))[0];
            $(e).on("keyup", function () {
                clearTimeout(typingTimer);
                const val = $(this).val();
                const container = $(this).data('result-container');
                const divParent = "#" + $(this).parent().attr('id');
                typingTimer = setTimeout(function () {
                    inst.Search(val, divParent, container);
                }, 1000);
            });

            $(e).on('keypress',
                function (e) {
                    if (e.which == 13) {
                        const searchUrl = $(this).data('search');
                        const val = $(this).val();
                        var url = `${searchUrl}?search=${val}`;
                        if ($(this).attr('id') == 'js-searchbox-input') {
                            var confidence = $('#searchConfidence').val();
                            url += "&Confidence=" + confidence;
                        }

                        location.href = url;
                    }
                });
        });

        document.addEventListener("click", function (e) {
            if (inst.box && inst.boxContent && inst.btn) {
                if (inst.box.contains(e.target) || inst.btn.contains(e.target) || inst.boxContent.contains(e.target)) {
                    return;
                }

                inst.HidePopover();
                inst.CollapseSearchBox();
            }
        });

        inst.ProcessImage();
    }

    ExpandSearchBox() {
        this.btn.style.display = "none";
        this.box.style.width = "324px";
        this.box.style.visibility = "visible";
        this.boxInput.focus();
    }

    CollapseSearchBox() {
        this.btn.style.display = "flex";
        this.box.style.width = "80px";
        const inst = this;
        setTimeout(
            function () {
                inst.box.style.visibility = "hidden";
                inst.HidePopover();
            },
            200
        );
    }

    Search(val, divInputElement, containerPopover) {
        var waitTimer;
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

            this.ShowPopover(containerPopover);
        } else {
            this.HidePopover();
        }
    }

    ShowPopover(containerPopover) {
        $(containerPopover).show();
    }

    HidePopover() {
        $('.searchbox-popover').hide();
    }


    // Search Image
    
    ProcessImage() {
        var inst = this;
        $('.jsSearchImage').each(function (i, e) {
            var fileId = $(e).data('input');
            $(e).click(function () {
                $(fileId).click();
            });

            $(fileId).change(function () {
                try {
                    $('.loading-box').show();
                    var files = this.files;
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
            var regexForExtension = /(?:\.([^.]+))?$/;
            var extension = regexForExtension.exec(files[0].name)[1];
            var size = files[0].size / 1024 / 1024;
            if ((size > inst.ImageSizeLimit)) {
                errorMessage = "Image Size Should be lesser than " + inst.ImageSizeLimit + "MB";
                $(".validateErrorMsg").text(errorMessage).show();
                return false;
            } else if ((extension != "jpg" && extension != "png" && extension != "jpeg")) {
                errorMessage = "Uploaded File Should Be An Image";
                $(".validateErrorMsg").text(errorMessage).show();
                return false;
            }
            var reader = new FileReader();
            reader.onload = function () {
                inst.ProcessImage.imageData = reader.result;
                var arrayBuffer = this.result, array = new Uint8Array(arrayBuffer);
                if (typeof (inst.ProcessImage.imageData) == "undefined") {
                    errorMessage = "Upload File A Vaild Image";
                    $(".validateErrorMsg").text(errorMessage).show();
                }
                inst.ImageProcess(inst.ProcessImage.imageData);
            };
            reader.readAsDataURL(files[0]);
        }
    }

    ImageProcess(DataURL) {
        const inst = this;
        var request = new XMLHttpRequest();
        request.open('POST', inst.ApiUrl);
        request.setRequestHeader('Content-Type', 'application/octet-stream');
        request.setRequestHeader('Ocp-Apim-Subscription-Key', inst.AuthKey);
        request.onreadystatechange = function () {
            if (this.readyState === 4) {
                var result = JSON.parse(this.response);
                if (result.description) {
                    $('#searchConfidence').val(result.description.captions[0].confidence);
                    $('#js-searchbox-input').val(result.description.captions[0].text);
                    var e = $.Event("keypress", { which: 13 });
                    $('#js-searchbox-input').trigger(e);
                } else {
                    errorMessage = "Uploaded Image has been failed.";
                    $(".validateErrorMsg").text(errorMessage).show();
                    return false;
                }
            }
        };
        var body = {
            'image': DataURL,
            'locale': 'en_US'
        };
        var response = request.send(inst.Makeblob(DataURL));
    }

    Makeblob(dataURL) {
        var BASE64_MARKER = ';base64,';
        if (dataURL.indexOf(BASE64_MARKER) == -1) {
            var parts = dataURL.split(',');
            var contentType = parts[0].split(':')[1];
            var raw = decodeURIComponent(parts[1]);
            return new Blob([raw], { type: contentType });
        }
        var base64parts = dataURL.split(BASE64_MARKER);
        var base64contentType = base64parts[0].split(':')[1];
        var base64raw = window.atob(base64parts[1]);
        var base64rawLength = base64raw.length;

        var uInt8Array = new Uint8Array(base64rawLength);

        for (var i = 0; i < base64rawLength; ++i) {
            uInt8Array[i] = base64raw.charCodeAt(i);
        }

        return new Blob([uInt8Array], { type: base64contentType });
    }
}