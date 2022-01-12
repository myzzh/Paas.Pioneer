using Microsoft.AspNetCore.Http;
using Paas.Pioneer.Domain.Shared.ModelValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Paas.Pioneer.Admin.Core.Application.Contracts.Document.Dto.Input
{
    public class DocumentUploadImageInput
    {
        /// <summary>
        /// �ϴ��ļ�
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// �ĵ����
        /// </summary>
        [Required(ErrorMessage = "��������ȷid")]
        [NotEqual("00000000-0000-0000-0000-000000000000", ErrorMessage = "��������ȷid")]
        public Guid Id { get; set; }
    }
}