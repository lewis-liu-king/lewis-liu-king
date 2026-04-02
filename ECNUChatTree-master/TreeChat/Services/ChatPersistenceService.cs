using Newtonsoft.Json;
using System.IO;
using TreeChat.Models;
using TreeChat.Models.Dto;

namespace TreeChat.Services
{
    /// <summary>
    /// 聊天数据持久化服务，负责保存和加载聊天树数据到JSON文件
    /// </summary>
    public class ChatPersistenceService
    {
        private const string DataDirectory = "data";
        private const string FileName = "chats.json";

        /// <summary>
        /// 保存聊天树列表到文件
        /// </summary>
        /// <param name="chatTrees">要保存的聊天树列表</param>
        public void Save(List<ChatTree> chatTrees)
        {
            try
            {
                EnsureDirectoryExists();
                var dtoList = chatTrees.Select(ChatTreeDto.FromModel).ToList();
                var json = JsonConvert.SerializeObject(dtoList, Formatting.Indented);
                File.WriteAllText(GetFilePath(), json);
            }
            catch (Exception ex)
            {
                NotificationService.Instance.ShowError(ErrorInfo.FromException(ex));
            }
        }

        /// <summary>
        /// 从文件加载聊天树列表
        /// </summary>
        /// <returns>加载的聊天树列表，如果文件不存在或加载失败则返回空列表</returns>
        public List<ChatTree> Load()
        {
            try
            {
                var filePath = GetFilePath();
                if (!File.Exists(filePath)) return new List<ChatTree>();

                var json = File.ReadAllText(filePath);
                var dtoList = JsonConvert.DeserializeObject<List<ChatTreeDto>>(json);
                
                return dtoList?.Select(dto => dto.ToModel()).ToList() ?? new List<ChatTree>();
            }
            catch (Exception ex)
            {
                NotificationService.Instance.ShowError(ErrorInfo.FromException(ex));
                return new List<ChatTree>();
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
