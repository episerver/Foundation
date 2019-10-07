class MobileNavigation {
    constructor(params) {
        this.SearchBoxId = params.searchBoxId;
        this.OpenSearchBoxId = params.openSearchBoxId;
        this.CloseSearchBoxId = params.closeSearchBoxId;
        this.OpenSideBarId = params.openSideBarId;
        this.SideBarId = params.sideBarId;
    }

    OpenSearchBox() {
        var inst = this;
        $(inst.OpenSearchBoxId).click(function () {
            $(inst.SearchBoxId).fadeIn();
            $(inst.CloseSearchBoxId).fadeIn();
            $(inst.OpenSearchBoxId).hide();
        })
    }

    CloseSearchBox() {
        var inst = this;
        $(inst.CloseSearchBoxId).click(function () {
            $(inst.SearchBoxId).fadeOut();
            $(inst.CloseSearchBoxId).hide();
            $(inst.OpenSearchBoxId).show();
        })
    }

    OpenOffSideNavigation() {
        var inst = this;
        $(inst.OpenSideBarId).click(function () {
            var cart = $(inst.SideBarId).find('.jsCartBtn').first();
            if (cart.hasClass('active') && cart.attr('reload') == '1') {
                cart.click();
            }

            setTimeout(() => {
                $(inst.SideBarId).addClass('show-side-nav');
            }, 10);

            setTimeout(() => {
                $(inst.OpenSideBarId + " .hamburger-menu").removeClass('is-active');
            }, 500);
        });
    }

    CloseOffSideNavigation() {
        var inst = this;
        $('body').click(function (e) {
            if ($('.offside-navbar').is(':visible')) {
                if ($(e.target).parents('.offside-navbar').length == 0
                    && !$(e.target).hasClass('offside-navbar')
                    && (!$(e.target).hasClass('modal') && $(e.target).parents('.modal').length == 0)) {
                    if ($(inst.SideBarId).hasClass('show-side-nav')) {
                        if ($(e.target).parents(inst.SideBarId).length == 0) {
                            $(inst.SideBarId).addClass('hide-side-nav');
                            setTimeout(() => {
                                $(inst.SideBarId).removeClass('show-side-nav');
                                $(inst.SideBarId).removeClass('hide-side-nav');
                            }, 500);
                        }
                    }
                }
            }
        });
    }

    ExpandCollapseMenu() {
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

    OpenCartClick() {
        var inst = this;
        $('.jsOpenCartMobile').each(function (i, e) {
            $(e).click(function () {
                $(inst.OpenSideBarId).click();
                $(inst.SideBarId).find('.jsCartBtn').first().click();
            })
        })
    }


    Init() {
        var menus = $(this.SideBarId).children('.offside-navbar--nav').first().children('.offside-navbar--nav__item');
        $(this.SideBarId).css('max-width', 81 * menus.length + "px");
        this.OpenSearchBox();
        this.CloseSearchBox();
        this.OpenOffSideNavigation();
        this.CloseOffSideNavigation();
        this.ExpandCollapseMenu();
        this.OpenCartClick();
    }
}
