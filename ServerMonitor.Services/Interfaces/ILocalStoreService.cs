using ServerMonitor.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitor.Services.Interfaces
{
    public interface ILocalStoreService
    {
        Task<List<RemoteServer>> GetAll();

        Task Save(List<RemoteServer> list);

        Task Add(RemoteServer server);

        Task Delete(RemoteServer server);

        //Task Update(RemoteServer server);


        Task DeleteMany(List<RemoteServer> list);
    }
}
