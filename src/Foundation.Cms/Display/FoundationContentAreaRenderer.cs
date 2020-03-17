﻿using EPiBootstrapArea;
using EPiServer.Core;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Cms.Display
{
    public class FoundationContentAreaRenderer : BootstrapAwareContentAreaRenderer
    {
        public FoundationContentAreaRenderer() : base(new FoundationDisplayModeProvider().GetAll())
        {
        }

        protected override void RenderContentAreaItems(HtmlHelper html, IEnumerable<ContentAreaItem> contentAreaItems)
        {
            TagBuilder currentRow;

            foreach (var contentAreaItem in contentAreaItems)
            {
                string templateTag = GetContentAreaItemTemplateTag(html, contentAreaItem);
                bool isScreenContentAreaItem = IsScreenWidthTag(templateTag);

                if (isScreenContentAreaItem)
                {
                    currentRow = new TagBuilder("div");
                    currentRow.AddCssClass("screen-width-block");
                    html.ViewContext.Writer.Write(currentRow.ToString(TagRenderMode.StartTag));
                    RenderContentAreaItem(html, contentAreaItem, templateTag, GetContentAreaItemHtmlTag(html, contentAreaItem), GetContentAreaItemCssClass(html, contentAreaItem, templateTag));
                    html.ViewContext.Writer.Write(currentRow.ToString(TagRenderMode.EndTag));
                }
                else
                {
                    RenderContentAreaItem(html, contentAreaItem, templateTag, GetContentAreaItemHtmlTag(html, contentAreaItem), GetContentAreaItemCssClass(html, contentAreaItem, templateTag));
                }
            }
        }

        protected virtual string GetContentAreaItemCssClass(HtmlHelper html, ContentAreaItem contentAreaItem, string templateTag)
        {
            var baseClass = base.GetContentAreaItemCssClass(html, contentAreaItem);

            if (!string.IsNullOrEmpty(baseClass))
            {
                return baseClass;
            }

            return string.Format("block {0}", templateTag);
        }

        protected virtual bool IsScreenWidthTag(string templateTag)
        {
            return templateTag == "displaymode-screen";
        }
    }
}
