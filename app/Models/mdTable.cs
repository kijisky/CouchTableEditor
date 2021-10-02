using System.Collections.Generic;
using CouchDB.Driver.Types;

namespace app.Controllers
{

    public class mdTable : CouchDocument
    {
        public string Name { get; set; }
        public List<mdField> fields { get; set; }
        public mdTable()
        {
            this.fields = new List<mdField>();
        }
    }
}