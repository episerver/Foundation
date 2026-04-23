import * as $ from "jquery";

export default class Blog {
    init() {
        let inst = this;
        $(document).on("click", ".get-blog-comment", this.getBlogComment);

        $('.jsPaginateBlog').each(function (i, e) {
            $(e).click(function () {
                let data = $(this).attr('data');
                inst.changeBlogListPage(data);
            })
        })

        $('.jsPageSizeBlog').each(function (i, e) {
            $(e).click(function () {
                let data = $(this).attr('data');
                inst.changeBlogListPageSize(data);
            })
        })

        inst.loadMore();
    }

    getBlogComment(e) {
        e.preventDefault();
        let page = $(e.target).attr("pageIndex");
        this.changePageComment(page);
        return false;
    }

    changePageComment(page) {
        let form = $(document).find('.jsBlogPagingForm');
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
        $('#PageSize').val(pageSize);
        this.getBlogList();
    }

    changeBlogListPage(page) {
        $('#PageNumber').val(page);
        this.getBlogList();
    }

    loadMore() {
        let inst = this;
        $('.jsLoadMoreBlogs').click(function () {
            let pageNumber = $(this).attr('pageNumber');
            let pageCount = $(this).attr('pageCount');
            let newPageNumber = parseInt(pageNumber) + 1;
            let pageCountNum = parseInt(pageCount);
            if (newPageNumber > pageCountNum) {
                $(this).html('No more');
                $(this).attr("disabled", "disabled");
            } else {
                $('#PageNumber').val(newPageNumber);
                $(this).attr('pageNumber', newPageNumber);

                inst.getBlogList(function (response) {
                    $('.jsBlogListLoadMore').append($(response.data + " .jsBlogListLoadMore").html());
                });
            }
        })
    }

    getBlogList(callback) {
        let inst = this;
        let form = $(document).find('#jsGetBlogItemListPage');
        let url = form.find('#RequestUrl').val();
        if (url == undefined || url == "") {
            url = "/BlogListPage/GetItemList";
        }
        if (!callback) {
            callback = function (response) {
                $('#blog-list').html($(response.data));
                $("html, body").animate({ scrollTop: 0 }, "slow");
                feather.replace();
                inst.init();
            }
        }
        axios({
            method: 'post',
            url: url,
            data: form.serialize()
        })
            .then(callback)
            .catch(function (response) {
                console.log(response);
            });
    }

    toJson(form) {
        let o = {};
        let a = $(form).serializeArray();
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