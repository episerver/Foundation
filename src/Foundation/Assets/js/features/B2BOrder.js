class B2BOrder {
    FilterByStatus() {
        $('.jsFilterOrderByStatus').change(function () {
            var status = $(this).val();
            if (status == '') {
                $('.jsOrderRow').each(function (i, e) {
                    $(e).show();
                })
            } else {
                $('.jsOrderRow').each(function (i, e) {
                    if ($(e).hasClass(status)) {
                        $(e).show();
                    } else {
                        $(e).hide();
                    }
                })
            }
        })
    }

    ApproveOder() {
        $('.jsApproveOrder').click(function () {
            $('.loading-box').show();
            var form = $(this).closest("form");
            var orderLink = $(this).data("order-link");
            var data = { orderGroupId: orderLink };
            var postData = convertFormData(data);
            axios.post(form[0].action, postData)
                .then(function (r) {
                    if (r.data.Status == true) {
                        notification.Success("Success");
                        setTimeout(function () { window.location.href = window.location.href; }, 500);
                    } else {
                        notification.Error("Something went wrong.");
                    }
                })
                .catch(function (e) {
                    notification.Error(e);
                })
                .finally(function () {
                    $('.loading-box').hide();
                })
        })
    }

    Init() {
        this.FilterByStatus();
        this.ApproveOder();
    }
}