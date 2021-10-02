using System;
using CouchDB.Driver.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace app.Controllers
{
    public class TableRow : CouchDocument
    {
        [JsonProperty("data")]
        public JToken data { get; set; }

        public TableRow(JToken data)
        {
            this.data = data;
        }
        public TableRow()
        {
            this.data = JObject.Parse("{}");
        }

        internal void SetData(TableRow tableRow)
        {
            foreach (var cell in tableRow.data)
            {
                this.data[cell.Path] = tableRow.data[cell.Path];
            }
        }
    }
}