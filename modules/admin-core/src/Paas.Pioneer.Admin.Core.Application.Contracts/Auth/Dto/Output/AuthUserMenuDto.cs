using System;

namespace Paas.Pioneer.Admin.Core.Application.Contracts.Auth.Dto.Output
{
    public class AuthUserMenuDto
    {
        /// <summary>
        /// Ȩ��Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// �����ڵ�
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// ���ʵ�ַ
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// ��ͼ��ַ
        /// </summary>
        public string ViewPath { get; set; }

        /// <summary>
        /// Ȩ������
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// ͼ��
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// ��
        /// </summary>
        public bool? Opened { get; set; }

        /// <summary>
        /// �ɹر�
        /// </summary>
        public bool? Closable { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// ���´���
        /// </summary>
        public bool? NewWindow { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public bool? External { get; set; }
    }
}