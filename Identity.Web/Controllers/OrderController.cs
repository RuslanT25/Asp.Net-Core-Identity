using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
    public class OrderController : Controller
    {
        [Authorize(Policy = "OrderReadAndDeleteStockDeletePolicy")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
