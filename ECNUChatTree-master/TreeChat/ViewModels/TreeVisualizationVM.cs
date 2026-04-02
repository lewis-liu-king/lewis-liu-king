using System;
using System.Collections.Generic;
using System.Linq;
using TreeChat.Models;
using TreeChat.Services;
using TreeChat.Commands;

namespace TreeChat.ViewModels
{
    /// <summary>
    /// 树可视化ViewModel，管理树节点显示和搜索功能
    /// </summary>
    public class TreeVisualizationVM : BaseViewModel
    {
        public TreeNodeVM? RootNode { get; private set; }

        private TreeNodeVM? _selectedNode;
        public TreeNodeVM? SelectedNode
        {
            get => _selectedNode;
            set
            {
                if (value != null && _selectedNode != value)
                    SelectedNodeChanged?.Invoke(value);
                SetProperty(ref _selectedNode, value);
            }
        }

        private string _searchKeyword = string.Empty;
        private readonly List<TreeNodeVM> _matchedNodes = new();
        private int _currentMatchIndex = -1;

        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string SearchKeyword
        {
            get => _searchKeyword;
            set => SetProperty(ref _searchKeyword, value);
        }

        /// <summary>
        /// 匹配信息
        /// </summary>
        public string MatchInfo => _matchedNodes.Count > 0
            ? $"第{_currentMatchIndex + 1}个/共{_matchedNodes.Count}个" : string.Empty;

        public RelayCommand SearchCommand { get; }
        public RelayCommand NavigateNextCommand { get; }
        public RelayCommand NavigatePreviousCommand { get; }

        public event Action? CanvasPropertyChanged;
        public event Action<TreeNodeVM>? SelectedNodeChanged;

        public TreeVisualizationVM()
        {
            SearchCommand = new RelayCommand(SearchNodes);
            NavigateNextCommand = new RelayCommand(NavigateNext, _ => _matchedNodes.Count > 0);
            NavigatePreviousCommand = new RelayCommand(NavigatePrevious, _ => _matchedNodes.Count > 0);
        }

        public void SetTree(TreeNodeVM rootNode)
        {
            RootNode = rootNode;
            TreeLayoutService.LayoutTree(RootNode);
            CanvasPropertyChanged?.Invoke();
            SelectedNode = rootNode;
        }

        public void UpdateTree(TreeNodeVM updateNode, TreeNodeVM selectedNode)
        {
            if (RootNode == null) return;
            TreeLayoutService.UpdateLayoutTree(updateNode);
            CanvasPropertyChanged?.Invoke();
        }

        /// <summary>
        /// 搜索节点并高亮匹配结果
        /// </summary>
        public void SearchNodes(object? parameter)
        {
            _matchedNodes.Clear();
            _currentMatchIndex = -1;

            if (string.IsNullOrEmpty(SearchKeyword) || RootNode == null)
            {
                ClearAllMatches(RootNode);
                CanvasPropertyChanged?.Invoke();
                OnPropertyChanged(nameof(MatchInfo));
                NavigateNextCommand.RaiseCanExecuteChanged();
                NavigatePreviousCommand.RaiseCanExecuteChanged();
                return;
            }

            FindMatches(RootNode, SearchKeyword);
            CanvasPropertyChanged?.Invoke();
            OnPropertyChanged(nameof(MatchInfo));
            NavigateNextCommand.RaiseCanExecuteChanged();
            NavigatePreviousCommand.RaiseCanExecuteChanged();

            if (_matchedNodes.Count > 0)
                NavigateToMatch(0);
        }

        /// <summary>
        /// 跳转到下一个匹配节点
        /// </summary>
        public void NavigateNext(object? parameter)
        {
            if (_matchedNodes.Count == 0) return;
            NavigateToMatch((_currentMatchIndex + 1) % _matchedNodes.Count);
        }

        /// <summary>
        /// 跳转到上一个匹配节点
        /// </summary>
        public void NavigatePrevious(object? parameter)
        {
            if (_matchedNodes.Count == 0) return;
            NavigateToMatch((_currentMatchIndex - 1 + _matchedNodes.Count) % _matchedNodes.Count);
        }

        private void NavigateToMatch(int index)
        {
            _currentMatchIndex = index;
            SelectedNode = _matchedNodes[index];
            OnPropertyChanged(nameof(MatchInfo));
        }

        private void FindMatches(TreeNodeVM node, string keyword)
        {
            node.IsMatched = node.ContainsKeyword(keyword);
            if (node.IsMatched) _matchedNodes.Add(node);
            foreach (var child in node.Children)
                FindMatches(child, keyword);
        }

        private void ClearAllMatches(TreeNodeVM? node)
        {
            if (node == null) return;
            node.IsMatched = false;
            foreach (var child in node.Children)
                ClearAllMatches(child);
        }
    }
}
