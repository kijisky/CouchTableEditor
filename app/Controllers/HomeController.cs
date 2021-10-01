using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using app.Models;

namespace app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/Test")]
        public IActionResult Test()
        {
            var db = new DBManagerClass("http://127.0.0.1:5984", "admin", "admin");


            //this.TestAddTable(db);

            var ans = db.GetTables();
            return Json(ans);
        }


    }
}
