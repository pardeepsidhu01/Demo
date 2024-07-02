using System.Threading.Tasks;
using Demo.Configuration.Dto;

namespace Demo.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
