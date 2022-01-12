using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Paas.Pioneer.Admin.Core.Application.Contracts.Permission.Dto.Input
{
    public class PermissionSaveTenantPermissionsInput
    {
        [Required(ErrorMessage = "�⻧����Ϊ�գ�")]
        public Guid TenantId { get; set; }

        [Required(ErrorMessage = "Ȩ�޲���Ϊ�գ�")]
        public List<Guid> PermissionIds { get; set; }
    }
}