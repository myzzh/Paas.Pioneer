﻿using Microsoft.AspNetCore.Http;
using Paas.Pioneer.Domain.Shared.Extensions;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace Paas.Pioneer.Admin.Core.Domain.Shared.Helpers
{
    public class IPHelper
    {
        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 获得IP地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetIP(HttpRequest request)
        {
            if (request == null)
            {
                return "";
            }

            string ip = request.Headers["X-Real-IP"].FirstOrDefault();
            if (ip.IsNullOrEmpty())
            {
                ip = request.Headers["X-Forwarded-For"].FirstOrDefault();
            }
            if (ip.IsNullOrEmpty())
            {
                ip = request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            }
            if (ip.IsNullOrEmpty() || !IsIP(ip.Split(":")[0]))
            {
                ip = "127.0.0.1";
            }

            return ip;
        }

        /// <summary>
        /// 获得MAC地址
        /// </summary>
        /// <returns></returns>
        public static string GetMACIp()
        {
            //本地计算机网络连接信息
            //IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            //获取本机电脑名
            //var HostName = computerProperties.HostName;
            //获取域名
            //var DomainName = computerProperties.DomainName;

            //获取本机所有网络连接
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            if (nics == null || nics.Length < 1)
            {
                return "";
            }

            var MACIp = "";
            foreach (NetworkInterface adapter in nics)
            {
                var adapterName = adapter.Name;

                var adapterDescription = adapter.Description;
                var NetworkInterfaceType = adapter.NetworkInterfaceType;
                if (adapterName == "本地连接" || adapterName == "WLAN")
                {
                    PhysicalAddress address = adapter.GetPhysicalAddress();
                    byte[] bytes = address.GetAddressBytes();

                    for (int i = 0; i < bytes.Length; i++)
                    {
                        MACIp += bytes[i].ToString("X2");

                        if (i != bytes.Length - 1)
                        {
                            MACIp += "-";
                        }
                    }
                }
            }

            return MACIp;
        }
    }
}