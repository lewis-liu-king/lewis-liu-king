# 聊天树保存与加载功能实现计划

## 一、需求分析

### 1.1 问题描述
当前应用程序关闭后，所有对话数据丢失，重新打开程序时无法恢复之前的对话内容。

### 1.2 需要保存的数据
- **ChatTree列表**：用户创建的所有对话树
- **每个ChatTree**：标题、根节点、当前节点位置
- **每个ChatTreeNode**：节点ID、父子关系、用户消息、AI回复

### 1.3 技术挑战
- ChatTreeNode存在循环引用（ParentNode属性），直接序列化会导致无限递归
- 需要保持树的完整结构

---

## 二、解决方案设计

### 2.1 整体架构
采用 **DTO（数据传输对象）模式** + **服务层** 的设计：

```
Models (现有)          Models/Dto (新增)           Services (新增)
┌─────────────┐       ┌──────────────────┐       ┌────────────────────┐
│ ChatTree    │ ────► │ ChatTreeDto      │       │ ChatPersistence    │
│ ChatTreeNode│ ────► │ ChatTreeNodeDto  │ ◄───► │ Service            │
│ ChatMessage │ ────► │ ChatMessageDto   │       └────────────────────┘
└─────────────┘       └──────────────────┘
```

### 2.2 文件存储方案
- **格式**：JSON（使用Newtonsoft.Json，项目已引用）
- **位置**：应用程序目录下创建 `data` 文件夹
- **文件名**：`chats.json`

---

## 三、详细实现步骤

### 步骤1：创建DTO模型类（3个文件）

#### 文件1：`Models/Dto/ChatMessageDto.cs`（约30行）
```csharp
/// <summary>
/// 聊天消息的数据传输对象，用于JSON序列化
/// </summary>
public class ChatMessageDto
{
    /// <summary>
    /// 消息角色（system/user/assistant）
    /// </summary>
    public string Role { get; set; }
    
    /// <summary>
    /// 消息内容
    /// </summary>
    public string Content { get; set; }
    
    // 从ChatMessage转换为Dto
    public static ChatMessageDto FromModel(ChatMessage message) { ... }
    
    // 从Dto转换为ChatMessage
    public ChatMessage ToModel() { ... }
}
```

#### 文件2：`Models/Dto/ChatTreeNodeDto.cs`（约50行）
```csharp
/// <summary>
/// 聊天树节点的数据传输对象，用于JSON序列化
/// 解决循环引用问题：使用ParentNodeId代替ParentNode引用
/// </summary>
public class ChatTreeNodeDto
{
    /// <summary>
    /// 节点唯一标识
    /// </summary>
    public int NodeId { get; set; }
    
    /// <summary>
    /// 父节点ID（根节点为null）
    /// </summary>
    public int? ParentNodeId { get; set; }
    
    /// <summary>
    /// 用户消息
    /// </summary>
    public ChatMessageDto UserMessage { get; set; }
    
    /// <summary>
    /// AI回复消息
    /// </summary>
    public ChatMessageDto ReplyMessage { get; set; }
    
    /// <summary>
    /// 子节点ID列表
    /// </summary>
    public List<int> ChildNodeIds { get; set; }
    
    // 转换方法
    public static ChatTreeNodeDto FromModel(ChatTreeNode node) { ... }
}
```

#### 文件3：`Models/Dto/ChatTreeDto.cs`（约40行）
```csharp
/// <summary>
/// 聊天树的数据传输对象，用于JSON序列化
/// </summary>
public class ChatTreeDto
{
    /// <summary>
    /// 对话标题
    /// </summary>
    public string TreeTitle { get; set; }
    
    /// <summary>
    /// 当前节点ID
    /// </summary>
    public int CurrentNodeId { get; set; }
    
    /// <summary>
    /// 所有节点列表（扁平化存储）
    /// </summary>
    public List<ChatTreeNodeDto> Nodes { get; set; }
    
    // 转换方法
    public static ChatTreeDto FromModel(ChatTree tree) { ... }
    public ChatTree ToModel() { ... }
}
```

---

### 步骤2：创建持久化服务（1个文件）

#### 文件4：`Services/ChatPersistenceService.cs`（约80行）
```csharp
/// <summary>
/// 聊天数据持久化服务，负责保存和加载聊天树数据
/// </summary>
public class ChatPersistenceService
{
    private static readonly string DataDirectory = "data";
    private static readonly string FileName = "chats.json";
    
    /// <summary>
    /// 保存聊天树列表到文件
    /// </summary>
    /// <param name="chatTrees">要保存的聊天树列表</param>
    public void Save(List<ChatTree> chatTrees) { ... }
    
    /// <summary>
    /// 从文件加载聊天树列表
    /// </summary>
    /// <returns>加载的聊天树列表，如果文件不存在则返回空列表</returns>
    public List<ChatTree> Load() { ... }
    
    /// <summary>
    /// 获取数据文件的完整路径
    /// </summary>
    private string GetFilePath() { ... }
    
    /// <summary>
    /// 确保数据目录存在
    /// </summary>
    private void EnsureDirectoryExists() { ... }
}
```

---

### 步骤3：修改现有类（最小改动）

