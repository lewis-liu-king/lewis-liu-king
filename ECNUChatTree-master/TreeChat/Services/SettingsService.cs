using Newtonsoft.Json;
using System.IO;
using TreeChat.Models;

namespace TreeChat.Services
{
    /// <summary>
    /// 应用程序设置服务，负责加载、保存和管理配置
    /// 使用单例模式确保全局只有一个配置管理实例
    /// </summary>
    public class SettingsService
    {
        private const string DataDirectory = "data";
        private const string FileName = "settings.json";

        private static SettingsService? _instance;
        public static SettingsService Instance => _instance ??= new SettingsService();

        public AppSettings Settings { get; private set; }

        private SettingsService()
        {
            Settings = AppSettings.CreateDefault();
            Load();
        }

        /// <summary>
        /// 加载配置文件，如果不存在则创建默认配置
        /// </summary>
        public void Load()
        {
            try
            {
                var filePath = GetFilePath();
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    var settings = JsonConvert.DeserializeObject<AppSettings>(json);
                    if (settings != null && settings.Profiles.Count > 0)
                        Settings = settings;
                }
            }
            catch (Exception ex)
            {
                NotificationService.Instance.ShowError(ErrorInfo.FromException(ex));
                Settings = AppSettings.CreateDefault();
            }
        }

        /// <summary>
        /// 保存当前配置到文件
        /// </summary>
        public void Save()
        {
            try
            {
                EnsureDirectoryExists();
                var json = JsonConvert.SerializeObject(Settings, Formatting.Indented);
                File.WriteAllText(GetFilePath(), json);
            }
            catch (Exception ex)
            {
                NotificationService.Instance.ShowError(ErrorInfo.FromException(ex));
            }
        }

        /// <summary>
        /// 获取当前激活的API配置档案
        /// </summary>
        /// <returns>当前配置档案</returns>
        public ApiProfile GetCurrentProfile() => Settings.GetCurrentProfile();

        /// <summary>
        /// 切换到指定的配置档案
        /// </summary>
        /// <param name="profileName">配置档案名称</param>
        public void SwitchProfile(string profileName)
        {
            if (Settings.Profiles.ContainsKey(profileName))
            {
                Settings.CurrentProfileName = profileName;
                Save();
            }
        }

        private string GetFilePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DataDirectory, FileName);
        }

        private void EnsureDirectoryExists()
        {
            var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DataDirectory);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }
    }
}
