/*!
 * Mvc.Grid 7.0.1
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
        width: string;
        hidden: boolean;
    }[];
}
export declare class MvcGrid {
    private static instances;
    static lang: MvcGridLanguage;
    element: HTMLElement;
    columns: MvcGridColumn[];
    pager?: MvcGridPager;
    controller: AbortController;
    url: URL;
    name: string;
    prefix: string;
    isAjax: boolean;
    loadingTimerId: number;
    loadingDelay: number | null;
    sort: Map<string, "asc" | "desc">;
    filterMode: "row" | "excel" | "header";
    filters: {
        [type: string]: typeof MvcGridFilter | undefined;
    };
    constructor(container: HTMLElement, options?: Partial<MvcGridOptions>);
    set(options: Partial<MvcGridOptions>): this;
    showConfiguration(anchor?: HTMLElement): void;
    getConfiguration(): MvcGridConfiguration;
    configure(configuration: MvcGridConfiguration): void;
    reload(): void;
    private buildSort;
    private findGrid;
    private cleanUp;
    private bind;
}
export declare class MvcGridColumn {
    name: string;
    grid: MvcGrid;
    isHidden: boolean;
    header: HTMLElement;
    sort: MvcGridColumnSort | null;
    filter: MvcGridColumnFilter | null;
    constructor(grid: MvcGrid, header: HTMLElement, rowFilter?: HTMLElement);
    private cleanUp;
}
export declare class MvcGridColumnSort {
    column: MvcGridColumn;
    button: HTMLButtonElement;
    first: "asc" | "desc";
    order: "asc" | "desc" | "";
    constructor(column: MvcGridColumn);
    toggle(multi: boolean): void;
    private bind;
}
export declare class MvcGridColumnFilter {
    name: string;
    isApplied: boolean;
    defaultMethod: string;
    type: "single" | "double" | "multi";
    first: {
        method: string;
        values: string[];
    };
    operator: string;
    second: {
        method: string;
        values: string[];
    };
    column: MvcGridColumn;
    rowFilter?: HTMLElement;
    instance?: MvcGridFilter;
    button: HTMLButtonElement;
    options: HTMLSelectElement | null;
    inlineInput: HTMLInputElement | null;
    constructor(column: MvcGridColumn, rowFilter?: HTMLElement);
    apply(): void;
    cancel(): void;
    private bind;
}
export declare class MvcGridPager {
    grid: MvcGrid;
    totalRows: number;
    currentPage: number;
    element: HTMLElement;
    showPageSizes: boolean;
    rowsPerPage: HTMLInputElement;
    pages: NodeListOf<HTMLElement>;
    constructor(grid: MvcGrid, element: HTMLElement);
    apply(page: string, rows?: string): void;
    private cleanUp;
    private bind;
}
export declare class MvcGridPopup {
    static draggedElement: HTMLElement | null;
    static draggedColumn: MvcGridColumn | null;
    static lastActiveElement: HTMLElement | null;
    static element: HTMLDivElement;
    static showConfiguration(grid: MvcGrid, anchor?: HTMLElement): void;
    static show(filter: MvcGridColumnFilter): void;
    static hide(e?: UIEvent): void;
    private static setValues;
    private static setValue;
    private static createPreference;
    private static createDropzone;
    private static reposition;
    private static bind;
}
export declare class MvcGridFilter {
    methods: string[];
    cssClasses: string;
    column: MvcGridColumn;
    mode: "row" | "excel" | "header";
    type: "single" | "double" | "multi";
    constructor(column: MvcGridColumn);
    init(): void;
    isValid(value: string): boolean;
    validate(input: HTMLInputElement): void;
    render(): string;
    renderFilter(name: "first" | "second"): string;
    renderOperator(): string;
    renderActions(): string;
    bindOperator(): void;
    bindMethods(): void;
    bindValues(): void;
    bindActions(): void;
}
export declare class MvcGridTextFilter extends MvcGridFilter {
    constructor(column: MvcGridColumn);
}
export declare class MvcGridNumberFilter extends MvcGridFilter {
    constructor(column: MvcGridColumn);
    isValid(value: string): boolean;
}
export declare class MvcGridDateFilter extends MvcGridFilter {
    constructor(column: MvcGridColumn);
}
export declare class MvcGridGuidFilter extends MvcGridFilter {
    constructor(column: MvcGridColumn);
    isValid(value: string): boolean;
}
