using System;
using CouchDB.Driver.Types;

namespace app.Controllers
{
    public class Vocabulary : CouchDocument
    {
        public string code { get; set; }
        public string name { get; set; }
        public Term[] termsList { get; set; }

        internal void UpdateFrom(Vocabulary updatedVoc)
        {
            this.code = updatedVoc.code;
            this.name = updatedVoc.name;
            this.termsList = updatedVoc.termsList;
        }
    }
}