/*!
 * Mvc.Grid 6.0.0
 * https://github.com/NonFactors/MVC6.Grid
 *
 * Copyright © NonFactors
 *
 * Licensed under the terms of the MIT License
 * http://www.opensource.org/licenses/mit-license.php
 */
interface MvcGridOptions {
    url: URL;
    id: string;
    query: string;
    isAjax: boolean;
    requestType: string;
    loadingDelay: number;
    filters: {
        [type: string]: typeof MvcGridFilter;
    };
}
interface MvcGridLanguage {
    [type: string]: {
        [method: string]: string;
    };
}
interface MvcGridColumnSort {
    order: string;
    first: string;
    button: Element;
}
interface MvcGridColumnFilter {
    type: string;
    name: string;
    isApplied: boolean;
    hasOptions: boolean;
    defaultMethod: string;
    second: {
        method: string;
        values: string[];
    };
    operator: string;
    first: {
        method: string;
        values: string[];
    };
    button: HTMLElement;
    instance: MvcGridFilter;
    options: HTMLSelectElement;
    inlineInput: HTMLInputElement;
}
export declare class MvcGrid {
    private static instances;
    static lang: MvcGridLanguage;
    element: HTMLElement;
    columns: MvcGridColumn[];
    popup: MvcGridPopup;
    pager?: MvcGridPager;
    loader?: HTMLDivElement;
    controller: AbortController;
    url: URL;
    name: string;
    prefix: string;
    isAjax: boolean;
    loading: number;
    filterMode: string;
    requestType: string;
    loadingDelay: number;
    filters: {
        [type: string]: typeof MvcGridFilter;
    };
    constructor(container: HTMLElement, options?: Partial<MvcGridOptions>);
    set(options: Partial<MvcGridOptions>): MvcGrid;
    reload(): void;
    applyFilters(initiator: MvcGridColumn): void;
    private startLoading;
    private stopLoading;
    private clearQuery;
    private findGrid;
    private cleanUp;
    private bind;
}
export declare class MvcGridColumn {
    name: string;
    grid: MvcGrid;
    isHidden: boolean;
    header: HTMLElement;
    sort?: MvcGridColumnSort;
    filter?: MvcGridColumnFilter;
    rowFilter: HTMLElement | null;
    constructor(grid: MvcGrid, header: HTMLElement, rowFilter: HTMLElement);
    updateFilter(): void;
    cancelFilter(): void;
    applySort(): void;
    private bindFilter;
    private bindSort;
    private cleanUp;
}
export declare class MvcGridPager {
    grid: MvcGrid;
    currentPage: number;
    element: HTMLElement;
    showPageSizes: boolean;
    rowsPerPage: HTMLInputElement;
    pages: NodeListOf<HTMLElement>;
    constructor(grid: MvcGrid, element: HTMLElement);
    apply(page: string | number): void;
    private cleanUp;
    private bind;
}
export declare class MvcGridPopup {
    static lastActiveElement: HTMLInputElement | null;
    static element: HTMLDivElement;
    grid: MvcGrid;
    constructor(grid: MvcGrid);
    render(filter: MvcGridFilter): void;
    show(column: MvcGridColumn): void;
    hide(e?: UIEvent): void;
    private updatePosition;
    private updateValues;
    private setValues;
    private bind;
}
export declare class MvcGridFilter {
    type: string;
    mode: string;
    methods: string[];
    cssClasses: string;
    popup: MvcGridPopup;
    lang: MvcGridLanguage;
    column: MvcGridColumn;
    constructor(column: MvcGridColumn);
    init(): void;
    show(): void;
    render(): string;
    renderFilter(name: string): string;
    renderOperator(): string;
    renderActions(): string;
    apply(): void;
    cancel(): void;
    isValid(value: string): boolean;
    validate(input: HTMLInputElement | HTMLSelectElement): void;
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
export declare class MvcGridEnumFilter extends MvcGridFilter {
    constructor(column: MvcGridColumn);
}
export declare class MvcGridGuidFilter extends MvcGridFilter {
    constructor(column: MvcGridColumn);
    isValid(value: string): boolean;
}
export declare class MvcGridBooleanFilter extends MvcGridFilter {
    constructor(column: MvcGridColumn);
}
export {};
