using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;

namespace TreeChat.Models
{
    /// <summary>
    /// 错误类型枚举，用于分类不同类型的错误
    /// </summary>
    public enum ErrorType
    {
        Network,
        Authentication,
        File,
        Configuration,
        Unknown
    }

    /// <summary>
    /// 错误信息模型，包含错误类型和用户友好的提示信息
    /// </summary>
    public class ErrorInfo
    {
        public ErrorType Type { get; }
        public string UserMessage { get; }
        public string? TechnicalMessage { get; }
        public Exception? Exception { get; }

        public ErrorInfo(ErrorType type, string userMessage, string? technicalMessage = null, Exception? exception = null)
        {
            Type = type;
            UserMessage = userMessage;
            TechnicalMessage = technicalMessage;
            Exception = exception;
        }

        /// <summary>
        /// 根据异常类型自动判断错误类型并创建ErrorInfo
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <returns>包含用户友好提示的ErrorInfo对象</returns>
        public static ErrorInfo FromException(Exception ex)
        {
            var (type, message) = ClassifyException(ex);
            return new ErrorInfo(type, message, ex.Message, ex);
        }

        private static (ErrorType type, string message) ClassifyException(Exception ex)
        {
            return ex switch
            {
                HttpRequestException httpEx => httpEx.StatusCode switch
                {
                    HttpStatusCode.Unauthorized => (ErrorType.Authentication, "API Key无效，请在设置中检查配置"),
                    HttpStatusCode.Forbidden => (ErrorType.Authentication, "访问被拒绝，请检查API权限"),
                    HttpStatusCode.TooManyRequests => (ErrorType.Network, "请求过于频繁，请稍后重试"),
                    _ => (ErrorType.Network, "网络连接失败，请检查网络后重试")
                },
                TaskCanceledException => (ErrorType.Network, "请求超时，请稍后重试"),
                SocketException => (ErrorType.Network, "网络连接失败，请检查网络设置"),
                TimeoutException => (ErrorType.Network, "操作超时，请稍后重试"),
                UnauthorizedAccessException => (ErrorType.File, "文件访问被拒绝，请检查文件权限"),
                FileNotFoundException => (ErrorType.File, "文件不存在，已自动创建"),
                IOException => (ErrorType.File, "文件操作失败，请检查文件是否被占用"),
                Newtonsoft.Json.JsonException => (ErrorType.Configuration, "数据解析失败，文件可能已损坏"),
                _ => (ErrorType.Unknown, "发生未知错误，请稍后重试")
            };
        }
    }
}
