export default class Selection {
    init() {
        this.expand();
        this.collapse();
        this.itemClick();
    }

    expand() {
        var selections = $('.selection--cm');
        selections.each(function (i, e) {
            $(e).find('.selection--cm__expand').each(function (j, s) {
                $(s).click(function () {
                    var self = this
                    $(this).addClass('hidden');
                    $(this).siblings('.selection--cm__collapse').removeClass('hidden');
                    $(this).siblings('.selection--cm__dropdown').slideToggle('hidden');

                    selections.each(function () {
                        if (!this.contains(self)) {
                            $(this).find('.selection--cm__dropdown').each(function () {
                                $(this).slideUp();
                            });
                            $(this).find('.selection--cm__collapse').each(function () {
                                $(this).addClass('hidden');
                            });
                            $(this).find('.selection--cm__expand').each(function () {
                                $(this).removeClass('hidden');
                            });
                        }
                    })
                });
            });
        });
    }

    collapse() {
        var selections = $('.selection--cm');
        selections.each(function (i, e) {
            $(e).find('.selection--cm__collapse').each(function (j, s) {
                $(s).click(function () {
                    $(this).addClass('hidden');
                    $(this).siblings('.selection--cm__expand').removeClass('hidden');
                    $(this).siblings('.selection--cm__dropdown').slideToggle('hidden');
                });
            });
        });
    }

    itemClick() {
        $('.selection--cm').each(function (i, e) {
            $(e).children('li').each(function (j, s) {
                $(s).click(function (event) {
                    if ($(event.target).hasClass('jsFirstLi') || $(event.target).hasClass('jsFirstSpan')) {
                        var child = $(this).children('.jsExpandCollapse').not('.hidden');
                        child.click();
                    }
                })
            })
        })

        $('.offside-navbar--menu').each(function (i, e) {
            $(e).children('li').each(function (j, s) {
                $(s).click(function (event) {
                    if ($(event.target).hasClass('jsFirstLi') || $(event.target).hasClass('jsFirstSpan')) {
                        var child = $(this).children('.jsExpandCollapse').not('.hidden');
                        child.click();
                    }
                })
            })
        })
    }
}