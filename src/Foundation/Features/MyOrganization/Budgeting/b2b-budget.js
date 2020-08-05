export default class B2bBudget {
    constructor(container) {
        this.divContainer = container != undefined ? container : document;
    }

    validateBudget(data) {
        let message = "";
        if (isNaN(parseFloat(data.amount)) || parseFloat(data.amount) == 0) {
            message += "<p>Allocated is invalid.</p>";
        }
        if (data.startDateTime == undefined || data.startDateTime == "") {
            message += "<p>StartDate is invalid.</p>";
        }

        if (data.finishDateTime == undefined || data.finishDateTime == "") {
            message += "<p>DueDate is invalid.</p>";
        }

        if (data.finishDateTime < data.startDateTime) {
            message += "<p>StartDate and DueDate are invalid.</p>";
        }
        return message;
    }

    saveNewBudget() {
        let inst = this;
        $(this.divContainer).find('.jsSaveBudget').each(function (i, e) {
            $(e).click(function () {
                let form = $(e).closest('form');

                let url = form[0].action;
                let model = new FormData();
                let budgetModel = {
                    amount: $(form).find('#amount').val(),
                    status: $(form).find('select[name="statusBudget"]').val(),
                    currency: $(form).find('select[name="currencyBudget"]').val(),
                    startDateTime: $(form).find('#startDate').val(),
                    finishDateTime: $(form).find('#endDate').val(),
                    userEmail: $(form).find('#userEmail').val(),
                    budgetId: $(this).data('budget-id'),
                    __RequestVerificationToken: $(form).find('input[name="__RequestVerificationToken"]').val(),
                }

                let error = inst.validateBudget(budgetModel);
                if (error != "") {
                    $(form).find('#BudgetWarningMessage').html(error);
                } else {
                    $(form).find('#BudgetWarningMessage').html("");
                    model = inst.convertFormData(budgetModel);
                    $('.loading-box').show();
                    axios.post(url, model)
                        .then(function (result) {
                            if (result.data.result == "true") {
                                notification.success("Success");
                            } else {
                                notification.error(result.data.result);
                            }
                        })
                        .catch(function (error) {
                            notification.error(error);
                        })
                        .finally(function () {
                            $('.loading-box').hide();
                        });
                }
                
            });
        });
    }

    convertFormData(data) {
        let formData = new FormData();
        for (let key in data) {
            formData.append(key, data[key]);
        }
        return formData;
    }
}