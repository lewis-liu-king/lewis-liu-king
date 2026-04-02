using System;
using System.Collections.Generic;
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
    /// 节点信息VM
    /// </summary>
    public class ChatInformationVM : BaseViewModel
    {
        private string? _userMessage;
        private string? _aiReply;
        private readonly ChatTitleGenerator _titleGenerator = new ChatTitleGenerator();
        private bool _hasGeneratedTitle = false;

        public string? UserMessage
        {
            get => _userMessage;
            set => SetProperty(ref _userMessage, value);
        }
        public string? AIReply
        {
            get => _aiReply;
            set => SetProperty(ref _aiReply, value);
        }

        private string _inputMessage;
        public string InputMessage
        {
            get => _inputMessage;
            set
            {
                SetProperty(ref _inputMessage, value);
                SendMessage.OnCanExecuteChanged();
            }
        }

        public AsyncRelayCommand SendMessage { get; }

        private TreeNodeVM? _selectedNode;
        public TreeNodeVM? SelectedNode
        {
            get => _selectedNode;
            set
            {
                _selectedNode = value;
                _hasGeneratedTitle = false;
                if(value != null)
                {
                    UserMessage = value.Node.UserMessage.Content;
                    AIReply = value.Node.ReplyMessage?.Content;
                }
                SendMessage.OnCanExecuteChanged();
            }
        }

        public event Action<TreeNodeVM, TreeNodeVM>? ChatTreeChanged;
        public event Action<string>? RequestUpdateChatTitle;

        public ChatInformationVM()
        {
            UserMessage = string.Empty;
            AIReply = string.Empty;

            SendMessage = new AsyncRelayCommand(
                execute: ExecuteSendMessageAsync,
                canExecute: CanExecuteSendMessage);
        }

        private bool CanExecuteSendMessage(object? parameter)
        {
            return !string.IsNullOrEmpty(InputMessage) && SelectedNode != null;
        }

        private async Task ExecuteSendMessageAsync(object? parameter)
        {
            if (string.IsNullOrWhiteSpace(InputMessage) || SelectedNode == null) return;

            try
            {
                ChatTreeNode newNode = new ChatTreeNode(SelectedNode.Node, new ChatMessage("user", InputMessage));
                TreeNodeVM newNodeVM = SelectedNode.AddChild(newNode);
                ChatTreeChanged?.Invoke(SelectedNode, newNodeVM);
                SelectedNode = newNodeVM;

                if (!_hasGeneratedTitle)
                {
                    string title = _titleGenerator.GenerateTitle(InputMessage);
                    RequestUpdateChatTitle?.Invoke(title);
                    _hasGeneratedTitle = true;
                }

                InputMessage = string.Empty;

                string aiReply = await OpenAIChat.Instance.CallAiApi(SelectedNode.Node.GetFullContext());

                SelectedNode.Node.SetAiReply(new ChatMessage("assistant", aiReply));
                AIReply = aiReply;
            }
            catch (Exception ex)
            {
                var error = ErrorInfo.FromException(ex);
                NotificationService.Instance.ShowError(error);
                AIReply = error.UserMessage;
            }
        }
    }
}
