using System;

namespace Paas.Pioneer.Admin.Core.Application.Contracts.Document.Dto.Output
{
    public class DocumentGetContentOutput
    {
        /// <summary>
        /// ���
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Content { get; set; }
    }
}