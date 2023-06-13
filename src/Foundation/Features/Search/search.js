import feather from "feather-icons";
import Selection from "wwwroot/js/common/selection";
import Product from "Features/CatalogContent/product";
import ProductDetail from "Features/CatalogContent/product-detail";

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
        let queryStringPos = window.location.href.indexOf('?');
        if (queryStringPos === -1) {
            this.rootUrl = window.location.href;
        } else {
            this.rootUrl = window.location.href.substr(0, window.location.href.indexOf('?'));
        }
        // to get information page
        this.pageInfoClass = ".jsPageInfo";
        this.pageSizeInfoClass = ".jsPageSizeInfo";
        this.sortInfoClass = ".jsSortInfo";
        this.sortDirectionInfoClass = ".jsSortDirectionInfo";
        this.viewModeInfoClass = ".jsViewModeInfo";
    }

    init() {
        let inst = this;

        Array.from(document.querySelectorAll(".jsUpdatePage")).forEach(function (el, i) {
            el.addEventListener("click", function () {
                let data = el.getAttribute('data');
                if (el.classList.contains(inst.pageClass)) {
                    inst.paginate(data);
                }

                if (el.classList.contains(inst.pageSizeClass)) {
                    inst.changePageSize(data);
                }

                if (el.classList.contains(inst.sortClass)) {
                    inst.sort(data);
                }

                if (el.classList.contains(inst.sortDirectionClass)) {
                    inst.sortDirection(data);
                }

                if (el.classList.contains(inst.viewModeClass)) {
                    inst.changeViewMode(data);
                }

                if (el.classList.contains(inst.facetClass)) {
                    el.parentNode.querySelector('input.jsSearchFacet').setAttribute('checked', true);
                }
            });
        });

        if (document.querySelector(".jsSearchFacet") != null) {
            Array.from(document.querySelectorAll(".jsSearchFacet")).forEach(function (el, i) {
                el.addEventListener("change", function () {
                    inst.search();
                });
            });
        }

        if (document.querySelector(".jsSearchFacetRemoveAll") != null) {
            document.querySelector('.jsSearchFacetRemoveAll').addEventListener("click", function () {
                inst.removeAllTag();
            });
        }

        if (document.querySelector(".jsRemoveTag") != null) {
            Array.from(document.querySelectorAll(".jsRemoveTag")).forEach(function (el, i) {
                el.addEventListener("click", function () {
                    let name = el.parentNode.querySelector('.jsSearchFacetSelected').getAttribute('name');
                    inst.removeTag(name);
                });
            });
        }


        if (document.querySelector('ul#jsCategoriesFilter') != null) {
            Array.from(document.querySelector('ul#jsCategoriesFilter').querySelectorAll("li")).forEach(function (el, i) {
                if (el.querySelector('a').classList.contains('active') && el.querySelector('.selection--cm__dropdown') != null) {
                    el.querySelector('.selection--cm__dropdown').style.display = "block";
                    el.querySelector('.selection--cm__expand').classList.add("hidden");
                    el.querySelector('.selection--cm__collapse').classList.remove("hidden");
                }
            });
        }
    }

    // Search, Filter handler
    paginate(page) {
        document.querySelector(this.pageInfoClass).value = page;
        this.search();
    }

    removeTag(inputName) {
        Array.from(document.getElementsByName(inputName)).forEach(function (el, i) {
            el.setAttribute('checked', false);
        });
        this.search();
    }

    removeAllTag() {
        Array.from(document.querySelectorAll(".jsSearchFacet")).forEach(function (el, i) {
            if (el.checked == true)
                el.removeAttribute('checked');
        });
        this.search();
    }

    sort(sortBy) {
        document.querySelector(this.sortInfoClass).value = sortBy;
        this.search();
    }

    sortDirection(direction) {
        document.querySelector(this.sortDirectionInfoClass).value = direction;
        this.search();
    }

    changePageSize(pageSize) {
        document.querySelector(this.pageSizeInfoClass).value = pageSize;
        this.search();
    }

    changeViewMode(mode) {
        document.querySelector(this.viewModeInfoClass).value = mode;
        this.search();
    }

    getFilter() {
        let q = new FilterOption();
        q.page = document.querySelector(this.pageInfoClass).value;
        q.pageSize = document.querySelector(this.pageSizeInfoClass).value;
        q.sort = document.querySelector(this.sortInfoClass).value;
        q.sortDirection = document.querySelector(this.sortDirectionInfoClass).value;
        q.ViewSwitcher = document.querySelector(this.viewModeInfoClass).value;

        this.params = this.getUrlWithFacets();

        return q;
    }

    search() {
        let inst = this;
        let data = this.getFilter();
        document.querySelector(".loading-box").style.display = 'block'; // show

        let expanding = document.querySelector('.selection--cm__collapse:not(.hidden)')
        let expandingFacetEl = expanding && expanding.closest('.selection--cm')
        let expandingFacet = expandingFacetEl && expandingFacetEl.dataset.facetkey

        axios({ url: inst.rootUrl + inst.params, params: { ...data }, method: 'get' })
            .then(function (result) {
                window.history.replaceState(null, null, inst.params == "" ? "?" : inst.params);

                const doc = document.createElement("div");
                doc.innerHTML = result.data;
                
                document.querySelector('.toolbar').innerHTML = doc.querySelector('.toolbar').innerHTML;
                document.querySelector('.jsFacets').innerHTML = doc.querySelector('.jsFacets').innerHTML;
                document.querySelector('.jsProducts').innerHTML = doc.querySelector('.jsProducts').innerHTML;

                feather.replace();
                new Selection().init();
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
                quickView.initQuickView();
                let product = new Product(".jsProducts");
                product.addToCartClick();
                product.addToWishlistClick();
                inst.init();
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {
                document.querySelector(".loading-box").style.display = 'none';
            });
    }

    getUrlWithFacets() {
        let facets = [];
        Array.from(document.querySelectorAll(".jsSearchFacet")).forEach(function (el, i) {
            if (el.checked == true) {
                let selectedFacet = encodeURIComponent(el.getAttribute('data-facetkey'));
                facets.push(selectedFacet);
            }
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
    init() {
        let inst = this;
        Array.from(document.querySelectorAll(".jsChangePageContent")).forEach(function (el, i) {
            el.addEventListener("click", function () {
                document.querySelector(".loading-box").style.display = 'block';
                let page = $(this).attr('page');
                inst.changePageContent(page);
            });
        });
    }

    changePageContent(page) {
        let search = new ProductSearch();
        let inst = this;
        let form = document.querySelector('.jsSearchContentForm');
        document.querySelector('.jsSearchContentPage').value = page;
        document.querySelector('.jsSelectedFacet').value = el.getAttribute('data-facetgroup') + ':' + el.getAttribute("data-facetkey");
        let url = search.getUrlWithFacets();
        inst.updatePageContent(url, form.serialize(), null);
    }

    updatePageContent(url, data, onSuccess) {
        let inst = this;
        axios.post(url || "", data)
            .then(function (result) {
                const doc = document.createElement("div");
                doc.innerHTML = result.data;
                document.querySelector('#contentResult').innerHTML = doc.querySelector('#contentResult').innerHTML;
                inst.init();
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {
                document.querySelector(".loading-box").style.display = 'none';
            });
    }
}

export class NewProductsSearch {
    init() {
        let inst = this;
        Array.from(document.querySelectorAll(".jsPaginateNewProductsPage")).forEach(function (el, i) {
            el.addEventListener("click", function () {
                document.querySelector(".loading-box").style.display = 'block';
                let page = el.getAttribute('page');
                inst.changePageContent(page);
            });
        });
    }

    changePageContent(page) {
        let inst = this;
        let url = window.location.href + "?page=" + page;
        inst.updatePageContent(url);
    }

    updatePageContent(url) {
        let inst = this;
        axios.get(url || "")
            .then(function (result) {
                const doc = document.createElement("div");
                doc.innerHTML = result.data;
                document.querySelector('#new-products-page').innerHTML = doc.querySelector('#new-products-page').innerHTML;
                inst.init();
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {
                document.querySelector(".loading-box").style.display = 'none';
            });
    }
}

export class SalesSearch {
    init() {
        let inst = this;
        Array.from(document.querySelectorAll(".jsPaginateSalesPage")).forEach(function (el, i) {
            el.addEventListener("click", function () {
                document.querySelector(".loading-box").style.display = 'block';
                let page = el.getAttribute('page');
                inst.changePageContent(page);
            });
        });
    }

    changePageContent(page) {
        let inst = this;
        let url = window.location.href + "?page=" + page;
        inst.updatePageContent(url);
    }

    updatePageContent(url) {
        let inst = this;
        axios.get(url || "")
            .then(function (result) {
                const doc = document.createElement("div");
                doc.innerHTML = result.data;
                document.querySelector('#sales-page').innerHTML = doc.querySelector('#sales-page').innerHTML;
                inst.init();
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {
                document.querySelector(".loading-box").style.display = 'none';
            });
    }
}