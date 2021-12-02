using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace NonFactors.Mvc.Grid;

public static class HtmlGridExtensions
{
    public static IHtmlGrid<T> Build<T>(this IHtmlGrid<T> html, Action<IGridColumnsOf<T>> builder)
    {
        builder(html.Grid.Columns);

        html.Grid.Processors.Add(html.Grid.Sort);

        return html;
    }

    public static IHtmlGrid<T> Filterable<T>(this IHtmlGrid<T> html, GridFilterType? type = null)
    {
        foreach (IGridColumn<T> column in html.Grid.Columns)
        {
            column.Filter.IsEnabled ??= true;
            column.Filter.Type ??= type;
        }

        return html;
    }
    public static IHtmlGrid<T> Filterable<T>(this IHtmlGrid<T> html, GridFilterCase filterCase)
    {
        foreach (IGridColumn<T> column in html.Grid.Columns)
        {
            column.Filter.Case ??= filterCase;
            column.Filter.IsEnabled ??= true;
        }

        return html;
    }
    public static IHtmlGrid<T> Sortable<T>(this IHtmlGrid<T> html)
    {
        foreach (IGridColumn<T> column in html.Grid.Columns)
            column.Sort.IsEnabled ??= true;

        return html;
    }

    public static IHtmlGrid<T> RowAttributed<T>(this IHtmlGrid<T> html, Func<T, Object> htmlAttributes)
    {
        html.Grid.Rows.Attributes = htmlAttributes;

        return html;
    }
    public static IHtmlGrid<T> Attributed<T>(this IHtmlGrid<T> html, Object htmlAttributes)
    {
        foreach ((String key, Object? value) in HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes))
            html.Grid.Attributes[key] = value;

        return html;
    }
    public static IHtmlGrid<T> AppendCss<T>(this IHtmlGrid<T> html, String cssClasses)
    {
        if (html.Grid.Attributes.ContainsKey("class"))
            html.Grid.Attributes["class"] = (html.Grid.Attributes["class"] + " " + cssClasses.TrimStart()).Trim();
        else
            html.Grid.Attributes["class"] = cssClasses.Trim();

        return html;
    }
    public static IHtmlGrid<T> Empty<T>(this IHtmlGrid<T> html, IHtmlContent content)
    {
        using StringWriter writer = new();

        content.WriteTo(writer, NullHtmlEncoder.Default);
        html.Grid.EmptyText = writer.ToString();

        return html;
    }
    public static IHtmlGrid<T> Css<T>(this IHtmlGrid<T> html, String cssClasses)
    {
        html.Grid.Attributes["class"] = cssClasses.Trim();

        return html;
    }
    public static IHtmlGrid<T> Empty<T>(this IHtmlGrid<T> html, String text)
    {
        html.Grid.EmptyText = HtmlEncoder.Default.Encode(text);

        return html;
    }
    public static IHtmlGrid<T> Named<T>(this IHtmlGrid<T> html, String name)
    {
        html.Grid.Name = name;

        return html;
    }
    public static IHtmlGrid<T> Id<T>(this IHtmlGrid<T> html, String? id)
    {
        html.Grid.Id = id;

        return html;
    }

    public static IHtmlGrid<T> UsingFooter<T>(this IHtmlGrid<T> html, String partialViewName)
    {
        html.Grid.FooterPartialViewName = partialViewName;

        return html;
    }
    public static IHtmlGrid<T> Using<T>(this IHtmlGrid<T> html, IGridProcessor<T> processor)
    {
        html.Grid.Processors.Add(processor);

        return html;
    }
    public static IHtmlGrid<T> Using<T>(this IHtmlGrid<T> html, GridProcessingMode mode)
    {
        html.Grid.Mode = mode;

        return html;
    }
    public static IHtmlGrid<T> Using<T>(this IHtmlGrid<T> html, GridFilterMode mode)
    {
        html.Grid.FilterMode = mode;

        return html;
    }
    public static IHtmlGrid<T> UsingUrl<T>(this IHtmlGrid<T> html, String url)
    {
        html.Grid.Url = url;

        return html;
    }

    public static IHtmlGrid<T> Pageable<T>(this IHtmlGrid<T> html, Action<IGridPager<T>>? builder = null)
    {
        html.Grid.Pager ??= new GridPager<T>(html.Grid);
        html.Grid.Processors.Add(html.Grid.Pager);

        builder?.Invoke(html.Grid.Pager);

        return html;
    }

    public static IHtmlGrid<T> Configure<T>(this IHtmlGrid<T> html, GridConfig grid)
    {
        List<IGridColumn<T>> columns = html.Grid.Columns.ToList();
        html.Grid.Columns.Clear();

        foreach (GridColumnConfig config in grid.Columns)
            if (columns.Find(col => String.Equals(col.Name, config.Name, StringComparison.OrdinalIgnoreCase)) is IGridColumn<T> column)
            {
                columns.Remove(column);
                html.Grid.Columns.Add(column);
                column.IsHidden = config.Hidden;
                column.Style = String.IsNullOrWhiteSpace(config.Width) ? column.Style : $"width: {config.Width.Split(";")[0]}";
            }

        foreach (IGridColumn<T> column in columns)
            html.Grid.Columns.Add(column);

        return html;
    }
}
