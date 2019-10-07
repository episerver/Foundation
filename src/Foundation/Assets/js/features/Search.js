class ProductSearch {
    constructor() {
        // for filtering
        this.PageClass = "jsPaginate";
        this.SortClass = "jsSort";
        this.PageSizeClass = "jsPageSize";
        this.ViewModeClass = "jsViewMode";
        this.SortDirectionClass = "jsSortDirection";
        this.FacetClass = "jsFacet";

        this.Params = "";
        this.RootUrl = window.location.href.substr(0, window.location.href.indexOf('?'));
        // to get information page
        this.Page = ".jsPageInfo";
        this.PageSize = ".jsPageSizeInfo";
        this.Sort = ".jsSortInfo";
        this.SortDirection = ".jsSortDirectionInfo";
        this.ViewMode = ".jsViewModeInfo";
    }

    /// Search, Filter handler
    paginate(page) {
        $(this.Page).val(page);
        this.search();
    }

    removeTag(inputId) {
        $('#' + inputId).prop('checked', false);
        this.search();
    }

    removeAllTag() {
        $('.jsSearchFacet:input:checked').each(function (i, e) {
            $(e).removeAttr('checked');
        });
        this.search();
    }

    sort(sortBy) {
        $(this.Sort).val(sortBy);
        this.search();
    }

    sortDirection(direction) {
        $(this.SortDirection).val(direction);
        this.search();
    }

    changePageSize(pageSize) {
        $(this.PageSize).val(pageSize);
        this.search();
    }

    changeViewMode(mode) {
        $(this.ViewMode).val(mode);
        this.search();
    }

    getFilter() {
        var q = new FilterOption();
        q.Page = $(this.Page).val();
        q.PageSize = $(this.PageSize).val();
        q.Sort = $(this.Sort).val();
        q.SortDirection = $(this.SortDirection).val();
        q.ViewSwitcher = $(this.ViewMode).val();

        this.Params = this.getUrlWithFacets();

        return q;
    }

    search() {
        var inst = this;
        var data = this.getFilter();
        $('body>.loading-box').show();

        axios({ url: inst.RootUrl + inst.Params, params: { ...data }, method: 'get' })
            .then(function (result) {
                window.history.replaceState(null, null, inst.Params == "" ? "?" : inst.Params);
                $('.toolbar').replaceWith($(result.data).find('.toolbar'));
                $('.jsFacets').replaceWith($(result.data).find('.jsFacets'));
                $('.jsProducts').replaceWith($(result.data).find('.jsProducts'));
                feather.replace();
                //$('.selection--cm').find('.selection--cm__expand').off();
                //$('.selection--cm').find('.selection--cm__collapse').off();
                let selection = new Selection();
                selection.Init();
                var quickView = new ProductDetail('#quickView');
                quickView.InitQuickView();
                var product = new Product(".jsProducts");
                product.AddToCartClick();
                product.AddToWishlistClick();
                inst.Init();
            })
            .catch(function (error) {
                notification.Error(error);
            })
            .finally(function () {
                $('.loading-box').hide();
            });
    }


    getUrlWithFacets() {
        var facets = [];
        $('.jsSearchFacet:input:checked').each(function () {
            var selectedFacet = encodeURIComponent($(this).data('facetkey'));
            facets.push(selectedFacet);
        });
        return this.getUrl(facets);
    }
    getUrl(facets) {
        var urlParams = this.getUrlParams();
        urlParams.facets = facets ? facets.join(',') : null;
        //var sort = $('.jsSearchSort')[0].value;
        urlParams.sort = '';
        var url = "?";
        for (var key in urlParams) {
            var value = urlParams[key];
            if (value) {
                url += key + '=' + value + '&';
            }
        }
        return url.slice(0, -1); //remove last char
    }
    getUrlParams() {
        var match,
            search = /([^&=]+)=?([^&]*)/g, //regex to find key value pairs in querystring
            query = window.location.search.substring(1);

        var urlParams = {};
        while (match = search.exec(query)) {
            urlParams[match[1]] = match[2];
        }
        return urlParams;
    }
    ///--- end

    Init() {
        var inst = this;

        // Init filter
        $('.jsUpdatePage').each(function (i, e) {
            $(e).click(function () {
                var data = $(this).attr('data');
                if ($(this).hasClass(inst.PageClass)) {
                    inst.paginate(data);
                }

                if ($(this).hasClass(inst.PageSizeClass)) {
                    inst.changePageSize(data);
                }

                if ($(this).hasClass(inst.SortClass)) {
                    inst.sort(data);
                }

                if ($(this).hasClass(inst.SortDirectionClass)) {
                    inst.sortDirection(data);
                }

                if ($(this).hasClass(inst.ViewModeClass)) {
                    inst.changeViewMode(data);
                }

                if ($(this).hasClass(inst.FacetClass)) {
                    $(this).siblings('input.jsSearchFacet').first().prop('checked', true);
                }
            });
        });

        $('.jsSearchFacet:checkbox:not(:checked)').each(function (i, e) {
            $(e).change(function () {
                inst.search();
            });
        });

        $('.jsSearchFacetRemoveAll').click(function () {
            inst.removeAllTag();
        });

        $('.jsRemoveTag').each(function (i, e) {
            $(e).click(function () {
                var id = $(this).siblings('.jsSearchFacet').attr('id');
                inst.removeTag(id);
            });
        });
        //-- end
    }
}

class FilterOption {
    constructor() {
        this.Page = 1;
        this.PageSize = 15;
        this.Sort = "Position";
        this.SortDirection = "Asc";
        this.ViewSwitcher = "Grid";
    }
}

class ContentSearch {

    changePageContent(page) {
        var search = new ProductSearch();
        var inst = this;
        var form = $(document).find('.jsSearchContentForm');
        $('.jsSearchContentPage').val(page);
        $('.jsSelectedFacet').val($(this).data('facetgroup') + ':' + $(this).data('facetkey'));
        var url = search.getUrlWithFacets();
        inst.updatePageContent(url, form.serialize(), null);
    }

    updatePageContent(url, data, onSuccess) {
        axios.post(url || "", data)
            .then(function (result) {
                $('#contentResult').replaceWith($(result.data).find('#contentResult'));
                if (onSuccess) {
                    onSuccess(result);
                }
            })
            .catch(function (error) {
                notification.Error(error);
            })
            .finally(function () {
                $('.loading-box').hide();
            });
    }

    Init() {
        var inst = this;
        $('.jsChangePageContent').each(function (i, e) {
            $(e).click(function () {
                $('.loading-box').show();
                var page = $(this).attr('page');
                inst.changePageContent(page);
            });
        });
    }
}