export default class B2bOrganization {
    init() {
        document.addEventListener('DOMContentLoaded', function () {
            Array.from(document.querySelectorAll(".js-cloner")).forEach(function (el, i) {
                el.addEventListener("click", function (e) {
                    e.preventDefault();

                    let rowToClone = el.parentNode.querySelector('.location-row');
                    let clone = rowToClone.cloneNode(true);
                    clone.querySelectorAll('input').forEach(function (clonedEl, clonedIndex) {

                        let nameAttr = clonedEl.getAttribute('name');
                        let arrNum = nameAttr.match(/\d+/);
                        let nr = arrNum ? arrNum[0] : 0;
                        let subStr = nameAttr.substring(0, nameAttr.indexOf(nr));
                        let endStr = nameAttr.substring(nameAttr.indexOf(nr) + 1, nameAttr.length);
                        let newName = subStr + (++nr) + endStr;
                        clonedEl.setAttribute('name', newName);
                        //console.log(newName);
                        //New Id
                        let idAttr = clonedEl.getAttribute('id');
                        let idAttrNum = nameAttr.match(/\d+/);
                        let idNr = idAttrNum ? idAttrNum[0] : 0;
                        let subIdStr = idAttr.substring(0, idAttr.indexOf(idNr));
                        let endIdStr = idAttr.substring(idAttr.indexOf(idNr) + 1, idAttr.length);
                        let newId = subIdStr + (++idNr) + endIdStr;
                        //console.log(newId);
                        clonedEl.setAttribute('id', newId);
                        clonedEl.value = '';

                        let validation = clonedEl.parentNode.lastChild;
                        console.log(validation);
                        //validation.setAttribute('data-valmsg-for', newName);
                    });
                    document.querySelector("#suborg-form").insertBefore(clone, rowToClone);
                });
            });

            function delegateSelector(selector, event, childSelector, handler) {
                let inst = this;
                var is = function (el, selector) {
                    return (el.matches || el.matchesSelector || el.msMatchesSelector || el.mozMatchesSelector || el.webkitMatchesSelector || el.oMatchesSelector).call(el, selector);
                };

                var elements = document.querySelectorAll(selector);
                [].forEach.call(elements, function (el, i) {
                    el.addEventListener(event, function (e) {
                        if (is(e.target, childSelector)) {
                            handler(e);
                        }
                    });
                });
            }


            delegateSelector('#suborg-form', "click", '.delete-address-icon', function (e) {
                let parent = e.target.closest('.location-row');
                    if (document.querySelector("#suborg-form").querySelectorAll(".location-row").length > 1) {
                        parent.style.display = "none";
                        parent.querySelector('input[name*=Name]').value = "removed";
                        parent.querySelector('input[name*=Street]').value = "0";
                        parent.querySelector('input[name*=City]').value = "0";
                        parent.querySelector('input[name*=PostalCode]').value = "0";
                        parent.querySelector('select[name*=CountryCode]').value = "0";
                        
                        parent.classList.remove('location-row')
                        parent.classList.add('location-row-removed');
                    }
            });


        }, false);

    }


}