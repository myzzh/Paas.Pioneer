using Paas.Pioneer.Domain.Shared.ModelValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Paas.Pioneer.Admin.Core.Application.Contracts.Document.Dto.Input
{
    public class DocumentUpdateContentInput
    {
        /// <summary>
        /// ���
        /// </summary>
        [Required(ErrorMessage = "��ѡ���ĵ��˵�")]
        [NotEqual("00000000-0000-0000-0000-000000000000", ErrorMessage = "��ѡ���ĵ��˵�")]
        public Guid Id { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Html
        /// </summary>
        public string Html { get; set; }
    }
}