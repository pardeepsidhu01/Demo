using System.Threading.Tasks;
using Abp.Application.Services;
using Demo.Authorization.Accounts.Dto;

namespace Demo.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
