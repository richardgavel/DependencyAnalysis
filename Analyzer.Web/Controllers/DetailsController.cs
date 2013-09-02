using System.Web.Mvc;

namespace Analyzer.Web.Controllers
{
    public class DetailsController : Controller
    {
        public ActionResult Index(long id)
        {
            var nodeController = new NodeController();
            return View(nodeController.Get(id));
        }
    }
}
