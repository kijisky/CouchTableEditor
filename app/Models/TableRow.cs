using CouchDB.Driver.Types;
using Newtonsoft.Json;

namespace app.Controllers
{
    public class TableRow : CouchDocument
    {
        [JsonProperty("data")]
        public dynamic data { get; set; }
    }
}