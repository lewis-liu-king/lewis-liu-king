# 树节点显示优化 - 详细实现计划

## 一、需求确认

根据讨论，需要实现以下功能：
1. **节点内容预览** - 显示用户消息前N个字符
2. **悬停显示完整内容** - Tooltip显示完整对话
3. **颜色编码** - 区分有回复/无回复节点
4. **节点尺寸自适应** - 内容过长时显示滚动条
5. **颜色编码模式选项** - 可选择开启/关闭

---

## 二、技术方案设计

### 2.1 节点尺寸调整

**当前**：固定 40x30 像素
**调整后**：宽度自适应，最大宽度限制

```
最小宽度：60px（显示至少4-6个汉字）
最大宽度：150px
高度：根据内容行数自适应（单行30px，多行最大60px）
```

### 2.2 内容显示策略

| 内容类型 | 显示方式 |
|---------|---------|
| 用户消息 | 节点内显示前12个字符 |
| AI回复 | Tooltip中显示 |
| 完整内容 | Tooltip悬停显示 |

### 2.3 颜色编码规则

| 节点状态 | 边框颜色 | 背景颜色 |
|---------|---------|---------|
| 无AI回复 | Gray | #DCE6F0（浅蓝） |
| 有AI回复 | Green | #D4EDDA（浅绿） |
| 选中节点 | Blue | #FFF3CD（浅黄） |

---

## 三、详细实现步骤

### 步骤1：修改 TreeNodeVM.cs

**改动内容**：
1. 修改 `DisplayContent` 属性 - 显示用户消息前12字符
2. 新增 `FullContent` 属性 - 用于Tooltip
3. 新增 `HasReply` 属性 - 用于颜色判断
4. 新增 `NodeWidth` 属性 - 动态宽度
5. 新增 `NodeHeight` 属性 - 动态高度

```csharp
/// <summary>
/// 节点显示内容（用户消息前12字符）
/// </summary>
public string DisplayContent
{
    get
    {
        var content = Node.UserMessage.Content;
        if (string.IsNullOrEmpty(content)) return Node.NodeID.ToString();
        return content.Length > 12 ? content.Substring(0, 12) + "..." : content;
    }
}

/// <summary>
/// 完整内容（用于Tooltip显示）
/// </summary>
public string FullContent
{
    get
    {
        var sb = new StringBuilder();
        sb.AppendLine($"用户: {Node.UserMessage.Content}");
        if (Node.ReplyMessage != null)
            sb.AppendLine($"AI: {Node.ReplyMessage.Content}");
        return sb.ToString();
    }
}

/// <summary>
/// 是否有AI回复
/// </summary>
public bool HasReply => Node.ReplyMessage != null && !string.IsNullOrEmpty(Node.ReplyMessage.Content);

/// <summary>
/// 节点宽度（根据内容自适应）
/// </summary>
public double NodeWidth => Math.Min(150, Math.Max(60, DisplayContent.Length * 12));
```

---

### 步骤2：修改 TreeVisualizationView.xaml.cs 的 DrawNodes 方法

**改动内容**：
1. 使用动态宽高
2. 添加Tooltip绑定
3. 添加颜色编码逻辑

```csharp
private void DrawNodes(TreeNodeVM rootNode)
{
    // 创建节点UI - 使用动态尺寸
    var nodeBorder = new Border
    {
        Width = rootNode.NodeWidth,
        Height = TreeNodeVM.HEIGHT,
        Background = GetNodeBackground(rootNode),
        BorderBrush = GetNodeBorderBrush(rootNode),
        BorderThickness = new Thickness(1),
        CornerRadius = new CornerRadius(3),
        Cursor = Cursors.Hand,
        ToolTip = rootNode.FullContent  // 添加Tooltip
    };

    // 节点内容 - 使用TextBlock支持换行
    var textBlock = new TextBlock
    {
        Text = rootNode.DisplayContent,
        TextWrapping = TextWrapping.Wrap,
        TextAlignment = TextAlignment.Center,
        VerticalAlignment = VerticalAlignment.Center,
        Margin = new Thickness(5),
        FontSize = 11
    };
    nodeBorder.Child = textBlock;
    
    // ... 其余代码不变
}

/// <summary>
/// 根据节点状态获取背景颜色
/// </summary>
private Brush GetNodeBackground(TreeNodeVM node)
{
    if (node.HasReply)
        return new SolidColorBrush(Color.FromRgb(212, 237, 218)); // 浅绿
    return new SolidColorBrush(Color.FromRgb(220, 230, 240)); // 浅蓝
}

/// <summary>
/// 根据节点状态获取边框颜色
/// </summary>
private Brush GetNodeBorderBrush(TreeNodeVM node)
{
    if (node.HasReply)
        return Brushes.Green;
    return Brushes.Gray;
}
```

