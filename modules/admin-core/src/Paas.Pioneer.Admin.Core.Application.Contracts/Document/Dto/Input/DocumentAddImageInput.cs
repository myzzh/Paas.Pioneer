using System;

namespace Paas.Pioneer.Admin.Core.Application.Contracts.Document.Dto.Input
{
    public class DocumentAddImageInput
    {
        /// <summary>
        /// �û�Id
        /// </summary>
        public Guid DocumentId { get; set; }

        /// <summary>
        /// ����·��
        /// </summary>
        public string Url { get; set; }
    }
}