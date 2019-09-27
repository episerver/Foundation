/*!
 * Mvc.Grid 7.0.1
 * https://github.com/NonFactors/MVC5.Grid
 *
 * Copyright © NonFactors
 *
 * Licensed under the terms of the MIT License
 * http://www.opensource.org/licenses/mit-license.php
 */
var MvcGrid = (function () {
    function MvcGrid(element, options) {
        var grid = this;
        options = options || {};
        element = grid.findGrid(element);
        if (element.dataset.id) {
            return grid.instances[parseInt(element.dataset.id)].set(options);
        }

        grid.columns = [];
        grid.element = element;
        grid.loadingDelay = 300;
        grid.requestType = 'get';
        grid.name = element.dataset.name;
        grid.popup = new MvcGridPopup(grid);
        grid.prefix = grid.name ? grid.name + '-' : '';
        grid.sourceUrl = grid.element.dataset.sourceUrl;
        grid.element.dataset.id = options.id || grid.instances.length;
        grid.filterMode = (element.dataset.filterMode || 'excel').toLowerCase();
        grid.filters = {
            'enum': MvcGridEnumFilter,
            'date': MvcGridDateFilter,
            'guid': MvcGridGuidFilter,
            'text': MvcGridTextFilter,
            'number': MvcGridNumberFilter,
            'boolean': MvcGridBooleanFilter
        };

        var rowFilters = element.querySelectorAll('.mvc-grid-row-filters th');
        [].forEach.call(element.querySelectorAll('.mvc-grid-headers th'), function (header, i) {
            grid.columns.push(new MvcGridColumn(grid, header, rowFilters[i]));
        });

        var pager = element.querySelector('.mvc-grid-pager');
        if (pager) {
            grid.pager = new MvcGridPager(grid, pager);
        }

        if (options.id) {
            grid.instances[parseInt(options.id)] = grid;
        } else {
            grid.instances.push(grid);
        }

        grid.set(options);
        grid.cleanUp();
        grid.bind();

        if (grid.sourceUrl && !element.children.length) {
            grid.reload();
        }
    }

    MvcGrid.prototype = {
        instances: [],
        lang: {
            text: {
                'contains': 'Contains',
                'equals': 'Equals',
                'not-equals': 'Not equals',
                'starts-with': 'Starts with',
                'ends-with': 'Ends with'
            },
            number: {
                'equals': 'Equals',
                'not-equals': 'Not equals',
                'less-than': 'Less than',
                'greater-than': 'Greater than',
                'less-than-or-equal': 'Less than or equal',
                'greater-than-or-equal': 'Greater than or equal'
            },
            date: {
                'equals': 'Equals',
                'not-equals': 'Not equals',
                'earlier-than': 'Earlier than',
                'later-than': 'Later than',
                'earlier-than-or-equal': 'Earlier than or equal',
                'later-than-or-equal': 'Later than or equal'
            },
            enum: {
                'equals': 'Equals',
                'not-equals': 'Not equals'
            },
            guid: {
                'equals': 'Equals',
                'not-equals': 'Not equals'
            },
            boolean: {
                'equals': 'Equals',
                'not-equals': 'Not equals'
            },
            filter: {
                'apply': '&#10003;',
                'remove': '&#10008;'
            },
            operator: {
                'select': '',
                'and': 'and',
                'or': 'or'
            }
        },

        findGrid: function (element) {
            var grid = element;

            if (!grid) {
                throw new Error('Grid element was not specified.');
            }

            while (grid && !grid.classList.contains('mvc-grid')) {
                grid = grid.parentElement;
            }

            if (!grid) {
                throw new Error('Grid can only be created from within mvc-grid structure.');
            }

            return grid;
        },

        set: function (options) {
            var grid = this;
            var filters = options.filters || {};

            for (var key in filters) {
                grid.filters[key] = filters[key];
            }

            grid.requestType = options.requestType || grid.requestType;
            grid.sourceUrl = options.sourceUrl === undefined ? grid.sourceUrl : options.sourceUrl;
            grid.loadingDelay = options.loadingDelay === undefined ? grid.loadingDelay : options.loadingDelay;

            if (grid.sourceUrl) {
                var urlsParts = grid.sourceUrl.split('?', 2);
                grid.sourceUrl = urlsParts[0];

                if (options.query !== undefined) {
                    grid.query = new MvcGridQuery(options.query);
                } else if (urlsParts[1] || !grid.query) {
                    grid.query = new MvcGridQuery(urlsParts[1]);
                }
            } else if (options.query === undefined) {
                grid.query = new MvcGridQuery(window.location.search);
            } else {
                grid.query = new MvcGridQuery(options.query);
            }

            grid.columns.forEach(function (column) {
                column.updateFilter();

                if (column.filter && grid.filters[column.filter.name]) {
                    column.filter.instance = new grid.filters[column.filter.name](column);
                    column.filter.instance.init();
                }
            });

            return this;
        },

        reload: function () {
            var grid = this;

            grid.dispatchEvent('reloadstart', { grid: grid });

            if (grid.sourceUrl) {
                grid.startLoading(function (result) {
                    var parent = grid.element.parentElement;
                    var i = [].indexOf.call(parent.children, grid.element);

                    grid.element.outerHTML = result;

                    if (!parent.children[i].classList.contains('mvc-grid')) {
                        throw new Error('Grid partial should only include grid declaration.');
                    }

                    var newGrid = new MvcGrid(parent.children[i], {
                        loadingDelay: grid.loadingDelay,
                        requestType: grid.requestType,
                        query: grid.query.toString(),
                        id: grid.element.dataset.id,
                        sourceUrl: grid.sourceUrl,
                        filters: grid.filters
                    });

                    newGrid.dispatchEvent('reloadend', { grid: newGrid });
                }, function (result) {
                    grid.dispatchEvent('reloadfail', { grid: grid, result: result });
                });
            } else {
                window.location.href = window.location.origin + window.location.pathname + grid.query;
            }
        },
        applyFilters: function (initiator) {
            var grid = this;
            var query = grid.query;
            var prefix = grid.prefix;
            var sort = grid.query.get(prefix + 'sort');
            var order = grid.query.get(prefix + 'order');

            grid.clearQuery();

            grid.columns.filter(function (column) {
                return column.filter && (column == initiator || column.filter.first.values[0]);
            }).forEach(function (column) {
                var filter = column.filter;

                query.append(prefix + column.name + '-' + filter.first.method, filter.first.values[0]);

                for (var i = 1; filter.type == 'multi' && i < filter.first.values.length; i++) {
                    query.append(prefix + column.name + '-' + filter.first.method, filter.first.values[i]);
                }

                if (grid.filterMode == 'excel' && filter.type == 'double') {
                    query.append(prefix + column.name + '-op', filter.operator);
                    query.append(prefix + column.name + '-' + filter.second.method, filter.second.values[0]);
                }
            });

            if (sort) {
                query.append(prefix + 'sort', sort);
            }

            if (order) {
                query.append(prefix + 'order', order);
            }

            if (grid.pager && grid.pager.showPageSizes) {
                query.append(prefix + 'rows', grid.pager.rowsPerPage.value);
            }

            grid.reload();
        },
        clearQuery: function () {
            var query = this.query;
            var prefix = this.prefix;

            this.columns.forEach(function (column) {
                query.deleteAllStartingWith(prefix + column.name + '-');
            });

            query.delete(prefix + 'order');
            query.delete(prefix + 'sort');
            query.delete(prefix + 'page');
            query.delete(prefix + 'rows');
        },
        startLoading: function (success, error) {
            var grid = this;
            var query = (grid.query.toString() ? grid.query + '&' : '?') + '_=' + Date.now();

            grid.stopLoading();
            if (grid.loadingDelay != null && !grid.element.querySelector('.mvc-grid-loader')) {
                var content = document.createElement('div');
                content.appendChild(document.createElement('div'));
                content.appendChild(document.createElement('div'));
                content.appendChild(document.createElement('div'));

                grid.loader = document.createElement('div');
                grid.loader.className = 'mvc-grid-loader';
                grid.loader.appendChild(content);

                grid.loading = setTimeout(function () {
                    grid.loader.classList.add('mvc-grid-loading');
                }, grid.loadingDelay);

                grid.element.appendChild(grid.loader);
            }

            grid.request = new XMLHttpRequest();
            grid.request.open(grid.requestType, grid.sourceUrl + query, true);
            grid.request.setRequestHeader('X-Requested-With', 'XMLHttpRequest');

            grid.request.onload = function () {
                if (200 <= grid.request.status && grid.request.status < 400) {
                    success(grid.request.responseText);
                } else if (error) {
                    error(grid.request.responseText);
                }
            };

            grid.request.onerror = error;

            grid.request.send();
        },
        stopLoading: function () {
            var grid = this;

            if (grid.request && grid.request.readyState != 4) {
                grid.request.abort();
            }

            clearTimeout(grid.loading);

            if (grid.loader) {
                grid.loader.parentElement.removeChild(grid.loader);
            }
        },

        dispatchEvent: function (type, detail) {
            var event;

            if (typeof Event === 'function') {
                event = new CustomEvent(type, {
                    detail: detail,
                    bubbles: true
                });
            } else {
                event = document.createEvent('Event');
                event.initEvent(type, true, false);
                event.detail = detail;
            }

            this.element.dispatchEvent(event);
        },
        bind: function () {
            var grid = this;

            [].forEach.call(grid.element.querySelectorAll('tbody tr'), function (row) {
                if (!row.classList.contains('mvc-grid-empty-row')) {
                    row.addEventListener('click', function (e) {
                        var data = {};
                        var typedEvent;
                        var detail = { grid: grid, data: data, originalEvent: e };

                        grid.columns.forEach(function (column, i) {
                            data[column.name] = row.cells[i].innerText;
                        });

                        if (typeof Event === 'function') {
                            typedEvent = new CustomEvent('rowclick', {
                                detail: detail,
                                bubbles: true
                            });
                        } else {
                            typedEvent = document.createEvent('Event');
                            typedEvent.initEvent('rowclick', true, false);
                            typedEvent.detail = detail;
                        }

                        this.dispatchEvent(typedEvent);
                    });
                }
            });
        },

        cleanUp: function () {
            delete this.element.dataset.sourceUrl;
            delete this.element.dataset.filterMode;
        }
    };

    return MvcGrid;
})();

