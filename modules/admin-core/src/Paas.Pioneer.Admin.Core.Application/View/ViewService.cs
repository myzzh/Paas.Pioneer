using Paas.Pioneer.Admin.Core.Application.Contracts.View;
using Paas.Pioneer.Admin.Core.Application.Contracts.View.Dto;
using Paas.Pioneer.Admin.Core.Application.Contracts.View.Dto.Input;
using Paas.Pioneer.Admin.Core.Application.Contracts.View.Dto.Output;
using Paas.Pioneer.Admin.Core.Domain.View;
using Paas.Pioneer.Domain.Shared.Dto.Input;
using Paas.Pioneer.AutoWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Paas.Pioneer.Domain.Shared.Dto.Output;

namespace Paas.Pioneer.Admin.Core.Application.View
{
    public class ViewService : ApplicationService, IViewService
    {
        private readonly IViewRepository _viewRepository;

        public ViewService(IViewRepository viewRepository)
        {
            _viewRepository = viewRepository;
        }

        #region ��ѯ������ͼ

        /// <summary>
        /// ��ѯ������ͼ
        /// </summary>
        /// <param name="id">��ͼID</param>
        /// <returns></returns>
        public async Task<ResponseOutput<ViewGetOutput>> GetAsync(Guid id)
        {
            var model = await _viewRepository.GetAsync(expression: x => x.Id == id, selector: x => new ViewGetOutput()
            {
                Id = x.Id,
                Description = x.Description,
                Enabled = x.Enabled,
                Label = x.Label,
                Name = x.Name,
                ParentId = x.ParentId,
                Path = x.Path,
            });
            return ResponseOutput.Succees(model);
        }

        #endregion

        #region ��ѯȫ����ͼ

        /// <summary>
        /// ��ѯȫ����ͼ
        /// </summary>
        /// <param name="key">��ͼ·��,��ͼ����</param>
        /// <returns></returns>
        public async Task<ResponseOutput<List<ViewGetOutput>>> GetListAsync(string key)
        {
            Expression<Func<Ad_ViewEntity, bool>> expression = x => true;
            if (!key.IsNullOrEmpty())
            {
                expression = a => a.Path.Contains(key) || a.Label.Contains(key);
            }
            return await _viewRepository.GetResponseOutputListAsync(expression: expression, selector: x => new ViewGetOutput()
            {
                Id = x.Id,
                Description = x.Description,
                Enabled = x.Enabled,
                Label = x.Label,
                Name = x.Name,
                ParentId = x.ParentId,
                Path = x.Path,
                Cache = x.Cache,
                Sort = x.Sort,
            });
        }

        #endregion

        #region ��ѯ��ҳ��ͼ

        /// <summary>
        /// ��ѯ��ҳ��ͼ
        /// </summary>
        /// <param name="input">��ҳģ��</param>
        /// <returns></returns>
        public async Task<ResponseOutput<Page<ViewGetOutput>>> GetPageListAsync(PageInput<ViewModel> input)
        {
            return await _viewRepository.GetResponseOutputPageListAsync(
                selector: x => new ViewGetOutput()
                {
                    Id = x.Id,
                    Description = x.Description,
                    Enabled = x.Enabled,
                    Label = x.Label,
                    Name = x.Name,
                    ParentId = x.ParentId,
                    Path = x.Path,
                    Cache = x.Cache,
                    Sort = x.Sort,
                },
                order: x => x.OrderByDescending(p => p.CreationTime),
                input);
        }
        #endregion

        #region ������ͼ

        /// <summary>
        /// ������ͼ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> AddAsync(ViewAddInput input)
        {
            var entity = ObjectMapper.Map<ViewAddInput, Ad_ViewEntity>(input);
            await _viewRepository.InsertAsync(entity);
            return ResponseOutput.Succees("��ӳɹ���");
        }

        #endregion

        #region �޸���ͼ

        /// <summary>
        /// �޸���ͼ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> UpdateAsync(ViewUpdateInput input)
        {
            var entity = await _viewRepository.FindAsync(input.Id);
            if (!(entity?.Id != Guid.Empty))
            {
                throw new BusinessException("��ͼ�����ڣ�");
            }
            ObjectMapper.Map(input, entity);
            await _viewRepository.UpdateAsync(entity);
            return ResponseOutput.Succees("�޸ĳɹ���");
        }

        #endregion

        #region ɾ����ͼ
        /// <summary>
        /// ɾ����ͼ
        /// </summary>
        /// <param name="id">��ͼID</param>
        /// <returns></returns>
        public async Task<IResponseOutput> DeleteAsync(Guid id)
        {
            await _viewRepository.DeleteAsync(m => m.Id == id);
            return ResponseOutput.Succees("ɾ���ɹ���");
        }
        #endregion

        #region ����ɾ����ͼ
        /// <summary>
        /// ����ɾ����ͼ
        /// </summary>
        /// <param name="ids">����ID</param>
        /// <returns></returns>
        public async Task<IResponseOutput> BatchSoftDeleteAsync(Guid[] ids)
        {
            await _viewRepository.DeleteAsync(a => ids.Contains(a.Id));
            return ResponseOutput.Succees("ɾ���ɹ���");
        }
        #endregion
    }
}