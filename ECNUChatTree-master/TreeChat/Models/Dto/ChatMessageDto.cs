namespace TreeChat.Models.Dto
{
    /// <summary>
    /// 聊天消息的数据传输对象，用于JSON序列化
    /// </summary>
    public class ChatMessageDto
    {
        /// <summary>
        /// 消息角色（system/user/assistant）
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 从ChatMessage模型转换为DTO
        /// </summary>
        /// <param name="message">原始消息对象</param>
        /// <returns>转换后的DTO对象</returns>
        public static ChatMessageDto FromModel(ChatMessage message)
        {
            return new ChatMessageDto
            {
                Role = message.Role,
                Content = message.Content
            };
        }

        /// <summary>
        /// 将DTO转换为ChatMessage模型对象
        /// </summary>
        /// <returns>转换后的ChatMessage对象</returns>
        public ChatMessage ToModel()
        {
            return new ChatMessage(Role, Content);
        }
    }
}
