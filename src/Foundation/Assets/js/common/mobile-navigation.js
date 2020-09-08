export default class MobileNavigation {
    constructor(params) {
        this.searchBoxId = params.searchBoxId;
        this.openSearchBoxId = params.openSearchBoxId;
        this.closeSearchBoxId = params.closeSearchBoxId;
        this.openSideBarId = params.openSideBarId;
        this.sideBarId = params.sideBarId;
    }

    init() {
        var menus = $(this.sideBarId).children('.offside-navbar--nav').first().children('.offside-navbar--nav__item');
        $(this.sideBarId).css('max-width', 81 * menus.length + "px");
        this.openSearchBox();
        this.closeSearchBox();
        this.openOffSideNavigation();
        this.closeOffSideNavigation();
        this.expandCollapseMenu();
        this.openCartClick();
    }

    openSearchBox() {
        var inst = this;
        $(inst.openSearchBoxId).click(function () {
            $(inst.searchBoxId).fadeIn();
            $(inst.closeSearchBoxId).fadeIn();
            $(inst.openSearchBoxId).hide();
        })
    }

    closeSearchBox() {
        var inst = this;
        $(inst.closeSearchBoxId).click(function () {
            $(inst.searchBoxId).fadeOut();
            $(inst.closeSearchBoxId).hide();
            $(inst.openSearchBoxId).show();
        })
    }

    openOffSideNavigation() {
        var inst = this;
        $(inst.openSideBarId).click(function () {
            var cart = $(inst.sideBarId).find('.jsCartBtn').first();
            if (cart.hasClass('active') && cart.attr('reload') == '1') {
                cart.click();
            }

            setTimeout(() => {
                $(inst.sideBarId).addClass('show-side-nav');
            }, 10);

            setTimeout(() => {
                $(inst.openSideBarId + " .hamburger-menu").removeClass('is-active');
            }, 500);
        });
    }

    closeOffSideNavigation() {
        var inst = this;
        $('body').click(function (e) {
            if ($('.offside-navbar').is(':visible')) {
                if ($(e.target).parents('.offside-navbar').length == 0
                    && !$(e.target).hasClass('offside-navbar')
                    && (!$(e.target).hasClass('modal') && $(e.target).parents('.modal').length == 0)) {
                    if ($(inst.sideBarId).hasClass('show-side-nav')) {
                        if ($(e.target).parents(inst.sideBarId).length == 0) {
                            $(inst.sideBarId).addClass('hide-side-nav');
                            setTimeout(() => {
                                $(inst.sideBarId).removeClass('show-side-nav');
                                $(inst.sideBarId).removeClass('hide-side-nav');
                            }, 500);
                        }
                    }
                }
            }
        });
    }

    expandCollapseMenu() {
        $('.offside-navbar--menu__item .expand-collapse-child').each(function (i, e) {
            $(e).click(function () {
                $(e).addClass('hidden');
                if ($(e).hasClass('expanded')) {
                    $(e).siblings('.collapsed').removeClass('hidden');
                    $(e).siblings('.child-menu').show();
                    $(e).parents('.offside-navbar--menu__item').first().addClass('expanded');
                } else {
                    $(e).siblings('.expanded').removeClass('hidden');
                    $(e).siblings('.child-menu').hide();
                    $(e).parents('.offside-navbar--menu__item').first().removeClass('expanded');
                }
            })
        })
    }

    openCartClick() {
        var inst = this;
        $('.jsOpenCartMobile').each(function (i, e) {
            $(e).click(function () {
                $(inst.openSideBarId).click();
                $(inst.sideBarId).find('.jsCartBtn').first().click();
            })
        })
    }
}