/*!
 * Mvc.Grid 7.0.0
 *
 * Copyright © NonFactors
 *
 * Licensed under the terms of the MIT License
 * https://www.opensource.org/licenses/mit-license.php
 */

export interface MvcGridOptions {
    url: URL;
    id: string;
    query: string;
    isAjax: boolean;
    loadingDelay: number | null;
    filters: {
        [type: string]: typeof MvcGridFilter | undefined;
    };
}

export interface MvcGridLanguage {
    [type: string]: {
        [method: string]: string;
    } | undefined;
}

export interface MvcGridConfiguration {
    name: string;
    columns: {
        name: string;
        hidden: boolean;
    }[];
}

export class MvcGrid {
    private static instances: MvcGrid[] = [];
    public static lang: MvcGridLanguage = {
        default: {
            "equals": "Equals",
            "not-equals": "Not equals"
        },
        text: {
            "contains": "Contains",
            "equals": "Equals",
            "not-equals": "Not equals",
            "starts-with": "Starts with",
            "ends-with": "Ends with"
        },
        number: {
            "equals": "Equals",
            "not-equals": "Not equals",
            "less-than": "Less than",
            "greater-than": "Greater than",
            "less-than-or-equal": "Less than or equal",
            "greater-than-or-equal": "Greater than or equal"
        },
        date: {
            "equals": "Equals",
            "not-equals": "Not equals",
            "earlier-than": "Earlier than",
            "later-than": "Later than",
            "earlier-than-or-equal": "Earlier than or equal",
            "later-than-or-equal": "Later than or equal"
        },
        guid: {
            "equals": "Equals",
            "not-equals": "Not equals"
        },
        filter: {
            "apply": "&#10003;",
            "remove": "&#10008;"
        },
        operator: {
            "select": "",
            "and": "and",
            "or": "or"
        }
    };

    public element: HTMLElement;
    public columns: MvcGridColumn[];

    public pager?: MvcGridPager;
    public loader?: HTMLDivElement;
    public controller: AbortController;

    public url: URL;
    public name: string;
    public prefix: string;
    public isAjax: boolean;
    public loadingTimerId: number;
    public loadingDelay: number | null;
    public sort: Map<string, "asc" | "desc">;
    public filterMode: "row" | "excel" | "header";
    public filters: {
        [type: string]: typeof MvcGridFilter | undefined;
    };

    public constructor(container: HTMLElement, options: Partial<MvcGridOptions> = {}) {
        const grid = this;
        const element = grid.findGrid(container);

        if (element.dataset.id) {
            return MvcGrid.instances[parseInt(element.dataset.id)].set(options);
        }

        grid.columns = [];
        grid.element = element;
        grid.loadingDelay = 300;
        grid.loadingTimerId = 0;
        grid.name = element.dataset.name!;
        grid.controller = new AbortController();
        grid.isAjax = Boolean(element.dataset.url);
        grid.prefix = grid.name ? `${grid.name}-` : "";
        grid.filterMode = (element.dataset.filterMode || "").toLowerCase() as any;
        element.dataset.id = options.id || MvcGrid.instances.length.toString();
        grid.url = element.dataset.url ? new URL(element.dataset.url, location.href) : new URL(location.href);
        grid.url = options.url ? new URL(options.url.toString(), location.href) : grid.url;
        grid.url = options.query ? new URL(`?${options.query}`, grid.url.href) : grid.url;
        grid.sort = grid.buildSort();
        grid.filters = {
            default: MvcGridFilter,
            date: MvcGridDateFilter,
            guid: MvcGridGuidFilter,
            text: MvcGridTextFilter,
            number: MvcGridNumberFilter
        };

        const headers = element.querySelector(".mvc-grid-headers");
        const rowFilters = element.querySelectorAll<HTMLTableHeaderCellElement>(".mvc-grid-row-filters th");

        if (headers) {
            for (const [i, header] of headers.querySelectorAll("th").entries()) {
                grid.columns.push(new MvcGridColumn(grid, header, rowFilters[i]));
            }
        }

        const pager = element.querySelector<HTMLElement>(".mvc-grid-pager");

        if (pager) {
            grid.pager = new MvcGridPager(grid, pager);
        }

        grid.set(options);
        grid.cleanUp();
        grid.bind();

        if (options.id) {
            MvcGrid.instances[parseInt(options.id)] = grid;
        } else {
            MvcGrid.instances.push(grid);
        }

        if (!element.children.length) {
            grid.reload();
        }
    }

