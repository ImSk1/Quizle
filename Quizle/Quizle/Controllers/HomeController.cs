using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Quizle.Core.Exceptions;
using Quizle.Web.Models;
using System.Diagnostics;

namespace Quizle.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
      
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("All", "Quiz");
            }
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature? exceptionHandlerPathFeature = this.HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            Exception error = exceptionHandlerPathFeature?.Error!;
            switch (error)
            {
                case NotFoundException:
                    return this.RedirectToAction("ErrorPage", new { statusCode = 404});
                case ArgumentException:
                    return this.RedirectToAction("ErrorPage", new { statusCode = 500 });
                case CannotBuyBadgeException:
                    this.TempData["errorMessage"] = "Error: Cannot purchase badge!";                   
                    return this.RedirectToAction("All", "Badge");
                default: return this.RedirectToAction("ErrorPage", new { statusCode = 500 });
            }
        }
        public IActionResult ErrorPage(int statusCode)
        {
            ViewData["statusCode"] = statusCode;
            return View();
        }
    }
}