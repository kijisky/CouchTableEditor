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
        }

        internal static Table GetByCode(CouchClient client, string pId)
        {
            var mdTables = Table.GetDB(client);
            var tableData = mdTables.Where(f => f.Id == pId).ToArray().FirstOrDefault();
            var ans = new Table(client, tableData);
            return ans;
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
                fields = fieldsList
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
            var fld = this.fields.Where(f => f.name == codeField).FirstOrDefault();
            if (fld == null)
            {
                this.fields.Add(field);
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
            var fld = this.fields.Where(f => f.name == codeField).FirstOrDefault();
            if (fld != null)
            {
                this.fields.Remove(fld);
            }
            this.Save();
        }
    }
}