using Q42.WinRT.Storage;
using ServerMonitor.Services.Interfaces;
using ServerMonitor.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitor.Services
{
    public class LocalStoreService : ILocalStoreService
    {
        public StorageHelper<List<RemoteServer>>  Storage { get; set; }

        private string _key = "list.json";

        public LocalStoreService()
        {
            Storage = new StorageHelper<List<RemoteServer>>(StorageType.Local);
        }
        public async Task<List<RemoteServer>> GetAll()
        {
            var list = await Storage.LoadAsync(_key);

            if (list == null)
                list = new List<RemoteServer>();

            return list;
        }

        public async Task Add(RemoteServer server)
        {
            var all = await GetAll();

            if (!all.Where(x => x.Id == server.Id).Any())
            {
                all.Add(server);

                await this.Save(all);
            }
        }

        public async Task Delete(RemoteServer server)
        {
            var all = await GetAll();

            var item = all.Where(x => x.Id == server.Id).FirstOrDefault();

            if (item != null)
            {
                all.Remove(item);

                await this.Save(all);
            }
        }

        public async Task DeleteMany(List<RemoteServer> servers)
        {
            var all = await GetAll();

            foreach (var server in servers)
            {
                var item = all.Where(x => x.Id == server.Id).FirstOrDefault();

                if (item != null)
                {
                    all.Remove(item);
                }
            }

            await this.Save(all);
        }

        //public Task Update(RemoteServer server)
        //{
        //     var all = await GetAll();

        //    var item = all.Where(x => x.Id == server.Id).FirstOrDefault();

        //    if (item != null)
        //    {
        //        all.Remove(item);
        //    }

        //      all.Add(server);

        //        await this.Save(all);
        //}


        public Task Save(List<RemoteServer> list)
        {
            return Storage.SaveAsync(list, _key);
        }
    }
}
