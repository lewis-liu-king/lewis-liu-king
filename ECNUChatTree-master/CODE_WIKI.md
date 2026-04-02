# ECNUChatTree - Code Wiki

## 1. 项目整体架构

ECNUChatTree是一个采用MVVM（Model-View-ViewModel）架构的WPF应用程序，主要功能是实现树形结构的AI聊天工具。项目使用C#语言开发，基于.NET 8.0框架。

### 1.1 架构图

```
┌─────────────────────────────────────────────────────────────────┐
│                          视图层 (Views)                        │
│ ┌───────────────┐ ┌───────────────────┐ ┌────────────────────┐ │
│ │ MainWindow    │ │ TreeVisualization │ │ ChatInformation   │ │
│ │ ChatManagement│ │ View              │ │ View              │ │
│ └───────────────┘ └───────────────────┘ └────────────────────┘ │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                       视图模型层 (ViewModels)                  │
│ ┌────────────────┐ ┌──────────────────┐ ┌───────────────────┐ │
│ │ MainWindowVM   │ │ TreeVisualization│ │ ChatInformationVM │ │
│ │                │ │ VM               │ │                   │ │
│ └────────────────┘ └──────────────────┘ └───────────────────┘ │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                         服务层 (Services)                      │
│ ┌────────────┐ ┌──────────────────┐ ┌──────────────────┐      │
│ │ OpenAIChat │ │ ChatPersistence  │ │ SettingsService  │      │
│ │            │ │ Service          │ │                  │      │
│ └────────────┘ └──────────────────┘ └──────────────────┘      │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                          模型层 (Models)                       │
│ ┌───────────┐ ┌──────────────┐ ┌──────────────┐ ┌──────────┐ │
│ │ ChatTree  │ │ ChatTreeNode │ │ ChatMessage  │ │ ApiSettings│ │
│ └───────────┘ └──────────────┘ └──────────────┘ └──────────┘ │
└─────────────────────────────────────────────────────────────────┘
```

### 1.2 架构说明

- **视图层 (Views)**: 负责用户界面的展示，包括MainWindow、TreeVisualizationView、ChatInformationView等。视图通过数据绑定与视图模型进行交互，不包含业务逻辑。

- **视图模型层 (ViewModels)**: 负责处理业务逻辑，连接视图和模型。包括MainWindowVM、TreeVisualizationVM、ChatInformationVM等。视图模型实现了INotifyPropertyChanged接口，支持数据绑定。

- **服务层 (Services)**: 提供各种服务功能，包括OpenAIChat（与AI模型交互）、ChatPersistenceService（数据持久化）、SettingsService（设置管理）等。服务层采用单例模式，确保全局只有一个实例。

- **模型层 (Models)**: 定义数据结构，包括ChatTree（聊天树）、ChatTreeNode（聊天树节点）、ChatMessage（聊天消息）、ApiSettings（API配置）等。模型层不包含业务逻辑，只负责数据的存储和访问。

## 2. 主要模块职责

### 2.1 Models模块

- **ChatTree**: 聊天树结构，包含根节点和当前节点等信息。负责管理整个聊天树的结构和状态。
- **ChatTreeNode**: 聊天树节点，包含用户消息、AI回复、父节点和子节点等信息。负责管理节点的上下文和子节点。
- **ChatMessage**: 聊天消息单元，包含角色和消息内容。用于存储聊天中的单条消息。
- **ApiSettings**: API配置相关模型，包括ApiProfile（API配置档案）和AppSettings（应用程序设置）。用于管理API的配置信息。
- **DTO**: 数据传输对象，用于JSON序列化和反序列化。包括ChatTreeDto、ChatTreeNodeDto、ChatMessageDto等。

### 2.2 Services模块

- **OpenAIChat**: 提供与大模型服务器交互的服务类。负责调用AI接口，获取AI回复。
- **ChatPersistenceService**: 聊天数据持久化服务，负责保存和加载聊天树数据到JSON文件。
- **SettingsService**: 应用程序设置服务，负责加载、保存和管理配置。
- **ApiConfig**: 大模型API配置类，提供静态属性访问当前API配置。
- **TreeLayoutService**: 树布局服务，负责计算和更新树节点的布局。
- **NotificationService**: 通知服务，负责显示错误信息和其他通知。

### 2.3 ViewModels模块

- **BaseViewModel**: 视图模型基类，实现了INotifyPropertyChanged接口，支持数据绑定。
- **MainWindowVM**: 主窗口视图模型，管理其他视图模型的生命周期和交互。
- **TreeVisualizationVM**: 树可视化视图模型，管理树节点显示和搜索功能。
- **ChatInformationVM**: 聊天信息视图模型，管理聊天消息的显示和发送。
- **ChatManagementPanelVM**: 聊天管理面板视图模型，管理聊天树的创建、保存和加载。
- **SettingsWindowVM**: 设置窗口视图模型，管理API配置的编辑和保存。
- **TreeNodeVM**: 树节点视图模型，管理单个树节点的显示和交互。

