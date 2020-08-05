import * as $ from "jquery";
import * as axios from "axios";

export default class OrderDetails {
    constructor(divContainer) {
        this.divContainer = divContainer == undefined ? document : divContainer;
        this.noteTemplate = `<div class="order-detail__note-block">
            <p class="title">@title</p>
            <p class="sub-title">Type: @type</p>
            <p>@detail</p>
        </div>`;
    }

    saveNoteClick() {
        let inst = this;
        $(this.divContainer).find('.jsAddNote').each(function (i, e) {
            $(e).click(function () {
                $('.loading-box').show();
                let form = $(this).closest('form');
                let url = form[0].action;
                let data = form.serialize();
                axios.post(url, data)
                    .then(function (result) {
                        let newNote = inst.noteTemplate.replace("@title", result.data.Title).replace("@type", result.data.Type).replace("@detail", result.data.Detail);
                        $('#noteListing').append(newNote);
                        form[0].reset();
                    })
                    .catch(function (error) {
                        notification.error(error);
                    })
                    .finally(function () {
                        $('.loading-box').hide();
                    });

                return false;
            });
        });
    }

    initNote() {
        this.saveNoteClick();
    }

    returnItemClick() {
        $(this.divContainer).find('.jsReturnLineItem').each(function (i, e) {
            $(e).click(function () {
                let modal = $('#returnSettingModal');
                let btnSubmitModal = modal.find('#btnSubmitReturnOrder');

                $(btnSubmitModal).attr("data-order-link", $(this).data('order-link'));
                $(btnSubmitModal).attr("data-shipment-link", $(this).data('shipment-link'));
                $(btnSubmitModal).attr("data-lineItem-link", $(this).data('lineitem-link'));
                $(btnSubmitModal).attr("data-total-return", $(this).data('total-return'));

                let txtQuantity = modal.find('input[id="txtQuantity"]');
                $(txtQuantity).val(parseInt($(this).data('total-return')));
            });
        });
    }

    submitReturnItemClick() {
        $(this.divContainer).find('.jsCreateReturn').each(function (i, e) {
            $(e).click(function () {
                $('.loading-box').show();
                let form = $(this).closest('form');
                let url = form[0].action;
                let data = new FormData();
                let itemId = $(this).data('lineitem-link');
                data.append("orderGroupId", $(this).data('order-link'));
                data.append("shipmentId", $(this).data('shipment-link'));
                data.append("lineItemId", $(this).data('lineitem-link'));
                data.append("returnQuantity", $(this).data('total-return'));
                data.append("reason", $("#optReason option:selected").text());
                data.append("__RequestVerificationToken", form.find('input[name="__RequestVerificationToken"]').first().val());
                //{
                //    orderGroupId: $(this).data('order-link'),
                //    shipmentId: $(this).data('shipment-link'),
                //    lineItemId: $(this).data('lineitem-link'),
                //    returnQuantity: $(this).data('total-return'),
                //    reason: $("#optReason option:selected").text(),
                //    __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
                //}
                axios.post(url, data)
                    .then(function (result) {
                        notification.success('Success');
                        $('#returnSettingModal').modal('hide');
                        $('#return-' + itemId).prop('disabled', true);
                    })
                    .catch(function (error) {
                        notification.error(error);
                    })
                    .finally(function () {
                        $('.loading-box').hide();
                    });
            });
        });
    }

    initReturnOrder() {
        this.returnItemClick();
        this.submitReturnItemClick();
    }
}