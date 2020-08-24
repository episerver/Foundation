import feather from "feather-icons";
import Dropdown from "../../Assets/js/common/dropdown";
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
        $(inst.divContainerId).find('.loading-box').show();
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
                $(inst.divContainerId).find('.loading-box').hide();
            });
    }

    inStorePickupClick() {
        let inst = this;
        $(this.divContainerId).find('.jsSelectDelivery').each(function (i, e) {
            $(e).click(function () {
                let valueChecked = $(this).find('input').first().val();
                $(inst.divContainerId).find('.addToCart').attr('store', valueChecked);
                $(inst.divContainerId).find('.jsBuyNow').attr('store', valueChecked);
                if (valueChecked === 'instore') {
                    let selectedStore = $(inst.divContainerId).find('#selectedStore').val();
                    $(inst.divContainerId).find('.addToCart').attr('selectedStore', selectedStore);
                    $(inst.divContainerId).find('.jsBuyNow').attr('selectedStore', selectedStore);
                    if (!$(inst.divContainerId).find('#pickupStoreBox').is(':visible')) {
                        $(inst.divContainerId).find('#pickupStoreBox').fadeToggle();
                    }
                } else {
                    $(inst.divContainerId).find('.addToCart').attr('selectedStore', '');
                    $(inst.divContainerId).find('.jsBuyNow').attr('selectedStore', '');
                    if ($(inst.divContainerId).find('#pickupStoreBox').is(':visible')) {
                        $(inst.divContainerId).find('#pickupStoreBox').fadeOut(300);
                    }
                }
            });
        });
    }

    selectStoreClick() {
        let inst = this;
        $(this.divContainerId).find('.jsSelectStore').each(function (i, e) {
            $(e).click(function () {
                let storeCode = $(this).attr('data');
                $(inst.divContainerId).find('#selectedStore').val(storeCode);
                $(inst.divContainerId).find('.selectedStoreIcon').each(function (j, s) {
                    $(s).hide();
                });
                $(inst.divContainerId).find('.jsSelectStore').each(function (j, s) {
                    $(s).show();
                });

                $(this).hide();
                $(this).siblings('.selectedStoreIcon').show();

                $(inst.divContainerId).find('.addToCart').attr('selectedStore', storeCode);
                $(inst.divContainerId).find('.jsBuyNow').attr('selectedStore', storeCode);
            });
        });
    }

    selectColorSizeClick(isQuickView, callback) {
        let inst = this;
        $(this.divContainerId).find(".jsSelectColorSize").each(function (i, e) {
            $(e).change(function () {
                let color = $(inst.divContainerId).find("select[name='color']").val();
                let size = $(inst.divContainerId).find("select[name='size']").val();
                let productCode = $(inst.divContainerId).find("#productCode").val();
                let data = { productCode: productCode, color: color, size: size, isQuickView: isQuickView };
                inst.changeVariant(data, callback);
            });
        });
    }

    changeQuantityKeyup() {
        $('#qty').change(function () {
            $('.addToCart').attr('qty', $(this).val());
            $('.jsBuyNow').attr('qty', $(this).val());
        });
    }

    changeImageClick() {
        $(this.divContainerId).find('.jsProductImageSelect').each(function (i, e) {
            $(e).click(function () {
                let type = "Image";
                let mediaTag = $(this).find('img');
                if (!mediaTag.is(":visible")) {
                    let type = "Video";
                    mediaTag = $(this).find('video');
                }
                let urlImg = mediaTag.attr('src');
                if (type == "Image") {
                    $('.jsProductImageShow').find('img').attr('src', urlImg);
                    $('.jsProductImageShow').find('img').css("display", "inline");
                    $('.jsProductImageShow').find('video').css("display", "none");
                    $('.zoomImg').attr('src', urlImg);
                } else {
                    $('.jsProductImageShow').find('video').attr('src', urlImg);
                    $('.jsProductImageShow').find('img').css("display", "none");
                    $('.jsProductImageShow').find('video').css("display", "inline");
                }
            });
        });
    }

    zoomImage() {
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
        $(this.divContainerId).find('.jsBuyNow').each(function (i, e) {
            $(e).click(async function () {
                $('.loading-box').show();
                let code = $(this).attr('data');
                let data = {
                    Code: code
                };

                if ($(this).attr('qty')) data.Quantity = $(this).attr('qty');
                if ($(this).attr('store')) data.Store = $(this).attr('store');
                if ($(this).attr('selectedStore')) data.SelectedStore = $(this).attr('selectedStore');
                let url = $(this).attr('url');

                try {
                    const r = await axios.post(url, data);
                    if (r.data.Message) {
                        notification.error(r.data.Message);
                        setTimeout(function () {
                            window.location.href = r.data.Redirect;
                        }, 1000);
                    } else {
                        window.location.href = r.data.Redirect;
                    }
                } catch (e) {
                    notification.error(e);
                } finally {
                    $('.loading-box').hide();
                }
            })
        })
    }

    initProductDetail() {
        let inst = this;
        this.inStorePickupClick();
        this.selectStoreClick();
        this.selectColorSizeClick(false,
            function (result) {
                if (result.status == 200) {
                    let breadCrumb = $('.bread-crumb').html();
                    let review = $('.jsReviewRating').html();
                    $(inst.divContainerId).html(result.data);
                    $('.bread-crumb').html(breadCrumb);
                    $('.jsReviewRating').html(review);
                    $(inst.divContainerId).off();
                    $(inst.divContainerId).val(productCode);
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
    }

    initQuickView() {
        let inst = this;
        $('.jsQuickView').each(function (i, e) {
            $(e).click(function () {
                let code = $(this).attr('data');
                let productCode = $(this).attr('productCode');
                let url = $(this).attr('urlQuickView');
                if (url == undefined || url == "") {
                    url = "/product/quickview";
                }

                inst.quickView(code, productCode, url);
            });
        });
    }
}