/*!
 * Mvc.Grid 6.0.0
 * https://github.com/NonFactors/AspNetCore.Grid
 *
 * Copyright © NonFactors
 *
 * Licensed under the terms of the MIT License
 * http://www.opensource.org/licenses/mit-license.php
 */
class MvcGrid {
    constructor(container, options) {
        const grid = this;
        const element = grid.findGrid(container);

        if (element.dataset.id) {
            return MvcGrid.instances[parseInt(element.dataset.id)].set(options || {});
        }

        grid.columns = [];
        grid.element = element;
        grid.loadingDelay = 300;
        grid.requestType = 'get';
        grid.name = element.dataset.name;
        grid.popup = new MvcGridPopup(grid);
        grid.controller = new AbortController();
        grid.prefix = grid.name ? `${grid.name}-` : '';
        grid.isAjax = grid.element.dataset.url ? true : false;
        grid.filterMode = (element.dataset.filterMode || 'excel').toLowerCase();
        grid.element.dataset.id = options && options.id || MvcGrid.instances.length.toString();
        grid.url = grid.element.dataset.url ? new URL(grid.element.dataset.url, location.href) : new URL(location.href);
        grid.filters = {
            enum: MvcGridEnumFilter,
            date: MvcGridDateFilter,
            guid: MvcGridGuidFilter,
            text: MvcGridTextFilter,
            number: MvcGridNumberFilter,
            boolean: MvcGridBooleanFilter
        };

        const rowFilters = element.querySelectorAll('.mvc-grid-row-filters th');

        element.querySelectorAll('.mvc-grid-headers th').forEach((header, i) => {
            grid.columns.push(new MvcGridColumn(grid, header, rowFilters[i]));
        });

        const pager = element.querySelector('.mvc-grid-pager');

        if (pager) {
            grid.pager = new MvcGridPager(grid, pager);
        }

        if (options && options.id) {
            MvcGrid.instances[parseInt(options.id)] = grid;
        } else {
            MvcGrid.instances.push(grid);
        }

        grid.set(options || {});
        grid.cleanUp();
        grid.bind();

        if (!element.children.length) {
            grid.reload();
        }
    }

    set(options) {
        const grid = this;
        const filters = options.filters || {};

        for (const key in filters) {
            if (Object.prototype.hasOwnProperty.call(filters, key)) {
                grid.filters[key] = filters[key];
            }
        }

        grid.requestType = options.requestType || grid.requestType;
        grid.isAjax = typeof options.isAjax == 'undefined' ? grid.isAjax : options.isAjax;
        grid.url = options.url == null ? grid.url : new URL(options.url.toString(), location.href);
        grid.url = typeof options.query == 'undefined' ? grid.url : new URL(`?${options.query}`, grid.url.href);
        grid.loadingDelay = typeof options.loadingDelay == 'undefined' ? grid.loadingDelay : options.loadingDelay;

        grid.columns.forEach(column => {
            column.updateFilter();

            if (column.filter && grid.filters[column.filter.name]) {
                column.filter.instance = new grid.filters[column.filter.name](column);
                column.filter.instance.init();
            }
        });

        return grid;
    }

    reload() {
        const grid = this;

        grid.element.dispatchEvent(new CustomEvent('reloadstart', {
            detail: { grid },
            bubbles: true
        }));

        if (grid.isAjax) {
            grid.startLoading()
                .then(response => {
                    const parent = grid.element.parentElement;
                    const template = document.createElement('template');
                    const i = [].indexOf.call(parent.children, grid.element);

                    template.innerHTML = response.trim();

                    if (template.content.firstElementChild.classList.contains('mvc-grid')) {
                        grid.element.outerHTML = response;
                    } else {
                        throw new Error('Grid partial should only include grid declaration.');
                    }

                    const newGrid = new MvcGrid(parent.children[i], {
                        loadingDelay: grid.loadingDelay,
                        requestType: grid.requestType,
                        id: grid.element.dataset.id,
                        filters: grid.filters,
                        isAjax: grid.isAjax,
                        url: grid.url
                    });

                    newGrid.element.dispatchEvent(new CustomEvent('reloadend', {
                        detail: { grid: newGrid },
                        bubbles: true
                    }));
                })
                .catch(result => {
                    grid.stopLoading();

                    grid.element.dispatchEvent(new CustomEvent('reloadfail', {
                        detail: { grid, result },
                        bubbles: true
                    }));

                    throw result;
                });
        } else {
            location.href = grid.url.href;
        }
    }

