export default class QuickOrderBlock {
    constructor(containerId) {
        this.container = containerId != undefined ? containerId : document;
        this.rowTemplate = (index, data) => `<div class="row js-product-row" data-order="${index}">
                <div class="form-group col-xs-12 col-sm-12 col-md-12 col-lg-3">
                    <input class="form-control square-box" readonly="readonly" id='ProductsList_${index}__ProductName' name='ProductsList[${index}].ProductName' type="text" value='${data.ProductName}' style="position: relative" placeholder="Product name">
                </div>
                <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-2">
                    <input class="form-control square-box" id="ProductsList_${index}__Sku" name="ProductsList[${index}].Sku" placeholder="Sku code" required="required" type="text" value="${data.Sku}">
                </div>
                <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-2">
                    <input class="form-control square-box" readonly="readonly" id="ProductsList_${index}__UnitPrice" name="ProductsList[${index}].UnitPrice" type="text"
                      value="${data.UnitPrice}" placeholder="Unit price">
                </div>
                <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-2">
                    <input class="form-control square-box" id="ProductsList_${index}__Quantity" name="ProductsList[${index}].Quantity" required="required" type="text"
                      value="${data.Quantity}" placeholder="Quantity">
                </div>
                <div class="form-group col-xs-12 col-sm-12 col-md-6 col-lg-2">
                    <input class="form-control square-box" readonly="readonly" id="ProductsList_${index}__TotalPrice" name="ProductsList[${index}].TotalPrice" type="text" value="${data.TotalPrice}" placeholder="Total price">
                </div>
                <div class="form-group col-xs-12 col-sm-12 col-md-12 col-lg-1">
                    <a href="javascript:void(0);" class="btn btn-danger jsDeleteRow">
                        <i data-feather="trash-2"></i>
                    </a>
                </div>
            </div>`;

        this.productListing = [];

        document.querySelectorAll('.jsQuickOrderBlockForm').forEach(form => {
            form.addEventListener('submit', event => {
                event.preventDefault()
                let data = serializeObject(form);
                let formData = convertFormData(data);

                axios.post(form.action, formData)
                    .then(function (r) {
                        cartHelper.setCartReload(r.data.TotalItem);
                        notification.success(r.data.Message);
                    })
                    .catch(function (e) {
                        notification.error(e);
                    })
            })
        })
    }

    init() {
        let products = $(this.container).find('.js-product-row');
        if (products.length > 0) {
            this.productListing = [];
            let inst = this;
            products.each(function (i, e) {
                let newProduct = new ProductViewModel();
                newProduct.ProductName = $(e).find('input[name*=ProductName]').val();
                newProduct.Sku = $(e).find('input[name*=Sku]').val();
                newProduct.UnitPrice = $(e).find('input[name*=UnitPrice]').val();
                newProduct.Quantity = $(e).find('input[name*=Quantity]').val();
                newProduct.TotalPrice = $(e).find('input[name*=TotalPrice]').val();
                inst.productListing.push(newProduct);
            });
        }

        $('.jsLabelUpload').html($('#fileUploaded').data('label'));
        this.addRowClick();
        this.deleteRowClick();
        this.autoComplete();
        this.onQuantityChange();
        this.uploadCSVClick();
    }

    renderRow(num, element) {
        return this.rowTemplate(num, element);
    }

    renderList() {
        return this.productListing.reduce(
            (acc, elem, index) => {
                const row = this.renderRow(index, elem);
                return acc + "" + row;
            },
            ''
        );
    }

    setupAutoComplete(e) {
        let $autocompleteInput = $(e).find('input[name*=Sku]');
        let options = {
            url: function (phrase) {
                return "/QuickOrderBlock/GetSku?query=" + phrase;
            },
            getValue: "Sku",
            requestDelay: 500,
            list: {
                match: {
                    enabled: false
                },
                onChooseEvent: () => this.onChooseEvent($autocompleteInput)
            },
            template: {
                type: "custom",
                method: function (value, item) {
                    if (item.UrlImage == "" || item.UrlImage == undefined) {
                        return value;
                    }
                    return "<img class='eac-icon' src='" + item.UrlImage + "' /> " + value;
                }
            },
            adjustWidth: false
        };
        $autocompleteInput.easyAutocomplete(options);
    }

    onChooseEvent(element) {
        let selectedItemData = element.getSelectedItemData();
        let parent = element.parents('.js-product-row').first();
        let currentOrder = parent.data('order');
        parent.find('input[name*=ProductName]').val(selectedItemData.ProductName);
        parent.find('input[name*=UnitPrice]').val(selectedItemData.UnitPrice);
        this.productListing[currentOrder].ProductName = selectedItemData.ProductName;
        this.productListing[currentOrder].UnitPrice = selectedItemData.UnitPrice;
        this.productListing[currentOrder].Sku = selectedItemData.Sku;
    }

