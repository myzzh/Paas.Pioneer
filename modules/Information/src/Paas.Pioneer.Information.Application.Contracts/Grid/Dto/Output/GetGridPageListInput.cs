﻿using System;
using Paas.Pioneer.Domain.Shared.Extensions;
using Paas.Pioneer.Information.Domain.Shared.Enum;

namespace Paas.Pioneer.Information.Application.Contracts.Grid.Dto.Output
{
    /// <summary>
    /// 获取栅格管理分页
    /// </summary>
    public class GetGridPageListOutput
    {

        /// <summary>
        /// 字典Id
        /// </summary>
        public Guid DictionaryId { get; set; }

        /// <summary>
        /// 字典Id
        /// </summary>
        public string DictionaryName { get; set; }

        /// <summary>
        /// 栅格管理类型
        /// </summary>
        public EGridType GridType { get; set; }

        /// <summary>
        /// 栅格管理类型
        /// </summary>
        public string GridTypeName { get => GridType.ToDescription(); }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 图片
        /// </summary>
        public string Portrait { get; set; }


        /// <summary>
        /// 拓展信息
        /// </summary>
        public string Expand { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }


        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

    }
}