    applyFilters(initiator) {
        const grid = this;
        const prefix = grid.prefix;
        const query = grid.url.searchParams;
        const sort = query.get(`${prefix}sort`);
        const order = query.get(`${prefix}order`);

        grid.clearQuery();

        grid.columns.filter(column => column.filter && (column == initiator || column.filter.first.values[0])).forEach(column => {
            const filter = column.filter;

            query.append(`${prefix + column.name}-${filter.first.method}`, filter.first.values[0] || '');

            for (let i = 1; filter.type == 'multi' && i < filter.first.values.length; i++) {
                query.append(`${prefix + column.name}-${filter.first.method}`, filter.first.values[i] || '');
            }

            if (grid.filterMode == 'excel' && filter.type == 'double') {
                query.append(`${prefix + column.name}-op`, filter.operator || '');
                query.append(`${prefix + column.name}-${filter.second.method}`, filter.second.values[0] || '');
            }
        });

        if (sort) {
            query.append(`${prefix}sort`, sort);
        }

        if (order) {
            query.append(`${prefix}order`, order);
        }

        if (grid.pager && grid.pager.showPageSizes) {
            query.append(`${prefix}rows`, grid.pager.rowsPerPage.value);
        }

        grid.reload();
    }
    startLoading() {
        const grid = this;
        const url = new URL(grid.url.href);

        grid.stopLoading();
        grid.controller = new AbortController();
        url.searchParams.set('_', Date.now().toString());

        if (grid.loadingDelay != null && !grid.element.querySelector('.mvc-grid-loader')) {
            const content = document.createElement('div');

            content.appendChild(document.createElement('div'));
            content.appendChild(document.createElement('div'));
            content.appendChild(document.createElement('div'));

            grid.loader = document.createElement('div');
            grid.loader.className = 'mvc-grid-loader';
            grid.loader.appendChild(content);

            grid.loading = setTimeout(() => {
                grid.loader.classList.add('mvc-grid-loading');
            }, grid.loadingDelay);

            grid.element.appendChild(grid.loader);
        }

        return fetch(url.href, {
            method: grid.requestType,
            signal: grid.controller.signal,
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        }).then(response => {
            if (response.ok) {
                return response.text();
            }

            return Promise.reject(new Error(`Invalid response status: ${response.status}`));
        });
    }
    stopLoading() {
        const grid = this;

        grid.controller.abort();

        clearTimeout(grid.loading);

        if (grid.loader) {
            grid.loader.parentElement.removeChild(grid.loader);
        }
    }
    clearQuery() {
        const prefix = this.prefix;
        const query = this.url.searchParams;

        this.columns.forEach(column => {
            for (const key of [...query.keys()]) {
                if (key.startsWith(`${prefix + column.name}-`)) {
                    query.delete(key);
                }
            }
        });

        query.delete(`${prefix}order`);
        query.delete(`${prefix}sort`);
        query.delete(`${prefix}page`);
        query.delete(`${prefix}rows`);
    }

    findGrid(element) {
        const grid = element.closest('.mvc-grid');

        if (!grid) {
            throw new Error('Grid can only be created from within mvc-grid structure.');
        }

        return grid;
    }
    cleanUp() {
        delete this.element.dataset.filterMode;
        delete this.element.dataset.url;
    }
    bind() {
        const grid = this;

        grid.element.querySelectorAll('tbody tr').forEach(row => {
            if (!row.classList.contains('mvc-grid-empty-row')) {
                row.addEventListener('click', function (e) {
                    const data = {};

                    grid.columns.forEach((column, i) => {
                        data[column.name] = row.cells[i].innerText;
                    });

                    this.dispatchEvent(new CustomEvent('rowclick', {
                        detail: { grid: grid, data: data, originalEvent: e },
                        bubbles: true
                    }));
                });
            }
        });
    }
}

