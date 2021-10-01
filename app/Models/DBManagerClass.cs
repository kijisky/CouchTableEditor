using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CouchDB.Driver;

namespace app.Controllers
{
    internal class DBManagerClass
    {
        private CouchClient client;

        public ICouchDatabase<mdTable> mdTables { get; private set; }
        public ICouchDatabase<Vocabulary> mdVocabulary { get; private set; }

        public DBManagerClass(string url, string user, string pass)
        {
            Init(url, user, pass).Wait();
        }

        private async Task Init(string url, string user, string pass)
        {
            this.client = new CouchClient(url, builder =>
            {
                builder.UseBasicAuthentication(user, pass);

            });

            this.mdTables = await this.client.GetOrCreateDatabaseAsync<mdTable>();
            this.mdVocabulary = await this.client.GetOrCreateDatabaseAsync<Vocabulary>();
        }

        internal Vocabulary GetVocabulry(string pCode)
        {
            var ans = this.mdVocabulary.FirstOrDefault(v => v.code == pCode);
            return ans;
        }

        internal List<Vocabulary> GetVocabulriesList()
        {
            return this.mdVocabulary.ToList();
        }

        internal Table[] GetTables()
        {
            var ans = Table.GetAll(this.client);
            return ans.ToArray();
        }

        internal void AddTable(mdTable newTbl)
        {
            this.mdTables.AddAsync(newTbl).Wait();
        }

        internal Table GetTable(string pId)
        {
            var ans = Table.GetByCode(this.client, pId);
            return ans;
        }
    }
}