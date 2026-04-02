using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TreeChat.Commands;
using TreeChat.Models;
using TreeChat.Services;
using TreeChat.Views;

namespace TreeChat.ViewModels
{
    /// <summary>
    /// 聊天管理面板的ViewModel，负责管理多个聊天树
    /// </summary>
    public class ChatManagementPanelVM : BaseViewModel
    {
        private ObservableCollection<ChatTree> _chatList;
        private ChatTree? _selectedChat;
        private readonly ChatPersistenceService _persistenceService;

        public ObservableCollection<ChatTree> ChatList => _chatList;

        public ChatTree? SelectedChat
        {
            get => _selectedChat;
            set
            {
                SetProperty(ref _selectedChat, value);
                if (value != null)
                    SelectedChatChanged?.Invoke(value);
                DeleteChatCommand.RaiseCanExecuteChanged();
                EditChatTitleCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand CreateNewChat { get; }
        public RelayCommand OpenSettingsCommand { get; }
        public RelayCommand DeleteChatCommand { get; }
        public RelayCommand EditChatTitleCommand { get; }

        public event Action<ChatTree>? SelectedChatChanged;
        public event Action? ChatTitleUpdated;

        public ChatManagementPanelVM()
        {
            _persistenceService = new ChatPersistenceService();
            _chatList = new ObservableCollection<ChatTree>();
            _chatList.CollectionChanged += (s, e) => SaveChats();

            LoadChats();
            CreateNewChat = new RelayCommand(ExecuteCreateNewChat);
            OpenSettingsCommand = new RelayCommand(OpenSettings);
            DeleteChatCommand = new RelayCommand(ExecuteDeleteChat, CanDeleteChat);
            EditChatTitleCommand = new RelayCommand(ExecuteEditChatTitle, CanEditChatTitle);
        }

        private void ExecuteCreateNewChat(object? parameter)
        {
            ChatTree newTree = new ChatTree();
            ChatList.Add(newTree);
            SelectedChat = newTree;
        }

        /// <summary>
        /// 打开设置窗口
        /// </summary>
        private void OpenSettings(object? parameter)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.Owner = Application.Current.MainWindow;
            settingsWindow.ShowDialog();
        }

        /// <summary>
        /// 从文件加载聊天记录
        /// </summary>
        public void LoadChats()
        {
            var loadedChats = _persistenceService.Load();
            _chatList.CollectionChanged -= (s, e) => SaveChats();
            _chatList.Clear();
            foreach (var chat in loadedChats)
                _chatList.Add(chat);
            _chatList.CollectionChanged += (s, e) => SaveChats();

            if (_chatList.Count > 0)
                SelectedChat = _chatList[0];
        }

        /// <summary>
        /// 保存当前所有聊天记录到文件
        /// </summary>
        public void SaveChats()
        {
            _persistenceService.Save(_chatList.ToList());
        }

        /// <summary>
        /// 判断是否可以删除当前选中的聊天
        /// </summary>
        private bool CanDeleteChat(object? parameter)
        {
            return SelectedChat != null;
        }

        /// <summary>
        /// 删除当前选中的聊天
        /// </summary>
        private void ExecuteDeleteChat(object? parameter)
        {
            if (SelectedChat == null) return;

            int index = _chatList.IndexOf(SelectedChat);
            _chatList.Remove(SelectedChat);

            if (_chatList.Count > 0)
            {
                SelectedChat = _chatList[Math.Min(index, _chatList.Count - 1)];
            }
            else
            {
                ExecuteCreateNewChat(null);
            }
        }

        /// <summary>
        /// 判断是否可以编辑当前选中聊天的标题
        /// </summary>
        private bool CanEditChatTitle(object? parameter)
        {
            return SelectedChat != null;
        }

        /// <summary>
        /// 编辑当前选中聊天的标题
        /// </summary>
        private void ExecuteEditChatTitle(object? parameter)
        {
            if (SelectedChat == null) return;

            string currentTitle = SelectedChat.TreeTitle;
            string? newTitle = Microsoft.VisualBasic.Interaction.InputBox("请输入新的对话名称:", "编辑对话名称", currentTitle);

            if (!string.IsNullOrWhiteSpace(newTitle) && newTitle != currentTitle)
            {
                SelectedChat.TreeTitle = newTitle;
                ChatTitleUpdated?.Invoke();
                SaveChats();
            }
        }

        /// <summary>
        /// 更新当前选中聊天的标题
        /// </summary>
        /// <param name="newTitle">新标题</param>
        public void UpdateSelectedChatTitle(string newTitle)
        {
            if (SelectedChat != null && !string.IsNullOrWhiteSpace(newTitle))
            {
                SelectedChat.TreeTitle = newTitle;
                ChatTitleUpdated?.Invoke();
                SaveChats();
            }
        }
    }
}
