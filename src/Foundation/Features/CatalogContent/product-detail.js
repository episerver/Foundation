import feather from "feather-icons";
import Dropdown from "../../wwwroot/js/common/dropdown";
import Product from "../../Features/CatalogContent/product";
require("jquery-zoom");

export default class ProductDetail {
    constructor(divContainerId) {
        if (divContainerId) {
            this.divContainerId = divContainerId;
        } else {
            this.divContainerId = document;
        }
    }

    quickView(code, productCode, url) {
        let inst = this;
        $(inst.divContainerId).find('.loading-box').show();
        axios.get(url, { params: { productCode: productCode, variantCode: code } })
            .then(function (result) {
                if (result.status == 200) {
                    $('#quickView .modal-body').html(result.data);
                    $('#quickView .modal-body').off();
                    $(inst.divContainerId).find("#productCode").val(productCode);
                    feather.replace();
                    let dropdown = new Dropdown("#quickView");
                    dropdown.init();
                    let product = new Product('#quickView');
                    product.addToCartClick();
                    product.addToWishlistClick();
                    product.addToSharedCartClick();

                    inst.inStorePickupClick();
                    inst.selectStoreClick();
                    inst.selectColorSizeClick();
                    inst.zoomImage();
                }
            })
            .catch(function (error) {
                notification.error(error);
                $('#quickView .modal-body').html('');
            })
            .finally(function () {
                $(inst.divContainerId).find('.loading-box').hide();
            });
    }

    changeVariant(data, callback) {
        let inst = this;
        document.querySelector('.loading-box').style.display = "block";
        axios.get('/product/selectVariant', { params: data })
            .then(function (result) {
                if (!callback) {
                    if (result.status == 200) {
                        $(inst.divContainerId).find('.modal-body').html(result.data);
                        $(inst.divContainerId).find('.modal-body').off();
                        let dropdown = new Dropdown(inst.divContainerId);
                        dropdown.init();
                        let product = new Product(inst.divContainerId);
                        product.addToCartClick();
                        product.addToWishlistClick();

                        inst.inStorePickupClick();
                        inst.selectStoreClick();
                        inst.selectColorSizeClick();
                        feather.replace();
                    }
                } else {
                    callback(result);
                    inst.selectColorSizeClick(false, callback);
                }
            })
            .catch(function (error) {
                notification.error(error);
                $('#quickView .modal-body').html('');
            })
            .finally(function () {
                document.querySelector('.loading-box').style.display = "none";
            });
    }

    inStorePickupClick() {
        let inst = this;
        Array.from(document.querySelectorAll(".jsSelectDelivery")).forEach(function (el, i) {
            el.addEventListener("click", function () {
                //let valueChecked = el.closest('input').firstChild().value;
                //$(inst.divContainerId).find('.addToCart').attr('store', valueChecked);
                //$(inst.divContainerId).find('.jsBuyNow').attr('store', valueChecked);
                //if (valueChecked === 'instore') {
                //    let selectedStore = $(inst.divContainerId).find('#selectedStore').val();
                //    $(inst.divContainerId).find('.addToCart').attr('selectedStore', selectedStore);
                //    $(inst.divContainerId).find('.jsBuyNow').attr('selectedStore', selectedStore);
                //    if (!$(inst.divContainerId).find('#pickupStoreBox').is(':visible')) {
                //        $(inst.divContainerId).find('#pickupStoreBox').fadeToggle();
                //    }
                //} else {
                //    $(inst.divContainerId).find('.addToCart').attr('selectedStore', '');
                //    $(inst.divContainerId).find('.jsBuyNow').attr('selectedStore', '');
                //    if ($(inst.divContainerId).find('#pickupStoreBox').is(':visible')) {
                //        $(inst.divContainerId).find('#pickupStoreBox').fadeOut(300);
                //    }
                //}
                console.log(el.querySelector('input').value);
                let valueChecked = el.querySelector('input').value;
                
                document.querySelector(inst.divContainerId).querySelector('.addToCart').setAttribute('store', valueChecked);
                if (document.querySelector(inst.divContainerId).querySelector('.jsBuyNow') != null)
                    document.querySelector(inst.divContainerId).querySelector('.jsBuyNow').setAttribute('store', valueChecked);
                if (valueChecked === 'instore') {
                    let selectedStore = document.querySelector(inst.divContainerId).querySelector('#selectedStore').value;
                    document.querySelector(inst.divContainerId).querySelector('.addToCart').setAttribute('selectedStore', selectedStore);
                    if (document.querySelector(inst.divContainerId).querySelector('.jsBuyNow') != null)
                        document.querySelector(inst.divContainerId).querySelector('.jsBuyNow').setAttribute('selectedStore', selectedStore);
                    if (!$(inst.divContainerId).find('#pickupStoreBox').is(':visible')) {
                        $(inst.divContainerId).find('#pickupStoreBox').fadeToggle(); //need to rewrite in javascript
                    }
                } else {
                    document.querySelector(inst.divContainerId).querySelector('.addToCart').setAttribute('selectedStore', "");
                    if (document.querySelector(inst.divContainerId).querySelector('.jsBuyNow') != null)
                        document.querySelector(inst.divContainerId).querySelector('.jsBuyNow').setAttribute('selectedStore', "");
                    if ($(inst.divContainerId).find('#pickupStoreBox').is(':visible')) {
                        $(inst.divContainerId).find('#pickupStoreBox').fadeOut(300); //need to rewrite in javascript
                    }
                }
            });
        });
    }

