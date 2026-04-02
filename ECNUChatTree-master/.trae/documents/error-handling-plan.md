# 错误信息处理功能实现计划

## 一、现状分析

### 1.1 现有错误处理方式

| 文件 | 错误处理方式 | 问题 |
|------|-------------|------|
| `OpenAIChat.cs` | 返回错误字符串 | 用户看到原始异常信息，不够友好 |
| `ChatPersistenceService.cs` | Debug.WriteLine | 用户无法感知保存/加载失败 |
| `SettingsService.cs` | 静默失败 | 用户无法感知配置保存失败 |
| `ChatInformationVM.cs` | 显示"请求失败：{ex.Message}" | 仅显示异常消息，缺少分类 |
| `AsyncRelayCommand.cs` | try-finally | 异常被吞没，无通知 |

### 1.2 存在的问题

1. **错误信息不一致**：不同模块使用不同的错误处理策略
2. **用户不可见**：文件操作失败时用户无感知
3. **缺少异常分类**：网络错误、认证错误、文件错误混为一谈
4. **缺少重试机制**：临时性错误无法自动重试
5. **缺少全局处理**：未捕获异常会导致程序崩溃

---

## 二、设计方案

### 2.1 整体架构

```
新增文件结构：
├── Services/
│   └── NotificationService.cs   (通知服务，~60行)
├── Models/
│   └── ErrorInfo.cs             (错误信息模型，~40行)
└── Views/
    └── Controls/
        └── ToastNotification.xaml(.cs) (Toast控件)

修改文件（最小改动）：
├── App.xaml.cs                  (添加全局异常处理)
├── Services/OpenAIChat.cs       (使用通知服务)
├── ViewModels/ChatInformationVM.cs (使用通知服务)
```

### 2.2 错误分类

| 错误类型 | 场景 | 用户提示 |
|---------|------|---------|
| 网络错误 | API请求失败 | "网络连接失败，请检查网络后重试" |
| 认证错误 | API Key无效 | "API Key无效，请在设置中检查配置" |
| 文件错误 | 保存/加载失败 | "文件操作失败，请检查文件权限" |
| 配置错误 | 配置文件损坏 | "配置文件损坏，已恢复默认配置" |
| 未知错误 | 其他异常 | "发生未知错误，请稍后重试" |

---

## 三、详细实现步骤

### 步骤1：创建错误信息模型

#### 文件：`Models/ErrorInfo.cs`（约40行）

```csharp
/// <summary>
/// 错误信息类型枚举
/// </summary>
public enum ErrorType
{
    Network,      // 网络错误
    Authentication, // 认证错误
    File,         // 文件错误
    Configuration, // 配置错误
    Unknown       // 未知错误
}

/// <summary>
/// 错误信息模型，包含错误类型和用户友好的提示信息
/// </summary>
public class ErrorInfo
{
    public ErrorType Type { get; }
    public string UserMessage { get; }
    public string? TechnicalMessage { get; }
    public Exception? Exception { get; }

    /// <summary>
    /// 根据异常类型自动判断错误类型并创建ErrorInfo
    /// </summary>
    public static ErrorInfo FromException(Exception ex) { ... }

    /// <summary>
    /// 获取用户友好的错误提示信息
    /// </summary>
    public static string GetUserFriendlyMessage(ErrorType type) { ... }
}
```

---

### 步骤2：创建通知服务

#### 文件：`Services/NotificationService.cs`（约60行）

```csharp
/// <summary>
/// 通知服务，提供统一的消息通知机制
/// 支持错误、警告、成功三种通知类型
/// </summary>
public class NotificationService
{
    private static NotificationService? _instance;
    public static NotificationService Instance => _instance ??= new();

    /// <summary>
    /// 显示错误通知
    /// </summary>
    /// <param name="error">错误信息对象</param>
    public void ShowError(ErrorInfo error) { ... }

    /// <summary>
    /// 显示错误通知（简化版）
    /// </summary>
    /// <param name="message">错误消息</param>
    public void ShowError(string message) { ... }

    /// <summary>
    /// 显示警告通知
    /// </summary>
    public void ShowWarning(string message) { ... }

    /// <summary>
    /// 显示成功通知
    /// </summary>
    public void ShowSuccess(string message) { ... }

    /// <summary>
    /// 显示Toast通知（内部方法）
    /// </summary>
    private void ShowToast(string message, NotificationType type) { ... }
}
```

---

### 步骤3：创建Toast通知控件

#### 文件：`Views/Controls/ToastNotification.xaml`

```xml
<UserControl x:Class="TreeChat.Views.Controls.ToastNotification"
             ...>
    <Border Background="{Binding BackgroundColor}"
            CornerRadius="4"
            Padding="15,10"
            Margin="10">
        <StackPanel Orientation="Horizontal">
            <TextBlock Text="{Binding Icon}" FontSize="16" Margin="0,0,10,0"/>
            <TextBlock Text="{Binding Message}" VerticalAlignment="Center"/>
        </StackPanel>
    </Border>
</UserControl>
```