var MvcGridColumn = (function () {
    function MvcGridColumn(grid, header, rowFilter) {
        var column = this;
        var data = header.dataset;

        column.grid = grid;
        column.header = header;
        column.name = data.name;
        column.rowFilter = rowFilter;
        column.isHidden = header.classList.contains('mvc-grid-hidden');

        if (data.filter == 'True' && data.filterName) {
            var options = header.querySelector('.mvc-grid-options');

            if (grid.filterMode == 'row') {
                options = rowFilter.querySelector('select');
            }

            if (options && options.classList.contains('mvc-grid-options')) {
                options.parentElement.removeChild(options);
            }

            column.filter = {
                button: (column.rowFilter || column.header).querySelector('.mvc-grid-filter'),
                inlineInput: rowFilter ? rowFilter.querySelector('.mvc-grid-value') : null,
                hasOptions: options && options.children.length > 0,
                type: data.filterType.toLowerCase() || 'single',
                defaultMethod: data.filterDefaultMethod,
                isApplied: data.filterApplied == 'True',
                name: data.filterName,
                options: options
            };

            column.bindFilter();
        }

        if (data.sort == 'True') {
            column.sort = {
                button: column.header.querySelector('.mvc-grid-sort'),
                order: data.sortOrder.toLowerCase(),
                first: data.sortFirst
            };

            column.bindSort();
        }

        column.cleanUp();
    }

    MvcGridColumn.prototype = {
        cancelFilter: function () {
            var column = this;
            var grid = column.grid;

            if (column.filter.isApplied) {
                grid.query.delete(grid.prefix + 'page');
                grid.query.delete(grid.prefix + 'rows');
                grid.query.deleteAllStartingWith(grid.prefix + column.name + '-');

                grid.reload();
            } else {
                column.filter.first.values = [];
                column.filter.second.values = [];

                if (column.grid.filterMode != 'excel') {
                    column.filter.inlineInput.value = '';
                }
            }
        },
        applySort: function () {
            var column = this;
            var grid = column.grid;

            grid.query.delete(grid.prefix + 'sort');
            grid.query.delete(grid.prefix + 'order');

            var order = column.sort.order == 'asc' ? 'desc' : 'asc';
            if (!column.sort.order && column.sort.first) {
                order = column.sort.first;
            }

            grid.query.append(grid.prefix + 'sort', column.name);
            grid.query.append(grid.prefix + 'order', order);

            grid.reload();
        },

        updateFilter: function () {
            var column = this;
            var filter = column.filter;
            var query = column.grid.query;
            var name = column.grid.prefix + column.name + '-';

            if (filter) {
                var parameters = query.entries().filter(function (parameter) {
                    return parameter.split('=', 1) != name + 'op' && parameter.indexOf(name) == 0;
                });
                var methods = parameters.map(function (parameter) {
                    return decodeURIComponent(parameter.split('=', 1)[0].substring(name.length) || '');
                });
                var values = parameters.map(function (parameter) {
                    return decodeURIComponent(parameter.split('=', 2)[1] || '');
                });

                filter.first = {
                    method: methods[0] || '',
                    values: filter.type == 'multi' ? values : values.slice(0, 1)
                };

                filter.operator = filter.type == 'double' && query.get(name + 'op') || '';

                filter.second = {
                    method: filter.type == 'double' ? methods[1] || '' : '',
                    values: filter.type == 'double' ? values.slice(1, 2) : []
                };
            }
        },

        bindFilter: function () {
            var column = this;
            var filter = column.filter;

            filter.button.addEventListener('click', function () {
                filter.instance.show();
            });

            if (filter.hasOptions) {
                if (column.grid.filterMode == 'row' && filter.type != 'multi') {
                    column.filter.inlineInput.addEventListener('change', function () {
                        filter.first.values = [this.value];

                        filter.instance.apply();
                    });
                } else if (column.grid.filterMode == 'header' || column.grid.filterMode == 'row') {
                    column.filter.inlineInput.addEventListener('click', function () {
                        if (this.selectionStart == this.selectionEnd) {
                            filter.instance.show();
                        }
                    });
                }
            } else if (column.grid.filterMode != 'excel') {
                column.filter.inlineInput.addEventListener('input', function () {
                    filter.first.values = [this.value];

                    filter.instance.validate(this);
                });

                column.filter.inlineInput.addEventListener('keyup', function (e) {
                    if (e.which == 13 && filter.instance.isValid(this.value)) {
                        filter.instance.apply();
                    }
                });
            }
        },
        bindSort: function () {
            var column = this;

            if (!column.filter || column.grid.filterMode != 'header') {
                column.header.addEventListener('click', function (e) {
                    if (!/mvc-grid-(sort|filter)/.test(e.target.className)) {
                        column.applySort();
                    }
                });
            }

            column.sort.button.addEventListener('click', function () {
                column.applySort();
            });
        },

        cleanUp: function () {
            var data = this.header.dataset;

            delete data.filterDefaultMethod;
            delete data.filterApplied;
            delete data.filterType;
            delete data.filterName;
            delete data.filter;

            delete data.sortOrder;
            delete data.sortFirst;
            delete data.sort;

            delete data.name;
        }
    };

    return MvcGridColumn;
})();

