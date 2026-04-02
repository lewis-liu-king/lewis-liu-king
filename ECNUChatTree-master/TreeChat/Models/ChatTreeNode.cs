namespace TreeChat.Models
{
    /// <summary>
    /// 聊天树节点，包含用户消息、AI回复、父节点和子节点等信息
    /// </summary>
    public class ChatTreeNode
    {
        public ChatTreeNode? ParentNode { get; }
        public List<ChatTreeNode> ChildNodes { get; } = new List<ChatTreeNode>();
        public ChatMessage UserMessage { get; }
        public ChatMessage? ReplyMessage { get; private set; }
        public int NodeID { get; }

        private static int _nextNodeID = 1;

        public ChatTreeNode(ChatTreeNode? parentNode, ChatMessage userMessage)
        {
            ParentNode = parentNode;
            UserMessage = userMessage;
            NodeID = _nextNodeID++;
        }

        /// <summary>
        /// 内部构造函数，用于从DTO重建节点时使用
        /// </summary>
        /// <param name="parentNode">父节点</param>
        /// <param name="userMessage">用户消息</param>
        /// <param name="nodeId">指定的节点ID</param>
        /// <param name="replyMessage">AI回复消息（可选）</param>
        internal ChatTreeNode(ChatTreeNode? parentNode, ChatMessage userMessage, int nodeId, ChatMessage? replyMessage = null)
        {
            ParentNode = parentNode;
            UserMessage = userMessage;
            NodeID = nodeId;
            ReplyMessage = replyMessage;
        }

        /// <summary>
        /// 重置节点ID计数器，用于加载存档时确保ID不冲突
        /// </summary>
        /// <param name="maxValue">当前已使用的最大节点ID</param>
        public static void ResetNodeIdCounter(int maxValue)
        {
            _nextNodeID = maxValue + 1;
        }

        /// <summary>
        /// 得到完整上下文，包括从根节点到当前节点的所有用户消息和AI回复，按照时间顺序排列
        /// </summary>
        /// <returns></returns>
        public List<ChatMessage> GetFullContext()
        {
            var context = new List<ChatMessage>();
            var currentNode = this;

            while (currentNode != null)
            {
                if (currentNode.ReplyMessage != null && !string.IsNullOrEmpty(currentNode.ReplyMessage.Content))
                    context.Add(currentNode.ReplyMessage);

                if (!string.IsNullOrEmpty(currentNode.UserMessage.Content))
                    context.Add(currentNode.UserMessage);

                currentNode = currentNode.ParentNode;
            }

            context.Reverse();
            return context;
        }

        /// <summary>
        /// 添加一个新的子节点，包含用户消息，并返回新创建的子节点
        /// </summary>
        /// <param name="userMessage"></param>
        /// <returns></returns>
        public ChatTreeNode AddChildNode(ChatMessage userMessage)
        {
            var childNode = new ChatTreeNode(this, userMessage);
            ChildNodes.Add(childNode);
            return childNode;
        }

        /// <summary>
        /// 设置AI回复消息
        /// </summary>
        /// <param name="replyMessage"></param>
        public void SetAiReply(ChatMessage replyMessage)
        {
            ReplyMessage = replyMessage;
        }

        /// <summary>
        /// 从子节点列表中移除指定的子节点
        /// </summary>
        /// <param name="childNode">要移除的子节点</param>
        /// <returns>是否成功移除</returns>
        public bool RemoveChildNode(ChatTreeNode childNode)
        {
            return ChildNodes.Remove(childNode);
        }
    }
}