    selectStoreClick() {
        let inst = this;
        if (document.querySelector(this.divContainerId) == null) return;
        if ( document.querySelector(this.divContainerId).querySelectorAll('.jsSelectStore') == null) return;
        Array.from(document.querySelector(this.divContainerId).querySelectorAll('.jsSelectStore')).forEach(function (el, i) {
            el.addEventListener("click", function () {
                let storeCode = el.getAttribute('data');
                document.querySelector(inst.divContainerId).querySelector('#selectedStore').setAttribute("value", storeCode);

                Array.from(document.querySelectorAll('.selectedStoreIcon')).forEach(function (obj, j) {
                    obj.style.display = "none";
                });

                    Array.from(document.querySelectorAll('.jsSelectStore')).forEach(function (obj, j) {
                    obj.style.display = "block";
                });

                el.style.display = "none";
                el.parentNode.querySelector('.selectedStoreIcon').style.display = "block";

                document.querySelector(inst.divContainerId).querySelector('.addToCart').setAttribute('selectedStore', storeCode);
                document.querySelector(inst.divContainerId).querySelector('.jsBuyNow').setAttribute('selectedStore', storeCode);
            });
        });
    }

    selectColorSizeClick(isQuickView, callback) {
        let inst = this;
        Array.from(document.querySelectorAll(".jsSelectColorSize")).forEach(function (el, i) {
            el.addEventListener("change", function () {
                var objColor = document.getElementsByName("color");
                var color = "";
                if (objColor.length > 0) {
                    var objColorVal = objColor[0];
                    var selectedOptionColor = objColorVal.options[objColorVal.selectedIndex];
                    //var selectedOptionColor = objColor.options[objColor.selectedIndex];
                    color = selectedOptionColor.value;
                }
                else {
                    color = "";
                }

                var objSize = document.getElementsByName("size");
                var size = "";
                if (objSize.length > 0) {
                    var objSizeVal = objSize[0];
                    var selectedOptionSize = objSizeVal.options[objSizeVal.selectedIndex];
                    //var selectedOptionColor = objColor.options[objColor.selectedIndex];
                    size = selectedOptionSize.value;
                }
                else {
                    size = "";
                }

                let productCode = document.querySelector("#productCode").value;
                let data = { productCode: productCode, color: color, size: size, isQuickView: isQuickView };
                inst.changeVariant(data, callback);
            });
        });
    }

    changeQuantityKeyup() {
        let obj = document.querySelector('#qty');
        if (obj == null) return;
        obj.addEventListener("change", function () {
            document.querySelector('.addToCart').setAttribute('qty', obj.value);
            if (document.querySelector('.jsBuyNow') != null)
                document.querySelector('.jsBuyNow').setAttribute('qty', obj.value);
        });
    }

    changeImageClick() {
        let inst = this;
        if (document.querySelector(this.divContainerId) == null) return;
        if (document.querySelector(this.divContainerId).querySelectorAll('.jsProductImageSelect') == null) return;
       Array.from(document.querySelector(this.divContainerId).querySelectorAll('.jsProductImageSelect')).forEach(function (el, i) {
           el.addEventListener("click", function () {
                let type = "Image";
               let mediaTag = el.querySelector('img');
               let isVisible = inst.isElementVisible(el);
               console.log(isVisible);
               if (isVisible) {
                    let type = "Video";
                   mediaTag = el.querySelector('video');
               }

                let urlImg = mediaTag.getAttribute('src');
                if (type == "Image") {
                    document.querySelector('.jsProductImageShow').querySelector('img').setAttribute('src', urlImg);
                    document.querySelector('.jsProductImageShow').querySelector('img').setAttribute("style", "display:inline;");
                    document.querySelector('.jsProductImageShow').querySelector('video').setAttribute("style", "display:none;");
                    document.querySelector('.zoomImg').setAttribute('src', urlImg);
                } else {
                    document.querySelector('.jsProductImageShow').querySelector('video').attr('src', urlImg);
                    document.querySelector('.jsProductImageShow').querySelector('img').setAttribute("style", "display:none;");
                    document.querySelector('.jsProductImageShow').querySelector('video').setAttribute("style", "display:inline;");
               }


            });
        });
    }

    isElementVisible(el) {
        let isVisible = (window.getComputedStyle(el).display === "none")
        return isVisible;
    }