var MvcGridPager = (function () {
    function MvcGridPager(grid, element) {
        var pager = this;

        pager.grid = grid;
        pager.element = element;
        pager.pages = element.querySelectorAll('[data-page]');
        pager.showPageSizes = element.dataset.showPageSizes == 'True';
        pager.rowsPerPage = element.querySelector('.mvc-grid-pager-rows');
        pager.currentPage = pager.pages.length ? parseInt(element.querySelector('.active').dataset.page) : 1;

        pager.cleanUp();
        pager.bind();
    }

    MvcGridPager.prototype = {
        apply: function (page) {
            var grid = this.grid;

            grid.query.delete(grid.prefix + 'page');
            grid.query.delete(grid.prefix + 'rows');

            grid.query.append(grid.prefix + 'page', page);

            if (this.showPageSizes) {
                grid.query.append(grid.prefix + 'rows', this.rowsPerPage.value);
            }

            grid.reload();
        },

        bind: function () {
            var pager = this;

            [].forEach.call(pager.pages, function (page) {
                page.addEventListener('click', function () {
                    pager.apply(this.dataset.page);
                });
            });

            pager.rowsPerPage.addEventListener('change', function () {
                pager.apply(pager.currentPage);
            });
        },

        cleanUp: function () {
            delete this.element.dataset.showPageSizes;
        }
    };

    return MvcGridPager;
})();

