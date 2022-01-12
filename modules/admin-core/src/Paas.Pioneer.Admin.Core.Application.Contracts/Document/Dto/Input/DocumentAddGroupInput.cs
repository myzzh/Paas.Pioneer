using Paas.Pioneer.Admin.Core.Domain.Shared.Enum;
using System;

namespace Paas.Pioneer.Admin.Core.Application.Contracts.Document.Dto.Input
{
    public class DocumentAddGroupInput
    {
        /// <summary>
        /// �����ڵ�
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public EDocumentType Type { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ��
        /// </summary>
        public bool Opened { get; set; }
    }
}