    public set(options: Partial<MvcGridOptions>) {
        const grid = this;

        grid.loadingDelay = typeof options.loadingDelay === "undefined" ? grid.loadingDelay : options.loadingDelay;
        grid.url = options.url ? new URL(options.url.toString(), location.href) : grid.url;
        grid.url = options.query ? new URL(`?${options.query}`, grid.url.href) : grid.url;
        grid.isAjax = typeof options.isAjax === "boolean" ? options.isAjax : grid.isAjax;
        grid.filters = Object.assign(grid.filters, options.filters);

        for (const column of grid.columns) {
            if (column.filter && grid.filters[column.filter.name]) {
                column.filter.instance = new grid.filters[column.filter.name]!(column);
                column.filter.instance.init();
            }
        }

        return grid;
    }
    public showConfiguration(anchor?: HTMLElement) {
        MvcGridPopup.showConfiguration(this, anchor);
    }
    public getConfiguration() {
        return {
            name: this.name,
            columns: this.columns.map(column => ({ name: column.name, hidden: column.isHidden }))
        } as MvcGridConfiguration;
    }
    public configure(configuration: MvcGridConfiguration) {
        configuration.columns.forEach((column, index) => {
            const rows = this.element.querySelectorAll("tr");
            const i = this.columns.findIndex(col => col.name.toLowerCase() === column.name.toLowerCase());

            if (i >= 0) {
                this.columns[i].isHidden = column.hidden;

                for (const tr of rows) {
                    if (column.hidden) {
                        tr.children[i].classList.add("mvc-grid-hidden");
                    } else {
                        tr.children[i].classList.remove("mvc-grid-hidden");
                    }

                    if (i !== index) {
                        tr.insertBefore(tr.children[i], tr.children[index]);
                    }
                }

                this.columns.splice(i - (index < i ? 1 : 0), 0, this.columns.splice(index, 1)[0]);
            }
        });
    }

    public reload() {
        const grid = this;

        grid.element.dispatchEvent(new CustomEvent("reloadstart", {
            detail: { grid },
            bubbles: true
        }));

        if (grid.isAjax) {
            const url = new URL(grid.url.href);

            grid.controller.abort();
            MvcGridPopup.lastActiveElement = null;
            grid.controller = new AbortController();
            url.searchParams.set("_", String(Date.now()));

            if (grid.loadingDelay !== null) {
                if (grid.loader?.parentElement) {
                    clearTimeout(grid.loadingTimerId);
                } else {
                    const loader = document.createElement("template");

                    loader.innerHTML = `<div class="mvc-grid-loader"><div><div></div><div></div><div></div></div></div>`;
                    grid.loader = loader.content.firstElementChild as HTMLDivElement;

                    grid.element.appendChild(grid.loader);
                }

                grid.loadingTimerId = setTimeout(() => {
                    grid.loader!.classList.add("mvc-grid-loading");
                }, grid.loadingDelay);
            }

            MvcGridPopup.hide();

            fetch(url.href, {
                signal: grid.controller.signal,
                headers: { "X-Requested-With": "XMLHttpRequest" }
            }).then(response => {
                if (!response.ok) {
                    throw new Error(`Invalid response status: ${response.status}`);
                }

                return response.text();
            }).then(response => {
                const parent = grid.element.parentElement!;
                const template = document.createElement("template");
                const i = Array.from(parent.children).indexOf(grid.element);

                template.innerHTML = response.trim();

                if (template.content.firstElementChild!.classList.contains("mvc-grid")) {
                    grid.element.outerHTML = response;
                } else {
                    throw new Error("Grid partial should only include grid declaration.");
                }

                const newGrid = new MvcGrid(parent.children[i] as HTMLElement, {
                    loadingDelay: grid.loadingDelay,
                    id: grid.element.dataset.id,
                    filters: grid.filters,
                    isAjax: grid.isAjax,
                    url: grid.url
                });

                newGrid.element.dispatchEvent(new CustomEvent("reloadend", {
                    detail: { grid: newGrid },
                    bubbles: true
                }));
            }).catch(reason => {
                if (reason.name === "AbortError") {
                    return Promise.resolve();
                }

                if (grid.loader?.parentElement) {
                    grid.loader.parentElement.removeChild(grid.loader);
                }

                const cancelled = !grid.element.dispatchEvent(new CustomEvent("reloadfail", {
                    detail: { grid, reason },
                    cancelable: true,
                    bubbles: true
                }));

                return cancelled ? Promise.resolve() : Promise.reject(reason);
            });
        } else {
            location.href = grid.url.href;
        }
    }

