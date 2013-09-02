using System.Web.Mvc;

namespace Analyzer.Web.Controllers
{
    public class SearchController : Controller
    {
        public ActionResult Index(string term)
        {
            var nodeController = new NodeController();
            return View(nodeController.Search(term));
        }
    }
}