var MvcGridPopup = (function () {
    function MvcGridPopup(grid) {
        this.element.className = 'mvc-grid-popup';
        this.grid = grid;

        this.bind();
    }

    MvcGridPopup.prototype = {
        lastActiveElement: null,
        element: document.createElement('div'),

        render: function (filter) {
            this.element.className = ('mvc-grid-popup ' + filter.cssClasses).trim();
            this.element.innerHTML = filter.render();

            this.updateValues(filter.column);
        },
        updatePosition: function (column) {
            var popup = this;
            var filter = column.filter.button;
            var width = popup.element.clientWidth;
            var filterPos = filter.getBoundingClientRect();
            var arrow = popup.element.querySelector('.popup-arrow');

            var top = window.pageYOffset + filterPos.top + filter.offsetHeight * 0.6 + arrow.offsetHeight;
            var left = window.pageXOffset + filterPos.left - 8;
            var arrowLeft = filter.offsetWidth / 2;

            if (left + width + 8 > window.pageXOffset + document.documentElement.clientWidth) {
                var offset = width - filter.offsetWidth - 16;
                arrowLeft += offset;
                left -= offset;
            }

            popup.element.style.left = left + 'px';
            popup.element.style.top = top + 'px';
            arrow.style.left = arrowLeft + 'px';
        },
        updateValues: function (column) {
            var popup = this;
            var filter = column.filter;

            popup.setValues('.mvc-grid-operator', [filter.operator]);
            popup.setValues('.mvc-grid-value[data-filter="first"]', filter.first.values);
            popup.setValues('.mvc-grid-value[data-filter="second"]', filter.second.values);
            popup.setValues('.mvc-grid-method[data-filter="first"]', [filter.first.method]);
            popup.setValues('.mvc-grid-method[data-filter="second"]', [filter.second.method]);
        },
        setValues: function (selector, values) {
            var input = this.element.querySelector(selector);

            if (input) {
                if (input.tagName == 'SELECT' && input.multiple) {
                    [].forEach.call(input.options, function (option) {
                        option.selected = values.indexOf(option.value) >= 0;
                    });
                } else {
                    input.value = values[0] || '';
                }
            }
        },

        show: function (column) {
            var popup = this;

            MvcGridPopup.prototype.lastActiveElement = document.activeElement;

            if (!popup.element.parentElement) {
                document.body.appendChild(popup.element);
            }

            popup.updatePosition(column);

            popup.element.querySelector('.mvc-grid-value').focus();
        },
        hide: function (e) {
            var target = e && e.target;
            var popup = MvcGridPopup.prototype;

            while (target && !/mvc-grid-(popup|filter)/.test(target.className)) {
                target = target.parentElement;
            }

            if ((!target || e.which == 27) && popup.element.parentNode && (!e || e.target != window)) {
                document.body.removeChild(popup.element);

                if (popup.lastActiveElement) {
                    popup.lastActiveElement.focus();
                    popup.lastActiveElement = null;
                }
            }
        },

        bind: function () {
            var popup = this;

            window.addEventListener('resize', popup.hide);
            window.addEventListener('keydown', popup.hide);
            window.addEventListener('mousedown', popup.hide);
            window.addEventListener('touchstart', popup.hide);
        }
    };

    return MvcGridPopup;
})();

