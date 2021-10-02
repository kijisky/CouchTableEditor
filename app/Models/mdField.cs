using System;

namespace app.Controllers
{
    public class mdField
    {
        public string name { get; set; }
        public string alias { get; set; }
        public string type { get; set; }
        public string code { get; set; }
        public string descr { get; set; }
        public string dictName { get; set; }

        internal void SetFrom(mdField field)
        {
            //this.name = field.name;
            this.alias = field.alias;
            this.dictName = field.dictName;
            this.type = field.type;
            this.descr = field.descr;
            this.code = field.code;
        }

        internal void SetValue(TableRow newRow, string val)
        {
            newRow.data[this.name] = val;
        }
    }
}