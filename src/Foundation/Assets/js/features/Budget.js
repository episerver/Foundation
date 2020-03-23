class Budget {
    constructor(container) {
        this.DivContainer = container != undefined ? container : document;
    }

    // Add new budget
    validateBudget(data) {
        var message = "";
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

    SaveNewBudget() {
        var inst = this;
        $(this.DivContainer).find('.jsSaveBudget').each(function (i, e) {
            $(e).click(function () {
                var form = $(e).closest('form');

                var url = form[0].action;
                var model = new FormData();
                var data = new BudgetModel();
                data.amount = $(form).find('#amount').val();
                data.status = $(form).find('select[name="statusBudget"]').val();
                data.currency = $(form).find('select[name="currencyBudget"]').val();
                data.startDateTime = $(form).find('#startDate').val();
                data.finishDateTime = $(form).find('#endDate').val();
                data.userEmail = $(form).find('#userEmail').val();
                data.budgetId = $(this).data('budget-id');
                data.__RequestVerificationToken = $(form).find('input[name="__RequestVerificationToken"]').val();
                var error = inst.validateBudget(data);
                if (error != "") {
                    $(form).find('#BudgetWarningMessage').html(error);
                } else {
                    $(form).find('#BudgetWarningMessage').html("");
                    model = inst.convertFormData(data);
                    $('.loading-box').show();
                    axios.post(url, model)
                        .then(function (result) {
                            if (result.data.result == "true") {
                                notification.Success("Success");
                            } else {
                                notification.Error(result.data.result);
                            }
                        })
                        .catch(function (error) {
                            notification.Error(error);
                        })
                        .finally(function () {
                            $('.loading-box').hide();
                        });
                }
                
            });
        });
    }
    // End add new budget

    convertFormData(data) {
        var formData = new FormData();
        for (var key in data) {
            formData.append(key, data[key]);
        }
        return formData;
    }
}

class BudgetModel {
    constructor() {
        this.__RequestVerificationToken = "";
        this.amount = "";
        this.startDateTime = "";
        this.finishDateTime = "";
        this.status = "";
        this.currency = "";
        this.budgetId = "";
        this.userEmail = "";
    }
    
}