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
    public class ApiDdlController : Controller
    {
        private readonly ILogger<ApiDdlController> _logger;
        private readonly DBManagerClass db;

        public ApiDdlController(IConfiguration config, ILogger<ApiDdlController> logger)
        {
            var host = config.GetValue<string>("Couchdb:host");
            var user = config.GetValue<string>("Couchdb:user");
            var pass = config.GetValue<string>("Couchdb:password");
            this.db = new DBManagerClass(host, user, pass);
            _logger = logger;
        }

        [HttpGet("/api/table")]
        public IActionResult Tables()
        {
            var ans = this.db.GetTables();
            return Json(ans);
        }

        [HttpGet("/api/table/{code}")]
        public IActionResult GetTable(string code)
        {
            var ans = this.db.GetTable(code);
            return Json(ans);
        }


        [HttpPut("/api/table")]
        public IActionResult AddTable()
        {
            var newTable = new mdTable();
            newTable.Name = "new table";
            this.db.AddTable(newTable);
            return Ok();
        }


        [HttpPut("/api/table/{codeTable}/field/{codeField}")]
        public IActionResult ChangeColumn(string codeTable, string codeField, [FromBody] mdField field)
        {
            var tbl = this.db.GetTable(codeTable);
            tbl.SetField(codeField, field);
            return Ok();
        }



        [HttpDelete("/api/table/{codeTable}/field/{codeField}")]
        public IActionResult DeleteColumn(string codeTable, string codeField)
        {
            var tbl = this.db.GetTable(codeTable);
            tbl.DeleteField(codeField);
            return Ok();
        }

        [HttpDelete("/api/table/{codeTable}/field/{codeField}/rename/{newCodeField}")]
        public IActionResult RenameColumn(string codeTable, string codeField, string newCodeField)
        {
            return Json("DDL");
        }

    }
}
