using System.Threading.Tasks;
using Abp.Application.Services;
using Demo.Sessions.Dto;

namespace Demo.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
