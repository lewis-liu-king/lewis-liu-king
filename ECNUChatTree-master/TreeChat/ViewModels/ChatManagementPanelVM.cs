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
            }
        }

        public RelayCommand CreateNewChat { get; }
        public RelayCommand OpenSettingsCommand { get; }

        public event Action<ChatTree>? SelectedChatChanged;

        public ChatManagementPanelVM()
        {
            _persistenceService = new ChatPersistenceService();
            _chatList = new ObservableCollection<ChatTree>();
            _chatList.CollectionChanged += (s, e) => SaveChats();

            LoadChats();
            CreateNewChat = new RelayCommand(ExecuteCreateNewChat);
            OpenSettingsCommand = new RelayCommand(OpenSettings);
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
    }
}