    /////need to rewrite in javascript
    zoomImage() {
        let inst = this;
        $(this.divContainerId).find('.jsProductImageShow').each(function (i, e) {
            if ($(e).find('img').is(":visible")) {
                let urlImg = $(e).find('img').attr('src');
                $(e).siblings('div').first().children('div').first().zoom({
                    url: urlImg,
                    magnify: 1.5,
                    onZoomIn: true,
                    onZoomOut: true
                });
            }
        });
    }

    buyNowClick() {
        if (document.querySelector(this.divContainerId) == null) return;
        if (document.querySelector(this.divContainerId).querySelectorAll('.jsBuyNow') == null) return;
        Array.from(document.querySelector(this.divContainerId).querySelectorAll('.jsBuyNow')).forEach(function (el, i) {
            el.addEventListener("click", async function () {
                document.querySelector(".loading-box").style.display = 'block'; // show
                let code = el.getAttribute('data');
                let data = {
                    Code: code
                };

                if (el.getAttribute('qty')) data.Quantity = el.getAttribute('qty');
                if (el.getAttribute('store')) data.Store = el.getAttribute('store');
                if (el.getAttribute('selectedStore')) data.SelectedStore = el.getAttribute('selectedStore');
                let url = el.getAttribute('url');
                try {
                    const r = await axios.post(url, data);
                    if (r.data.message) {
                        notification.error(r.data.message);
                        setTimeout(function () {
                            window.location.href = r.data.redirect;
                        }, 1000);
                    } else {
                        window.location.href = r.data.redirect;
                    }
                } catch (e) {
                    notification.error(e);
                } finally {
                    document.querySelector(".loading-box").style.display = 'none'; // hide
                }
            })
        })
    }

    selectDynamicVariantChange() {
        if (document.querySelectorAll(".jsDynamicVariants") == null) return;
        Array.from(document.querySelectorAll(".jsDynamicVariants")).forEach(function (el, i) {
            el.addEventListener("change", function () {
                document.querySelector(".loading-box").style.display = 'block'; 
                let search = new URLSearchParams(location.search);
                search.set('variationCode', el.value);
                location.search = search.toString();
            })
        })
    }

    onToggleVariantSubgroup() {
        if (document.querySelector(this.divContainerId) == null) return;
        if (document.querySelector(this.divContainerId).querySelector('.variant-options-section .nav-tabs a') == null) return;
        documetn.querySelector(this.divContainerId).querySelector('.variant-options-section .nav-tabs a').addEventListener('click', function (event) {
            let tabId = event.getAttribute('href').substring(1);
            let tabElement = document.querySelector('.tab-pane#' + tabId);
            tabElement.querySelector('.jsDynamicOptionsInSubgroup').addEventListener("click", function () { });
        });
    }

    initProductDetail() {
        let inst = this;
        this.inStorePickupClick();
        this.selectStoreClick();
        this.selectColorSizeClick(false,
            function (result) {
                if (result.status == 200) {
                    let breadCrumb = document.querySelector('.bread-crumb').innerHTML;
                    let review = "";
                    if (document.querySelector('.jsReviewRating') != null)
                        review = document.querySelector('.jsReviewRating').innerHTML;
                    document.querySelector(inst.divContainerId).innerHTML = result.data;
                    document.querySelector('.bread-crumb').innerHTML = breadCrumb;
                    if (document.querySelector('.jsReviewRating') != null)
                        document.querySelector('.jsReviewRating').innerHTML = review;
                    //$(inst.divContainerId).off();
                    document.querySelector(inst.divContainerId).value = productCode;
                    feather.replace();
                    let dropdown = new Dropdown(inst.divContainerId);
                    dropdown.init();
                    let product = new Product(inst.divContainerId);
                    product.addToCartClick();
                    product.addToWishlistClick();
                    inst.changeQuantityKeyup();
                    inst.inStorePickupClick();
                    inst.selectStoreClick();
                    inst.changeImageClick();
                    inst.zoomImage();
                    inst.buyNowClick();
                }
            }
        );
        this.zoomImage();
        this.changeImageClick();
        this.changeQuantityKeyup();
        this.buyNowClick();
        this.selectDynamicVariantChange();
        this.onToggleVariantSubgroup();
        this.closeQuickViewModal();
    }

    initQuickView() {
        let inst = this;
        Array.from(document.querySelectorAll(".jsQuickView")).forEach(function (el, i) {
            el.addEventListener("click", function () {
                let code = el.getAttribute('data');
                let productCode = el.getAttribute('productcode');
                let url = el.getAttribute('urlquickview');
                if (url == undefined || url == "") {
                    url = "/product/quickview";
                }

                inst.quickView(code, productCode, url);
            });
        });
    }

    //need to rewrite in javascript
    closeQuickViewModal() {
        document.querySelector(".close").addEventListener("click", function () {
            console.log("inside close click event");

            //$(document).ready(function ($) {
            //    //event.preventDefault();
            //    jQuery.noConflict();
            //    $('#quickView').modal('hide');

            //});
        });
        
    }



    
}