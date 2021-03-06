using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using app.Models;
using Microsoft.Extensions.Configuration;

namespace app.Controllers
{
    public class PagesController : Controller
    {
        private readonly ILogger<PagesController> _logger;
        private readonly DBManagerClass db;

        public PagesController(DBManagerClass dbMgr, ILogger<PagesController> logger)
        {
            _logger = logger;
            this.db = dbMgr;
        }

        [HttpGet("/DDL/")]
        public IActionResult DDL()
        {
            return View("DDL");
        }


        [HttpGet("/vocabulary/")]
        public IActionResult VocabularyEditor()
        {
            return View("Vocabulary");
        }

        [HttpGet("/Table/{tableCode}")]
        public IActionResult ShowTable(string tableCode)
        {
            var model = new TableViewModel();
            model.tableCode = tableCode;
            model.table = this.db.GetTable(tableCode);
            model.columns = model.table.GetColumnsBreadthsFirst();

            return View("Table", model);
        }

        [HttpGet("/Table/")]
        public IActionResult ShowTablesList()
        {
            var model = new TableViewModel();
            model.tables = this.db.GetTables();

            return View("TablesList", model);
        }

    }
}
