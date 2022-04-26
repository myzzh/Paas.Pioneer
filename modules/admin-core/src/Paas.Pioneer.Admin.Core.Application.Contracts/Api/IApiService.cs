using Paas.Pioneer.Admin.Core.Application.Contracts.Api.Dto.Input;
using Paas.Pioneer.Admin.Core.Application.Contracts.Api.Dto.Output;
using Paas.Pioneer.Domain.Shared.Dto.Input;
using Paas.Pioneer.AutoWrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Paas.Pioneer.Domain.Shared.Dto.Output;

namespace Paas.Pioneer.Admin.Core.Application.Contracts.Api
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>
    public interface IApiService : IApplicationService
    {
        /// <summary>
        /// ���һ����¼
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiGetOutput> GetAsync(Guid id);

        /// <summary>
        /// ����б�
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<List<ApiListOutput>> GetListAsync(string key);

        /// <summary>
        /// ��÷�ҳ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Page<ApiListOutput>> GetPageListAsync(PageInput<ApiInput> model);

        /// <summary>
        /// ���
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AddAsync(ApiAddInput input);

        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateAsync(ApiUpdateInput input);

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task BatchSoftDeleteAsync(IEnumerable<Guid> ids);
    }
}