using CouchDB.Driver.Types;

namespace app.Controllers
{
    internal class Vocabulary : CouchDocument
    {
        public string code { get; set; }
        public string name { get; set; }
        public Term terms { get; set; }
    }
}