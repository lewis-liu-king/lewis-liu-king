namespace TreeChat.Models
{
    /// <summary>
    /// API配置档案，存储单个API的完整配置信息
    /// </summary>
    public class ApiProfile
    {
        public string Name { get; set; } = "默认配置";
        public string ApiKey { get; set; } = "";
        public string ApiEndpoint { get; set; } = "";
        public string ModelName { get; set; } = "";
        public double Temperature { get; set; } = 0.7;
        public double TopP { get; set; } = 0.8;
        public int TopK { get; set; } = 20;

        /// <summary>
        /// 创建默认的ECNU配置档案
        /// </summary>
        public static ApiProfile CreateDefault()
        {
            return new ApiProfile
            {
                Name = "ECNU Plus",
                ApiKey = "",
                ApiEndpoint = "https://chat.ecnu.edu.cn/open/api/v1/chat/completions",
                ModelName = "ecnu-plus",
                Temperature = 0.7,
                TopP = 0.8,
                TopK = 20
            };
        }
    }

    /// <summary>
    /// 应用程序设置，包含所有配置档案和当前选中的档案名称
    /// </summary>
    public class AppSettings
    {
        public string CurrentProfileName { get; set; } = "default";
        public Dictionary<string, ApiProfile> Profiles { get; set; } = new();

        /// <summary>
        /// 获取当前激活的配置档案
        /// </summary>
        /// <returns>当前配置档案，如果不存在则返回默认配置</returns>
        public ApiProfile GetCurrentProfile()
        {
            if (Profiles.TryGetValue(CurrentProfileName, out var profile))
                return profile;
            if (Profiles.Count > 0)
                return Profiles.Values.First();
            return ApiProfile.CreateDefault();
        }

        /// <summary>
        /// 创建包含默认配置的应用设置
        /// </summary>
        public static AppSettings CreateDefault()
        {
            var settings = new AppSettings();
            settings.Profiles["default"] = ApiProfile.CreateDefault();
            return settings;
        }
    }
}
