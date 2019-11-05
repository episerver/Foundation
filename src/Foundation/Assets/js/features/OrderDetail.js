class OrderDetail {
    constructor(divContainer) {
        this.DivContainer = divContainer == undefined ? document : divContainer;
        this.NoteTemplate = `<div class="order-detail__note-block">
            <p class="title">@title</p>
            <p class="sub-title">Type: @type</p>
            <p>@detail</p>
        </div>`;


    }
    // Notes 
    SaveNoteClick() {
        var inst = this;
        $(this.DivContainer).find('.jsAddNote').each(function (i, e) {
            $(e).click(function () {
                $('.loading-box').show();
                var form = $(this).closest('form');
                var url = form[0].action;
                var data = form.serialize();
                axios.post(url, data)
                    .then(function (result) {
                        var newNote = inst.NoteTemplate.replace("@title", result.data.Title).replace("@type", result.data.Type).replace("@detail", result.data.Detail);
                        $('#noteListing').append(newNote);
                        form[0].reset();
                    })
                    .catch(function (error) {
                        notification.Error(error);
                    })
                    .finally(function () {
                        $('.loading-box').hide();
                    });

                return false;
            });
        });
    }

    InitNote() {
        this.SaveNoteClick();
    }
    // --- End


    // Return Order
    ReturnItemClick() {
        $(this.DivContainer).find('.jsReturnLineItem').each(function (i, e) {
            $(e).click(function () {
                var modal = $('#returnSettingModal');
                var btnSubmitModal = modal.find('#btnSubmitReturnOrder');

                $(btnSubmitModal).attr("data-order-link", $(this).data('order-link'));
                $(btnSubmitModal).attr("data-shipment-link", $(this).data('shipment-link'));
                $(btnSubmitModal).attr("data-lineItem-link", $(this).data('lineitem-link'));
                $(btnSubmitModal).attr("data-total-return", $(this).data('total-return'));

                var txtQuantity = modal.find('input[id="txtQuantity"]');
                $(txtQuantity).val(parseInt($(this).data('total-return')));
            });
        });
    }


    SubmitReturnItemClick() {
        $(this.DivContainer).find('.jsCreateReturn').each(function (i, e) {
            $(e).click(function () {
                $('.loading-box').show();
                var form = $(this).closest('form');
                var url = form[0].action;
                var data = new FormData();
                var itemId = $(this).data('lineitem-link');
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
                        notification.Success('Success');
                        $('#returnSettingModal').modal('hide');
                        $('#return-' + itemId).prop('disabled', true);
                    })
                    .catch(function (error) {
                        notification.Error(error);
                    })
                    .finally(function () {
                        $('.loading-box').hide();
                    });
            });
        });
    }

    InitReturnOrder() {
        this.ReturnItemClick();
        this.SubmitReturnItemClick();
    }
    // --- End
}