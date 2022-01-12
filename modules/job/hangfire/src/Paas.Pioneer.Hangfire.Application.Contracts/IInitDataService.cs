using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Paas.Pioneer.Hangfire.Application.Contracts
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>
    public interface IInitDataService : IApplicationService
    {
        Task<bool> EmptyDataAsync();

        Task<bool> GenerateDataAsync();

        Task<bool> WriteDataAsync();
    }
}