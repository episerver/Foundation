class Organization {
    init() {
        $(document).ready(function () {
            var $cloner = $('.js-cloner');

            $cloner.each(function () {
                $(this).click(function (e) {
                    var $this = $(this);

                    e.preventDefault();
                    var $rowToClone = $this.siblings('.location-row').last();
                    var $clone = $rowToClone.clone();
                    $clone.find('input').each(function () {
                        var $this = $(this);
                        //New Name
                        var nameAttr = $this.attr('name');
                        var arrNum = nameAttr.match(/\d+/);
                        var nr = arrNum ? arrNum[0] : 0;
                        var subStr = nameAttr.substring(0, nameAttr.indexOf(nr));
                        var endStr = nameAttr.substring(nameAttr.indexOf(nr) + 1, nameAttr.length);
                        var newName = subStr + (++nr) + endStr;
                        $this.attr('name', newName);
                        //New Id
                        var idAttr = $this.attr('id');
                        var idAttrNum = nameAttr.match(/\d+/);
                        var idNr = idAttrNum ? idAttrNum[0] : 0;
                        var subIdStr = idAttr.substring(0, idAttr.indexOf(idNr));
                        var endIdStr = idAttr.substring(idAttr.indexOf(idNr) + 1, idAttr.length);
                        var newId = subIdStr + (++idNr) + endIdStr;
                        $this.attr('id', newId);
                        $this.val('');

                        var validation = $this.siblings().last();
                        validation.attr('data-valmsg-for', newName);
                    });
                    $clone.insertBefore($this);
                });
            });

            $('#suborg-form').on('click', '.delete-address-icon', function (e) {
                e.preventDefault();

                var $deleteIcon = $(this);
                if ($('#suborg-form').find('.location-row').length > 1) {
                    var parent = $deleteIcon.closest('.location-row');
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