var MvcGridQuery = (function () {
    function MvcGridQuery(value) {
        this.parameters = (value || '').replace('?', '').split('&').filter(Boolean);
    }

    MvcGridQuery.prototype = {
        entries: function () {
            return this.parameters.slice();
        },

        get: function (name) {
            return this.parameters.filter(function (parameter) {
                return parameter == name || parameter.indexOf(name + '=') == 0;
            }).map(function (parameter) {
                return decodeURIComponent(parameter.split('=', 2)[1]);
            })[0];
        },
        set: function (name, value) {
            this.delete(name);
            this.append(name, value);
        },

        append: function (name, value) {
            this.parameters.push(encodeURIComponent(name) + '=' + encodeURIComponent(value || ''));
        },
        delete: function (name) {
            name = encodeURIComponent(name);

            this.parameters = this.parameters.filter(function (parameter) {
                return parameter.indexOf(name + '=');
            });
        },
        deleteAllStartingWith: function (name) {
            name = encodeURIComponent(name);

            this.parameters = this.parameters.filter(function (parameter) {
                return parameter.split('=', 1)[0].indexOf(name);
            });
        },

        toString: function () {
            return this.parameters.length ? '?' + this.parameters.join('&') : '';
        }
    };

    return MvcGridQuery;
})();

function MvcGridExtends(subclass, base) {
    Object.setPrototypeOf(subclass, base);

    function Subclass() {
        this.constructor = subclass;
    }

    subclass.prototype = (Subclass.prototype = base.prototype, new Subclass());
}

