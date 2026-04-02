namespace TreeChat.Models.Dto
{
    /// <summary>
    /// 聊天树的数据传输对象，用于JSON序列化
    /// 将树结构扁平化存储，便于序列化和反序列化
    /// </summary>
    public class ChatTreeDto
    {
        public string TreeTitle { get; set; } = "新对话";
        public int CurrentNodeId { get; set; }
        public List<ChatTreeNodeDto> Nodes { get; set; } = new List<ChatTreeNodeDto>();

        /// <summary>
        /// 从ChatTree模型转换为DTO
        /// </summary>
        public static ChatTreeDto FromModel(ChatTree tree)
        {
            var dto = new ChatTreeDto
            {
                TreeTitle = tree.TreeTitle,
                CurrentNodeId = tree.CurrentNode.NodeID,
                Nodes = new List<ChatTreeNodeDto>()
            };
            FlattenNodes(tree.RootNode, dto.Nodes);
            return dto;
        }

        private static void FlattenNodes(ChatTreeNode node, List<ChatTreeNodeDto> nodes)
        {
            nodes.Add(ChatTreeNodeDto.FromModel(node));
            foreach (var child in node.ChildNodes)
                FlattenNodes(child, nodes);
        }

        /// <summary>
        /// 将DTO转换为ChatTree模型对象
        /// </summary>
        public ChatTree ToModel()
        {
            if (Nodes.Count == 0) return new ChatTree();

            var nodeDict = BuildNodeDictionary();
            RebuildChildRelationships(nodeDict);
            var rootNode = Nodes.FirstOrDefault(n => n.ParentNodeId == null);

            return rootNode != null && nodeDict.TryGetValue(rootNode.NodeId, out var root)
                ? new ChatTree(root, CurrentNodeId, nodeDict)
                : new ChatTree();
        }

        private Dictionary<int, ChatTreeNode> BuildNodeDictionary()
        {
            var dict = new Dictionary<int, ChatTreeNode>();
            foreach (var dto in Nodes)
            {
                var parent = dto.ParentNodeId.HasValue && dict.TryGetValue(dto.ParentNodeId.Value, out var p) ? p : null;
                var node = new ChatTreeNode(parent, dto.UserMessage!.ToModel(), dto.NodeId, dto.ReplyMessage?.ToModel());
                dict[dto.NodeId] = node;
            }
            return dict;
        }

        private void RebuildChildRelationships(Dictionary<int, ChatTreeNode> dict)
        {
            foreach (var dto in Nodes)
            {
                if (!dict.TryGetValue(dto.NodeId, out var node)) continue;
                foreach (var childId in dto.ChildNodeIds)
                    if (dict.TryGetValue(childId, out var childNode))
                        node.ChildNodes.Add(childNode);
            }
        }
    }
}
