using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CouchDB.Driver;

namespace app.Controllers
{
    public class DBManagerClass
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

        internal Vocabulary GetVocabulry(string pId)
        {
            /// ошибки драйвер: один только FirstOrDewfault - не работает фильтр, ToArray нужен, без него падает
            var ans = this.mdVocabulary.Where(v => v.Id == pId).ToArray().FirstOrDefault();
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

        internal void AddVocabulary(Vocabulary newVoc)
        {
            newVoc.Id = null;
            this.mdVocabulary.AddAsync(newVoc).Wait();
        }

        internal void UpdateVocabulary(string codeVoc, Vocabulary updatedVoc)
        {
            var voc = this.GetVocabulry(codeVoc);
            if (voc == null)
            {
                throw new Exception("voc Not found");
            }
            voc.UpdateFrom(updatedVoc);
            this.mdVocabulary.AddOrUpdateAsync(voc).Wait();
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