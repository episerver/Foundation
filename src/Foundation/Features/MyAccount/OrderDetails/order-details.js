import * as $ from "jquery";
//declare var $: any;
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
        Array.from(document.querySelectorAll(".jsAddNote")).forEach(function (el, i) {
            el.addEventListener("click", function () {
                document.querySelector(".loading-box").style.display = 'block'; // show
                let form = el.closest('form');
                let url = form.getAttribute("action");
                let data = new FormData(form);
                axios.post(url, data)
                    .then(function (result) {
                        let newNote = inst.noteTemplate.replace("@title", result.data.title).replace("@type", result.data.type).replace("@detail", result.data.detail);
                        document.querySelector("#noteListing").insertAdjacentHTML("afterend" ,newNote);
                        form.reset();
                    })
                    .catch(function (error) {
                        notification.error(error);
                    })
                    .finally(function () {
                        document.querySelector(".loading-box").style.display = 'none'; // show
                    });

                return false;
            });
        });
    }

    initNote() {
        this.saveNoteClick();
    }

    returnItemClick() {
        Array.from(document.querySelectorAll(".jsReturnLineItem")).forEach(function (el, i) {
            el.addEventListener("click", function () {
                let btnSubmitModal = document.querySelector('#btnSubmitReturnOrder');
                btnSubmitModal.setAttribute("data-order-link", el.getAttribute('data-order-link'));
                btnSubmitModal.setAttribute("data-shipment-link", el.getAttribute('data-shipment-link'));
                btnSubmitModal.setAttribute("data-lineItem-link", el.getAttribute('data-lineitem-link'));
                btnSubmitModal.setAttribute("data-total-return", el.getAttribute('data-total-return'));

                let txtQuantity = document.querySelector('input[id="txtQuantity"]');
                txtQuantity.setAttribute("value", parseInt(btnSubmitModal.getAttribute("data-total-return")));
            });
        });
    }

    
    submitReturnItemClick() {
        Array.from(document.querySelectorAll(".jsCreateReturn")).forEach(function (el, i) {
            el.addEventListener("click",function () {
                document.querySelector(".loading-box").style.display = 'block'; // show
                let form = el.closest('form');
                let url = form.getAttribute("action");
                let data = new FormData(form);
                let itemId = el.getAttribute('data-lineitem-link');
                data.append("orderGroupId", el.getAttribute('data-order-link'));
                data.append("shipmentId", el.getAttribute('data-shipment-link'));
                data.append("lineItemId", el.getAttribute('data-lineitem-link'));
                data.append("returnQuantity", el.getAttribute('data-total-return'));
                var sel = document.getElementById("optReason");
                var text = sel.options[sel.selectedIndex].text;
                data.append("reason", text);
                axios.post(url, data)
                    .then(function (result) {
                        notification.success('Success');
                        document.querySelector('#return-' + itemId).setAttribute("disabled", "true");
                        //$("#returnSettingModal").modal("hide");
                    })
                    .catch(function (error) {
                        notification.error(error);
                    })
                    .finally(function () {
                        document.querySelector(".loading-box").style.display = 'none'; // show
                    });
            });
        });
    }

    

    initReturnOrder() {
        this.returnItemClick();
        this.submitReturnItemClick();
    }

}