﻿class QuickOrderBlock {
    constructor(containerId) {
        this.Container = containerId != undefined ? containerId : document;
        this.RowTemplate = (index, data) => `<div class="row js-product-row" data-order="${index}">
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

        this.ProductListing = [];

        document.querySelectorAll('.jsQuickOrderBlockForm').forEach(form => {
            form.addEventListener('submit', event => {
                event.preventDefault()
                const rows = Array.from(form.querySelectorAll('.js-product-row'))
                const lines = rows.reduce(
                    (acc, elem) => {
                        const product = {}
                        const inputs = Array.from(elem.querySelectorAll('input'))
                        inputs.forEach(input => {
                            if (input.name.match(/ProductsList\[\d\]\.ProductName/i)) {
                                product.ProductName = input.value
                            }
                            else if (input.name.match(/ProductsList\[\d\]\.Sku/i)) {
                                product.Sku = input.value
                            }
                            else if (input.name.match(/ProductsList\[\d\]\.UnitPrice/i)) {
                                product.UnitPrice = input.value
                            }
                            else if (input.name.match(/ProductsList\[\d\]\.Quantity/i)) {
                                product.Quantity = parseInt(input.value, 10)
                            }
                            else if (input.name.match(/ProductsList\[\d\]\.TotalPrice/i)) {
                                product.TotalPrice = input.value
                            }
                        })
                        acc.push(product)
                        return acc
                    },
                    []
                )

                axios.post('/QuickOrderBlock/Import', lines)
                    .then(({ data }) => {
                        if (data.Message.length === 1) {
                            document.querySelector('.jsShowMessage').innerHTML = `
                                <div class="success">
                                    <p>${data.Message[0]}</p>
                                </div>
                            `
                        }
                        else {
                            document.querySelector('.jsShowMessage').innerHTML = `
                                <div class="error">
                                    ${data.Message.map(msg => `<p>* ${msg}</p>`).join('')}
                                </div>
                            `
                        }

                        const oldTotal = parseInt(document.querySelector('.icon-menu__badge').innerHTML, 10)
                        const diff = lines.reduce((acc, elem) => acc + elem.Quantity, 0)
                        cartHelper.SetCartReload(oldTotal + diff)
                    })
            })
        })
    }

    renderRow(num, element) {
        return this.RowTemplate(num, element);
    }


    renderList() {
        return this.ProductListing.reduce(
            (acc, elem, index) => {
                const row = this.renderRow(index, elem);
                return acc + "" + row;
            },
            ''
        );
    }

    setupAutoComplete(e) {
        var $autocompleteInput = $(e).find('input[name*=Sku]');
        var options = {
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
        var selectedItemData = element.getSelectedItemData();
        var parent = element.parents('.js-product-row').first();
        var currentOrder = parent.data('order');
        parent.find('input[name*=ProductName]').val(selectedItemData.ProductName);
        parent.find('input[name*=UnitPrice]').val(selectedItemData.UnitPrice);
        this.ProductListing[currentOrder].ProductName = selectedItemData.ProductName;
        this.ProductListing[currentOrder].UnitPrice = selectedItemData.UnitPrice;
        this.ProductListing[currentOrder].Sku = selectedItemData.Sku;
    }

    initRenderList(inst) {
        const template = inst.renderList(inst.ProductListing);
        $(this.Container).find('.jsProductListing').html(template);
        feather.replace();
        inst.DeleteRowClick();
        inst.AutoComplete();
        inst.OnQuantityChange();
    }

    AddRowClick(container) {
        var inst = this;
        if (container == undefined) {
            $(this.Container).find('.jsAddNewRow').each(function (i, e) {
                $(e).click(function () {
                    inst.ProductListing.push(new ProductViewModel());
                    inst.initRenderList(inst);
                });
            });
        } else {
            $(container).find('.jsAddNewRow').each(function (i, e) {
                $(e).click(function () {
                    inst.ProductListing.push(new ProductViewModel());
                    inst.initRenderList(inst);
                });
            });
        }
    }

    DeleteRowClick(container) {
        var inst = this;
        if (container == undefined) {
            $(this.Container).find('.jsDeleteRow').each(function (i, e) {
                $(e).click(function () {
                    var currentOrder = $(this).parents('.js-product-row').data('order');
                    inst.ProductListing.splice(currentOrder, 1);
                    inst.initRenderList(inst);
                });
            });
        } else {
            $(container).find('.jsDeleteRow').each(function (i, e) {
                $(e).click(function () {
                    var currentOrder = $(this).parents('.js-product-row').data('order');
                    inst.ProductListing.splice(currentOrder, 1);
                    inst.initRenderList(inst);
                });
            });
        }
    }

    AutoComplete(container) {
        var inst = this;

        if (container != undefined) {
            $(container).find('.js-product-row').each(function (i, e) {
                inst.setupAutoComplete($(e));
            });
        } else {
            $(this.Container).find('.js-product-row').each(function (i, e) {
                inst.setupAutoComplete($(e));
            });
        }
    }

    quantityChange(element, inst) {
        $(element).keyup(function () {
            var currentOrder = $(this).parents('.js-product-row').first().data('order');
            var quantity = $(this).val();
            var unitPrice = $(this).parents('.js-product-row').find('input[name*=UnitPrice]').val();
            var totalPrice = parseFloat(parseFloat(unitPrice) * parseInt(quantity)).toFixed(2);
            inst.ProductListing[currentOrder].Quantity = quantity;
            inst.ProductListing[currentOrder].TotalPrice = totalPrice;

            $(this).parents('.js-product-row').find('input[name*=TotalPrice]').val(totalPrice);
        });
    }

    OnQuantityChange(container) {
        var inst = this;
        if (container != undefined) {
            var inputsQuantity = $(container).find('input[name*=Quantity]');
            inputsQuantity.each(function (i, e) {
                inst.quantityChange(e, inst);
            });
        } else {
            $(this.Container).find('input[name*=Quantity]').each(function (i, e) {
                inst.quantityChange($(e), inst);
            });
        }
    }

    AddToCartQuickOrderClick(container) {
        var inst = this;
        if (container == undefined) {
            container = this.Container;
        }
        $(container).find('.jsAddToCartQuickOrderBtn').click(function () {
            var formData = new FormData();
            var url = "/QuickOrderBlock/Import"
            var form = $(this).parents('.jsQuickOrderBlockForm').first();
            var data = serializeObject(form);
            formData = convertFormData(data);
            url = form[0].action;

            axios.post(url, formData)
                .then(function (r) {
                    cartHelper.SetCartReload(r.data.TotalItem);
                    notification.Success(r.data.Message);
                })
                .catch(function (e) {
                    notification.Error(e);
                })
        });
    }

    UploadCSVClick() {
        var inst = this;
        $('.jsUploadCSVBtn').click(function () {
            $('#fileUploaded').click();
        });

        $('#fileUploaded').change(function () {
            $('.loading-box').show();
            var file = $("#fileUploaded")[0].files[0];
            var formData = new FormData();
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
                            if (inst.ProductListing.length > 0) {
                                for (var i = inst.ProductListing.length - 1; i >= 0; i--) {
                                    if (inst.ProductListing[i].Sku == "") {
                                        inst.ProductListing.splice(i, 1);
                                    }
                                }
                            }

                            for (var i = 0; i < res.data.Products.length; i++) {
                                inst.ProductListing.push(res.data.Products[i]);
                            }
                            inst.initRenderList(inst);
                        }
                    }
                })
                .catch(function (err) {
                    notification.Error(err);
                })
                .finally(function () {
                    $('.loading-box').hide();
                });
        });
    }

    Init() {
        var products = $(this.Container).find('.js-product-row');
        if (products.length > 0) {
            this.ProductListing = [];
            var inst = this;
            products.each(function (i, e) {
                var newProduct = new ProductViewModel();
                newProduct.ProductName = $(e).find('input[name*=ProductName]').val();
                newProduct.Sku = $(e).find('input[name*=Sku]').val();
                newProduct.UnitPrice = $(e).find('input[name*=UnitPrice]').val();
                newProduct.Quantity = $(e).find('input[name*=Quantity]').val();
                newProduct.TotalPrice = $(e).find('input[name*=TotalPrice]').val();
                inst.ProductListing.push(newProduct);
            });
        }

        $('.jsLabelUpload').html($('#fileUploaded').data('label'));
        this.AddRowClick();
        this.DeleteRowClick();
        this.AutoComplete();
        this.OnQuantityChange();
        this.UploadCSVClick();
        this.AddToCartQuickOrderClick();
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