---

### 步骤3：修改 HighlightSelectedNode 方法

**改动内容**：选中节点使用不同颜色

```csharp
private void HighlightSelectedNode(TreeNodeVM node)
{
    foreach (var element in _nodeElements.Values)
    {
        if (element is Border border)
        {
            // 根据节点状态恢复默认颜色
            var nodeId = _nodeElements.FirstOrDefault(x => x.Value == border).Key;
            var nodeVM = FindNodeVM(nodeId);
            if (nodeVM != null)
            {
                border.BorderBrush = GetNodeBorderBrush(nodeVM);
                border.Background = GetNodeBackground(nodeVM);
            }
        }
    }

    // 高亮选中节点
    if (_nodeElements.TryGetValue(node.ID, out var selectedElement) && selectedElement is Border selectedBorder)
    {
        selectedBorder.BorderBrush = Brushes.Blue;
        selectedBorder.Background = new SolidColorBrush(Color.FromRgb(255, 243, 205)); // 浅黄
    }
}
```

---

### 步骤4：添加颜色编码模式选项（可选）

**新增类**：`ViewModels/TreeDisplayOptionsVM.cs`

```csharp
/// <summary>
/// 树显示选项ViewModel，管理显示模式设置
/// </summary>
public class TreeDisplayOptionsVM : BaseViewModel
{
    private bool _useColorCoding = true;

    /// <summary>
    /// 是否启用颜色编码
    /// </summary>
    public bool UseColorCoding
    {
        get => _useColorCoding;
        set => SetProperty(ref _useColorCoding, value);
    }
}
```

---

## 四、文件清单

### 修改文件（2个）

| 文件路径 | 改动量 | 说明 |
|---------|--------|------|
| `ViewModels/TreeNodeVM.cs` | +25行 | 添加新属性 |
| `Views/TreeVisualizationView.xaml.cs` | +30行 | 修改绘制逻辑 |

### 新增文件（可选）

| 文件路径 | 预计行数 | 说明 |
|---------|---------|------|
| `ViewModels/TreeDisplayOptionsVM.cs` | ~30行 | 显示选项管理 |

---

## 五、数据流程

### 节点渲染流程
```
TreeNodeVM.DisplayContent
    ↓
获取用户消息前12字符
    ↓
TreeVisualizationView.DrawNodes()
    ↓
创建Border + TextBlock
    ↓
设置Tooltip = FullContent
    ↓
设置颜色 = HasReply ? Green : Gray
```

### 选中节点流程
```
用户点击节点
    ↓
SelectedNode = node
    ↓
HighlightSelectedNode()
    ↓
重置所有节点颜色
    ↓
高亮选中节点（蓝色边框+黄色背景）
```

---

## 六、注意事项

1. **不改动原有核心逻辑**：只在现有方法中添加功能
2. **保持MVVM模式**：新属性放在ViewModel中
3. **代码注释规范**：public方法需要注释
4. **文件长度控制**：每个文件不超过100行
5. **向后兼容**：保留原有的节点ID显示逻辑作为备选

---

## 七、实现顺序

1. ✅ 修改 `TreeNodeVM.cs` - 添加新属性
2. ✅ 修改 `TreeVisualizationView.xaml.cs` - 修改DrawNodes方法
3. ✅ 修改 `TreeVisualizationView.xaml.cs` - 修改HighlightSelectedNode方法
4. ✅ 测试节点显示效果
5. ✅ （可选）添加颜色编码模式选项

---

## 八、效果预览

### 改造前
```
┌────┐
│ 1  │  ← 只显示数字编号
└────┘
```

### 改造后
```
┌──────────────┐
│ 你好，请帮我... │  ← 显示用户消息预览
└──────────────┘
    ↓ 悬停显示
┌────────────────────┐
│ 用户: 你好，请帮我... │
│ AI: 你好！我是...    │
└────────────────────┘

颜色编码：
- 无回复：灰色边框 + 浅蓝背景
- 有回复：绿色边框 + 浅绿背景
- 选中：蓝色边框 + 浅黄背景
```
