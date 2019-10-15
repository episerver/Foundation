using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Foundation.Cms.Blocks;
using Foundation.Cms.ViewModels.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Foundation.Features.Blocks
{
    [TemplateDescriptor(Default = true)]
    public class RssReaderBlockController : BlockController<RssReaderBlock>
    {
        public override ActionResult Index(RssReaderBlock currentBlock)
        {
            var viewModel = new RssReaderBlockViewModel
            {
                RssList = new List<RssReaderBlockViewModel.RssItem>(),
                CurrentBlock = currentBlock
            };

            try
            {
                if ((currentBlock.RssUrl != null) && (!currentBlock.RssUrl.IsEmpty()))
                {
                    var rssDocument = XDocument.Load(Convert.ToString(currentBlock.RssUrl));

                    var posts = from item in rssDocument.Descendants("item").Take(currentBlock.MaxCount)
                                select new RssReaderBlockViewModel.RssItem
                                {
                                    Title = item.Element("title").Value,
                                    Url = item.Element("link").Value,
                                    PublishDate = item.Element("pubDate").Value,
                                };

                    viewModel.RssList = posts.ToList();
                    viewModel.HasHeadingText = HasHeadingText(currentBlock);
                    viewModel.Heading = currentBlock.Heading;
                    viewModel.DescriptiveText = currentBlock.MainBody;
                }
            }
            catch (Exception)
            {
                viewModel.HasHeadingText = true;
                viewModel.Heading = "Invalid RSS Feed URL.";
            }


            return PartialView("~/Features/Blocks/Views/RssReaderBlock.cshtml", viewModel);
        }

        private bool HasHeadingText(RssReaderBlock currentBlock)
        {
            return ((!string.IsNullOrEmpty(currentBlock.Heading)) || ((currentBlock.MainBody != null) && (!currentBlock.MainBody.IsEmpty)));
        }
    }
}
