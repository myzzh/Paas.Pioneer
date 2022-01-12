using Paas.Pioneer.Admin.Core.Application.Contracts.Cache.Dto.Input;
using Paas.Pioneer.Domain.Shared.Dto.Output;
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
        ResponseOutput<List<object>> GetList();

        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> ClearAsync(CacheDeleteInput model);
    }
}