### 2.4 Views模块

- **MainWindow**: 主窗口，包含聊天管理面板、树可视化视图和聊天信息视图。
- **TreeVisualizationView**: 树可视化视图，负责显示聊天树的结构，支持节点选择、搜索和缩放。
- **ChatInformationView**: 聊天信息视图，负责显示当前节点的消息内容和发送新消息。
- **ChatManagementPanel**: 聊天管理面板，负责管理聊天树的创建、保存和加载。
- **SettingsWindow**: 设置窗口，负责编辑和保存API配置。

### 2.5 Commands模块

- **RelayCommand**: 通用命令基类，实现了ICommand接口，支持同步命令。
- **AsyncRelayCommand**: 并发命令基类，支持异步命令，防止重复点击。

## 3. 关键类与函数说明

### 3.1 Models模块

#### ChatTree

- **构造函数**: `ChatTree(string? systemPrompt = null)` - 创建一个新的聊天树，可选参数为系统提示。
- **RootNode**: 获取根节点。
- **CurrentNode**: 获取或设置当前节点。
- **TreeTitle**: 获取或设置树的标题。

#### ChatTreeNode

- **构造函数**: `ChatTreeNode(ChatTreeNode? parentNode, ChatMessage userMessage)` - 创建一个新的聊天树节点。
- **ParentNode**: 获取父节点。
- **ChildNodes**: 获取子节点列表。
- **UserMessage**: 获取用户消息。
- **ReplyMessage**: 获取或设置AI回复消息。
- **NodeID**: 获取节点ID。
- **GetFullContext()**: 得到完整上下文，包括从根节点到当前节点的所有用户消息和AI回复。
- **AddChildNode(ChatMessage userMessage)**: 添加一个新的子节点，包含用户消息，并返回新创建的子节点。
- **SetAiReply(ChatMessage replyMessage)**: 设置AI回复消息。

#### ChatMessage

- **构造函数**: `ChatMessage(string role, string content)` - 创建一个新的聊天消息。
- **Role**: 获取消息角色。
- **Content**: 获取消息内容。

### 3.2 Services模块

#### OpenAIChat

- **Instance**: 获取OpenAIChat的单例实例。
- **CallAiApi(List<ChatMessage> context)**: 调用AI接口，传入上下文，返回AI回复。
- **UpdateAuthentication()**: 更新HttpClient的认证头，配置变更后调用此方法。

#### ChatPersistenceService

- **Save(List<ChatTree> chatTrees)**: 保存聊天树列表到文件。
- **Load()**: 从文件加载聊天树列表。

#### SettingsService

- **Instance**: 获取SettingsService的单例实例。
- **Load()**: 加载配置文件，如果不存在则创建默认配置。
- **Save()**: 保存当前配置到文件。
- **GetCurrentProfile()**: 获取当前激活的API配置档案。
- **SwitchProfile(string profileName)**: 切换到指定的配置档案。

### 3.3 ViewModels模块

#### MainWindowVM

- **构造函数**: `MainWindowVM()` - 创建主窗口视图模型，初始化其他视图模型并绑定事件。
- **ChatManagementPanelVM**: 获取聊天管理面板视图模型。
- **TreeVisualizationVM**: 获取树可视化视图模型。
- **ChatInformationVM**: 获取聊天信息视图模型。
- **ChangeNodeVMTree(ChatTree tree)**: 切换聊天树，更新树可视化视图模型。

#### TreeVisualizationVM

- **RootNode**: 获取根节点视图模型。
- **SelectedNode**: 获取或设置选中的节点视图模型。
- **SearchKeyword**: 获取或设置搜索关键词。
- **MatchInfo**: 获取匹配信息。
- **SearchCommand**: 搜索命令。
- **NavigateNextCommand**: 导航到下一个匹配节点命令。
- **NavigatePreviousCommand**: 导航到上一个匹配节点命令。
- **SetTree(TreeNodeVM rootNode)**: 设置树，更新布局并选中根节点。
- **UpdateTree(TreeNodeVM updateNode, TreeNodeVM selectedNode)**: 更新树，重新计算布局。
- **SearchNodes(object? parameter)**: 搜索节点并高亮匹配结果。

#### ChatInformationVM

- **UserMessage**: 获取或设置用户消息。
- **AIReply**: 获取或设置AI回复。
- **InputMessage**: 获取或设置输入消息。
- **SendMessage**: 发送消息命令。
- **SelectedNode**: 获取或设置选中的节点视图模型。
- **ExecuteSendMessageAsync(object? parameter)**: 执行发送消息操作，调用AI接口获取回复。

### 3.4 Views模块

#### TreeVisualizationView

