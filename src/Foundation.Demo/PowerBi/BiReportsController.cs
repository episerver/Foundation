using System.Web.Mvc;

namespace Foundation.Demo.PowerBi
{
    public class BiReportsController : Controller
    {
        public ActionResult Index(string id)
        {
            var action = id;
            if (string.IsNullOrEmpty(action))
            {
                action = "commerce";
            }



            var model = new ReportsViewModel
            {
                Action = action
            };

            if (action.Equals("commerce"))
            {
                model.FrameSource = "https://app.powerbi.com/view?r=eyJrIjoiMzhkNjk0MzUtOWI1NC00ZGVjLWE2MDctYTFkNjg0MzA3M2JmIiwidCI6IjNlYzAwZDc5LTAyMWEtNDJkNC1hYWM4LWRjYjM1OTczZGZmMiIsImMiOjh9";
            }
            else if (action.Equals("marketing"))
            {
                model.FrameSource = "https://app.powerbi.com/view?r=eyJrIjoiMmUyNTU0YzMtOWJiYy00OGJiLTllMjItZWNiYWFhYmNjYjE0IiwidCI6IjNlYzAwZDc5LTAyMWEtNDJkNC1hYWM4LWRjYjM1OTczZGZmMiIsImMiOjh9";
            }
            else if (action.Equals("google"))
            {
                model.FrameSource = "https://app.powerbi.com/view?r=eyJrIjoiMDFjYWE5NjItNmJhOC00MTlkLThlYjMtYzI3ZWY3MDYyNGU2IiwidCI6IjNlYzAwZDc5LTAyMWEtNDJkNC1hYWM4LWRjYjM1OTczZGZmMiIsImMiOjh9";
            }
            else if (action.Equals("segments"))
            {
                model.FrameSource = "https://app.powerbi.com/view?r=eyJrIjoiY2ZlMTNmODYtNTVkNS00ZWMwLTk4ZTAtMjE0ODg2MzBjMWZkIiwidCI6IjNlYzAwZDc5LTAyMWEtNDJkNC1hYWM4LWRjYjM1OTczZGZmMiIsImMiOjh9";
            }
            return View(model);
        }
    }
}
