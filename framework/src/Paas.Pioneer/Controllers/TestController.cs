﻿using EasyCaching.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Paas.Pioneer.AutoWrapper;
using Paas.Pioneer.DynamicProxy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Paas.Pioneer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : AbpController
    {
        private readonly ProxyFactory _proxyFactory;
        private readonly IEasyCachingProvider _redisCachingProvider;
        public TestController(ProxyFactory proxyFactory,
            IEasyCachingProvider redisCachingProvider)
        {
            _proxyFactory = proxyFactory;
            _redisCachingProvider = redisCachingProvider;
        }

        // GET: api/<TestController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _redisCachingProvider.GetAsync<string>("key1");
            // 从工厂获取代理类
            var testService = _proxyFactory.Create<ITestService>();
            testService.GetUserId();
            return new string[] { "value1", "value2" };
        }

        // GET api/<TestController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TestController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("SuitableAsync")]
        public async Task<bool> SuitableAsync()
        {
            await Task.CompletedTask;
            return Random.Shared.Next(-20, 55) >= 20;
        }

        [HttpPost("Empty")]
        public void Empty([FromBody] user user)
        {
            //throw new BusinessException("已经存在该表");
        }

        [HttpGet("EmptyTask")]
        public Task EmptyTask()
        {
            var a = "213f";
            int.Parse(a);
            return Task.CompletedTask;
        }

        [HttpGet("EmptyTaskResponseOutput")]
        public IResponseOutput EmptyTaskResponseOutput()
        {
            return ResponseOutput.Succees();
        }

        [HttpGet("EmptyObj")]
        public object EmptyObj()
        {
            return new
            {
                id = 10,
                name = "dd"
            };
        }
    }

    public class user
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "请输入业务Id")]
        public string Name { get; set; }
    }
}
