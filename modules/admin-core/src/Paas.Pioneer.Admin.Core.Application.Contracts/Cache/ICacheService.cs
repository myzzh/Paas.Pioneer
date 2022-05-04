using Paas.Pioneer.Admin.Core.Application.Contracts.Cache.Dto.Input;
using Paas.Pioneer.AutoWrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Paas.Pioneer.Admin.Core.Application.Contracts.Cache
{
    /// <summary>
    /// �������
    /// </summary>
    public interface ICacheService : IApplicationService
    {
        /// <summary>
        /// �����б�
        /// </summary>
        /// <returns></returns>
        List<object> GetList();

        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        Task ClearAsync(CacheDeleteInput model);
    }
}