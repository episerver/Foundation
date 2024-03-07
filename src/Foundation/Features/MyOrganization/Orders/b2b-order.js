export default class B2bOrder {
    init() {
        this.filterByStatus();
        this.approveOder();
    }

    filterByStatus() {
        if (document.querySelector('.jsFilterOrderByStatus') == null) return;
        document.querySelector('.jsFilterOrderByStatus').addEventListener("change", function (element) {
            var status = element.target.value;
            if (status == '') {
                Array.from(document.querySelectorAll(".jsOrderRow")).forEach(function (el, i) {
                    el.style.display = 'block'; 
                })
            } else {
                Array.from(document.querySelectorAll(".jsOrderRow")).forEach(function (el, i) {
                    if (el.classList.contains(status)) {
                        //el.style.display = 'block';
                        el.removeAttribute("style");
                    } else {
                        el.style.display = 'none';
                    }
                })
            }
        })
    }

    approveOder() {
        if (document.querySelector('.jsApproveOrder') == null) return;
        document.querySelector('.jsApproveOrder').addEventListener("click", function (element) {
            document.querySelector(".loading-box").style.display = 'block'; 
            var form = element.closest("form");
            var orderLink = element.getAttirbute("data-order-link");
            var data = { orderGroupId: orderLink };
            var postData = convertFormData(data);
            var url = form.getAttirbute("action");
            axios.post(url, postData)
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
                    document.querySelector(".loading-box").style.display = 'none';
                })
        })
    }
}