using EPiServer;
using EPiServer.Core;
using EPiServer.Core.Html.StringParsing;
using EPiServer.Web.Mvc.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Foundation.Infrastructure.Display
{
    /// <summary>
    /// Extends the default <see cref="ContentAreaRenderer"/> to apply custom CSS classes to each <see cref="ContentFragment"/>.
    /// </summary>
    public class FoundationContentAreaRenderer : ContentAreaRenderer
    {
        protected override string GetContentAreaItemCssClass(IHtmlHelper htmlHelper, ContentAreaItem contentAreaItem)
        {
            var baseItemClass = base.GetContentAreaItemCssClass(htmlHelper, contentAreaItem);

            var tag = GetContentAreaItemTemplateTag(htmlHelper, contentAreaItem);
            return $"block {baseItemClass} {GetTypeSpecificCssClasses(contentAreaItem, ContentRepository)} {GetCssClassForTag(tag)} {tag}";
        }

        /// <summary>
        /// Gets a CSS class used for styling based on a tag name (ie a Bootstrap class name)
        /// </summary>
        /// <param name="tagName">Any tag name available, see <see cref="Global.ContentAreaTags"/></param>
        private static string GetCssClassForTag(string tagName)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                return "col-12";
            }
            switch (tagName.ToLower())
            {
                case "col-12":
                    return "full";
                case "col-8":
                    return "wide";
                case "col-6":
                    return "half";
                case "col-4":
                    return "narrow";
                case "col-3":
                    return "one-quarter";
                default:
                    return string.Empty;
            }
        }

        private static string GetTypeSpecificCssClasses(ContentAreaItem contentAreaItem, IContentRepository contentRepository)
        {
            var content = contentAreaItem.GetContent();
            var cssClass = content == null ? String.Empty : content.GetOriginalType().Name.ToLowerInvariant();

            var customClassContent = content as ICustomCssInContentArea;
            if (customClassContent != null && !string.IsNullOrWhiteSpace(customClassContent.ContentAreaCssClass))
            {
                cssClass += string.Format(" {0}", customClassContent.ContentAreaCssClass);
            }

            return cssClass;
        }
    }

    interface ICustomCssInContentArea
    {
        string ContentAreaCssClass { get; }
    }

    //public class FoundationContentAreaRenderer : BootstrapAwareContentAreaRenderer
    //{
    //    public FoundationContentAreaRenderer() : base(new FoundationDisplayModeProvider().GetAll())
    //    {
    //    }

    //    protected override void RenderContentAreaItems(HtmlHelper htmlHelper, IEnumerable<ContentAreaItem> contentAreaItems)
    //    {
    //        bool? result = null;
    //        var actualValue = htmlHelper.ViewContext.ViewData["rowsupport"];
    //        if (actualValue is bool)
    //        {
    //            result = (bool)actualValue;
    //        }
    //        var isRowSupported = result;
    //        var addRowMarkup = ConfigurationContext.Current.RowSupportEnabled && isRowSupported.HasValue && isRowSupported.Value;

    //        // there is no need to proceed if row rendering support is disabled
    //        if (!addRowMarkup)
    //        {
    //            CustomizedRenderContentAreaItems(htmlHelper, contentAreaItems);
    //            return;
    //        }

    //        var rowRender = new RowRenderer();
    //        rowRender.Render(contentAreaItems,
    //                         htmlHelper,
    //                         GetContentAreaItemTemplateTag,
    //                         GetColumnWidth,
    //                         CustomizedRenderContentAreaItems);
    //    }

    //    protected virtual void CustomizedRenderContentAreaItems(HtmlHelper htmlHelper, IEnumerable<ContentAreaItem> contentAreaItems)
    //    {
    //        TagBuilder currentRow;
    //        foreach (var contentAreaItem in contentAreaItems)
    //        {
    //            var templateTag = GetContentAreaItemTemplateTag(htmlHelper, contentAreaItem);
    //            var isScreenContentAreaItem = IsScreenWidthTag(templateTag);

    //            if (isScreenContentAreaItem)
    //            {
    //                currentRow = new TagBuilder("div");
    //                currentRow.AddCssClass("screen-width-block");
    //                htmlHelper.ViewContext.Writer.Write(currentRow.ToString(TagRenderMode.StartTag));
    //                RenderContentAreaItem(htmlHelper, contentAreaItem, templateTag, GetContentAreaItemHtmlTag(htmlHelper, contentAreaItem), GetContentAreaItemCssClass(htmlHelper, contentAreaItem, templateTag));
    //                htmlHelper.ViewContext.Writer.Write(currentRow.ToString(TagRenderMode.EndTag));
    //            }
    //            else
    //            {
    //                RenderContentAreaItem(htmlHelper, contentAreaItem, templateTag, GetContentAreaItemHtmlTag(htmlHelper, contentAreaItem), GetContentAreaItemCssClass(htmlHelper, contentAreaItem, templateTag));
    //            }
    //        }
    //    }

    //    protected virtual string GetContentAreaItemCssClass(HtmlHelper htmlHelper, ContentAreaItem contentAreaItem, string templateTag)
    //    {
    //        var baseClass = base.GetContentAreaItemCssClass(htmlHelper, contentAreaItem);

    //        if (!string.IsNullOrEmpty(baseClass))
    //        {
    //            return baseClass;
    //        }

    //        return string.Format("block {0}", templateTag);
    //    }

    //    protected virtual bool IsScreenWidthTag(string templateTag) => templateTag == "displaymode-screen";

    //    protected virtual int GetColumnWidth(string templateTag)
    //    {
    //        var displayModes = new FoundationDisplayModeProvider().GetAll();
    //        var displayMode = displayModes.FirstOrDefault(x => x.Tag == templateTag);
    //        return displayMode?.LargeScreenWidth ?? 12;
    //    }
    //}
}
