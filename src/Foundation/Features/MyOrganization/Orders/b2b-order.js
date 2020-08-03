export default class B2bOrder {
    init() {
        this.filterByStatus();
        this.approveOder();
    }

    filterByStatus() {
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

    approveOder() {
        $('.jsApproveOrder').click(function () {
            $('.loading-box').show();
            var form = $(this).closest("form");
            var orderLink = $(this).data("order-link");
            var data = { orderGroupId: orderLink };
            var postData = convertFormData(data);
            axios.post(form[0].action, postData)
                .then(function (r) {
                    if (r.data.Status == true) {
                        notification.success("Success");
                        setTimeout(function () { window.location.href = window.location.href; }, 500);
                    } else {
                        notification.error("Something went wrong.");
                    }
                })
                .catch(function (e) {
                    notification.error(e);
                })
                .finally(function () {
                    $('.loading-box').hide();
                })
        })
    }
}