var MvcGridFilter = (function () {
    function MvcGridFilter(column) {
        var filter = this;

        filter.methods = [];
        filter.column = column;
        filter.cssClasses = '';
        filter.popup = column.grid.popup;
        filter.type = column.filter.type;
        filter.mode = column.grid.filterMode;
    }

    MvcGridFilter.prototype = {
        init: function () {
            var filter = this;
            var column = filter.column;

            if (!column.filter.hasOptions && filter.mode != 'excel') {
                filter.validate(column.filter.inlineInput);
            }

            if (!column.filter.first.method) {
                column.filter.first.method = column.filter.defaultMethod;
            }

            if (!column.filter.second.method) {
                column.filter.second.method = column.filter.defaultMethod;
            }

            if (filter.methods.indexOf(column.filter.first.method) < 0) {
                column.filter.first.method = filter.methods[0];
            }

            if (filter.methods.indexOf(column.filter.second.method) < 0) {
                column.filter.second.method = filter.methods[0];
            }
        },

        show: function () {
            var filter = this;

            filter.popup.render(filter);

            filter.bindOperator();
            filter.bindMethods();
            filter.bindValues();
            filter.bindActions();

            filter.popup.show(filter.column);
        },

        render: function () {
            var filter = this;

            filter.lang = filter.column.grid.lang;

            return '<div class="popup-arrow"></div>' +
                '<div class="popup-content">' +
                '<div class="popup-filter">' +
                filter.renderFilter('first') +
                '</div>' +
                (filter.mode == 'excel' && filter.type == 'double'
                    ? filter.renderOperator() +
                    '<div class="popup-filter">' +
                    filter.renderFilter('second') +
                    '</div>'
                    : '') +
                filter.renderActions() +
                '</div>';
        },
        renderFilter: function (name) {
            var filter = this;
            var hasOptions = filter.column.filter.hasOptions;
            var lang = filter.lang[filter.column.filter.name] || {};
            var multiple = filter.type == 'multi' ? ' multiple' : '';

            return '<div class="popup-group">' +
                '<select class="mvc-grid-method" data-filter="' + name + '">' +
                filter.methods.map(function (method) {
                    return '<option value="' + method + '">' + (lang[method] || '') + '</option>';
                }).join('') +
                '</select>' +
                '</div>' +
                '<div class="popup-group">' + (hasOptions
                    ? '<select class="mvc-grid-value" data-filter="' + name + '"' + multiple + '>' +
                    filter.column.filter.options.innerHTML +
                    '</select>'
                    : '<input class="mvc-grid-value" data-filter="' + name + '">') +
                '</div>';
        },
        renderOperator: function () {
            return '<div class="popup-operator">' +
                '<div class="popup-group">' +
                '<select class="mvc-grid-operator">' +
                '<option value="">' + this.lang.operator.select + '</option>' +
                '<option value="and">' + this.lang.operator.and + '</option>' +
                '<option value="or">' + this.lang.operator.or + '</option>' +
                '</select>' +
                '</div>' +
                '</div>';
        },
        renderActions: function () {
            return '<div class="popup-actions">' +
                '<button type="button" class="mvc-grid-apply" type="button">' + this.lang.filter.apply + '</button>' +
                '<button type="button" class="mvc-grid-cancel" type="button">' + this.lang.filter.remove + '</button>' +
                '</div>';
        },

        apply: function () {
            MvcGridPopup.prototype.lastActiveElement = null;

            this.column.grid.applyFilters(this.column);

            this.popup.hide();
        },
        cancel: function () {
            if (this.column.filter.isApplied) {
                MvcGridPopup.prototype.lastActiveElement = null;
            }

            this.column.cancelFilter();

            this.popup.hide();
        },
        isValid: function () {
            return true;
        },
        validate: function (input) {
            if (this.isValid(input.value)) {
                input.classList.remove('invalid');
            } else {
                input.classList.add('invalid');
            }
        },

        bindOperator: function () {
            var filter = this.column.filter;
            var operator = this.popup.element.querySelector('.mvc-grid-operator');

            if (operator) {
                operator.addEventListener('change', function () {
                    filter.operator = this.value;
                });
            }
        },
        bindMethods: function () {
            var filter = this.column.filter;

            [].forEach.call(this.popup.element.querySelectorAll('.mvc-grid-method'), function (method) {
                method.addEventListener('change', function () {
                    filter[this.dataset.filter].method = this.value;
                });
            });
        },
        bindValues: function () {
            var filter = this;

            [].forEach.call(filter.popup.element.querySelectorAll('.mvc-grid-value'), function (input) {
                if (input.tagName == 'SELECT') {
                    input.addEventListener('change', function () {
                        filter.column.filter[input.dataset.filter].values = [].filter.call(input.options, function (option) {
                            return option.selected;
                        }).map(function (option) {
                            return option.value;
                        });

                        if (filter.mode != 'excel') {
                            var inlineInput = filter.column.filter.inlineInput;

                            if (filter.mode == 'header' || filter.mode == 'row' && filter.type == 'multi') {
                                inlineInput.value = [].filter.call(input.options, function (option) {
                                    return option.selected;
                                }).map(function (option) {
                                    return option.text;
                                }).join(', ');
                            } else {
                                inlineInput.value = input.value;
                            }

                            filter.validate(inlineInput);
                        }

                        filter.validate(input);
                    });
                } else {
                    input.addEventListener('input', function () {
                        filter.column.filter[input.dataset.filter].values = [input.value];

                        if (filter.mode != 'excel') {
                            var inlineInput = filter.column.filter.inlineInput;

                            inlineInput.value = filter.column.filter[input.dataset.filter].values.join(', ');

                            filter.validate(inlineInput);
                        }

                        filter.validate(input);
                    });

                    input.addEventListener('keyup', function (e) {
                        if (e.which == 13 && filter.isValid(this.value)) {
                            filter.apply();
                        }
                    });
                }

                filter.validate(input);
            });
        },
        bindActions: function () {
            var popup = this.popup.element;

            popup.querySelector('.mvc-grid-apply').addEventListener('click', this.apply.bind(this));
            popup.querySelector('.mvc-grid-cancel').addEventListener('click', this.cancel.bind(this));
        }
    };

    return MvcGridFilter;
})();

