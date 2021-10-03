using System;
using System.Collections.Generic;

namespace app.Controllers
{
    public class mdField
    {
        public int spanDepth;
        public int spanDepthDiff;
        public int spanWidth;

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


        public string extUrlGet { get; set; }
        public string extJsonPath { get; set; }
        public string extFieldID { get; set; }
        public string extFieldName { get; set; }


        public mdField[] children { get; set; }
        public string Path { get; internal set; }

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


            this.extUrlGet = field.extUrlGet;
            this.extJsonPath = field.extJsonPath;
            this.extFieldID = field.extFieldID;
            this.extFieldName = field.extFieldName;

            this.children = field.children;
        }

        internal void SetValue(TableRow newRow, string val)
        {
            newRow.data[this.name] = val;
        }

        /// Получить имена родительских "путей" (если мы "a.b.f.t", то  ["a", "a.b", "a.b.f"] )
        public IEnumerable<string> GetDependentParentPaths()
        {
            if (this.Path == null)
            {
                return new string[] { };
            }

            var pathParts = this.Path.Split(".");
            var dependPaths = new List<string>();
            var currentPath = "";
            for (int i = 0; i < pathParts.Length - 1; i++)
            {
                currentPath = currentPath + (currentPath.Length > 0 ? "." : "") + pathParts[i];
                dependPaths.Add(currentPath);
            }
            return dependPaths;
        }

        public bool HasChildren()
        {
            return this.children != null && this.children.Length > 0;
        }
    }
}