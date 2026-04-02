using Systemusing System;
using System.Collections.Generic;
using System.Linq;
using TreeChat.Models;
usingusing System;
using System.Collections.Generic;
using System.Linq;
using TreeChat.Models;
using TreeChat.Services;
using TreeChat.Commandsusing System;
using System.Collections.Generic;
using System.Linq;
using TreeChat.Models;
using TreeChat.Services;
using TreeChat.Commands;

namespace TreeChat.ViewModels
{
    /// <summary>
    ///using System;
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
    publicusing System;
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
        public TreeNodeVM? RootNode {using System;
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

        privateusing System;
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
                if (value != null &&using System;
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
                SetPropertyusing System;
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeusing System;
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeCommand.RaiseCanExecuteChanged();
            }
        }

        private string _using System;
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeCommand.RaiseCanExecuteChanged();
            }
        }

        private string _searchKeyword = string.Empty;
        private readonly List<TreeNodeVM> _matchedNodes =using System;
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeCommand.RaiseCanExecuteChanged();
            }
        }

        private string _searchKeyword = string.Empty;
        private readonly List<TreeNodeVM> _matchedNodes = new();
        private int _currentMatchIndex = -1;

        /// <using System;
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeCommand.RaiseCanExecuteChanged();
            }
        }

        private string _searchKeyword = string.Empty;
        private readonly List<TreeNodeVM> _matchedNodes = new();
        private int _currentMatchIndex = -1;

        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string SearchKeywordusing System;
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeCommand.RaiseCanExecuteChanged();
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
            set => SetProperty(ref _searchusing System;
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeCommand.RaiseCanExecuteChanged();
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
        /// 匹配using System;
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeCommand.RaiseCanExecuteChanged();
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
        public string MatchInfo => _matchedNodes.Count >using System;
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeCommand.RaiseCanExecuteChanged();
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
            ? $"第{_currentusing System;
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeCommand.RaiseCanExecuteChanged();
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
            ? $"第{_currentMatchIndex + 1}个/共{_matchedNodes.Count}个" : string.Emptyusing System;
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeCommand.RaiseCanExecuteChanged();
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
        public RelayCommand Navigateusing System;
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeCommand.RaiseCanExecuteChanged();
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
        public Relayusing System;
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeCommand.RaiseCanExecuteChanged();
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
        public RelayCommand DeleteNodeCommand { get;using System;
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeCommand.RaiseCanExecuteChanged();
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
        public RelayCommand DeleteNodeCommand { get; }

        public event Action? CanvasPropertyChanged;
        public event Action<TreeNodeusing System;
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeCommand.RaiseCanExecuteChanged();
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
        public RelayCommand DeleteNodeCommand { get; }

        public event Action? CanvasPropertyChanged;
        public event Action<TreeNodeVM>? SelectedNodeChanged;

        public TreeVisualizationVM()
        {
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeCommand.RaiseCanExecuteChanged();
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
        public RelayCommand DeleteNodeCommand { get; }

        public event Action? CanvasPropertyChanged;
        public event Action<TreeNodeVM>? SelectedNodeChanged;

        public TreeVisualizationVM()
        {
            SearchCommand = new RelayCommand(SearchNodes);
            NavigateNextCommand = newusing System;
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeCommand.RaiseCanExecuteChanged();
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
        public RelayCommand DeleteNodeCommand { get; }

        public event Action? CanvasPropertyChanged;
        public event Action<TreeNodeVM>? SelectedNodeChanged;

        public TreeVisualizationVM()
        {
            SearchCommand = new RelayCommand(SearchNodes);
            NavigateNextCommand = new RelayCommand(NavigateNext, _ => _matchedNodes.Count > 0);
            Navusing System;
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeCommand.RaiseCanExecuteChanged();
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
        public RelayCommand DeleteNodeCommand { get; }

        public event Action? CanvasPropertyChanged;
        public event Action<TreeNodeVM>? SelectedNodeChanged;

        public TreeVisualizationVM()
        {
            SearchCommand = new RelayCommand(SearchNodes);
            NavigateNextCommand = new RelayCommand(NavigateNext, _ => _matchedNodes.Count > 0);
            NavigatePreviousCommand = new RelayCommand(NavigatePrevious, _ => _matchedNodes.Count > 0using System;
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeCommand.RaiseCanExecuteChanged();
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
        public RelayCommand DeleteNodeCommand { get; }

        public event Action? CanvasPropertyChanged;
        public event Action<TreeNodeVM>? SelectedNodeChanged;

        public TreeVisualizationVM()
        {
            SearchCommand = new RelayCommand(SearchNodes);
            NavigateNextCommand = new RelayCommand(NavigateNext, _ => _matchedNodes.Count > 0);
            NavigatePreviousCommand = new RelayCommand(NavigatePrevious, _ => _matchedNodes.Count > 0);
            DeleteNodeCommand = new RelayCommand(DeleteNode, CanDeleteNode);
        }

        public void SetTreeusing System;
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeCommand.RaiseCanExecuteChanged();
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
        public RelayCommand DeleteNodeCommand { get; }

        public event Action? CanvasPropertyChanged;
        public event Action<TreeNodeVM>? SelectedNodeChanged;

        public TreeVisualizationVM()
        {
            SearchCommand = new RelayCommand(SearchNodes);
            NavigateNextCommand = new RelayCommand(NavigateNext, _ => _matchedNodes.Count > 0);
            NavigatePreviousCommand = new RelayCommand(NavigatePrevious, _ => _matchedNodes.Count > 0);
            DeleteNodeCommand = new RelayCommand(DeleteNode, CanDeleteNode);
        }

        public void SetTree(TreeNodeVM rootNode)
        {
            RootNode = rootNode;
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
                // 当选中节点变化时，重新评估删除命令的可执行性
                DeleteNodeCommand.RaiseCanExecuteChanged();
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
        public RelayCommand DeleteNodeCommand { get; }

        public event Action? CanvasPropertyChanged;
        public event Action<TreeNodeVM>? SelectedNodeChanged;

        public TreeVisualizationVM()
        {
            SearchCommand = new RelayCommand(SearchNodes);
            NavigateNextCommand = new RelayCommand(NavigateNext, _ => _matchedNodes.Count > 0);
            NavigatePreviousCommand = new RelayCommand(NavigatePrevious, _ => _matchedNodes.Count > 0);
            DeleteNodeCommand = new RelayCommand(DeleteNode, CanDeleteNode);
        }

        public void SetTree(TreeNodeVM rootNode)
        {
            RootNode = rootNode;
            TreeLayoutService.LayoutTree(RootNode);
            CanvasPropertyChanged?.Invoke();
