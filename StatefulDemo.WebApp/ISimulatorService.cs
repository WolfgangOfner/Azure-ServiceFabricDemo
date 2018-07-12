using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace StatefulDemo.WebApp
{
    public interface ISimulatorService : IService
    {
        Task<long> GetLeads();
    }
}