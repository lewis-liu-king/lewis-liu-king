using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TreeChat.Models;

namespace TreeChat.ViewModels
{
    /// <summary>
    /// 节点VM，包含节点数据和绘图属性
    /// </summary>
    public class TreeNodeVM : BaseViewModel
    {
        public const double WIDTH = 80;
        public const double HEIGHT = 30;

        public double X { get; set; }
        public double Y { get; set; }

        public List<double> SubtreeWidth { get; set; } = new List<double>();

        public ChatTreeNode Node { get; }

        public int ID => Node.NodeID;

        public TreeNodeVM? ParentNode { get; }

        private readonly ObservableCollection<TreeNodeVM> _children;
        public ReadOnlyObservableCollection<TreeNodeVM> Children { get; }

        /// <summary>
        /// 节点显示内容（用户消息前12字符）
        /// </summary>
        public string DisplayContent
        {
            get
            {
                var content = Node.UserMessage.Content;
                if (string.IsNullOrEmpty(content)) return Node.NodeID.ToString();
                return content.Length > 12 ? content.Substring(0, 12) + "..." : content;
            }
        }

        /// <summary>
        /// 完整内容（用于Tooltip显示）
        /// </summary>
        public string FullContent
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendLine($"用户: {Node.UserMessage.Content}");
                if (Node.ReplyMessage != null && !string.IsNullOrEmpty(Node.ReplyMessage.Content))
                    sb.AppendLine($"AI: {Node.ReplyMessage.Content}");
                return sb.ToString().TrimEnd();
            }
        }

        /// <summary>
        /// 是否有AI回复
        /// </summary>
        public bool HasReply => Node.ReplyMessage != null && !string.IsNullOrEmpty(Node.ReplyMessage.Content);

        /// <summary>
        /// 节点宽度（根据内容自适应）
        /// </summary>
        public double NodeWidth => Math.Min(150, Math.Max(80, DisplayContent.Length * 10));

        private bool _isMatched;
        /// <summary>
        /// 是否匹配搜索条件（用于高亮显示）
        /// </summary>
        public bool IsMatched
        {
            get => _isMatched;
            set => SetProperty(ref _isMatched, value);
        }

        /// <summary>
        /// 检查节点是否包含指定关键词
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns>是否匹配</returns>
        public bool ContainsKeyword(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) return false;
            var lowerKeyword = keyword.ToLower();
            if (Node.UserMessage.Content?.ToLower().Contains(lowerKeyword) == true) return true;
            if (Node.ReplyMessage?.Content?.ToLower().Contains(lowerKeyword) == true) return true;
            return false;
        }

        public TreeNodeVM(ChatTreeNode node, TreeNodeVM? parentNode)
        {
            Node = node;
            _children = new ObservableCollection<TreeNodeVM>();
            Children = new ReadOnlyObservableCollection<TreeNodeVM>(_children);

            foreach (var child in Node.ChildNodes)
                _children.Add(new TreeNodeVM(child, this));

            ParentNode = parentNode;
        }

        /// <summary>
        /// 添加子节点，并返回对应的子节点VM
        /// </summary>
        public TreeNodeVM AddChild(ChatTreeNode childNode)
        {
            Node.ChildNodes.Add(childNode);
            var childViewModel = new TreeNodeVM(childNode, this);
            _children.Add(childViewModel);
            return childViewModel;
        }
    }
}
