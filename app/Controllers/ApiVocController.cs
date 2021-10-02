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
    public class ApiVocController : Controller
    {
        private readonly ILogger<ApiVocController> _logger;
        private readonly DBManagerClass db;

        public ApiVocController(ILogger<ApiVocController> logger)
        {
            _logger = logger;
            this.db = new DBManagerClass("http://127.0.0.1:5984", "admin", "admin");
        }

        [HttpGet("/api/vocabulary")]
        public IActionResult VocabulariesList()
        {
            var ans = this.db.GetVocabulriesList();
            return Json(ans);
        }

        [HttpGet("/api/vocabulary/{codeVoc}")]
        public IActionResult GetVocabulary(string codeVoc)
        {
            var ans = this.db.GetVocabulry(codeVoc);
            return Json(ans);
        }


        [HttpPut("/api/vocabulary/")]
        public IActionResult AddVocabulary([FromBody] Vocabulary updatedVoc)
        {
            this.db.AddVocabulary(updatedVoc);
            return Ok();
        }

        [HttpPut("/api/vocabulary/{codeVoc}")]
        public IActionResult UpdateVocabulary(string codeVoc, [FromBody] Vocabulary updatedVoc)
        {
            this.db.UpdateVocabulary(codeVoc, updatedVoc);
            return Ok();
        }


        // [HttpPut("/api/vocabulary/{codeVoc}/term/{codeTerm}")]
        // public IActionResult ChangeTerm(string codeVoc, string codeTerm, [FromBody] Term term)
        // {
        //     return Json("DDL");
        // }

        // [HttpPut("/aapi/vocabulary/{codeVoc}/term/")]
        // public IActionResult AddTerm(string codeVoc, [FromBody] Term term)
        // {
        //     return Json("DDL");
        // }

        // [HttpDelete("/api/table/{codeVoc}/field/{codeTerm}")]
        // public IActionResult DeleteTerm(string codeVoc, string codeTerm)
        // {
        //     return Json("DDL");
        // }



    }
}
