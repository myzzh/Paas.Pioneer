using Paas.Pioneer.Admin.Core.Domain.Shared.Enum;
using System;

namespace Paas.Pioneer.Admin.Core.Application.Contracts.Permission.Dto.Input
{
    public class PermissionAddGroupInput
    {
        /// <summary>
        /// Ȩ������
        /// </summary>
        public EPermissionType Type { get; set; }

        /// <summary>
        /// �����ڵ�
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Ȩ������
        /// </summary>
        public string Label { get; set; }

        ///// <summary>
        ///// ˵��
        ///// </summary>
        //public string Description { get; set; }

        /// <summary>
        /// ����
        /// </summary>
		public bool Hidden { get; set; }

        ///// <summary>
        ///// ����
        ///// </summary>
        //public bool Enabled { get; set; }

        /// <summary>
        /// ͼ��
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// ��
        /// </summary>
        public bool Opened { get; set; }
    }
}