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

        Array.from(document.querySelectorAll(".jsSaveBudget")).forEach(function (el, i) {
            el.addEventListener("click", function () {
                let form = el.closest('form');

                let url = form.action;
                let model = new FormData();
                let budgetModel = {
                    amount: form.querySelector('#amount').value,
                    status: form.querySelector('select[name="statusBudget"]').value,
                    currency: form.querySelector('select[name="currencyBudget"]').value,
                    startDateTime: form.querySelector('#startDate').value,
                    finishDateTime: form.querySelector('#endDate').value,
                    budgetId: el.getAttribute('data-budget-id'),
                        __RequestVerificationToken: document.querySelector('input[name="__RequestVerificationToken"]').value
                };

                let error = inst.validateBudget(budgetModel);
                if (error != "") {
                    form.querySelector('#BudgetWarningMessage').innerHTML = error;
                } else {
                    form.querySelector('#BudgetWarningMessage').innerHTML = "";
                    model = inst.convertFormData(budgetModel);
                    document.querySelector(".loading-box").style.display = 'block';
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
                            document.querySelector(".loading-box").style.display = 'none';
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