export class ProductSearch {
    constructor() {
        // for filtering
        this.pageClass = "jsPaginate";
        this.sortClass = "jsSort";
        this.pageSizeClass = "jsPageSize";
        this.viewModeClass = "jsViewMode";
        this.sortDirectionClass = "jsSortDirection";
        this.facetClass = "jsFacet";

        this.params = "";
        this.rootUrl = window.location.href.substr(0, window.location.href.indexOf('?'));
        // to get information page
        this.page = ".jsPageInfo";
        this.pageSize = ".jsPageSizeInfo";
        this.sort = ".jsSortInfo";
        this.sortDirection = ".jsSortDirectionInfo";
        this.viewMode = ".jsViewModeInfo";
    }

    //Search, Filter handler
    paginate(page) {
        $(this.page).val(page);
        this.search();
    }

    removeTag(inputName) {
        $(`input[name='${inputName}']`).prop('checked', false);
        this.search();
    }

    removeAllTag() {
        $('.jsSearchFacet:input:checked').each(function (i, e) {
            $(e).removeAttr('checked');
        });
        this.search();
    }

    sort(sortBy) {
        $(this.sort).val(sortBy);
        this.search();
    }

    sortDirection(direction) {
        $(this.sortDirection).val(direction);
        this.search();
    }

    changePageSize(pageSize) {
        $(this.pageSize).val(pageSize);
        this.search();
    }

    changeViewMode(mode) {
        $(this.viewMode).val(mode);
        this.search();
    }

    getFilter() {
        let q = new FilterOption();
        q.page = $(this.page).val();
        q.pageSize = $(this.pageSize).val();
        q.sort = $(this.sort).val();
        q.sortDirection = $(this.sortDirection).val();
        q.ViewSwitcher = $(this.viewMode).val();

        this.params = this.getUrlWithFacets();

        return q;
    }

    search() {
        let inst = this;
        let data = this.getFilter();
        $('body>.loading-box').show();

        let expanding = document.querySelector('.selection--cm__collapse:not(.hidden)')
        let expandingFacetEl = expanding && expanding.closest('.selection--cm')
        let expandingFacet = expandingFacetEl && expandingFacetEl.dataset.facetkey

        axios({ url: inst.rootUrl + inst.params, params: { ...data }, method: 'get' })
            .then(function (result) {
                window.history.replaceState(null, null, inst.params == "" ? "?" : inst.params);
                $('.toolbar').replaceWith($(result.data).find('.toolbar'));
                $('.jsFacets').replaceWith($(result.data).find('.jsFacets'));
                $('.jsProducts').replaceWith($(result.data).find('.jsProducts'));
                feather.replace();
                new Selection().Init();
                if (expandingFacet) {
                    let ul = document.querySelector(`.selection--cm[data-facetkey=${expandingFacet}]`)
                    let dropdown = ul.querySelector('.selection--cm__dropdown')
                    let collapse = ul.querySelector('.selection--cm__collapse')
                    let expand = ul.querySelector('.selection--cm__expand')
                    dropdown.style.display = 'block'
                    collapse.classList.remove('hidden')
                    expand.classList.add('hidden')
                }
                let quickView = new ProductDetail('#quickView');
                quickView.InitQuickView();
                let product = new Product(".jsProducts");
                product.AddToCartClick();
                product.AddToWishlistClick();
                inst.Init();
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {
                $('.loading-box').hide();
            });
    }


    getUrlWithFacets() {
        let facets = [];
        $('.jsSearchFacet:input:checked').each(function () {
            let selectedFacet = encodeURIComponent($(this).data('facetkey'));
            facets.push(selectedFacet);
        });
        return this.getUrl(facets);
    }
    getUrl(facets) {
        let urlParams = this.getUrlParams();
        urlParams.facets = facets ? facets.join(',') : null;
        //let sort = $('.jsSearchSort')[0].value;
        urlParams.sort = '';
        let url = "?";
        for (let key in urlParams) {
            let value = urlParams[key];
            if (value) {
                url += key + '=' + value + '&';
            }
        }
        return url.slice(0, -1); //remove last char
    }
    getUrlParams() {
        let match,
            search = /([^&=]+)=?([^&]*)/g, //regex to find key value pairs in querystring
            query = window.location.search.substring(1);

        let urlParams = {};
        while (match = search.exec(query)) {
            urlParams[match[1]] = match[2];
        }
        return urlParams;
    }
    ///--- end

    init() {
        let inst = this;

        // Init filter
        $('.jsUpdatePage').each(function (i, e) {
            $(e).click(function () {
                let data = $(this).attr('data');
                if ($(this).hasClass(inst.pageClass)) {
                    inst.paginate(data);
                }

                if ($(this).hasClass(inst.pageSizeClass)) {
                    inst.changePageSize(data);
                }

                if ($(this).hasClass(inst.sortClass)) {
                    inst.sort(data);
                }

                if ($(this).hasClass(inst.sortDirectionClass)) {
                    inst.sortDirection(data);
                }

                if ($(this).hasClass(inst.viewModeClass)) {
                    inst.changeViewMode(data);
                }

                if ($(this).hasClass(inst.facetClass)) {
                    $(this).siblings('input.jsSearchFacet').first().prop('checked', true);
                }
            });
        });

        $('.jsSearchFacet:checkbox').each(function (i, e) {
            $(e).change(function () {
                inst.search();
            });
        });

        $('.jsSearchFacetRemoveAll').click(function () {
            inst.removeAllTag();
        });

        $('.jsRemoveTag').each(function (i, e) {
            $(e).click(function () {
                let name = $(this).siblings('.jsSearchFacetSelected').attr('name');
                inst.removeTag(name);
            });
        });
        //-- end
    }
}

class FilterOption {
    constructor() {
        this.page = 1;
        this.pageSize = 15;
        this.sort = "Position";
        this.sortDirection = "Asc";
        this.ViewSwitcher = "Grid";
    }
}

export class ContentSearch {
    changePageContent(page) {
        let search = new ProductSearch();
        let inst = this;
        let form = $(document).find('.jsSearchContentForm');
        $('.jsSearchContentPage').val(page);
        $('.jsSelectedFacet').val($(this).data('facetgroup') + ':' + $(this).data('facetkey'));
        let url = search.getUrlWithFacets();
        inst.updatePageContent(url, form.serialize(), null);
    }