MvcGrid.instances = [];
MvcGrid.lang = {
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
};

class MvcGridColumn {
    constructor(grid, header, rowFilter) {
        const column = this;
        const data = header.dataset;

        column.grid = grid;
        column.header = header;
        column.name = data.name;
        column.rowFilter = rowFilter;
        column.isHidden = header.classList.contains('mvc-grid-hidden');

        if (data.filter == 'True' && data.filterName) {
            let options = header.querySelector('.mvc-grid-options');

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

    updateFilter() {
        const column = this;
        const filter = column.filter;

        if (filter) {
            const values = [];
            const methods = [];
            const query = column.grid.url.searchParams;
            const name = `${column.grid.prefix + column.name}-`;

            for (const parameter of query.entries()) {
                if (parameter[0] != `${name}op` && parameter[0].startsWith(name)) {
                    methods.push(decodeURIComponent(parameter[0].substring(name.length) || ''));
                    values.push(parameter[1] || '');
                }
            }

            filter.first = {
                method: methods[0] || '',
                values: filter.type == 'multi' ? values : values.slice(0, 1)
            };

            filter.operator = filter.type == 'double' && query.get(`${name}op`) || '';

            filter.second = {
                method: filter.type == 'double' ? methods[1] || '' : '',
                values: filter.type == 'double' ? values.slice(1, 2) : []
            };
        }
    }
    cancelFilter() {
        const column = this;
        const grid = column.grid;
        const query = grid.url.searchParams;

        if (column.filter.isApplied) {
            query.delete(`${grid.prefix}page`);
            query.delete(`${grid.prefix}rows`);

            for (const key of [...query.keys()]) {
                if (key.startsWith(`${grid.prefix + column.name}-`)) {
                    query.delete(key);
                }
            }

            grid.reload();
        } else {
            column.filter.first.values = [];
            column.filter.second.values = [];

            if (column.grid.filterMode != 'excel') {
                column.filter.inlineInput.value = '';
            }
        }
    }
    applySort() {
        const column = this;
        const grid = column.grid;
        const query = grid.url.searchParams;
        let order = column.sort.order == 'asc' ? 'desc' : 'asc';

        query.delete(`${grid.prefix}sort`);
        query.delete(`${grid.prefix}order`);

        if (!column.sort.order && column.sort.first) {
            order = column.sort.first;
        }

        query.append(`${grid.prefix}sort`, column.name || '');
        query.append(`${grid.prefix}order`, order);

        grid.reload();
    }

    bindFilter() {
        const column = this;
        const filter = column.filter;

        filter.button.addEventListener('click', () => {
            filter.instance.show();
        });

        if (filter.hasOptions) {
            if (column.grid.filterMode == 'row' && filter.type != 'multi') {
                filter.inlineInput.addEventListener('change', function () {
                    filter.first.values = [this.value];

                    filter.instance.apply();
                });
            } else if (column.grid.filterMode == 'header' || column.grid.filterMode == 'row') {
                filter.inlineInput.addEventListener('click', function () {
                    if (this.selectionStart == this.selectionEnd) {
                        filter.instance.show();
                    }
                });
            }
        } else if (column.grid.filterMode != 'excel') {
            filter.inlineInput.addEventListener('input', function () {
                filter.first.values = [this.value];

                filter.instance.validate(this);
            });

            filter.inlineInput.addEventListener('keyup', function (e) {
                if (e.which == 13 && filter.instance.isValid(this.value)) {
                    filter.instance.apply();
                }
            });
        }
    }
    bindSort() {
        const column = this;

        if (!column.filter || column.grid.filterMode != 'header') {
            column.header.addEventListener('click', e => {
                if (!/mvc-grid-(sort|filter)/.test(e.target.className)) {
                    column.applySort();
                }
            });
        }

        column.sort.button.addEventListener('click', () => {
            column.applySort();
        });
    }
    cleanUp() {
        const data = this.header.dataset;

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
}

class MvcGridPager {
    constructor(grid, element) {
        const pager = this;

        pager.grid = grid;
        pager.element = element;
        pager.pages = element.querySelectorAll('[data-page]');
        pager.showPageSizes = element.dataset.showPageSizes == 'True';
        pager.rowsPerPage = element.querySelector('.mvc-grid-pager-rows');
        pager.currentPage = pager.pages.length ? parseInt(element.querySelector('.active').dataset.page) : 1;

        pager.cleanUp();
        pager.bind();
    }

    apply(page) {
        const grid = this.grid;
        const query = grid.url.searchParams;

        query.delete(`${grid.prefix}page`);
        query.delete(`${grid.prefix}rows`);

        query.append(`${grid.prefix}page`, page || '');

        if (this.showPageSizes) {
            query.append(`${grid.prefix}rows`, this.rowsPerPage.value);
        }

        grid.reload();
    }

    cleanUp() {
        delete this.element.dataset.showPageSizes;
    }
    bind() {
        const pager = this;

        pager.pages.forEach(page => {
            page.addEventListener('click', function () {
                pager.apply(this.dataset.page);
            });
        });

        pager.rowsPerPage.addEventListener('change', () => {
            pager.apply(pager.currentPage);
        });
    }
}

class MvcGridPopup {
    constructor(grid) {
        MvcGridPopup.element.className = 'mvc-grid-popup';
        this.grid = grid;

        this.bind();
    }

    render(filter) {
        MvcGridPopup.element.className = `mvc-grid-popup ${filter.cssClasses}`.trim();
        MvcGridPopup.element.innerHTML = filter.render();

        this.updateValues(filter.column);
    }
    show(column) {
        const popup = this;

        MvcGridPopup.lastActiveElement = document.activeElement;

        if (!MvcGridPopup.element.parentElement) {
            document.body.appendChild(MvcGridPopup.element);
        }

        popup.updatePosition(column);

        MvcGridPopup.element.querySelector('.mvc-grid-value').focus();
    }
    hide(e) {
        const popup = MvcGridPopup;
        const target = e && e.target && e.target.closest && e.target.closest('.mvc-grid-popup,.mvc-grid-filter');

        if ((!target || e.which == 27) && popup.element.parentNode && (!e || e.target != window)) {
            document.body.removeChild(popup.element);

            if (popup.lastActiveElement) {
                popup.lastActiveElement.focus();
                popup.lastActiveElement = null;
            }
        }
    }

    updatePosition(column) {
        const popup = MvcGridPopup;
        const filter = column.filter.button;
        const width = popup.element.clientWidth;
        const filterPos = filter.getBoundingClientRect();
        const arrow = popup.element.querySelector('.popup-arrow');
        const top = window.pageYOffset + filterPos.top + filter.offsetHeight * 0.6 + arrow.offsetHeight;

        let left = window.pageXOffset + filterPos.left - 8;
        let arrowLeft = filter.offsetWidth / 2;

        if (left + width + 8 > window.pageXOffset + document.documentElement.clientWidth) {
            const offset = width - filter.offsetWidth - 16;

            arrowLeft += offset;
            left -= offset;
        }

        popup.element.style.left = `${left}px`;
        popup.element.style.top = `${top}px`;
        arrow.style.left = `${arrowLeft}px`;
    }
    updateValues(column) {
        const popup = this;
        const filter = column.filter;

        popup.setValues('.mvc-grid-operator', [filter.operator]);
        popup.setValues('.mvc-grid-value[data-filter="first"]', filter.first.values);
        popup.setValues('.mvc-grid-value[data-filter="second"]', filter.second.values);
        popup.setValues('.mvc-grid-method[data-filter="first"]', [filter.first.method]);
        popup.setValues('.mvc-grid-method[data-filter="second"]', [filter.second.method]);
    }
    setValues(selector, values) {
        const input = MvcGridPopup.element.querySelector(selector);

        if (input) {
            if (input.tagName == 'SELECT' && input.multiple) {
                [].forEach.call(input.options, option => {
                    option.selected = values.indexOf(option.value) >= 0;
                });
            } else {
                input.value = values[0] || '';
            }
        }
    }
    bind() {
        const popup = this;

        window.addEventListener('resize', popup.hide);
        window.addEventListener('keydown', popup.hide);
        window.addEventListener('mousedown', popup.hide);
        window.addEventListener('touchstart', popup.hide);
    }
}
MvcGridPopup.element = document.createElement('div');

class MvcGridFilter {
    constructor(column) {
        const filter = this;

        filter.methods = [];
        filter.column = column;
        filter.cssClasses = '';
        filter.popup = column.grid.popup;
        filter.type = column.filter.type;
        filter.mode = column.grid.filterMode;
    }

    init() {
        const filter = this;
        const column = filter.column;
        const columnFilter = column.filter;

        if (!columnFilter.hasOptions && filter.mode != 'excel') {
            filter.validate(columnFilter.inlineInput);
        }

        if (!columnFilter.first.method) {
            columnFilter.first.method = columnFilter.defaultMethod;
        }

        if (!columnFilter.second.method) {
            columnFilter.second.method = columnFilter.defaultMethod;
        }

        if (filter.methods.indexOf(columnFilter.first.method) < 0) {
            columnFilter.first.method = filter.methods[0];
        }

        if (filter.methods.indexOf(columnFilter.second.method) < 0) {
            columnFilter.second.method = filter.methods[0];
        }
    }

    show() {
        const filter = this;

        filter.popup.render(filter);

        filter.bindOperator();
        filter.bindMethods();
        filter.bindValues();
        filter.bindActions();

        filter.popup.show(filter.column);
    }

    render() {
        const filter = this;

        filter.lang = MvcGrid.lang;

        return `<div class="popup-arrow"></div>
                <div class="popup-content">
                    <div class="popup-filter">
                       ${filter.renderFilter('first')}
                    </div>
                   ${filter.mode == 'excel' && filter.type == 'double'
                       ? `${filter.renderOperator()}
                       <div class="popup-filter">
                           ${filter.renderFilter('second')}
                       </div>`
                       : ''}
                   ${filter.renderActions()}
                </div>`;
    }
    renderFilter(name) {
        const filter = this;
        const methods = filter.methods;
        const hasOptions = filter.column.filter.hasOptions;
        const lang = filter.lang[filter.column.filter.name] || {};
        const multiple = filter.type == 'multi' ? ' multiple' : '';
        const options = methods.map(method => `<option value="${method}">${lang[method] || ''}</option>`).join('');

        return `<div class="popup-group">
                    <select class="mvc-grid-method" data-filter="${name}">
                        ${options}
                    </select>
                </div>
                <div class="popup-group">${hasOptions
                    ? `<select class="mvc-grid-value" data-filter="${name}"${multiple}>
                          ${filter.column.filter.options.innerHTML}
                       </select>`
                    : `<input class="mvc-grid-value" data-filter="${name}">`}
                </div>`;
    }
    renderOperator() {
        const lang = this.lang.operator;

        return `<div class="popup-operator">
                    <div class="popup-group">
                        <select class="mvc-grid-operator">
                            <option value="">${lang.select}</option>
                            <option value="and">${lang.and}</option>
                            <option value="or">${lang.or}</option>
                        </select>
                    </div>
                </div>`;
    }
    renderActions() {
        const lang = this.lang.filter;

        return `<div class="popup-actions">
                    <button type="button" class="mvc-grid-apply" type="button">${lang.apply}</button>
                    <button type="button" class="mvc-grid-cancel" type="button">${lang.remove}</button>
                </div>`;
    }

    apply() {
        MvcGridPopup.lastActiveElement = null;

        this.column.grid.applyFilters(this.column);

        this.popup.hide();
    }
    cancel() {
        if (this.column.filter.isApplied) {
            MvcGridPopup.lastActiveElement = null;
        }

        this.column.cancelFilter();

        this.popup.hide();
    }
    isValid() {
        return true;
    }
    validate(input) {
        if (this.isValid(input.value)) {
            input.classList.remove('invalid');
        } else {
            input.classList.add('invalid');
        }
    }

    bindOperator() {
        const filter = this.column.filter;
        const operator = MvcGridPopup.element.querySelector('.mvc-grid-operator');

        if (operator) {
            operator.addEventListener('change', function () {
                filter.operator = this.value;
            });
        }
    }
    bindMethods() {
        const filter = this.column.filter;

        MvcGridPopup.element.querySelectorAll('.mvc-grid-method').forEach(method => {
            method.addEventListener('change', function () {
                filter[this.dataset.filter].method = this.value;
            });
        });
    }
    bindValues() {
        const filter = this;

        MvcGridPopup.element.querySelectorAll('.mvc-grid-value').forEach(input => {
            if (input.tagName == 'SELECT') {
                input.addEventListener('change', () => {
                    filter.column.filter[input.dataset.filter].values = [].filter.call(input.options, option => option.selected).map(option => option.value);

                    if (filter.mode != 'excel') {
                        const inlineInput = filter.column.filter.inlineInput;

                        if (filter.mode == 'header' || filter.mode == 'row' && filter.type == 'multi') {
                            inlineInput.value = [].filter
                                .call(input.options, option => option.selected)
                                .map(option => option.text)
                                .join(', ');
                        } else {
                            inlineInput.value = input.value;
                        }

                        filter.validate(inlineInput);
                    }

                    filter.validate(input);
                });
            } else {
                input.addEventListener('input', () => {
                    filter.column.filter[input.dataset.filter].values = [input.value];

                    if (filter.mode != 'excel') {
                        const inlineInput = filter.column.filter.inlineInput;

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
    }
    bindActions() {
        const popup = MvcGridPopup.element;

        popup.querySelector('.mvc-grid-apply').addEventListener('click', this.apply.bind(this));
        popup.querySelector('.mvc-grid-cancel').addEventListener('click', this.cancel.bind(this));
    }
}

class MvcGridTextFilter extends MvcGridFilter {
    constructor(column) {
        super(column);

        this.methods = ['contains', 'equals', 'not-equals', 'starts-with', 'ends-with'];
    }
}

class MvcGridNumberFilter extends MvcGridFilter {
    constructor(column) {
        super(column);

        this.methods = ['equals', 'not-equals', 'less-than', 'greater-than', 'less-than-or-equal', 'greater-than-or-equal'];
    }

    isValid(value) {
        return !value || /^(?=.*\d+.*)[-+]?\d*[.,]?\d*$/.test(value);
    }
}

class MvcGridDateFilter extends MvcGridFilter {
    constructor(column) {
        super(column);

        this.methods = ['equals', 'not-equals', 'earlier-than', 'later-than', 'earlier-than-or-equal', 'later-than-or-equal'];
    }
}

class MvcGridEnumFilter extends MvcGridFilter {
    constructor(column) {
        super(column);

        this.methods = ['equals', 'not-equals'];
    }
}

class MvcGridGuidFilter extends MvcGridFilter {
    constructor(column) {
        super(column);

        this.methods = ['equals', 'not-equals'];
        this.cssClasses = 'mvc-grid-guid-filter';
    }

    isValid(value) {
        return !value || /^[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}$/i.test(value);
    }
}

class MvcGridBooleanFilter extends MvcGridFilter {
    constructor(column) {
        super(column);

        this.methods = ['equals', 'not-equals'];
    }
}
