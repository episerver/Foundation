class ProductDetail {
  constructor(divContainerId) {
    if (divContainerId) {
      this.DivContainerId = divContainerId;
    } else {
      this.DivContainerId = document;
    }
  }

  quickView(code, productCode, url) {
    var inst = this;
    $(inst.DivContainerId).find('.loading-box').show();
    axios.get(url, { params: { productCode: productCode, variantCode: code } })
      .then(function (result) {
        if (result.status == 200) {
          $('#quickView .modal-body').html(result.data);
          $('#quickView .modal-body').off();
          $(inst.DivContainerId).find("#productCode").val(productCode);
          feather.replace();
          var dropdown = new Dropdown("#quickView");
          dropdown.Init();
          var product = new Product('#quickView');
          product.AddToCartClick();
          product.AddToWishlistClick();
          product.AddToSharedCartClick();

          inst.InStorePickupClick();
          inst.SelectStoreClick();
          inst.SelectColorSizeClick();
          inst.ZoomImage();
        }
      })
      .catch(function (error) {
        notification.Error(error);
        $('#quickView .modal-body').html('');
      })
      .finally(function () {
        $(inst.DivContainerId).find('.loading-box').hide();
      });
  }

  changeVariant(data, callback) {
    var inst = this;
    $(inst.DivContainerId).find('.loading-box').show();
    axios.get('/product/selectVariant', { params: data })
      .then(function (result) {
        if (!callback) {
          if (result.status == 200) {
            $(inst.DivContainerId).find('.modal-body').html(result.data);
            $(inst.DivContainerId).find('.modal-body').off();
            //$(inst.DivContainerId).find("#productCode").val(productCode);
            var dropdown = new Dropdown(inst.DivContainerId);
            dropdown.Init();
            var product = new Product(inst.DivContainerId);
            product.AddToCartClick();
            product.AddToWishlistClick();

            inst.InStorePickupClick();
            inst.SelectStoreClick();
            inst.SelectColorSizeClick();
            feather.replace();
          }
        } else {
          callback(result);
          inst.SelectColorSizeClick(false, callback);
        }
      })
      .catch(function (error) {
        notification.Error(error);
        $('#quickView .modal-body').html('');
      })
      .finally(function () {
        $(inst.DivContainerId).find('.loading-box').hide();
      });
  }

  InStorePickupClick() {
    var inst = this;
    $(this.DivContainerId).find('.jsSelectDelivery').each(function (i, e) {
      $(e).click(function () {
        var valueChecked = $(this).find('input').first().val();
        $(inst.DivContainerId).find('.addToCart').attr('store', valueChecked);
        $(inst.DivContainerId).find('.jsBuyNow').attr('store', valueChecked);
        if (valueChecked === 'instore') {
          var selectedStore = $(inst.DivContainerId).find('#selectedStore').val();
          $(inst.DivContainerId).find('.addToCart').attr('selectedStore', selectedStore);
          $(inst.DivContainerId).find('.jsBuyNow').attr('selectedStore', selectedStore);
          if (!$(inst.DivContainerId).find('#pickupStoreBox').is(':visible')) {
            $(inst.DivContainerId).find('#pickupStoreBox').fadeToggle();
          }
        } else {
          $(inst.DivContainerId).find('.addToCart').attr('selectedStore', '');
          $(inst.DivContainerId).find('.jsBuyNow').attr('selectedStore', '');
          if ($(inst.DivContainerId).find('#pickupStoreBox').is(':visible')) {
            $(inst.DivContainerId).find('#pickupStoreBox').fadeOut(300);
          }
        }
      });
    });
  }

  SelectStoreClick() {
    var inst = this;
    $(this.DivContainerId).find('.jsSelectStore').each(function (i, e) {
      $(e).click(function () {
        var storeCode = $(this).attr('data');
        $(inst.DivContainerId).find('#selectedStore').val(storeCode);
        $(inst.DivContainerId).find('.selectedStoreIcon').each(function (j, s) {
          $(s).hide();
        });
        $(inst.DivContainerId).find('.jsSelectStore').each(function (j, s) {
          $(s).show();
        });

        $(this).hide();
        $(this).siblings('.selectedStoreIcon').show();

        $(inst.DivContainerId).find('.addToCart').attr('selectedStore', storeCode);
        $(inst.DivContainerId).find('.jsBuyNow').attr('selectedStore', storeCode);
      });
    });
  }

  SelectColorSizeClick(isQuickView, callback) {
    var inst = this;
    $(this.DivContainerId).find(".jsSelectColorSize").each(function (i, e) {
      $(e).change(function () {
        var color = $(inst.DivContainerId).find("select[name='color']").val();
        var size = $(inst.DivContainerId).find("select[name='size']").val();
        var productCode = $(inst.DivContainerId).find("#productCode").val();
        var data = { productCode: productCode, color: color, size: size, isQuickView: isQuickView };
        inst.changeVariant(data, callback);
      });
    });
  }

  ChangeQuantityKeyup() {
    $('#qty').change(function () {
      $('.addToCart').attr('qty', $(this).val());
      $('.jsBuyNow').attr('qty', $(this).val());
    });
  }

  ChangeImageClick() {
    $(this.DivContainerId).find('.jsProductImageSelect').each(function (i, e) {
      $(e).click(function () {
        var type = "Image";
        var mediaTag = $(this).find('img');
        if (!mediaTag.is(":visible")) {
          var type = "Video";
          mediaTag = $(this).find('video');
        }
        var urlImg = mediaTag.attr('src');
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

  ZoomImage() {
    $(this.DivContainerId).find('.jsProductImageShow').each(function (i, e) {
      if ($(e).find('img').is(":visible")) {
        var urlImg = $(e).find('img').attr('src');
        $(e).siblings('div').first().children('div').first().zoom({
          url: urlImg,
          magnify: 1.5,
          onZoomIn: true,
          onZoomOut: true
        });
      }
      
    });
  }

  BuyNowClick() {
    $(this.DivContainerId).find('.jsBuyNow').each(function (i, e) {
      $(e).click(async function () {
        $('.loading-box').show();
        var code = $(this).attr('data');
        var data = {
          Code: code
        };

        if ($(this).attr('qty')) data.Quantity = $(this).attr('qty');
        if ($(this).attr('store')) data.Store = $(this).attr('store');
        if ($(this).attr('selectedStore')) data.SelectedStore = $(this).attr('selectedStore');
        var url = $(this).attr('url');

        try {
          const r = await axios.post(url, data);
          if (r.data.Message) {
            notification.Error(r.data.Message);
            setTimeout(function () {
              window.location.href = r.data.Redirect;
            }, 1000);
          } else {
            window.location.href = r.data.Redirect;
          }
        } catch (e) {
          notification.Error(e);
        } finally {
          $('.loading-box').hide();
        }
      })
    })
  }

  InitProductDetail() {
    var inst = this;
    this.InStorePickupClick();
    this.SelectStoreClick();
    this.SelectColorSizeClick(false,
      function (result) {
        if (result.status == 200) {
          var breadCrumb = $('.bread-crumb').html();
          var review = $('.jsReviewRating').html();
          $(inst.DivContainerId).html(result.data);
          $('.bread-crumb').html(breadCrumb);
          $('.jsReviewRating').html(review);
          $(inst.DivContainerId).off();
          $(inst.DivContainerId).val(productCode);
          feather.replace();
          var dropdown = new Dropdown(inst.DivContainerId);
          dropdown.Init();
          var product = new Product(inst.DivContainerId);
          product.AddToCartClick();
          product.AddToWishlistClick();
          inst.ChangeQuantityKeyup();
          inst.InStorePickupClick();
          inst.SelectStoreClick();
          inst.ChangeImageClick();
          inst.ZoomImage();
          inst.BuyNowClick();
        }
      }
    );
    this.ZoomImage();
    this.ChangeImageClick();
    this.ChangeQuantityKeyup();
    this.BuyNowClick();
  }

  InitQuickView() {
    var inst = this;
    // Init quickview
    $('.jsQuickView').each(function (i, e) {
      $(e).click(function () {
        var code = $(this).attr('data');
        var productCode = $(this).attr('productCode');
        var url = $(this).attr('urlQuickView');
        if (url == undefined || url == "") {
          url = "/product/quickview";
        }

        inst.quickView(code, productCode, url);
      });
    });
    //-- end
  }
}