    updatePageContent(url, data, onSuccess) {
        let inst = this;
        axios.post(url || "", data)
            .then(function (result) {
                $('#contentResult').replaceWith($(result.data).find('#contentResult'));
                inst.Init();
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {
                $('.loading-box').hide();
            });
    }

    init() {
        let inst = this;
        $('.jsChangePageContent').each(function (i, e) {
            $(e).click(function () {
                $('.loading-box').show();
                let page = $(this).attr('page');
                inst.changePageContent(page);
            });
        });
    }
}

export class NewProductsSearch {
    changePageContent(page) {
        let inst = this;
        let url = window.location.href + "?page=" + page;
        inst.updatePageContent(url);
    }

    updatePageContent(url) {
        let inst = this;
        axios.get(url || "")
            .then(function (result) {
                $('#new-products-page').replaceWith($(result.data).find('#new-products-page'));
                inst.Init();
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {
                $('.loading-box').hide();
            });
    }

    init() {
        let inst = this;
        $('.jsPaginateNewProductsPage').each(function (i, e) {
            $(e).click(function () {
                $('.loading-box').show();
                let page = $(this).attr('page');
                inst.changePageContent(page);
            });
        });
    }
}

export class SalesSearch {
    changePageContent(page) {
        let inst = this;
        let url = window.location.href + "?page=" + page;
        inst.updatePageContent(url);
    }

    updatePageContent(url) {
        let inst = this;
        axios.get(url || "")
            .then(function (result) {
                $('#sales-page').replaceWith($(result.data).find('#sales-page'));
                inst.Init();
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {
                $('.loading-box').hide();
            });
    }

    init() {
        let inst = this;
        $('.jsPaginateSalesPage').each(function (i, e) {
            $(e).click(function () {
                $('.loading-box').show();
                let page = $(this).attr('page');
                inst.changePageContent(page);
            });
        });
    }
}