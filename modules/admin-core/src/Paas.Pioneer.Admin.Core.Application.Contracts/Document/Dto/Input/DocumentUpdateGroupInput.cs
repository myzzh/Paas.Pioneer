using Paas.Pioneer.Admin.Core.Domain.Shared.Enum;
using Paas.Pioneer.Domain.Shared.ModelValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Paas.Pioneer.Admin.Core.Application.Contracts.Document.Dto.Input
{
    public class DocumentUpdateGroupInput
    {
        /// <summary>
        /// ���
        /// </summary>
        [Required(ErrorMessage = "��������ȷ������Ϣ")]
        [NotEqual("00000000-0000-0000-0000-000000000000", ErrorMessage = "��������ȷid")]
        public Guid Id { get; set; }

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