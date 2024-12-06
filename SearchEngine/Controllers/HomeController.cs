using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using SearchEngine.Models;
using SearchEngine.Repositories;
using System.Diagnostics;
using System.Web;

namespace SearchEngine.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
