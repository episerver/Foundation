using EPiBootstrapArea;
using EPiServer.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Infrastructure.Display
{
    public class FoundationContentAreaRenderer : BootstrapAwareContentAreaRenderer
    {
        public FoundationContentAreaRenderer() : base(new FoundationDisplayModeProvider().GetAll())
        {
        }

        protected override void RenderContentAreaItems(HtmlHelper htmlHelper, IEnumerable<ContentAreaItem> contentAreaItems)
        {
            bool? result = null;
            var actualValue = htmlHelper.ViewContext.ViewData["rowsupport"];
            if (actualValue is bool)
            {
                result = (bool)actualValue;
            }
            var isRowSupported = result;
            var addRowMarkup = ConfigurationContext.Current.RowSupportEnabled && isRowSupported.HasValue && isRowSupported.Value;

            // there is no need to proceed if row rendering support is disabled
            if (!addRowMarkup)
            {
                CustomizedRenderContentAreaItems(htmlHelper, contentAreaItems);
                return;
            }

            var rowRender = new RowRenderer();
            rowRender.Render(contentAreaItems,
                             htmlHelper,
                             GetContentAreaItemTemplateTag,
                             GetColumnWidth,
                             CustomizedRenderContentAreaItems);
        }

        protected virtual void CustomizedRenderContentAreaItems(HtmlHelper htmlHelper, IEnumerable<ContentAreaItem> contentAreaItems)
        {
            TagBuilder currentRow;
            foreach (var contentAreaItem in contentAreaItems)
            {
                var templateTag = GetContentAreaItemTemplateTag(htmlHelper, contentAreaItem);
                var isScreenContentAreaItem = IsScreenWidthTag(templateTag);

                if (isScreenContentAreaItem)
                {
                    currentRow = new TagBuilder("div");
                    currentRow.AddCssClass("screen-width-block");
                    htmlHelper.ViewContext.Writer.Write(currentRow.ToString(TagRenderMode.StartTag));
                    RenderContentAreaItem(htmlHelper, contentAreaItem, templateTag, GetContentAreaItemHtmlTag(htmlHelper, contentAreaItem), GetContentAreaItemCssClass(htmlHelper, contentAreaItem, templateTag));
                    htmlHelper.ViewContext.Writer.Write(currentRow.ToString(TagRenderMode.EndTag));
                }
                else
                {
                    RenderContentAreaItem(htmlHelper, contentAreaItem, templateTag, GetContentAreaItemHtmlTag(htmlHelper, contentAreaItem), GetContentAreaItemCssClass(htmlHelper, contentAreaItem, templateTag));
                }
            }
        }

        protected virtual string GetContentAreaItemCssClass(HtmlHelper htmlHelper, ContentAreaItem contentAreaItem, string templateTag)
        {
            var baseClass = base.GetContentAreaItemCssClass(htmlHelper, contentAreaItem);

            if (!string.IsNullOrEmpty(baseClass))
            {
                return baseClass;
            }

            return string.Format("block {0}", templateTag);
        }

        protected virtual bool IsScreenWidthTag(string templateTag) => templateTag == "displaymode-screen";

        protected virtual int GetColumnWidth(string templateTag)
        {
            var displayModes = new FoundationDisplayModeProvider().GetAll();
            var displayMode = displayModes.FirstOrDefault(x => x.Tag == templateTag);
            return displayMode?.LargeScreenWidth ?? 12;
        }
    }
}
