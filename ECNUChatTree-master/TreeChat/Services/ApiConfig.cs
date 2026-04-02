﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeChat.Services
{
    /// <summary>
    /// 大模型API配置类，提供静态属性访问当前API配置
    /// 配置从SettingsService动态读取，支持运行时修改
    /// </summary>
    public static class ApiConfig
    {
        /// <summary>
        /// 获取当前API密钥
        /// </summary>
        public static string ApiKey => SettingsService.Instance.GetCurrentProfile().ApiKey;

        /// <summary>
        /// 获取当前API端点地址
        /// </summary>
        public static string ApiEndpoint => SettingsService.Instance.GetCurrentProfile().ApiEndpoint;

        /// <summary>
        /// 获取当前模型名称
        /// </summary>
        public static string ModelName => SettingsService.Instance.GetCurrentProfile().ModelName;

        /// <summary>
        /// 获取温度参数（控制回复随机性，0-2之间）
        /// </summary>
        public static double Temperature => SettingsService.Instance.GetCurrentProfile().Temperature;

        /// <summary>
        /// 获取TopP参数（核采样，0-1之间）
        /// </summary>
        public static double TopP => SettingsService.Instance.GetCurrentProfile().TopP;

        /// <summary>
        /// 获取TopK参数（候选词数量）
        /// </summary>
        public static int TopK => SettingsService.Instance.GetCurrentProfile().TopK;
    }
}
