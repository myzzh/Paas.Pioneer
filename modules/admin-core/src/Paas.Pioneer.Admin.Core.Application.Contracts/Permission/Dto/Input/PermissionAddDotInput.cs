using Paas.Pioneer.Admin.Core.Domain.Shared.Enum;
using System;
using System.Collections.Generic;

namespace Paas.Pioneer.Admin.Core.Application.Contracts.Permission.Dto.Input
{
    public class PermissionAddDotInput
    {
        /// <summary>
        /// Ȩ������
        /// </summary>
        public EPermissionType Type { get; set; } = EPermissionType.Dot;

        /// <summary>
        /// �����ڵ�
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// �����ӿ�
        /// </summary>
        public IEnumerable<Guid> ApiIds { get; set; }

        /// <summary>
        /// Ȩ������
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Ȩ�ޱ���
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// ˵��
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ͼ��
        /// </summary>
        public string Icon { get; set; }
    }
}