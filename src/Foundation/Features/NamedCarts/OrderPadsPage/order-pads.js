export default class OrderPadsComponent {
    constructor(table) {

    }

    init() {
        let minusIcon = '<line x1="5" y1="12" x2="19" y2="12"></line>';
        let plusIcon = '<line x1="12" y1="5" x2="12" y2="19"></line><line x1="5" y1="12" x2="19" y2="12"></line>';

        let $table,
            $firstRows,
            $secondRows,
            $thirdRows,
            $expandFirstRowsBtn,
            $expandSecondRowsBtn;

        let firstRow = '.first-row';
        let secondRow = '.second-row';
        let thirdRow = '.third-row';

        $table = $(table);
        $firstRows = $(firstRow, $table); // $('.sub-organization-row');
        $secondRows = $(secondRow, $table); // $('.user-row');
        $thirdRows = $(thirdRow, $table); // $('.product-row');

        if ($table.length > 0) {
            if ($secondRows.length > 0) {
                $expandFirstRowsBtn = $firstRows.find('.btn-xs');

                if ($thirdRows.length > 0)
                    $expandSecondRowsBtn = $secondRows.find('.btn-xs');

                bindEvents();
            }
        }
    }

    expandUserRows(e) {
        e.preventDefault();
        let $this = $(this);
        let $thisIcon = $this.find('svg');
        let dataToExpandClassForUsers = $this.attr('data-expand');
        let $usersRows = $firstRows.siblings('.' + dataToExpandClassForUsers);

        if ($this.hasClass('js-second-row-collapsed')) {
            $thisIcon.addClass('feather-minus').removeClass('feather-plus');
            $thisIcon.html(minusIcon);
            $usersRows.addClass('tr-show');
            $this.removeClass('js-second-row-collapsed');
        }
        else {
            $usersRows.each(function () {

                let $this = $(this);
                let $btn = $this.find('.btn-xs');
                let $icon = $btn.find('svg');
                let dataToExpandClassForProducts = $btn.attr('data-expand');

                if (!$btn.hasClass('js-third-row-collapsed')) {
                    $firstRows.siblings('.' + dataToExpandClassForProducts).removeClass('tr-show');
                    $btn.addClass('js-third-row-collapsed');
                    $icon.addClass('feather-plus').removeClass('feather-minus');
                    $icon.html(plusIcon);
                }

                $this.removeClass('tr-show');
            });
            $thisIcon.addClass('feather-plus').removeClass('feather-minus');
            $thisIcon.html(plusIcon);
            $this.addClass('js-second-row-collapsed');
        }
    }

    expandProductRows(e) {
        e.preventDefault();
        let $this = $(this);
        let $thisIcon = $this.find('svg');
        let dataToExpandClassForProducts = $this.attr('data-expand');

        if ($this.hasClass('js-third-row-collapsed')) {
            $this.removeClass('js-third-row-collapsed');
            $thisIcon.addClass('feather-minus').removeClass('feather-plus');
            $thisIcon.html(minusIcon);
            $firstRows.siblings('.' + dataToExpandClassForProducts).addClass('tr-show');
        }
        else {
            $thisIcon.addClass('feather-plus').removeClass('feather-minus');
            $thisIcon.html(plusIcon);
            $firstRows.siblings('.' + dataToExpandClassForProducts).removeClass('tr-show');
            $this.addClass('js-third-row-collapsed');
        }
    }

    bindEvents() {
        $expandFirstRowsBtn.click(expandUserRows);

        if ($thirdRows.length > 0) {
            $expandSecondRowsBtn.click(expandProductRows);
        }
    }
};
