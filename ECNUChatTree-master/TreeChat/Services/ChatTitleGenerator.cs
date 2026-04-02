namespace TreeChat.Services
{
    /// <summary>
    /// 聊天标题生成服务，负责从用户消息中智能生成简洁的对话标题
    /// </summary>
    public class ChatTitleGenerator
    {
        private const int MaxTitleLength = 20;

        /// <summary>
        /// 从用户输入消息生成聊天标题
        /// </summary>
        /// <param name="userMessage">用户的第一条消息</param>
        /// <returns>生成的简洁标题</returns>
        public string GenerateTitle(string userMessage)
        {
            if (string.IsNullOrWhiteSpace(userMessage))
                return "新对话";

            string cleanedMessage = CleanMessage(userMessage);
            string title = ExtractMeaningfulPart(cleanedMessage);
            title = TruncateIfNeeded(title);

            return string.IsNullOrWhiteSpace(title) ? "新对话" : title;
        }

        private string CleanMessage(string message)
        {
            return message.Trim();
        }

        private string ExtractMeaningfulPart(string message)
        {
            int firstSentenceEnd = FindFirstSentenceEnd(message);
            if (firstSentenceEnd > 0)
                return message.Substring(0, firstSentenceEnd);

            int firstNewLine = message.IndexOfAny(new[] { '\n', '\r' });
            if (firstNewLine > 0)
                return message.Substring(0, firstNewLine);

            return message;
        }

        private int FindFirstSentenceEnd(string message)
        {
            char[] sentenceEnders = { '。', '！', '？', '.', '!', '?' };
            int minIndex = -1;

            foreach (char ender in sentenceEnders)
            {
                int index = message.IndexOf(ender);
                if (index >= 0 && (minIndex == -1 || index < minIndex))
                {
                    minIndex = index;
                }
            }

            return minIndex >= 0 ? minIndex + 1 : -1;
        }

        private string TruncateIfNeeded(string title)
        {
            if (title.Length <= MaxTitleLength)
                return title;

            return title.Substring(0, MaxTitleLength - 1) + "…";
        }
    }
}
