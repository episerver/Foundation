using EPiServer.Shell.ObjectEditing;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace Foundation.Cms.EditorDescriptors
{
    public class FontAwesomeSelectionFactory : ISelectionFactory
    {
        private static IEnumerable<ISelectItem> _allFonts;

        /// <summary>
        /// Adapts all font information for dropdownlist to display.
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var fontNames = GetFontAwesomeNames();

            if (_allFonts == null)
            {
                _allFonts = fontNames.Select(f => new SelectItem() { Text = f, Value = f });
            }
            return _allFonts;
        }

        /// <summary>
        /// Retrieves all font names.
        /// </summary>
        /// <returns>List of all Font-awesome's class names</returns>
        private IEnumerable<string> GetFontAwesomeNames()
        {
            var fonts = new List<string>();
            var path = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"Assets\scss\vendors\font-awesome-5.9.0\font-awesome-list.json");
            var data = File.ReadAllText(path);
            IList<JToken> results = JObject.Parse(data)["icons"].Children().ToList();
            foreach (var item in results)
            {
                string font = item.ToObject<string>();
                fonts.Add(font);
            }

            return fonts.OrderBy(x => x);
        }
    }
}