    private buildSort() {
        const map = new Map<string, "asc" | "desc">();
        const definitions = /(^|,)(.*?) (asc|desc)(?=$|,)/g;
        const sort = this.url.searchParams.get(`${this.prefix}sort`) || "";

        let match = definitions.exec(sort);

        while (match) {
            map.set(match[2], match[3] as any);

            match = definitions.exec(sort);
        }

        return map;
    }
    private findGrid(element: HTMLElement) {
        const grid = element.closest<HTMLElement>(".mvc-grid");

        if (!grid) {
            throw new Error("Grid can only be created from within mvc-grid structure.");
        }

        return grid;
    }
    private cleanUp() {
        delete this.element.dataset.filterMode;
        delete this.element.dataset.url;
    }
    private bind() {
        const grid = this;

        for (const row of grid.element.querySelectorAll<HTMLTableRowElement>("tbody > tr")) {
            if (!row.classList.contains("mvc-grid-empty-row")) {
                row.addEventListener("click", function (e) {
                    const data: { [type: string]: string; } = {};

                    for (const [i, column] of grid.columns.entries()) {
                        if (row.cells.length <= i) {
                            return;
                        }

                        data[column.name] = row.cells[i].innerText;
                    }

                    this.dispatchEvent(new CustomEvent("rowclick", {
                        detail: { grid: grid, data: data, originalEvent: e },
                        bubbles: true
                    }));
                });
            }
        }
    }
}

export class MvcGridColumn {
    public name: string;
    public grid: MvcGrid;
    public isHidden: boolean;
    public header: HTMLElement;
    public sort: MvcGridColumnSort | null;
    public filter: MvcGridColumnFilter | null;

    public constructor(grid: MvcGrid, header: HTMLElement, rowFilter?: HTMLElement) {
        const column = this;
        const data = header.dataset;

        column.grid = grid;
        column.header = header;
        column.name = data.name || "";
        column.isHidden = header.classList.contains("mvc-grid-hidden");
        column.filter = data.filter ? new MvcGridColumnFilter(column, rowFilter) : null;
        column.sort = header.classList.contains("sortable") ? new MvcGridColumnSort(column) : null;

        column.cleanUp();
    }

    private cleanUp() {
        const data = this.header.dataset;

        delete data.filterDefaultMethod;
        delete data.filterApplied;
        delete data.filterType;
        delete data.filter;

        delete data.sortFirst;
        delete data.sort;

        delete data.name;
    }
}

export class MvcGridColumnSort {
    public column: MvcGridColumn;
    public button: HTMLButtonElement;

    public first: "asc" | "desc";
    public order: "asc" | "desc" | "";

    public constructor(column: MvcGridColumn) {
        const sort = this;

        sort.column = column;
        sort.button = column.header.querySelector<HTMLButtonElement>(".mvc-grid-sort")!;
        sort.order = (column.header.dataset.sort || "").toLowerCase() as any;
        sort.first = (column.header.dataset.sortFirst || "asc").toLowerCase() as any;

        sort.bind();
    }

    public toggle(multi: boolean) {
        const sort = this;
        const grid = sort.column.grid;
        const map = sort.column.grid.sort;
        const query = grid.url.searchParams;

        if (sort.order === sort.first) {
            sort.order = sort.order === "asc" ? "desc" : "asc";
        } else if (sort.order) {
            sort.order = "";
        } else {
            sort.order = sort.first;
        }

        if (!multi) {
            map.clear();
        }

        if (sort.order) {
            map.set(sort.column.name, sort.order);
        } else {
            map.delete(sort.column.name);
        }

        const order = Array.from(map).map(value => value.join(" ")).join(",");

        query.delete(`${grid.prefix}sort`);

        if (order) {
            query.set(`${grid.prefix}sort`, order);
        }

        grid.reload();
    }

    private bind() {
        const sort = this;
        const column = sort.column;

        column.header.addEventListener("click", e => {
            if (!column.filter || column.grid.filterMode !== "header") {
                if (!/mvc-grid-(sort|filter)/.test((e.target as Element).className)) {
                    sort.toggle(e.ctrlKey || e.shiftKey);
                }
            }
        });

        sort.button.addEventListener("click", e => {
            sort.toggle(e.ctrlKey || e.shiftKey);
        });
    }
}

