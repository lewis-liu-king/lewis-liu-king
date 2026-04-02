# API配置图形化界面功能实现计划

## 一、需求分析

### 1.1 问题描述
当前API配置硬编码在 `ApiConfig.cs` 中，用户无法在运行时修改配置，存在以下问题：
- API Key暴露在代码中，存在安全风险
- 无法切换不同的API端点
- 无法调整模型参数（Temperature、TopK、TopP）
- 无法保存用户的个性化配置

### 1.2 功能目标
- 提供图形化界面配置API参数
- 支持配置持久化（保存到文件）
- 支持多个API配置切换
- 不改动原有核心代码

---

## 二、设计方案

### 2.1 整体架构

```
新增文件结构：
├── Models/
│   └── ApiSettings.cs          (配置模型，~50行)
├── Services/
│   └── SettingsService.cs      (配置服务，~60行)
├── ViewModels/
│   └── SettingsWindowVM.cs     (设置窗口VM，~80行)
└── Views/
    ├── SettingsWindow.xaml     (设置窗口UI)
    └── SettingsWindow.xaml.cs  (设置窗口代码，~20行)

修改文件（最小改动）：
├── Services/ApiConfig.cs       (改为从配置读取)
├── Services/OpenAIChat.cs      (添加配置更新方法)
├── Views/MainWindow.xaml       (添加设置按钮)
└── ViewModels/MainWindowVM.cs  (添加打开设置命令)
```

### 2.2 配置文件格式
存储位置：`data/settings.json`

```json
{
  "CurrentProfile": "default",
  "Profiles": {
    "default": {
      "Name": "ECNU Plus",
      "ApiKey": "sk-xxx",
      "ApiEndpoint": "https://chat.ecnu.edu.cn/open/api/v1/chat/completions",
      "ModelName": "ecnu-plus",
      "Temperature": 0.7,
      "TopP": 0.8,
      "TopK": 20
    }
  }
}
```

---

## 三、详细实现步骤

### 步骤1：创建配置模型类

#### 文件：`Models/ApiSettings.cs`（约50行）

```csharp
/// <summary>
/// API配置档案，存储单个API的完整配置信息
/// </summary>
public class ApiProfile
{
    public string Name { get; set; } = "默认配置";
    public string ApiKey { get; set; } = "";
    public string ApiEndpoint { get; set; } = "";
    public string ModelName { get; set; } = "";
    public double Temperature { get; set; } = 0.7;
    public double TopP { get; set; } = 0.8;
    public int TopK { get; set; } = 20;
}

/// <summary>
/// 应用程序设置，包含所有配置档案和当前选中的档案
/// </summary>
public class AppSettings
{
    public string CurrentProfileName { get; set; } = "default";
    public Dictionary<string, ApiProfile> Profiles { get; set; } = new();
    
    /// <summary>
    /// 获取当前激活的配置档案
    /// </summary>
    public ApiProfile GetCurrentProfile() { ... }
}
```

---

### 步骤2：创建配置服务类

#### 文件：`Services/SettingsService.cs`（约60行）

```csharp
/// <summary>
/// 应用程序设置服务，负责加载、保存和管理配置
/// </summary>
public class SettingsService
{
    private static SettingsService? _instance;
    public static SettingsService Instance => _instance ??= new SettingsService();
    
    public AppSettings Settings { get; private set; }
    
    /// <summary>
    /// 加载配置文件，如果不存在则创建默认配置
    /// </summary>
    public void Load() { ... }
    
    /// <summary>
    /// 保存当前配置到文件
    /// </summary>
    public void Save() { ... }
    
    /// <summary>
    /// 获取当前激活的API配置
    /// </summary>
    public ApiProfile GetCurrentProfile() { ... }
    
    /// <summary>
    /// 切换到指定的配置档案
    /// </summary>
    public void SwitchProfile(string profileName) { ... }
}
```

---

### 步骤3：创建设置窗口ViewModel

#### 文件：`ViewModels/SettingsWindowVM.cs`（约80行）

```csharp
/// <summary>
/// 设置窗口的ViewModel，处理配置的编辑和保存
/// </summary>
public class SettingsWindowVM : BaseViewModel
{
    // 可编辑的配置属性
    public string ApiKey { get; set; }
    public string ApiEndpoint { get; set; }
    public string ModelName { get; set; }
    public double Temperature { get; set; }
    public double TopP { get; set; }
    public int TopK { get; set; }
    
    // 配置档案列表（用于多配置切换）
    public ObservableCollection<string> ProfileNames { get; }
    public string SelectedProfileName { get; set; }
    
    // 命令
    public RelayCommand SaveCommand { get; }
    public RelayCommand AddProfileCommand { get; }
    public RelayCommand DeleteProfileCommand { get; }
    
    /// <summary>
    /// 保存当前配置并通知主程序更新
    /// </summary>
    private void Save() { ... }
    
    /// <summary>
    /// 添加新的配置档案
    /// </summary>
    private void AddProfile() { ... }
    
    /// <summary>
    /// 删除当前选中的配置档案
    /// </summary>
    private void DeleteProfile() { ... }
}
```

---

### 步骤4：创建设置窗口View

#### 文件：`Views/SettingsWindow.xaml`

