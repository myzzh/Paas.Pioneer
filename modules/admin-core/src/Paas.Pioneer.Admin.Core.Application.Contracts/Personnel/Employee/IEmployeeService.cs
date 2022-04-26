using Paas.Pioneer.Admin.Core.Application.Contracts.Personnel.Employee.Dto;
using Paas.Pioneer.Admin.Core.Application.Contracts.Personnel.Employee.Dto.Input;
using Paas.Pioneer.Admin.Core.Application.Contracts.Personnel.Employee.Dto.Output;
using Paas.Pioneer.Domain.Shared.Dto.Input;
using Paas.Pioneer.AutoWrapper;
using System;
using System.Threading.Tasks;
using Paas.Pioneer.Domain.Shared.Dto.Output;

namespace Paas.Pioneer.Admin.Core.Application.Contracts.Personnel.Employee
{
    /// <summary>
    /// Ա������
    /// </summary>
    public interface IEmployeeService
    {
        Task<ResponseOutput<EmployeeGetOutput>> GetAsync(Guid id);

        Task<ResponseOutput<Page<EmployeeListOutput>>> GetPageListAsync(PageInput<EmployeeDataOutput> input);

        Task<IResponseOutput> AddAsync(EmployeeAddInput input);

        Task<IResponseOutput> UpdateAsync(EmployeeUpdateInput input);

        Task<IResponseOutput> DeleteAsync(Guid id);

        Task<IResponseOutput> BatchSoftDeleteAsync(Guid[] ids);
    }
}