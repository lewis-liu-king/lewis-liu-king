namespace TreeChat.Models
{
    /// <summary>
    /// 聊天树结构，包含根节点和当前节点等信息
    /// </summary>
    public class ChatTree
    {
        public ChatTreeNode RootNode { get; }
        public ChatTreeNode CurrentNode { get; private set; }
        public string TreeTitle { get; set; } = "新对话";

        public ChatTree(string? systemPrompt = null)
        {
            if (!string.IsNullOrWhiteSpace(systemPrompt))
            {
                RootNode = new ChatTreeNode(null, new ChatMessage("system", systemPrompt));
            }
            else
            {
                RootNode = new ChatTreeNode(null, new ChatMessage("system", "你是一个有帮助的AI助手。"));
            }
            CurrentNode = RootNode;
        }

        /// <summary>
        /// 内部构造函数，用于从DTO重建树结构
        /// </summary>
        /// <param name="root">根节点</param>
        /// <param name="currentNodeId">当前节点ID</param>
        /// <param name="nodeDict">节点ID到节点的映射字典</param>
        internal ChatTree(ChatTreeNode root, int currentNodeId, Dictionary<int, ChatTreeNode> nodeDict)
        {
            RootNode = root;
            CurrentNode = FindNodeById(root, currentNodeId) ?? root;
            if (nodeDict.Count > 0)
            {
                int maxId = nodeDict.Keys.Max();
                ChatTreeNode.ResetNodeIdCounter(maxId);
            }
        }

        private ChatTreeNode? FindNodeById(ChatTreeNode startNode, int nodeID)
        {
            if (startNode.NodeID == nodeID) return startNode;
            foreach (var child in startNode.ChildNodes)
            {
                var found = FindNodeById(child, nodeID);
                if (found != null) return found;
            }
            return null;
        }
    }
}
