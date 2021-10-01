﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using app.Models;
using Newtonsoft.Json;
using System.IO;

namespace app.Controllers
{
    public class ApiDataController : Controller
    {
        private readonly ILogger<ApiDataController> _logger;
        private readonly DBManagerClass db;

        public ApiDataController(ILogger<ApiDataController> logger)
        {
            _logger = logger;
            this.db = new DBManagerClass("http://127.0.0.1:5984", "admin", "admin");
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

            return Json("DDL");
        }


        [HttpPut("/api/table/{tableCode}/rows")]
        public IActionResult AddTableRow(string tableCode, [FromBody] TableRow tableRow)
        {
            return Json("DDL");
        }


        [HttpPut("/api/table/{tableCode}/rows/{rowID}")]
        public IActionResult ChangeTableRow(string tableCode, string rowID)//, [FromBody] string tableRowJson)
        {
            TableRow tableRow = null;
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {

                string body = stream.ReadToEnd();
                // body = "param=somevalue&param2=someothervalue"
                tableRow = JsonConvert.DeserializeObject<TableRow>(body);
            }
            var tbl = this.db.GetTable(tableCode);
            var ans = tbl.UpdateRow(rowID, tableRow);
            return Json(ans);
        }



        [HttpDelete("/api/table/{tableCode}/rows/{rowID}")]
        public IActionResult DeleteTableRow(string tableCode, string rowID)
        {
            return Json("DDL");
        }



    }
}