export class MvcGridColumnFilter {
    public name: string;
    public isApplied: boolean;
    public defaultMethod: string;
    public type: "single" | "double" | "multi";
    public first: {
        method: string;
        values: string[];
    };
    public operator: string;
    public second: {
        method: string;
        values: string[];
    };
    public column: MvcGridColumn;
    public rowFilter?: HTMLElement;
    public instance?: MvcGridFilter;
    public button: HTMLButtonElement;
    public options: HTMLSelectElement | null;
    public inlineInput: HTMLInputElement | null;

    public constructor(column: MvcGridColumn, rowFilter?: HTMLElement) {
        const values = [];
        const methods = [];
        const filter = this;
        const data = column.header.dataset;
        const query = column.grid.url.searchParams;
        const name = `${column.grid.prefix + column.name}-`;
        let options = column.header.querySelector<HTMLSelectElement>(".mvc-grid-options");

        if (column.grid.filterMode === "row") {
            options = rowFilter!.querySelector("select");
        }

        if (options?.classList.contains("mvc-grid-options")) {
            options.parentElement!.removeChild(options);
        }

        for (const parameter of query.entries()) {
            if (parameter[0] !== `${name}op` && parameter[0].startsWith(name)) {
                methods.push(parameter[0].substring(name.length));
                values.push(parameter[1]);
            }
        }

        filter.column = column;
        filter.rowFilter = rowFilter;
        filter.name = data.filter || "default";
        filter.isApplied = data.filterApplied === "True";
        filter.defaultMethod = data.filterDefaultMethod || "";
        filter.type = (data.filterType || "single").toLowerCase() as any;
        filter.options = options && options.children.length > 0 ? options : null;
        filter.button = (rowFilter || column.header).querySelector<HTMLButtonElement>(".mvc-grid-filter")!;
        filter.inlineInput = rowFilter ? rowFilter.querySelector<HTMLInputElement>(".mvc-grid-value") : null;

        filter.first = {
            method: methods[0] || "",
            values: filter.type === "multi" ? values : values.slice(0, 1)
        };

        filter.operator = filter.type === "double" ? query.get(`${name}op`) || "" : "";

        filter.second = {
            method: filter.type === "double" ? methods[1] || "" : "",
            values: filter.type === "double" ? values.slice(1, 2) : []
        };

        this.bind();
    }

    public apply() {
        const grid = this.column.grid;
        const query = grid.url.searchParams;
        const prefix = this.column.grid.prefix;
        const order = query.get(`${prefix}sort`);

        for (const column of grid.columns) {
            for (const key of [...query.keys()]) {
                if (key.startsWith(`${prefix + column.name}-`)) {
                    query.delete(key);
                }
            }
        }

        query.delete(`${prefix}sort`);
        query.delete(`${prefix}page`);
        query.delete(`${prefix}rows`);

        for (const column of grid.columns.filter(col => col.filter && (col === this.column || col.filter.isApplied || col.filter.first.values[0]))) {
            const filter = column.filter!;

            query.set(`${prefix + column.name}-${filter.first.method}`, filter.first.values[0] || "");

            for (let i = 1; filter.type === "multi" && i < filter.first.values.length; i++) {
                query.append(`${prefix + column.name}-${filter.first.method}`, filter.first.values[i] || "");
            }

            if (grid.filterMode === "excel" && filter.type === "double") {
                query.set(`${prefix + column.name}-op`, filter.operator || "");
                query.append(`${prefix + column.name}-${filter.second.method}`, filter.second.values[0] || "");
            }
        }

        if (order) {
            query.set(`${prefix}sort`, order);
        }

        if (grid.pager?.showPageSizes) {
            query.set(`${prefix}rows`, grid.pager.rowsPerPage.value);
        }

        grid.reload();
    }
    public cancel() {
        const filter = this;
        const column = filter.column;
        const grid = filter.column.grid;
        const query = grid.url.searchParams;

        if (filter.isApplied) {
            query.delete(`${grid.prefix}page`);
            query.delete(`${grid.prefix}rows`);

            for (const key of [...query.keys()]) {
                if (key.startsWith(`${grid.prefix + column.name}-`)) {
                    query.delete(key);
                }
            }

            grid.reload();
        } else {
            filter.first.values = [];
            filter.second.values = [];

            if (column.grid.filterMode !== "excel") {
                filter.inlineInput!.value = "";
            }

            MvcGridPopup.hide();
        }
    }

