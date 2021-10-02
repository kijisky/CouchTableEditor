using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using app.Models;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace app.Controllers
{
    public class ApiDataController : Controller
    {
        private readonly ILogger<ApiDataController> _logger;
        private readonly DBManagerClass db;

        public ApiDataController(IConfiguration config, ILogger<ApiDataController> logger)
        {
            var host = config.GetValue<string>("Couchdb:host");
            var user = config.GetValue<string>("Couchdb:user");
            var pass = config.GetValue<string>("Couchdb:password");
            this.db = new DBManagerClass(host, user, pass);
            _logger = logger;
        }

        [HttpGet("/api/table/{tableCode}/rows")]
        public IActionResult GetTableData(string tableCode, string[] fields = null, string selector = null)
        {
            var fieldsList = fields.Select(f => "data." + f);
            fieldsList = fieldsList.Append("_id").Append("_rev");

            dynamic selectorObject =
                selector != null ?
                new { data = JsonConvert.DeserializeObject<dynamic>(selector) } :
                new { };


            var tbl = this.db.GetTable(tableCode);
            var ans = tbl.ReadAllRows(fieldsList, selectorObject);
            var ansJson = JsonConvert.SerializeObject(ans);
            return this.Content(ansJson);
        }

        [HttpGet("/api/table/{tableCode}/rows/{rowID}")]
        public IActionResult GetTableRow(string tableCode, string rowID)
        {
            var tbl = this.db.GetTable(tableCode);
            var dbTableRow = tbl.GetRow(rowID);
            var dbTableRowJson = JsonConvert.SerializeObject(dbTableRow);
            return this.Content(dbTableRowJson);
        }


        [HttpPut("/api/table/{tableCode}/rows/")]
        public IActionResult AddTableRow(string tableCode)
        {
            TableRow tableRow = null;
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {

                string body = stream.ReadToEnd();
                // body = "param=somevalue&param2=someothervalue"
                tableRow = JsonConvert.DeserializeObject<TableRow>(body);
            }
            var tbl = this.db.GetTable(tableCode);


            var ans = tbl.UpdateRow(null, tableRow);
            return Json(ans);
        }


        [HttpPut("/api/table/{tableCode}/rows/{rowID}")]
        public IActionResult ChangeTableRow(string tableCode, string rowID = null)//, [FromBody] string tableRowJson)
        {
            TableRow tableRow = null;
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {

                string body = stream.ReadToEnd();
                // body = "param=somevalue&param2=someothervalue"
                tableRow = JsonConvert.DeserializeObject<TableRow>(body);
            }
            var tbl = this.db.GetTable(tableCode);

            var dbTableRow = tbl.GetRow(rowID);
            dbTableRow.SetData(tableRow);
            tbl.UpdateRow(rowID, dbTableRow);
            return this.GetTableRow(tableCode, rowID);
        }



        [HttpDelete("/api/table/{tableCode}/rows/{rowID}")]
        public IActionResult DeleteTableRow(string tableCode, string rowID)
        {
            return Json("DDL");
        }



    }
}
