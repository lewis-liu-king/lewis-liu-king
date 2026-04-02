# 流式输出功能实现计划

## 一、需求分析

### 1.1 问题描述
当前AI回复需要等待完整响应才能显示，用户体验较差。主流LLM应用都支持流式输出，实时显示AI回复内容。

### 1.2 功能目标
- 实时显示AI回复内容（打字机效果）
- 支持停止生成功能
- 不改动原有核心代码结构
- 保持MVVM架构

---

## 二、技术方案

### 2.1 流式输出原理
OpenAI兼容API支持`stream: true`参数，返回SSE（Server-Sent Events）格式的数据流：
```
data: {"choices":[{"delta":{"content":"你"}}]}
data: {"choices":[{"delta":{"content":"好"}}]}
data: [DONE]
```

### 2.2 实现架构

```
Services层：
├── OpenAIChat.cs          (修改：添加流式调用方法)
└── StreamingHelper.cs     (新增：流式响应解析)

ViewModels层：
└── ChatInformationVM.cs   (修改：处理流式更新)

Views层：
└── ChatInformationView.xaml (修改：添加停止按钮)
```

---

## 三、详细实现步骤

### 步骤1：创建流式响应解析器

#### 文件：`Services/StreamingHelper.cs`（约40行）

```csharp
/// <summary>
/// 流式响应解析器，解析SSE格式的数据流
/// </summary>
public static class StreamingHelper
{
    /// <summary>
    /// 解析SSE数据行，提取内容片段
    /// </summary>
    /// <param name="line">SSE数据行</param>
    /// <returns>内容片段，如果解析失败或结束则返回null</returns>
    public static string? ParseSseLine(string line)
    {
        if (string.IsNullOrEmpty(line)) return null;
        if (!line.StartsWith("data: ")) return null;
        
        var jsonPart = line.Substring(6);
        if (jsonPart == "[DONE]") return null;
        
        // 解析JSON获取delta.content
        // ...
    }
}
```

---

### 步骤2：修改OpenAIChat服务

#### 文件：`Services/OpenAIChat.cs`（添加流式方法）

```csharp
/// <summary>
/// 流式调用AI接口，通过回调实时返回内容
/// </summary>
/// <param name="context">完整上下文</param>
/// <param name="onChunk">内容片段回调</param>
/// <param name="cancellationToken">取消令牌</param>
/// <returns>完整的AI回复内容</returns>
public async Task<string> CallAiApiStreamAsync(
    List<ChatMessage> context, 
    Action<string> onChunk,
    CancellationToken cancellationToken = default)
{
    // 设置stream: true
    // 使用StreamReader逐行读取
    // 解析SSE数据并回调
    // ...
}
```

---

### 步骤3：修改ChatInformationVM

#### 文件：`ViewModels/ChatInformationVM.cs`（修改发送逻辑）

```csharp
private CancellationTokenSource? _cancellationTokenSource;
public bool IsGenerating { get; private set; }

public AsyncRelayCommand SendMessage { get; }
public RelayCommand StopGeneration { get; }

private async Task ExecuteSendMessageAsync(object? parameter)
{
    // 创建CancellationTokenSource
    // 调用流式API
    // 实时更新AIReply
    // ...
}

private void StopGenerationExecute(object? parameter)
{
    _cancellationTokenSource?.Cancel();
    IsGenerating = false;
}
```

---

### 步骤4：修改ChatInformationView

#### 文件：`Views/ChatInformationView.xaml`（添加停止按钮）

```xml
<!-- 发送按钮区域 -->
<Grid.ColumnDefinitions>
    <ColumnDefinition Width="*"/>
    <ColumnDefinition Width="80"/>
    <ColumnDefinition Width="80"/>  <!-- 新增停止按钮列 -->
</Grid.ColumnDefinitions>

<!-- 发送按钮 -->
<Button Grid.Column="1" Content="发送" Command="{Binding SendMessage}"
        Visibility="{Binding IsGenerating, Converter={StaticResource BoolToVisibilityConverter}, ConverterParameter=Invert}"/>

<!-- 停止按钮 -->
<Button Grid.Column="2" Content="停止" Command="{Binding StopGeneration}"
        Background="#D13438"
        Visibility="{Binding IsGenerating, Converter={StaticResource BoolToVisibilityConverter}}"/>
```

---

## 四、文件清单

### 新增文件（1个）
| 文件路径 | 预计行数 | 说明 |
|---------|---------|------|
| `Services/StreamingHelper.cs` | ~40行 | SSE解析器 |

### 修改文件（3个）
| 文件路径 | 改动量 | 说明 |
|---------|--------|------|
| `Services/OpenAIChat.cs` | +40行 | 添加流式调用方法 |
| `ViewModels/ChatInformationVM.cs` | +25行 | 处理流式更新和取消 |
| `Views/ChatInformationView.xaml` | +15行 | 添加停止按钮 |

---

## 五、数据流程

### 流式输出流程
```
用户点击"发送"
    ↓
创建CancellationTokenSource
    ↓
设置IsGenerating = true
    ↓
调用OpenAIChat.CallAiApiStreamAsync()
    ↓
┌─────────────────────────────┐
│  循环读取SSE数据流          │
│  ↓                          │
│  解析delta.content          │
│  ↓                          │
│  回调onChunk(content)       │
│  ↓                          │
│  UI线程更新AIReply          │
│  ↓                          │
│  检查CancellationToken      │
└─────────────────────────────┘
    ↓
流结束或取消
    ↓
设置IsGenerating = false
    ↓
保存完整回复到节点
```

---

## 六、注意事项

1. **线程安全**：UI更新需要在Dispatcher上执行
2. **取消处理**：正确处理取消令牌，释放资源
3. **错误处理**：网络中断时的友好提示
4. **向后兼容**：保留原有的非流式调用方法

---

## 七、实现顺序

1. ✅ 创建 `Services/StreamingHelper.cs`（SSE解析器）
2. ✅ 修改 `Services/OpenAIChat.cs`（添加流式方法）
3. ✅ 修改 `ViewModels/ChatInformationVM.cs`（处理流式更新）
4. ✅ 修改 `Views/ChatInformationView.xaml`（添加停止按钮）
5. ✅ 测试流式输出功能