    private bind() {
        const filter = this;
        const column = filter.column;
        const mode = column.grid.filterMode;

        filter.button.addEventListener("click", () => {
            MvcGridPopup.show(filter);
        });

        if (filter.options) {
            if (mode === "row" && filter.type !== "multi") {
                filter.inlineInput!.addEventListener("change", function () {
                    filter.first.values = [this.value];

                    column.filter!.apply();
                });
            } else if (mode === "header" || mode === "row") {
                filter.inlineInput!.addEventListener("click", function () {
                    if (this.selectionStart === this.selectionEnd) {
                        MvcGridPopup.show(filter);
                    }
                });
            }
        } else if (mode !== "excel") {
            filter.inlineInput!.addEventListener("input", function () {
                filter.first.values = [this.value];

                filter.instance!.validate(this);
            });

            filter.inlineInput!.addEventListener("keyup", function (e) {
                if (e.which === 13 && filter.instance!.isValid(this.value)) {
                    column.filter!.apply();
                }
            });
        }
    }
}

export class MvcGridPager {
    public grid: MvcGrid;
    public totalRows: number;
    public currentPage: number;
    public element: HTMLElement;
    public showPageSizes: boolean;
    public rowsPerPage: HTMLInputElement;
    public pages: NodeListOf<HTMLElement>;

    public constructor(grid: MvcGrid, element: HTMLElement) {
        const pager = this;

        pager.grid = grid;
        pager.element = element;
        pager.totalRows = parseInt(element.dataset.totalRows!);
        pager.pages = grid.element.querySelectorAll<HTMLElement>("[data-page]");
        pager.showPageSizes = element.dataset.showPageSizes === "True";
        pager.rowsPerPage = element.querySelector<HTMLInputElement>(".mvc-grid-pager-rows")!;
        pager.currentPage = pager.pages.length ? parseInt(element.querySelector<HTMLElement>(".active")!.dataset.page!) : 1;

        pager.cleanUp();
        pager.bind();
    }

    public apply(page: string, rows?: string) {
        const grid = this.grid;
        const query = grid.url.searchParams;

        query.delete(`${grid.prefix}page`);
        query.delete(`${grid.prefix}rows`);

        query.set(`${grid.prefix}page`, page);

        if (this.showPageSizes) {
            query.set(`${grid.prefix}rows`, typeof rows === "string" ? rows : this.rowsPerPage.value);
        }

        grid.reload();
    }

    private cleanUp() {
        delete this.element.dataset.showPageSizes;
        delete this.element.dataset.totalPages;
        delete this.element.dataset.totalRows;
    }
    private bind() {
        const pager = this;

        for (const page of pager.pages) {
            page.addEventListener("click", function () {
                pager.apply(this.dataset.page!);
            });
        }

        pager.grid.element.querySelectorAll<HTMLInputElement>(".mvc-grid-pager-rows").forEach(rowsPerPage => {
            rowsPerPage.addEventListener("change", function () {
                const rows = parseInt(this.value);

                if (!isNaN(rows) && rows >= 0) {
                    const totalPages = rows === 0 ? 1 : Math.ceil(pager.totalRows / rows);

                    pager.apply(Math.min(pager.currentPage, totalPages).toString(), rows.toString());
                }
            });
        });
    }
}

export class MvcGridPopup {
    public static draggedElement: HTMLElement | null;
    public static draggedColumn: MvcGridColumn | null;
    public static lastActiveElement: HTMLElement | null;
    public static element = document.createElement("div");

