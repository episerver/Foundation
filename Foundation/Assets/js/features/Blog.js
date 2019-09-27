class Blog {
    init() {
        var inst = this;
        $(document).on("click", ".get-blog-comment", this.getBlogComment);

        $('.jsPaginateBlog').each(function (i, e) {
            $(e).click(function () {
                var data = $(this).attr('data');
                inst.changeBlogListPage(data);
            })
        })

        $('.jsPageSizeBlog').each(function (i, e) {
            $(e).click(function () {
                var data = $(this).attr('data');
                inst.changeBlogListPageSize(data);
            })
        })
    }

    getBlogComment(e) {
        e.preventDefault();
        var page = $(e.target).attr("pageIndex");
        this.changePageComment(page);
        return false;
    }

    changePageComment(page) {
        var form = $(document).find('.jsBlogPagingForm');
        $('#PageNumber').val(page);
        axios({
            method: 'post',
            url: "/BlogCommentBlock/GetComment",
            data: form.serialize()
        }).then(function (response) {
            $('#blogCommentBlock').replaceWith($(response).find('#blogCommentBlock'));
        }).catch(function (response) {
            console.log(response);
        });
    }

    changeBlogListPageSize(pageSize) {
        var form = $(document).find('.jsGetBlogItemListPage');
        $('#PageSize').val(pageSize);
        this.getBlogList();
    }

    changeBlogListPage(page) {
        var form = $(document).find('#jsGetBlogItemListPage');
        $('#PageNumber').val(page);
        this.getBlogList();
    }

    getBlogList() {
        var inst = this;
        var form = $(document).find('#jsGetBlogItemListPage');
        axios({
            method: 'post',
            url: "/BlogListBlock/GetItemList",
            data: form.serialize()
        }).then(function (response) {
            $('#blog-list').html($(response.data));
            $("html, body").animate({ scrollTop: 0 }, "slow");
            feather.replace();
            inst.init();
        }).catch(function (response) {
            console.log(response);
        });
    }

    toJson(form) {
        var o = {};
        var a = $(form).serializeArray();
        $.each(a, function () {
            if (o[this.name] !== undefined) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return JSON.stringify(o);
    }
}