var MvcGridTextFilter = (function (base) {
    MvcGridExtends(MvcGridTextFilter, base);

    function MvcGridTextFilter(column) {
        base.call(this, column);

        this.methods = ['contains', 'equals', 'not-equals', 'starts-with', 'ends-with'];
    }

    return MvcGridTextFilter;
})(MvcGridFilter);

var MvcGridNumberFilter = (function (base) {
    MvcGridExtends(MvcGridNumberFilter, base);

    function MvcGridNumberFilter(column) {
        base.call(this, column);

        this.methods = ['equals', 'not-equals', 'less-than', 'greater-than', 'less-than-or-equal', 'greater-than-or-equal'];
    }

    MvcGridNumberFilter.prototype.isValid = function (value) {
        return !value || /^(?=.*\d+.*)[-+]?\d*[.,]?\d*$/.test(value);
    };

    return MvcGridNumberFilter;
})(MvcGridFilter);

var MvcGridDateFilter = (function (base) {
    MvcGridExtends(MvcGridDateFilter, base);

    function MvcGridDateFilter(column) {
        base.call(this, column);

        this.methods = ['equals', 'not-equals', 'earlier-than', 'later-than', 'earlier-than-or-equal', 'later-than-or-equal'];
    }

    return MvcGridDateFilter;
})(MvcGridFilter);

var MvcGridEnumFilter = (function (base) {
    MvcGridExtends(MvcGridEnumFilter, base);

    function MvcGridEnumFilter(column) {
        base.call(this, column);

        this.methods = ['equals', 'not-equals'];
    }

    return MvcGridEnumFilter;
})(MvcGridFilter);

var MvcGridGuidFilter = (function (base) {
    MvcGridExtends(MvcGridGuidFilter, base);

    function MvcGridGuidFilter(column) {
        base.call(this, column);

        this.methods = ['equals', 'not-equals'];
        this.cssClasses = 'mvc-grid-guid-filter';
    }

    MvcGridGuidFilter.prototype.isValid = function (value) {
        return !value || /^[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}$/i.test(value);
    };

    return MvcGridGuidFilter;
})(MvcGridFilter);

var MvcGridBooleanFilter = (function (base) {
    MvcGridExtends(MvcGridBooleanFilter, base);

    function MvcGridBooleanFilter(column) {
        base.call(this, column);

        this.methods = ['equals', 'not-equals'];
    }

    return MvcGridBooleanFilter;
})(MvcGridFilter);