using System;
using System.Collections.Generic;
using System.Linq;
using CouchDB.Driver;

namespace app.Controllers
{
    public class Table
    {
        private CouchClient client;
        private mdTable tableData = new mdTable();
        public string name { get { return this.tableData.Name; } }
        public string id { get { return this.tableData.Id; } }
        public List<mdField> fields { get { return this.tableData.fields; } }

        internal Table(CouchClient client, mdTable tableData)
        {
            this.client = client;
            this.tableData = tableData;
            if (this.tableData == null)
            {
                this.tableData = new mdTable();
            }

            this.CalculateFeildsDepth();
            this.CalculateFeildsPaths(this.fields);
        }

        internal static Table GetByCode(CouchClient client, string pId)
        {
            var mdTables = Table.GetDB(client);
            var tableData = mdTables.Where(f => f.Id == pId).ToArray().FirstOrDefault();
            var ans = new Table(client, tableData);
            return ans;
        }

        internal IEnumerable<mdField> GetFieldsList()
        {
            /// Функция нужна для получения линейного списка из иерархического
            var ans = new List<mdField>();
            foreach (var fld in this.fields)
            {
                ans.Add(fld);
            }
            return ans;
        }

        internal IEnumerable<IEnumerable<mdField>> GetColumnsBreadthsFirst()
        {
            var ans = new List<List<mdField>>();

            ans.Add(this.fields);

            for (var level = 0; true; level++)
            {
                var levelFields = new List<mdField>();

                foreach (var fld in this.fields)
                {
                    var subfields = fld.GetSubFieldsOfLevel(level);
                    levelFields.AddRange(subfields);
                }

                if (levelFields.Count() == 0) { return ans; }
                ans.Add(levelFields);
            }
        }

        internal IEnumerable<TableRow> ReadAllRows(IEnumerable<string> fieldsList, object filter = null)
        {
            var rowsDB = this.GetTableDB(this.id);
            if (filter == null)
            {
                filter = new { };
            }
            var mangoQuery = new
            {
                selector = filter,
                fields = fieldsList,
                limit = 2000
            };
            var queryTask = rowsDB.QueryAsync(mangoQuery);
            queryTask.Wait();
            var ans = queryTask.Result;
            // var ans = rowsDB.Where(f => f.Id != null).ToArray();
            return ans;
        }

        internal TableRow UpdateRow(string rowID, TableRow tableRow)
        {
            var rowsDB = this.GetTableDB(this.id);

            var task = tableRow.Id == null ?
                 rowsDB.AddAsync(tableRow) :
                 rowsDB.AddOrUpdateAsync(tableRow);
            task.Wait();

            var ans = this.GetTableRow(rowID);
            return (ans);
        }

        internal TableRow GetRow(string rowID)
        {
            var rowsDB = this.GetTableDB(this.id);
            var ans = rowsDB.Where(r => r.Id == rowID).ToArray().FirstOrDefault();
            return ans;
        }

        private TableRow GetTableRow(string rowID)
        {
            var rowsDB = this.GetTableDB(this.id);
            var ans = rowsDB.Where(r => r.Id == rowID).ToArray().FirstOrDefault();
            return ans;
        }

        private ICouchDatabase<TableRow> GetTableDB(string tableID)
        {
            var dbTableName = "tbl_" + tableID;
            var task = client.GetOrCreateDatabaseAsync<TableRow>(dbTableName);
            task.Wait();
            var rowsDB = task.Result;
            return rowsDB;
        }

        private static ICouchDatabase<mdTable> GetDB(CouchClient client)
        {
            var task = client.GetOrCreateDatabaseAsync<mdTable>();
            task.Wait();
            var mdTables = task.Result;
            return mdTables;
        }

        internal static IEnumerable<Table> GetAll(CouchClient client)
        {
            var mdTables = Table.GetDB(client);
            var tablesMetadata = mdTables.Where(f => f.Id != "9").ToArray();
            var ans = tablesMetadata.Select(md => new Table(client, md));
            return ans;
        }

        internal void SetField(string codeField, mdField field)
        {
            var fld = this.FindFieldByPath(codeField);

            if (fld == null)
            {
                var fldParent = this.FindFieldByPath(codeField, -1);
                if (fldParent != null)
                {
                    fldParent.children.Add(field);
                }
                else
                {
                    this.fields.Add(field);
                }
            }
            else
            {
                fld.SetFrom(field);
            }
            this.Save();
        }

        private void Save()
        {
            var task = Table.GetDB(this.client).AddOrUpdateAsync(this.tableData);
            task.Wait();
            return;
        }

        internal void DeleteField(string codeField)
        {
            var fldParent = this.FindFieldByPath(codeField, -1);
            var fld = this.FindFieldByPath(codeField);
            if (fld == null)
            {
                return;
            }
            if (fldParent != null)
            {
                fldParent.children.Remove(fld);
            }
            else
            {
                this.fields.Remove(fld);
            }
            this.Save();
        }

        private mdField FindFieldByPath(string codeField, int pDepth = 0)
        {
            var pathParts = codeField.Split(".");

            if (pDepth == 0) pDepth = pathParts.Length;
            if (pDepth < 0) pDepth = pathParts.Length + pDepth;
            if (pDepth > pathParts.Length) pDepth = pathParts.Length;

            var currLevelFieldsList = this.fields;
            mdField levelField = null;
            for (int i = 0; i < pDepth; i++)
            {
                var curName = pathParts[i];
                if (currLevelFieldsList == null)
                {
                    return null;
                }
                levelField = currLevelFieldsList.Where(f => f.name == curName).FirstOrDefault();
                if (levelField == null)
                {
                    return null;
                }
                currLevelFieldsList = levelField.children;
            }
            return levelField;
        }

        private void CalculateFeildsPaths(IEnumerable<mdField> fieldsList, string baseName = null)
        {
            foreach (var fld in fieldsList)
            {
                fld.Path = (baseName != null ? baseName + "." : "") + fld.name;
                if (fld.children != null)
                {
                    this.CalculateFeildsPaths(fld.children, fld.Path);
                }
            }
        }


        private void CalculateFeildsDepth()
        {
            var maxDepth = this.GetMaxDepth(this.fields);
            this.CalculateSpans(this.fields, 0, maxDepth);
        }

        private void CalculateSpans(IEnumerable<mdField> fieldsList, int vCurDepth, int maxDepth)
        {
            if (fieldsList == null || fieldsList.Count() == 0) return;
            foreach (var fld in fieldsList)
            {
                fld.spanDepth = fld.HasChildren() ? 1 : maxDepth - vCurDepth;
                fld.spanDepthDiff = vCurDepth;
                fld.spanWidth = this.CalcFieldsWidth(fld);
                this.CalculateSpans(fld.children, vCurDepth + 1, maxDepth);
            }
        }

        private int CalcFieldsWidth(mdField pField)
        {
            if (pField.children == null || pField.children.Count() == 0)
            {
                return 1;
            }

            int totalWidth = 0;
            foreach (var chldField in pField.children)
            {
                totalWidth += this.CalcFieldsWidth(chldField);
            }
            return totalWidth;

        }

        private int GetMaxDepth(IEnumerable<mdField> fields)
        {
            if (fields == null || fields.Count() == 0)
            {
                return 0;
            }
            var maxDepth = 0;
            foreach (var fld in fields)
            {
                var depth = this.GetMaxDepth(fld.children);
                if (depth > maxDepth)
                {
                    maxDepth = depth;
                }
            }
            return maxDepth + 1;
        }
    }
}