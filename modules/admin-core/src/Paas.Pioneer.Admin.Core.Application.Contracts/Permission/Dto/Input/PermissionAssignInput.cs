using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Paas.Pioneer.Admin.Core.Application.Contracts.Permission.Dto.Input
{
    public class PermissionAssignInput
    {
        [Required(ErrorMessage = "��ɫ����Ϊ�գ�")]
        public Guid RoleId { get; set; }

        [Required(ErrorMessage = "Ȩ�޲���Ϊ�գ�")]
        public List<Guid> PermissionIds { get; set; }
    }
}