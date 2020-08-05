export default class B2bOrganization {
    init() {
        $(document).ready(function () {
            let $cloner = $('.js-cloner');

            $cloner.each(function () {
                $(this).click(function (e) {
                    let $this = $(this);

                    e.preventDefault();
                    let $rowToClone = $this.siblings('.location-row').last();
                    let $clone = $rowToClone.clone();
                    $clone.find('input').each(function () {
                        let $this = $(this);
                        //New Name
                        let nameAttr = $this.attr('name');
                        let arrNum = nameAttr.match(/\d+/);
                        let nr = arrNum ? arrNum[0] : 0;
                        let subStr = nameAttr.substring(0, nameAttr.indexOf(nr));
                        let endStr = nameAttr.substring(nameAttr.indexOf(nr) + 1, nameAttr.length);
                        let newName = subStr + (++nr) + endStr;
                        $this.attr('name', newName);
                        //New Id
                        let idAttr = $this.attr('id');
                        let idAttrNum = nameAttr.match(/\d+/);
                        let idNr = idAttrNum ? idAttrNum[0] : 0;
                        let subIdStr = idAttr.substring(0, idAttr.indexOf(idNr));
                        let endIdStr = idAttr.substring(idAttr.indexOf(idNr) + 1, idAttr.length);
                        let newId = subIdStr + (++idNr) + endIdStr;
                        $this.attr('id', newId);
                        $this.val('');

                        let validation = $this.siblings().last();
                        validation.attr('data-valmsg-for', newName);
                    });
                    $clone.insertBefore($this);
                });
            });

            $('#suborg-form').on('click', '.delete-address-icon', function (e) {
                e.preventDefault();

                let $deleteIcon = $(this);
                if ($('#suborg-form').find('.location-row').length > 1) {
                    let parent = $deleteIcon.closest('.location-row');
                    parent.hide();
                    parent.find('input[name*=Name]').val("removed");
                    parent.find('input[name*=Street]').val("0");
                    parent.find('input[name*=City]').val("0");
                    parent.find('input[name*=PostalCode]').val("0");
                    parent.find('input[name*=Country]').val("0");
                    parent.removeClass('location-row').addClass('location-row-removed');
                }
            });
        });
    }
}