#### 修改1：`ChatTreeNode.cs` - 添加静态ID重置方法
**改动原因**：加载时需要重置节点ID计数器
**改动内容**：添加一个静态方法

```csharp
/// <summary>
/// 重置节点ID计数器（用于加载存档时）
/// </summary>
public static void ResetNodeIdCounter(int maxValue)
{
    _nextNodeID = maxValue + 1;
}
```

#### 修改2：`ChatTree.cs` - 添加构造函数重载
**改动原因**：加载时需要根据已有节点重建树
**改动内容**：添加一个内部构造函数

```csharp
/// <summary>
/// 内部构造函数，用于从DTO重建树结构
/// </summary>
internal ChatTree(ChatTreeNode root, int currentNodeId)
{
    RootNode = root;
    CurrentNode = FindNodeById(root, currentNodeId) ?? root;
}
```

#### 修改3：`ChatManagementPanelVM.cs` - 添加保存和加载逻辑
**改动内容**：
1. 添加持久化服务实例
2. 在构造函数中调用加载
3. 添加保存方法供外部调用

```csharp
private readonly ChatPersistenceService _persistenceService;

public ChatManagementPanelVM()
{
    _persistenceService = new ChatPersistenceService();
    _chatList = new ObservableCollection<ChatTree>();
    
    // 加载保存的数据
    LoadChats();
    
    CreateNewChat = new RelayCommand(ExecuteCreateNewChat);
}

/// <summary>
/// 从文件加载聊天记录
/// </summary>
public void LoadChats() { ... }

/// <summary>
/// 保存当前所有聊天记录到文件
/// </summary>
public void SaveChats() { ... }
```

---

### 步骤4：添加自动保存触发机制

#### 修改4：`MainWindow.xaml.cs` - 窗口关闭时保存
**改动内容**：在窗口关闭事件中调用保存

```csharp
protected override void OnClosing(CancelEventArgs e)
{
    // 获取ViewModel并保存数据
    if (DataContext is MainWindowVM vm)
    {
        vm.ChatManagementPanelVM.SaveChats();
    }
    base.OnClosing(e);
}
```

#### 修改5：`ChatManagementPanelVM.cs` - 添加自动保存触发
**改动内容**：在ChatList变化时触发保存

```csharp
// 在构造函数中添加集合变化监听
_chatList.CollectionChanged += (s, e) => SaveChats();
```

---

## 四、文件清单

### 新增文件（4个）
| 文件路径 | 预计行数 | 说明 |
|---------|---------|------|
| `Models/Dto/ChatMessageDto.cs` | ~30行 | 消息DTO |
| `Models/Dto/ChatTreeNodeDto.cs` | ~50行 | 节点DTO |
| `Models/Dto/ChatTreeDto.cs` | ~40行 | 树DTO |
| `Services/ChatPersistenceService.cs` | ~80行 | 持久化服务 |

### 修改文件（4个）
| 文件路径 | 改动量 | 说明 |
|---------|--------|------|
| `Models/ChatTreeNode.cs` | +5行 | 添加ID重置方法 |
| `Models/ChatTree.cs` | +10行 | 添加重建构造函数 |
| `ViewModels/ChatManagementPanelVM.cs` | +25行 | 添加加载/保存逻辑 |
| `Views/MainWindow.xaml.cs` | +10行 | 添加关闭保存 |

---

## 五、数据流程图

### 保存流程
```
用户关闭窗口
    ↓
MainWindow.OnClosing()
    ↓
ChatManagementPanelVM.SaveChats()
    ↓
ChatPersistenceService.Save()
    ↓
ChatTreeDto.FromModel() → ChatTreeNodeDto.FromModel() → ChatMessageDto.FromModel()
    ↓
JSON序列化 → 写入 data/chats.json
```

### 加载流程
```
应用程序启动
    ↓
MainWindowVM构造函数
    ↓
ChatManagementPanelVM构造函数
    ↓
ChatPersistenceService.Load()
    ↓
JSON反序列化 → ChatTreeDto.ToModel() → 重建树结构
    ↓
ChatManagementPanelVM.ChatList 填充数据
```

---

## 六、注意事项

1. **不改动原有核心逻辑**：所有新增功能通过扩展实现
2. **遵循MVVM模式**：服务类放在Services文件夹，DTO放在Models/Dto文件夹
3. **代码注释规范**：
   - 类级别注释说明功能
   - public方法注释说明用途和参数
   - private方法不需要注释
4. **文件长度控制**：每个文件不超过100行
5. **错误处理**：加载失败时返回空列表，不影响程序启动

---

## 七、实现顺序

1. ✅ 创建 `Models/Dto/` 文件夹
2. ✅ 实现 `ChatMessageDto.cs`
3. ✅ 实现 `ChatTreeNodeDto.cs`
4. ✅ 实现 `ChatTreeDto.cs`
5. ✅ 实现 `ChatPersistenceService.cs`
6. ✅ 修改 `ChatTreeNode.cs`（添加ID重置）
7. ✅ 修改 `ChatTree.cs`（添加构造函数）
8. ✅ 修改 `ChatManagementPanelVM.cs`（添加加载/保存）
9. ✅ 修改 `MainWindow.xaml.cs`（添加关闭保存）
10. ✅ 测试保存和加载功能
