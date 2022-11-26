using Microsoft.AspNetCore.Mvc;

namespace Quizle.Web.Controllers
{
    public class BadgeController : Controller
    {
        public IActionResult All()
        {
            return View();
        }
        public IActionResult AddToCollection(int badgeId)
        {
            return View();
        }

    }
}
