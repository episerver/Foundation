export default class OrderPadsComponent {
    constructor(table) {
        this.init();
    }

    init() {
        let inst = this;
        let $table,
            $firstRows,
            $secondRows,
            $thirdRows,
            $expandFirstRowsBtn,
            $expandSecondRowsBtn;

        let firstRow = '.first-row';
        let secondRow = '.second-row';
        let thirdRow = '.third-row';

        $table = document.querySelector(".table");
        if (document.querySelector(".table") == null) {
            return;
        }
        $firstRows = $table.querySelector(firstRow);   //$(firstRow, $table); // $('.sub-organization-row');
        $secondRows = $table.querySelector(secondRow); //$(secondRow, $table); // $('.user-row');
        $thirdRows = $table.querySelector(thirdRow); //$(thirdRow, $table); // $('.product-row');

        if ($table != null) {
            if ($secondRows != null) {
                $expandFirstRowsBtn = $firstRows.querySelector('.btn-xs');

                if ($thirdRows != null)
                    $expandSecondRowsBtn = $secondRows.querySelector('.btn-xs');

                inst.bindEvents();
            }
         }
        
    }





    bindEvents() {
        let minusIcon = '<line x1="5" y1="12" x2="19" y2="12"></line>';
        let plusIcon = '<line x1="12" y1="5" x2="12" y2="19"></line><line x1="5" y1="12" x2="19" y2="12"></line>';

        let inst = this;
        let $firstRows = document.querySelector(".table").querySelector(".first-row");
        let $expandFirstRowsBtn = $firstRows.querySelector(".btn-xs");

        $expandFirstRowsBtn.addEventListener("click", function (e) {
            let $this = this;
            let $thisIcon = $this.querySelector('svg');
            let dataToExpandClassForUsers = $this.getAttribute('data-expand');
            let $usersRows = $firstRows.parentNode.querySelectorAll('.second-row');

            if ($this.classList.contains('js-second-row-collapsed')) {

                $thisIcon.classList.add('feather-minus')
                $thisIcon.classList.remove('feather-plus');
                $thisIcon.innerHTML = minusIcon;
                $usersRows[0].classList.add('tr-show');
                $this.classList.remove('js-second-row-collapsed');
            }
            else {

                $usersRows.forEach(function (iUserRow) {

                    let $this = iUserRow.target;
                    let $btn = $this.querySelector('.btn-xs');
                    let $icon = $btn.querySelector('svg');
                    let dataToExpandClassForProducts = $btn.getAttribute('data-expand');

                    if (!$btn.classList.contains('js-third-row-collapsed')) {
                        $firstRows.parentNode.querySelector('.' + dataToExpandClassForProducts).classList.remove('tr-show');
                        $btn.classList.add('js-third-row-collapsed');
                        $icon.classList.add('feather-plus')
                        $icon.classList.remove('feather-minus');
                        $icon.innerHTML = plusIcon;
                    }

                    $this.classList.remove('tr-show');
                });
                document.querySelector($thisIcon);;
                $thisIcon.classList.add('feather-plus')
                $thisIcon.classList.remove('feather-minus');
                $thisIcon.innerHTML = plusIcon;
                $this.classList.add('js-second-row-collapsed');
            }
        });

        let $thirdRows = document.querySelector(".table").querySelector(".third-row")
        let $expandSecondRowsBtn = document.querySelector(".second-row").querySelector('.btn-xs');
        if ($thirdRows != null) {
            $expandSecondRowsBtn.addEventListener("click", function (e) {

                let $this = this;
                let $thisIcon = $this.querySelector('svg');
                let dataToExpandClassForProducts = $this.getAttribute('data-expand');

                if ($this.classList.contains('js-third-row-collapsed')) {
                    $this.classList.remove('js-third-row-collapsed');
                    $thisIcon.classList.add('feather-minus');
                    $thisIcon.classList.remove('feather-plus');
                    $thisIcon.innerHTML = minusIcon;
                    $firstRows.parentNode.querySelector('.third-row' ).classList.add('tr-show');
                }
                else {
                    $thisIcon.classList.add('feather-plus');
                    $thisIcon.classList.remove('feather-minus');
                    $thisIcon.innerHTML = plusIcon;
                    $firstRows.parentNode.querySelector('.third-row').classList.remove('tr-show');
                    $this.classList.add('js-third-row-collapsed');
                }
            });
        }
    }
};