    public static showConfiguration(grid: MvcGrid, anchor?: HTMLElement) {
        const popup = this;

        popup.lastActiveElement = document.activeElement as HTMLElement;
        popup.element.className = "mvc-grid-popup mvc-grid-configuration";
        popup.element.innerHTML = `<div class="popup-arrow"></div><div class="popup-content"></div>`;

        const content = popup.element.querySelector(".popup-content")!;

        content.appendChild(popup.createDropzone());

        for (const column of grid.columns) {
            content.appendChild(popup.createPreference(column));
            content.appendChild(popup.createDropzone());
        }

        if (grid.columns.length) {
            document.body.appendChild(popup.element);
        }

        popup.reposition(grid, anchor);
        popup.bind();
    }
    public static show(filter: MvcGridColumnFilter) {
        if (!filter.instance) {
            return;
        }

        const popup = this;
        const filterer = filter.instance;

        popup.lastActiveElement = document.activeElement as HTMLElement;
        popup.element.className = `mvc-grid-popup ${filterer.cssClasses}`.trim();
        popup.element.innerHTML = `<div class="popup-arrow"></div><div class="popup-content">${filterer.render()}</div>`;

        document.body.appendChild(popup.element);

        popup.bind();
        popup.setValues(filter);
        popup.reposition(filter.column.grid, filter.button);

        filterer.bindOperator();
        filterer.bindMethods();
        filterer.bindValues();
        filterer.bindActions();

        popup.element.querySelector<HTMLInputElement>(".mvc-grid-value")!.focus();
    }
    public static hide(e?: UIEvent) {
        const popup = MvcGridPopup;
        const initiator = e && e.target as Element;
        const visible = popup.element.parentNode;
        const outside = !initiator?.closest(".mvc-grid-popup,.mvc-grid-filter");

        if (visible && outside) {
            document.body.removeChild(popup.element);

            if (popup.lastActiveElement) {
                popup.lastActiveElement.focus();
                popup.lastActiveElement = null;
            }
        }
    }

    private static setValues(filter: MvcGridColumnFilter) {
        const popup = this;

        popup.setValue(`.mvc-grid-operator`, [filter.operator]);
        popup.setValue(`.mvc-grid-value[data-filter="first"]`, filter.first.values);
        popup.setValue(`.mvc-grid-value[data-filter="second"]`, filter.second.values);
        popup.setValue(`.mvc-grid-method[data-filter="first"]`, [filter.first.method]);
        popup.setValue(`.mvc-grid-method[data-filter="second"]`, [filter.second.method]);
    }
    private static setValue(selector: string, values: string[]) {
        const input = this.element.querySelector<HTMLElement>(selector);

        if (input) {
            if (input.tagName === "SELECT" && (input as HTMLSelectElement).multiple) {
                for (const option of Array.from((input as HTMLSelectElement).options)) {
                    option.selected = values.indexOf(option.value) >= 0;
                }
            } else {
                (input as HTMLInputElement).value = values[0] || "";
            }
        }
    }

    private static createPreference(column: MvcGridColumn) {
        const popup = this;
        const name = document.createElement("span");
        const checkbox = document.createElement("input");
        const preference = document.createElement("label");

        checkbox.type = "checkbox";
        preference.draggable = true;
        preference.className = "mvc-grid-column";

        if (column.filter?.inlineInput) {
            name.innerText = column.filter.inlineInput.placeholder;
        } else {
            name.innerText = column.header.innerText.trim();
        }

        checkbox.checked = !column.isHidden;

        checkbox.addEventListener("change", () => {
            const i = column.grid.columns.indexOf(column);

            for (const tr of column.grid.element.querySelectorAll("tr")) {
                if (checkbox.checked) {
                    tr.children[i].classList.remove("mvc-grid-hidden");
                } else {
                    tr.children[i].classList.add("mvc-grid-hidden");
                }
            }

            column.isHidden = !checkbox.checked;

            column.grid.element.dispatchEvent(new CustomEvent("gridconfigure", {
                detail: { grid: column.grid },
                bubbles: true
            }));
        });

        preference.addEventListener("dragstart", () => {
            popup.draggedColumn = column;
            popup.draggedElement = preference;
            preference.style.opacity = "0.4";
            preference.parentElement!.classList.add("mvc-grid-dragging");
        });

        preference.addEventListener("dragend", () => {
            popup.draggedColumn = null;
            popup.draggedElement = null;
            preference.style.opacity = "";
            preference.parentElement!.classList.remove("mvc-grid-dragging");
        });

        preference.appendChild(checkbox);
        preference.appendChild(name);

        return preference;
    }
    private static createDropzone() {
        const dropzone = document.createElement("div");

        dropzone.className = "mvc-grid-dropzone";

        dropzone.addEventListener("dragenter", () => {
            dropzone.classList.add("hover");
        });

        dropzone.addEventListener("dragover", e => {
            e.preventDefault();
        });

        dropzone.addEventListener("dragleave", () => {
            dropzone.classList.remove("hover");
        });

        dropzone.addEventListener("drop", () => {
            const popup = this;
            const dragged = popup.draggedElement!;
            const grid = popup.draggedColumn!.grid;

            if (dropzone !== dragged.previousElementSibling && dropzone !== dragged.nextElementSibling) {
                const index = Array.from(popup.element.querySelectorAll(".mvc-grid-dropzone")).indexOf(dropzone);
                const i = grid.columns.indexOf(popup.draggedColumn!);

                dropzone.parentElement!.insertBefore(dragged.previousElementSibling!, dropzone);
                dropzone.parentElement!.insertBefore(dragged, dropzone);

                for (const tr of grid.element.querySelectorAll("tr")) {
                    tr.insertBefore(tr.children[i], tr.children[index]);
                }

                grid.columns.splice(index - (i < index ? 1 : 0), 0, grid.columns.splice(i, 1)[0]);

                grid.element.dispatchEvent(new CustomEvent("gridconfigure", {
                    detail: { grid },
                    bubbles: true
                }));
            }

            dropzone.classList.remove("hover");
        });

        return dropzone;
    }