界面布局：
```
┌─────────────────────────────────────────┐
│  API 设置                          [X]  │
├─────────────────────────────────────────┤
│  配置档案: [下拉选择框▼] [+添加] [删除] │
├─────────────────────────────────────────┤
│  API Key:     [____________________]    │
│  API端点:     [____________________]    │
│  模型名称:    [____________________]    │
├─────────────────────────────────────────┤
│  Temperature: [====●=====] 0.7          │
│  Top P:       [====●=====] 0.8          │
│  Top K:       [====●=====] 20           │
├─────────────────────────────────────────┤
│                    [取消]  [保存]       │
└─────────────────────────────────────────┘
```

#### 文件：`Views/SettingsWindow.xaml.cs`（约20行）

```csharp
/// <summary>
/// 设置窗口，提供API配置的图形化编辑界面
/// </summary>
public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
        DataContext = new SettingsWindowVM();
    }
}
```

---

### 步骤5：修改现有文件（最小改动）

#### 修改1：`Services/ApiConfig.cs`
**改动内容**：改为从SettingsService读取配置

```csharp
public static string ApiKey => SettingsService.Instance.GetCurrentProfile().ApiKey;
public static string ApiEndpoint => SettingsService.Instance.GetCurrentProfile().ApiEndpoint;
public static string ModelName => SettingsService.Instance.GetCurrentProfile().ModelName;
public static double Temperature => SettingsService.Instance.GetCurrentProfile().Temperature;
public static double TopP => SettingsService.Instance.GetCurrentProfile().TopP;
public static int TopK => SettingsService.Instance.GetCurrentProfile().TopK;
```

#### 修改2：`Services/OpenAIChat.cs`
**改动内容**：添加更新HttpClient的方法

```csharp
/// <summary>
/// 更新HttpClient的认证头（配置变更后调用）
/// </summary>
public void UpdateAuthentication(string apiKey)
{
    _httpClient.DefaultRequestHeaders.Authorization = 
        new AuthenticationHeaderValue("Bearer", apiKey);
}
```

#### 修改3：`Views/MainWindow.xaml`
**改动内容**：添加设置按钮

```xml
<!-- 在窗口标题栏或菜单区域添加 -->
<Button Content="⚙ 设置" Command="{Binding OpenSettingsCommand}" />
```

#### 修改4：`ViewModels/MainWindowVM.cs`
**改动内容**：添加打开设置窗口的命令

```csharp
public RelayCommand OpenSettingsCommand { get; }

public MainWindowVM()
{
    // ... 现有代码 ...
    OpenSettingsCommand = new RelayCommand(OpenSettings);
}

private void OpenSettings(object? parameter)
{
    var settingsWindow = new SettingsWindow();
    settingsWindow.Owner = Application.Current.MainWindow;
    settingsWindow.ShowDialog();
}
```

---

## 四、文件清单

### 新增文件（4个）
| 文件路径 | 预计行数 | 说明 |
|---------|---------|------|
| `Models/ApiSettings.cs` | ~50行 | 配置模型类 |
| `Services/SettingsService.cs` | ~60行 | 配置服务类 |
| `ViewModels/SettingsWindowVM.cs` | ~80行 | 设置窗口VM |
| `Views/SettingsWindow.xaml` | ~80行 | 设置窗口UI |
| `Views/SettingsWindow.xaml.cs` | ~20行 | 设置窗口代码 |

### 修改文件（4个）
| 文件路径 | 改动量 | 说明 |
|---------|--------|------|
| `Services/ApiConfig.cs` | ~15行 | 改为从配置服务读取 |
| `Services/OpenAIChat.cs` | +8行 | 添加认证更新方法 |
| `Views/MainWindow.xaml` | +5行 | 添加设置按钮 |
| `ViewModels/MainWindowVM.cs` | +10行 | 添加打开设置命令 |

---

## 五、数据流程

### 启动流程
```
应用程序启动
    ↓
SettingsService.Instance.Load()
    ↓
读取 data/settings.json
    ↓
如果不存在，创建默认配置
    ↓
ApiConfig 从 SettingsService 获取配置
    ↓
OpenAIChat 使用 ApiConfig 发起请求
```

### 配置修改流程
```
用户点击"设置"按钮
    ↓
打开 SettingsWindow
    ↓
SettingsWindowVM 加载当前配置
    ↓
用户编辑配置
    ↓
用户点击"保存"
    ↓
SettingsService.Save() 保存到文件
    ↓
触发配置变更事件
    ↓
OpenAIChat.UpdateAuthentication() 更新认证
```

---

## 六、注意事项

1. **不改动原有核心逻辑**：配置读取通过属性实现，保持原有调用方式
2. **遵循MVVM模式**：设置窗口完全遵循MVVM架构
3. **代码注释规范**：
   - 类级别注释说明功能
   - public方法注释说明用途和参数
   - private方法不需要注释
4. **文件长度控制**：每个文件不超过100行
5. **向后兼容**：如果配置文件不存在，使用默认值

---

## 七、实现顺序

1. ✅ 创建 `Models/ApiSettings.cs`（配置模型）
2. ✅ 创建 `Services/SettingsService.cs`（配置服务）
3. ✅ 修改 `Services/ApiConfig.cs`（从服务读取）
4. ✅ 修改 `Services/OpenAIChat.cs`（添加更新方法）
5. ✅ 创建 `ViewModels/SettingsWindowVM.cs`（设置窗口VM）
6. ✅ 创建 `Views/SettingsWindow.xaml`（设置窗口UI）
7. ✅ 创建 `Views/SettingsWindow.xaml.cs`（设置窗口代码）
8. ✅ 修改 `Views/MainWindow.xaml`（添加设置按钮）
9. ✅ 修改 `ViewModels/MainWindowVM.cs`（添加打开设置命令）
10. ✅ 测试配置保存和加载功能
