export default class Dropdown {
    constructor(divId) {
        if (divId) {
            this.DivContainer = divId;
        } else {
            this.DivContainer = document;
        }
    }

    init() {
        this.expandCollapse();
        this.initShowSelectedText();
        this.selectItem();
        this.customizeDropdownMenu();
    }

    expandCollapse() {
        $(this.DivContainer).find('.dropdown').each(function (i, e) {
            $(e).children('.dropdown__selected').first().click(function () {
                var dropdown = $(this).siblings('.dropdown__group');
                if (dropdown.is(':visible')) {
                    dropdown.hide();
                } else {
                    dropdown.show();
                }
            });
        });
        $(document).on('click', function (e) {
            if (!($(e.target).parents('.dropdown').length > 0 || $(e.target).hasClass('.dropdown'))) {
                $('.dropdown__group').hide();
            }
        });
    }

    showSelected(e) {
        var selectedText = "";
        $(e).find('input:checked').each(function (j, s) {
            selectedText += $(s).parents('label').text() + ", ";
        });
        selectedText = selectedText.substr(0, selectedText.lastIndexOf(","));
        if (selectedText == "") selectedText = "Click to expand";
        $(e).find('.dropdown__selected .current').first().html(selectedText);
    }

    initShowSelectedText() {
        var inst = this;
        $(this.DivContainer).find('.dropdown').each(function (i, e) {
            inst.showSelected(e);
        });
    }

    selectItem() {
        var inst = this;
        $(this.DivContainer).find('.dropdown').each(function (i, e) {
            $(e).find('input').each(function (j, s) {
                $(s).change(function () {
                    inst.showSelected(e);
                    $('.dropdown__group').hide();
                });
            });
        });
    }

    customizeDropdownMenu() {
        // Prevent Bootstrap dropdown from closing when clicking inside it
        $('.dropdown-menu.dropdown-menu--customized').on('click', (e) => {
            e.stopPropagation();
        });

        // Enable Bootstrap tabs which are inside Bootstrap dropdown clickable 
        $('.dropdown-menu--customized > ul > li > a').on('click', function (event) {
            event.stopPropagation();
            $(this).tab('show');
        });
    }
}