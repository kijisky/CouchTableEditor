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
    public class PagesController : Controller
    {
        private readonly ILogger<PagesController> _logger;
        private readonly DBManagerClass db;

        public PagesController(ILogger<PagesController> logger)
        {
            _logger = logger;
            this.db = new DBManagerClass("http://127.0.0.1:5984", "admin", "admin");
        }

        [HttpGet("/DDL/")]
        public IActionResult DDL()
        {
            return View("DDL");
        }

        [HttpGet("/Table/{tableCode}")]
        public IActionResult ShowTable(string tableCode)
        {
            var model = new TableViewModel();
            model.tableCode = tableCode;
            model.table = this.db.GetTable(tableCode);

            return View("Table", model);
        }

    }
}
