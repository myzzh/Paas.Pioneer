﻿using Volo.Abp;

namespace Paas.Pioneer.WeChat.Application.MiniPrograms.Login
{
    public class PcLoginAuthorizeTooFrequentlyException : UserFriendlyException
    {
        public PcLoginAuthorizeTooFrequentlyException() : base("授权登录操作太过频繁，请稍后重试", "PcLoginAuthorizeTooFrequently")
        {

        }
    }
}