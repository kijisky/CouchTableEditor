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
        public string dictId { get; set; }
        // public string dictMultiSelect { get; set; }

        public bool showUrl { get; set; }
        public string urlSubfield { get; set; }
        public string urlPrefix { get; set; }
        public string urlPostfix { get; set; }

        internal void SetFrom(mdField field)
        {
            //this.name = field.name;
            this.alias = field.alias;
            this.type = field.type;
            this.descr = field.descr;
            this.code = field.code;

            this.dictId = field.dictId;
            // this.dictMultiSelect = field.dictMultiSelect;

            this.showUrl = field.showUrl;
            this.urlSubfield = field.urlSubfield;
            this.urlPrefix = field.urlPrefix;
            this.urlPostfix = field.urlPostfix;
        }

        internal void SetValue(TableRow newRow, string val)
        {
            newRow.data[this.name] = val;
        }
    }
}