#### 文件：`Views/Controls/ToastNotification.xaml.cs`（约30行）

```csharp
/// <summary>
/// Toast通知控件，用于显示临时消息提示
/// </summary>
public partial class ToastNotification : UserControl
{
    public ToastNotification(string message, NotificationType type)
    {
        InitializeComponent();
        // 设置背景色和图标
    }
}
```

---

### 步骤4：添加全局异常处理

#### 修改：`App.xaml.cs`（+15行）

```csharp
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // 注册全局异常处理
        DispatcherUnhandledException += OnDispatcherUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;
        
        base.OnStartup(e);
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        NotificationService.Instance.ShowError(ErrorInfo.FromException(e.Exception));
        e.Handled = true;
    }

    // ... 其他异常处理方法
}
```

---

### 步骤5：修改现有服务（最小改动）

#### 修改：`Services/OpenAIChat.cs`（+5行）

```csharp
catch (HttpRequestException ex)
{
    var error = ErrorInfo.FromException(ex);
    NotificationService.Instance.ShowError(error);
    return error.UserMessage;
}
catch (Exception ex)
{
    var error = ErrorInfo.FromException(ex);
    NotificationService.Instance.ShowError(error);
    return error.UserMessage;
}
```

#### 修改：`Services/ChatPersistenceService.cs`（+3行）

```csharp
catch (Exception ex)
{
    NotificationService.Instance.ShowError(ErrorInfo.FromException(ex));
}
```

#### 修改：`ViewModels/ChatInformationVM.cs`（+3行）

```csharp
catch (Exception ex)
{
    var error = ErrorInfo.FromException(ex);
    NotificationService.Instance.ShowError(error);
    AIReply = error.UserMessage;
}
```

---

## 四、文件清单

### 新增文件（3个）
| 文件路径 | 预计行数 | 说明 |
|---------|---------|------|
| `Models/ErrorInfo.cs` | ~40行 | 错误信息模型 |
| `Services/NotificationService.cs` | ~60行 | 通知服务 |
| `Views/Controls/ToastNotification.xaml` | ~30行 | Toast控件UI |
| `Views/Controls/ToastNotification.xaml.cs` | ~30行 | Toast控件代码 |

### 修改文件（5个）
| 文件路径 | 改动量 | 说明 |
|---------|--------|------|
| `App.xaml.cs` | +20行 | 添加全局异常处理 |
| `Services/OpenAIChat.cs` | +5行 | 使用通知服务 |
| `Services/ChatPersistenceService.cs` | +3行 | 使用通知服务 |
| `Services/SettingsService.cs` | +3行 | 使用通知服务 |
| `ViewModels/ChatInformationVM.cs` | +3行 | 使用通知服务 |

---

## 五、数据流程

### 错误处理流程
```
异常发生
    ↓
ErrorInfo.FromException(ex)
    ↓
自动判断错误类型
    ↓
NotificationService.ShowError(error)
    ↓
显示Toast通知
    ↓
用户看到友好提示
```

### 全局异常处理流程
```
未捕获异常
    ↓
App.DispatcherUnhandledException
    ↓
NotificationService.ShowError()
    ↓
标记 e.Handled = true
    ↓
程序继续运行
```

---

## 六、用户友好提示对照表

| 异常类型 | 技术信息 | 用户提示 |
|---------|---------|---------|
| HttpRequestException | 网络请求失败 | 网络连接失败，请检查网络后重试 |
| TaskCanceledException | 请求超时 | 请求超时，请稍后重试 |
| UnauthorizedAccessException | 认证失败 | API Key无效，请在设置中检查配置 |
| FileNotFoundException | 文件不存在 | 数据文件不存在，已创建新文件 |
| JsonException | JSON解析失败 | 数据文件损坏，已恢复默认数据 |
| IOException | 文件读写错误 | 文件操作失败，请检查文件权限 |
| 其他异常 | ex.Message | 发生未知错误，请稍后重试 |

---

## 七、注意事项

1. **不改动原有核心逻辑**：只在catch块中添加通知调用
2. **遵循MVVM模式**：通知服务独立于ViewModel
3. **代码注释规范**：public方法需要注释
4. **文件长度控制**：每个文件不超过100行
5. **向后兼容**：保留原有的错误返回方式

---

## 八、实现顺序

1. ✅ 创建 `Models/ErrorInfo.cs`（错误信息模型）
2. ✅ 创建 `Services/NotificationService.cs`（通知服务）
3. ✅ 创建 `Views/Controls/ToastNotification.xaml(.cs)`（Toast控件）
4. ✅ 修改 `App.xaml.cs`（添加全局异常处理）
5. ✅ 修改 `Services/OpenAIChat.cs`（使用通知服务）
6. ✅ 修改 `Services/ChatPersistenceService.cs`（使用通知服务）
7. ✅ 修改 `Services/SettingsService.cs`（使用通知服务）
8. ✅ 修改 `ViewModels/ChatInformationVM.cs`（使用通知服务）
9. ✅ 测试错误处理功能