    private static reposition(grid: MvcGrid, anchor?: HTMLElement) {
        const element = this.element;
        const style = getComputedStyle(element);
        const arrow = element.querySelector<HTMLElement>(".popup-arrow")!;
        let { top, left } = (anchor || grid.element).getBoundingClientRect();

        top += window.pageYOffset - parseFloat(style.borderTopWidth);
        left += window.pageXOffset - parseFloat(style.borderLeftWidth);

        if (anchor) {
            left -= parseFloat(style.marginLeft) - anchor.offsetWidth / 2 + 26;

            const arrowLeft = 26 - parseFloat(getComputedStyle(arrow).borderLeftWidth);
            const width = parseFloat(style.marginLeft) + element.offsetWidth + parseFloat(style.marginRight);
            const offset = Math.max(0, left + width - window.pageXOffset - document.documentElement.clientWidth);

            top += anchor.offsetHeight / 3 * 2 + arrow.offsetHeight - parseFloat(style.marginTop);
            arrow.style.left = `${Math.max(0, arrowLeft + offset)}px`;
            left -= offset;
        }

        element.style.left = `${Math.max(0, left)}px`;
        element.style.top = `${Math.max(0, top)}px`;
        arrow.style.display = anchor ? "" : "none";
    }
    private static bind() {
        const popup = this;

        window.addEventListener("mousedown", popup.hide);
        window.addEventListener("touchstart", popup.hide);
    }
}

export class MvcGridFilter {
    public methods: string[];
    public cssClasses: string;
    public column: MvcGridColumn;
    public mode: "row" | "excel" | "header";
    public type: "single" | "double" | "multi";

    public constructor(column: MvcGridColumn) {
        const filter = this;

        filter.column = column;
        filter.type = column.filter!.type;
        filter.mode = column.grid.filterMode;
        filter.methods = ["equals", "not-equals"];
        filter.cssClasses = "mvc-grid-default-filter";
    }

