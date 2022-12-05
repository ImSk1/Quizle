using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizle.Core.Models;
using Quizle.Web.Models;
using System.Data;

namespace Quizle.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class BaseAdminController : Controller
    {
       

    }
}