- **SelectedNode**: 获取或设置选中的节点视图模型。
- **RenderTree()**: 渲染树，绘制节点和连接线。
- **DrawConnections(TreeNodeVM rootNode)**: 绘制节点之间的连接线。
- **DrawNodes(TreeNodeVM rootNode)**: 绘制节点。
- **HighlightSelectedNode(TreeNodeVM node)**: 高亮选中的节点。

#### ChatInformationView

- 负责显示当前节点的消息内容和发送新消息，通过数据绑定与ChatInformationVM进行交互。

### 3.5 Commands模块

#### RelayCommand

- **构造函数**: `RelayCommand(Action<object?> execute, Func<object?, bool>? canExecte = null)` - 创建一个新的同步命令。
- **CanExecute(object? parameter)**: 判断命令是否可以执行。
- **Execute(object? parameter)**: 执行命令。
- **RaiseCanExecuteChanged()**: 触发CanExecuteChanged事件，通知UI重新评估命令可用性。

#### AsyncRelayCommand

- **构造函数**: `AsyncRelayCommand(Func<object?, Task> execute, Func<object?, bool>? canExecute = null)` - 创建一个新的异步命令。
- **CanExecute(object? parameter)**: 判断命令是否可以执行，执行中禁用命令。
- **Execute(object? parameter)**: 执行异步命令，设置执行状态并处理异常。

## 4. 依赖关系

### 4.1 外部依赖

| 依赖项 | 版本 | 用途 |
|--------|------|------|
| Newtonsoft.Json | 13.0.4 | JSON序列化和反序列化 |

### 4.2 内部依赖

- **Models模块**: 被Services、ViewModels模块依赖。
- **Services模块**: 被ViewModels模块依赖。
- **ViewModels模块**: 被Views模块依赖。
- **Views模块**: 依赖ViewModels模块。
- **Commands模块**: 被ViewModels模块依赖。

## 5. 项目运行方式

### 5.1 启动流程

1. 应用程序启动，执行App.xaml.cs中的OnStartup方法。
2. 注册全局异常处理器，捕获未处理的异常。
3. 调用基类的OnStartup方法，启动MainWindow窗口。
4. MainWindowVM初始化，创建并初始化其他视图模型。
5. 加载配置文件和聊天数据。
6. 显示主窗口，用户可以开始使用应用程序。

### 5.2 运行步骤

1. **配置API**:
   - 打开设置窗口，输入API密钥、API端点地址和模型名称。
   - 保存配置。

2. **创建聊天树**:
   - 在聊天管理面板中点击"新建聊天"按钮。
   - 输入系统提示（可选）。

3. **发送消息**:
   - 在聊天信息视图中输入消息。
   - 点击"发送"按钮或按Enter键。
   - 等待AI回复。

4. **管理聊天树**:
   - 在树可视化视图中点击节点，查看消息内容。
   - 发送消息时，会在当前节点下创建一个新的子节点。
   - 使用搜索功能查找特定节点。

5. **保存和加载聊天**:
   - 聊天数据会自动保存到data/chats.json文件。
   - 启动应用程序时会自动加载保存的聊天数据。

## 6. 代码示例

### 6.1 创建聊天树

```csharp
// 创建一个新的聊天树，带有系统提示
var chatTree = new ChatTree("你是一个有帮助的AI助手。");

// 添加用户消息并获取AI回复
var userMessage = new ChatMessage("user", "你好，如何使用这个应用？");
var newNode = chatTree.RootNode.AddChildNode(userMessage);

// 调用AI接口获取回复
var context = newNode.GetFullContext();
var aiReply = await OpenAIChat.Instance.CallAiApi(context);

// 设置AI回复
newNode.SetAiReply(new ChatMessage("assistant", aiReply));
```

### 6.2 保存和加载聊天树

```csharp
// 保存聊天树列表
var persistenceService = new ChatPersistenceService();
var chatTrees = new List<ChatTree> { chatTree };
persistenceService.Save(chatTrees);

// 加载聊天树列表
var loadedChatTrees = persistenceService.Load();
```

### 6.3 使用命令

```csharp
// 创建同步命令
SearchCommand = new RelayCommand(SearchNodes);

// 创建异步命令
SendMessage = new AsyncRelayCommand(
    execute: ExecuteSendMessageAsync,
    canExecute: CanExecuteSendMessage);
```

## 7. 总结

ECNUChatTree是一个功能完整的树形AI聊天工具，采用MVVM架构设计，具有以下特点：

- **树形结构**: 支持聊天历史的树形展示，方便用户查看和管理不同的聊天分支。
- **AI集成**: 集成了OpenAI API，支持与AI模型进行交互。
- **数据持久化**: 支持聊天数据的保存和加载，确保用户数据不会丢失。
- **配置管理**: 支持多套API配置，方便用户切换不同的AI模型和服务。
- **用户友好**: 提供了直观的用户界面，支持节点搜索、缩放和平移等功能。

项目结构清晰，代码组织合理，遵循了MVVM架构的设计原则，便于维护和扩展。