    public init() {
        const filter = this;
        const column = filter.column;
        const columnFilter = column.filter!;

        if (!columnFilter.options && filter.mode !== "excel") {
            filter.validate(columnFilter.inlineInput!);
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
    public isValid(value: string) {
        return !value || true;
    }
    public validate(input: HTMLInputElement) {
        if (this.isValid(input.value)) {
            input.classList.remove("invalid");
        } else {
            input.classList.add("invalid");
        }
    }

    public render() {
        const filter = this;

        return `<div class="popup-filter">
                    ${filter.renderFilter("first")}
                </div>
                ${filter.mode === "excel" && filter.type === "double"
                    ? `${filter.renderOperator()}
                    <div class="popup-filter">
                        ${filter.renderFilter("second")}
                    </div>`
                    : ""}
                ${filter.renderActions()}`;
    }
    public renderFilter(name: "first" | "second") {
        const filter = this;
        const options = filter.column.filter!.options;
        const lang = MvcGrid.lang[filter.column.filter!.name] || {};
        const multiple = filter.type === "multi" ? " multiple" : "";
        const methods = filter.methods.map(method => `<option value="${method}">${lang[method] || ""}</option>`).join("");

        return `<div class="popup-group">
                    <select class="mvc-grid-method" data-filter="${name}">
                        ${methods}
                    </select>
                </div>
                <div class="popup-group">${options
                    ? `<select class="mvc-grid-value" data-filter="${name}"${multiple}>
                          ${options.innerHTML}
                       </select>`
                    : `<input class="mvc-grid-value" data-filter="${name}">`}
                </div>`;
    }
    public renderOperator() {
        const lang = MvcGrid.lang.operator!;

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
    public renderActions() {
        const lang = MvcGrid.lang.filter!;

        return `<div class="popup-actions">
                    <button type="button" class="mvc-grid-apply" type="button">${lang.apply}</button>
                    <button type="button" class="mvc-grid-cancel" type="button">${lang.remove}</button>
                </div>`;
    }

    public bindOperator() {
        const filter = this.column.filter!;
        const operator = MvcGridPopup.element.querySelector<HTMLSelectElement>(".mvc-grid-operator");

        if (operator) {
            operator.addEventListener("change", function () {
                filter.operator = this.value;
            });
        }
    }
    public bindMethods() {
        const filter = this.column.filter!;

        for (const method of MvcGridPopup.element.querySelectorAll<HTMLInputElement>(".mvc-grid-method")) {
            method.addEventListener("change", function () {
                filter[this.dataset.filter as "first" | "second"].method = this.value;
            });
        }
    }
    public bindValues() {
        const filter = this;

        for (const input of MvcGridPopup.element.querySelectorAll<HTMLInputElement | HTMLSelectElement>(".mvc-grid-value")) {
            if (input.tagName === "SELECT") {
                input.addEventListener("change", () => {
                    const options = Array.from((input as HTMLSelectElement).options).filter(option => option.selected);

                    filter.column.filter![input.dataset.filter as "first" | "second"].values = options.map(option => option.value);

                    if (filter.mode !== "excel") {
                        const inlineInput = filter.column.filter!.inlineInput!;

                        if (filter.mode === "header" || filter.type === "multi") {
                            inlineInput.value = options.map(option => option.text).join(", ");
                        } else {
                            inlineInput.value = input.value;
                        }

                        filter.validate(inlineInput);
                    }
                });
            } else {
                input.addEventListener("input", () => {
                    filter.column.filter![input.dataset.filter as "first" | "second"].values = [input.value];

                    if (filter.mode !== "excel") {
                        const inlineInput = filter.column.filter!.inlineInput!;

                        inlineInput.value = filter.column.filter![input.dataset.filter as "first" | "second"].values.join(", ");

                        filter.validate(inlineInput);
                    }

                    filter.validate(input as HTMLInputElement);
                });

                (input as HTMLInputElement).addEventListener("keyup", function (e) {
                    if (e.which === 13 && filter.isValid(this.value)) {
                        filter.column.filter!.apply();
                    }
                });

                filter.validate(input as HTMLInputElement);
            }
        }
    }
    public bindActions() {
        const filter = this.column.filter!;
        const popup = MvcGridPopup.element;

        popup.querySelector(".mvc-grid-apply")!.addEventListener("click", filter.apply.bind(filter));
        popup.querySelector(".mvc-grid-cancel")!.addEventListener("click", filter.cancel.bind(filter));
    }
}

export class MvcGridTextFilter extends MvcGridFilter {
    public constructor(column: MvcGridColumn) {
        super(column);

        this.cssClasses = "mvc-grid-text-filter";
        this.methods = ["contains", "equals", "not-equals", "starts-with", "ends-with"];
    }
}

export class MvcGridNumberFilter extends MvcGridFilter {
    public constructor(column: MvcGridColumn) {
        super(column);

        this.cssClasses = "mvc-grid-number-filter";
        this.methods = ["equals", "not-equals", "less-than", "greater-than", "less-than-or-equal", "greater-than-or-equal"];
    }

    public isValid(value: string) {
        return !value || /^(?=.*\d+.*)[-+]?\d*[.,]?\d*$/.test(value);
    }
}

export class MvcGridDateFilter extends MvcGridFilter {
    public constructor(column: MvcGridColumn) {
        super(column);

        this.cssClasses = "mvc-grid-date-filter";
        this.methods = ["equals", "not-equals", "earlier-than", "later-than", "earlier-than-or-equal", "later-than-or-equal"];
    }
}

export class MvcGridGuidFilter extends MvcGridFilter {
    public constructor(column: MvcGridColumn) {
        super(column);

        this.cssClasses = "mvc-grid-guid-filter";
    }

    public isValid(value: string) {
        return !value || /^[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}$/i.test(value);
    }
}
