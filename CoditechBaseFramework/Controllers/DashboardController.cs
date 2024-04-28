using Coditech.Filters;

using System.Web.Mvc;

namespace Coditech.Controllers
{
    [SessionTimeoutAttribute]
    public class DashboardController : BaseController
    {
        public DashboardController()
        {
        }

        public ActionResult Index()
        {
            return View($"~/Views/Dashboard/Index.cshtml");
        }

    }
}