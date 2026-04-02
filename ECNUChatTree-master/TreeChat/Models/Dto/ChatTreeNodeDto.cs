namespace TreeChat.Models.Dto
{
    /// <summary>
    /// 聊天树节点的数据传输对象，用于JSON序列化
    /// 使用ParentNodeId替代ParentNode引用，解决循环引用问题
    /// </summary>
    public class ChatTreeNodeDto
    {
        /// <summary>
        /// 节点唯一标识
        /// </summary>
        public int NodeId { get; set; }

        /// <summary>
        /// 父节点ID（根节点为null）
        /// </summary>
        public int? ParentNodeId { get; set; }

        /// <summary>
        /// 用户消息
        /// </summary>
        public ChatMessageDto? UserMessage { get; set; }

        /// <summary>
        /// AI回复消息
        /// </summary>
        public ChatMessageDto? ReplyMessage { get; set; }

        /// <summary>
        /// 子节点ID列表
        /// </summary>
        public List<int> ChildNodeIds { get; set; } = new List<int>();

        /// <summary>
        /// 从ChatTreeNode模型转换为DTO
        /// </summary>
        /// <param name="node">原始节点对象</param>
        /// <returns>转换后的DTO对象</returns>
        public static ChatTreeNodeDto FromModel(ChatTreeNode node)
        {
            var dto = new ChatTreeNodeDto
            {
                NodeId = node.NodeID,
                ParentNodeId = node.ParentNode?.NodeID,
                UserMessage = ChatMessageDto.FromModel(node.UserMessage),
                ChildNodeIds = node.ChildNodes.Select(c => c.NodeID).ToList()
            };

            if (node.ReplyMessage != null)
            {
                dto.ReplyMessage = ChatMessageDto.FromModel(node.ReplyMessage);
            }

            return dto;
        }
    }
}
