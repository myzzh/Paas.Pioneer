﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Paas.Pioneer.Admin.Core.Application.Contracts.Document;
using Paas.Pioneer.Admin.Core.Application.Contracts.Document.Dto.Input;
using Paas.Pioneer.Admin.Core.Application.Contracts.Document.Dto.Output;
using Paas.Pioneer.Domain.Shared.Configs;
using Paas.Pioneer.AutoWrapper;
using Paas.Pioneer.Domain.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Paas.Pioneer.Admin.Core.HttpApi.Controllers
{
    /// <summary>
    /// 文档管理
    /// </summary>
    [Route("api/admin/[controller]/[action]")]
    [Authorize]
    public class DocumentController : AbpControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly UploadConfig _uploadConfig;
        private readonly UploadHelper _uploadHelper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="uploadHelper"></param>
        /// <param name="uploadConfig"></param>
        /// <param name="documentService"></param>
        public DocumentController(
            UploadHelper uploadHelper,
            IOptionsMonitor<UploadConfig> uploadConfig,
            IDocumentService documentService
        )
        {
            _uploadHelper = uploadHelper;
            _uploadConfig = uploadConfig.CurrentValue;
            _documentService = documentService;
        }

        /// <summary>
        /// 获取分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<DocumentGetGroupOutput> GetGroupAsync(Guid id)
        {
            return await _documentService.GetGroupAsync(id);
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<DocumentGetMenuOutput> GetMenuAsync(Guid id)
        {
            return await _documentService.GetMenuAsync(id);
        }

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<DocumentGetContentOutput> GetContentAsync(Guid id)
        {
            return await _documentService.GetContentAsync(id);
        }

        /// <summary>
        /// 查询文档列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<DocumentListOutput>> GetList(string key, DateTime? start, DateTime? end)
        {
            return await _documentService.GetListAsync(key, start, end);
        }

        /// <summary>
        /// 查询文档图片列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<string>> GetImageList(Guid id)
        {
            return await _documentService.GetImageListAsync(id);
        }

        /// <summary>
        /// 查询精简文档列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<DocumentGetPlainListOutput>> GetPlainList()
        {
            return await _documentService.GetPlainListAsync();
        }

        /// <summary>
        /// 新增分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddGroup([FromBody] DocumentAddGroupInput input)
        {
            await _documentService.AddGroupAsync(input);
        }

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddMenu([FromBody] DocumentAddMenuInput input)
        {
            await _documentService.AddMenuAsync(input);
        }

        /// <summary>
        /// 修改分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateGroup([FromBody] DocumentUpdateGroupInput input)
        {
            await _documentService.UpdateGroupAsync(input);
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateMenu([FromBody] DocumentUpdateMenuInput input)
        {
            await _documentService.UpdateMenuAsync(input);
        }

        /// <summary>
        /// 修改文档内容
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateContent([FromBody] DocumentUpdateContentInput input)
        {
            await _documentService.UpdateContentAsync(input);
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeleteImage(Guid documentId, string url)
        {
            await _documentService.DeleteImageAsync(documentId, url);
        }

        /// <summary>
        /// 上传文档图片
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UploadImage([FromForm] DocumentUploadImageInput input)
        {
            var config = _uploadConfig.Document;
            var res = await _uploadHelper.UploadAsync(input.File, config, new { input.Id });
            if (res.Success)
            {
                //保存文档图片
                await _documentService.AddImageAsync(
                new DocumentAddImageInput
                {
                    DocumentId = input.Id,
                    Url = res.Data.FileRequestPath
                });
                return HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + res.Data.FileRequestPath;
            }
            throw new BusinessException("上传失败！");
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task SoftDelete(Guid id)
        {
            await _documentService.SoftDeleteAsync(id);
        }
    }
}