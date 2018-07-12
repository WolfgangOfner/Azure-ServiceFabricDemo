using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace StatefulDemo
{
    public interface ISimulatorService : IService
    {
        Task<long> GetLeads();
    }
}