    initRenderList(inst) {
        const template = inst.renderList(inst.productListing);
        $(this.container).find('.jsProductListing').html(template);
        feather.replace();
        inst.deleteRowClick();
        inst.autoComplete();
        inst.onQuantityChange();
    }

    addRowClick(container) {
        let inst = this;
        if (container == undefined) {
            $(this.container).find('.jsAddNewRow').each(function (i, e) {
                $(e).click(function () {
                    inst.productListing.push(new ProductViewModel());
                    inst.initRenderList(inst);
                });
            });
        } else {
            $(container).find('.jsAddNewRow').each(function (i, e) {
                $(e).click(function () {
                    inst.productListing.push(new ProductViewModel());
                    inst.initRenderList(inst);
                });
            });
        }
    }

    deleteRowClick(container) {
        let inst = this;
        if (container == undefined) {
            $(this.container).find('.jsDeleteRow').each(function (i, e) {
                $(e).click(function () {
                    let currentOrder = $(this).parents('.js-product-row').data('order');
                    inst.productListing.splice(currentOrder, 1);
                    inst.initRenderList(inst);
                });
            });
        } else {
            $(container).find('.jsDeleteRow').each(function (i, e) {
                $(e).click(function () {
                    let currentOrder = $(this).parents('.js-product-row').data('order');
                    inst.productListing.splice(currentOrder, 1);
                    inst.initRenderList(inst);
                });
            });
        }
    }

    autoComplete(container) {
        let inst = this;

        if (container != undefined) {
            $(container).find('.js-product-row').each(function (i, e) {
                inst.setupAutoComplete($(e));
            });
        } else {
            $(this.container).find('.js-product-row').each(function (i, e) {
                inst.setupAutoComplete($(e));
            });
        }
    }

    quantityChange(element, inst) {
        $(element).keyup(function () {
            let currentOrder = $(this).parents('.js-product-row').first().data('order');
            let quantity = $(this).val();
            let unitPrice = $(this).parents('.js-product-row').find('input[name*=UnitPrice]').val();
            let totalPrice = parseFloat(parseFloat(unitPrice) * parseInt(quantity)).toFixed(2);
            inst.productListing[currentOrder].Quantity = quantity;
            inst.productListing[currentOrder].TotalPrice = totalPrice;

            $(this).parents('.js-product-row').find('input[name*=TotalPrice]').val(totalPrice);
        });
    }

    onQuantityChange(container) {
        let inst = this;
        if (container != undefined) {
            let inputsQuantity = $(container).find('input[name*=Quantity]');
            inputsQuantity.each(function (i, e) {
                inst.quantityChange(e, inst);
            });
        } else {
            $(this.container).find('input[name*=Quantity]').each(function (i, e) {
                inst.quantityChange($(e), inst);
            });
        }
    }

    uploadCSVClick() {
        let inst = this;
        $('.jsUploadCSVBtn').click(function () {
            $('#fileUploaded').click();
        });

        $('#fileUploaded').change(function () {
            $('.loading-box').show();
            let file = $("#fileUploaded")[0].files[0];
            let formData = new FormData();
            formData.append('file', file);
            formData.append('__RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());

            axios.post('/QuickOrderBlock/AddFromFile', formData)
                .then(function (res) {
                    if (res.data.Status != "OK") {
                        $('.jsShowMessage').html(`<div class="error">` + res.data.Message + `</div>`);
                    } else {
                        $('.jsShowMessage').html(`<div class="success">` + res.data.Message + `</div>`);
                        if (res.data.Products.length > 0) {
                            // remove empty product
                            if (inst.productListing.length > 0) {
                                for (let i = inst.productListing.length - 1; i >= 0; i--) {
                                    if (inst.productListing[i].Sku == "") {
                                        inst.productListing.splice(i, 1);
                                    }
                                }
                            }

                            for (let i = 0; i < res.data.Products.length; i++) {
                                inst.productListing.push(res.data.Products[i]);
                            }
                            inst.initRenderList(inst);
                        }
                    }
                })
                .catch(function (err) {
                    notification.error(err);
                })
                .finally(function () {
                    $('.loading-box').hide();
                });
        });
    }
}

class ProductViewModel {
    constructor() {
        this.ProductName = "";
        this.Sku = "";
        this.UnitPrice = 0.0;
        this.Quantity = 0;
        this.TotalPrice = 0.0;
    }
}