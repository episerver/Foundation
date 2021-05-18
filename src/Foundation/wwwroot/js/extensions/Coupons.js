var Coupons = {
    init: function () {
        $(document)
            .on('click', '.jsUpdateCoupon', Coupons.updateCoupon)
            .on('click', '.jsDeleteCoupon', Coupons.deleteCoupon);
    },

    getFormData(form, rowData, actionType) {
        var record = {
            id: rowData.find('input[id$="Id"]').val(),
            promotionId: rowData.find('input[id$="PromotionId"]').val(),
            code: rowData.find('input[id$="Code"]').val(),
            created: rowData.find('input[id$="Created"]').val(),
            validFrom: rowData.find('input[id$="ValidFrom"]').val(),
            expiration: rowData.find('input[id$="Expiration"]').val(),
            maxRedemptions: rowData.find('input[id$="MaxRedemptions"]').val(),
            usedRedemptions: rowData.find('input[id$="UsedRedemptions"]').val(),
        }

        var formData = new FormData();
        for (var key in record) {
            formData.append(key, record[key]);
        }
        formData.append("__RequestVerificationToken", form.find('input[name="__RequestVerificationToken"').val());
        formData.append("actionType", actionType);

        return formData;
    },

    updateCoupon() {
        var form = $(this).closest('form');
        var rowData = $(this).closest('tr');
        var formData = Coupons.getFormData(form, rowData, "update");

        $('.coupon-status').fadeIn(500);
        axios.post(form[0].action, formData)
            .then(function (result) {
                if (result.data === "update_ok") {
                    $('.coupon-status').fadeOut(500, function () {
                        $('.coupon-alert').addClass('alert-primary').html("Coupon updated").fadeIn(1000);
                    });
                }
            })
            .catch(function (error) {
                notification.error(error.response.statusText);
            })
            .finally(function () {
                setTimeout(function () {
                    $('.coupon-alert').fadeOut(0).removeClass('alert-primary');
                }, 5000);
            });
    },

    deleteCoupon() {
        var form = $(this).closest('form');
        var rowData = $(this).closest('tr');
        var formData = Coupons.getFormData(form, rowData, "delete");
        var deleteRow = $(this).closest('tr');

        if (confirm("Do you really want to delete this coupon?")) {
            $('.coupon-status').fadeIn(500);
            axios.post(form[0].action, formData)
                .then(function (result) {
                    if (result.data === "delete_ok") {
                        $('.coupon-status').fadeOut(500, function () {
                            $('.coupon-alert').addClass('alert-danger').html("Coupon deleted").fadeIn(1000);
                            deleteRow.remove();
                        });
                    }
                })
                .catch(function (error) {
                    notification.error(error.response.statusText);
                })
                .finally(function () {
                    setTimeout(function () {
                        $('.coupon-alert').fadeOut(0).removeClass('alert-danger');
                    }, 5000);
                });
        }
    }
}