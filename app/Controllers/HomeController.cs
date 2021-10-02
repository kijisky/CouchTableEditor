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
        private static Random random = new Random();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/Test/filldata/{tableCode}")]
        public IActionResult TestFillData(string tableCode, int rows = 100)
        {
            var db = new DBManagerClass("http://127.0.0.1:5984", "admin", "admin");
            var tblMetadata = db.GetTable(tableCode);



            for (int i = 0; i < rows; i++)
            {
                var newRow = new TableRow();
                foreach (var fld in tblMetadata.GetFieldsList())
                {
                    var val = this.GetRandomString(20);
                    fld.SetValue(newRow, val);
                }
                tblMetadata.UpdateRow(null, newRow);
            }

            return Ok();
        }

        private string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz!@#$%^&*()             ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
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
