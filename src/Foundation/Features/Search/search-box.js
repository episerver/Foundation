import { createPopper } from "@popperjs/core";

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

        if (document.querySelector("#js-searchbutton") != null) {
            document.querySelector("#js-searchbutton").addEventListener("click", function () {
                inst.expandSearchBox();
            });
        }

        if (document.querySelector("#js-searchbox-close") != null) {
            document.querySelector("#js-searchbox-close").addEventListener("click", function () {
                inst.collapseSearchBox();
            });
        }
        if (document.querySelector(".jsSearchText") != null) {
            Array.from(document.querySelectorAll(".jsSearchText")).forEach(function (el, i) {

                inst.boxContent = el.getAttribute('data-result-container');
                if (document.querySelector("#searchOption").value != "QuickSearch") {
                    inst.AutoSearch(el);
                    el.addEventListener("keyup", function () {
                        clearTimeout(typingTimer);
                        const val = el.value;
                        if (val != "") {
                            typingTimer = setTimeout(function () {
                                //let e = $.Event("keypress", { which: 13 });
                                var ev = new KeyboardEvent('keypress', { which: 13 });
                                el.dispatchEvent(ev);
                                //$('.js-searchbox-input').trigger(e);
                            }, 5000);
                        }
                    });
                } else {
                    el.addEventListener("keyup", function () {
                        clearTimeout(typingTimer);
                        const val = el.value;
                        const container = el.getAttribute('data-result-container');
                        const divParent = "#" + el.parentNode.getAttribute('id');
                        if (val != "") {
                            typingTimer = setTimeout(function () {
                                inst.Search(val, divParent, container);
                            }, 1000);
                        }
                    });
                }

                el.addEventListener('keypress',
                    function (e) {
                        if (e.which == 13) {
                            const searchUrl = el.getAttribute('data-search');
                            const val = el.value;
                            if (val != "") {
                                let url = `${searchUrl}?search=${val}`;
                                if (el.classList.contains('js-searchbox-input')) {
                                    let confidence = document.querySelector('#searchConfidence').value;
                                    url += "&Confidence=" + confidence;
                                }
                                location.href = url;
                            }
                        }
                    });
            });
        }
        document.addEventListener("click", function (e) {
            if (inst.box && inst.boxContent && inst.btn) {
                if (inst.box.contains(e.target) || inst.btn.contains(e.target) || document.querySelector(inst.boxContent).contains(e.target)) {
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
            document.querySelector(containerPopover + ' .loading-cart').style.display = "block";
        }, 500);
        const inst = this;
        if (val) {
            if (!this.desktop && containerPopover === '#jsResultSearch') {
                const reference = document.querySelector(divInputElement);
                const popover = document.querySelector(containerPopover);
                this.desktop = createPopper(reference, popover);
            } else if (!this.mobile && containerPopover === '#jsResultSearchMobile') {
                const reference = document.querySelector(divInputElement);
                const popover = document.querySelector(containerPopover);
                this.mobile = createPopper(reference, popover, {
                    modifiers: [{
                        name: 'preventOverflow', 
                        options: {
                            padding: 0,
                        },
                    },]
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
                    "search": val
                },
                {
                    cancelToken: new CancelToken(function (c) {
                        inst.cancel = c;
                    })
                })
                .then(function ({ data }) {
                    inst.searching = false;
                    document.querySelector(containerPopover).querySelector('.js-searchbox-content').innerHTML = data;
                    clearTimeout(waitTimer);
                    document.querySelector(containerPopover + ' .loading-cart').style.display = "none";
                })
                .catch(function (response) {
                    if (!axios.isCancel(response)) {
                        inst.searching = false;
                        clearTimeout(waitTimer);
                        document.querySelector(containerPopover + ' .loading-cart').style.display = "none";
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
                    console.log("keyword");

                    console.log(document.querySelector(e).getSelectedItemData().query)
                    let keyword = document.querySelector(e).getSelectedItemData().query;
                    document.querySelector(e).value = keyword;
                    var ev = new KeyboardEvent('keypress', { which: 13 });
                    e.dispatchEvent(ev);
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
        document.querySelector(containerPopover).style.display = "block";
    }

    hidePopover() {
        document.querySelector('.searchbox-popover').style.dsiplay = "none";
    }

    // Search Image
    ProcessImage() {
        let inst = this;
        Array.from(document.querySelectorAll(".jsSearchImage")).forEach(function (el, i) {
            //console.log("inside process image");
            let fileId = el.getAttribute('data-input');
            el.addEventListener("click", function () {
                console.log(fileId);
                document.querySelector(fileId).addEventListener("click", function (event) {
                    // If the clicked element doesn't have the right selector, bail
                    if (!event.target.matches('.click-me')) return;

                    // Don't follow the link
                    event.preventDefault();

                    // Log the clicked element in the console
                    console.log(event.target);

                }, false);
            });

            $(fileId).change(function () {
                try {
                    document.querySelector('.loading-box').style.display = "block";
                    let files = this.files;
                    document.querySelector(".validateErrorMsg").style.display = "none";
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