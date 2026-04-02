# 节点搜索功能实现计划

## 一、需求分析

### 1.1 功能目标
- 提供搜索框，支持关键词搜索
- 搜索范围：用户消息 + AI回复
- 匹配节点高亮显示
- 支持跳转到匹配节点

### 1.2 现有代码分析

| 文件 | 现有功能 | 可利用点 |
|------|---------|---------|
| `TreeVisualizationVM.cs` | 管理树节点和选中状态 | 可添加搜索方法 |
| `TreeNodeVM.cs` | 节点数据 | 可添加匹配状态属性 |
| `TreeVisualizationView.xaml.cs` | 绘制节点 | 可添加高亮逻辑 |
| `MainWindow.xaml` | 布局 | 可添加搜索框UI |

---

## 二、技术方案设计

### 2.1 整体架构

```
新增/修改文件：
├── ViewModels/
│   └── TreeVisualizationVM.cs   (修改：添加搜索方法)
├── ViewModels/
│   └── TreeNodeVM.cs            (修改：添加IsMatched属性)
├── Views/
│   └── TreeVisualizationView.xaml (修改：添加搜索框UI)
└── Views/
    └── TreeVisualizationView.xaml.cs (修改：添加高亮逻辑)
```

### 2.2 搜索流程

```
用户输入关键词
    ↓
TreeVisualizationVM.SearchNodes(keyword)
    ↓
遍历所有节点，检查消息内容
    ↓
设置节点 IsMatched = true/false
    ↓
触发重绘，高亮匹配节点
    ↓
选中第一个匹配节点
```

### 2.3 UI布局

```
┌─────────────────────────────────────┐
│ [🔍 搜索框...] [上一个] [下一个]     │  ← 新增搜索栏
├─────────────────────────────────────┤
│                                     │
│           树可视化区域               │
│                                     │
└─────────────────────────────────────┘
```

---

## 三、详细实现步骤

### 步骤1：修改 TreeNodeVM.cs

**改动内容**：添加 `IsMatched` 属性

```csharp
private bool _isMatched;
/// <summary>
/// 是否匹配搜索条件（用于高亮显示）
/// </summary>
public bool IsMatched
{
    get => _isMatched;
    set => SetProperty(ref _isMatched, value);
}

/// <summary>
/// 检查节点是否包含指定关键词
/// </summary>
/// <param name="keyword">搜索关键词</param>
/// <returns>是否匹配</returns>
public bool ContainsKeyword(string keyword)
{
    if (string.IsNullOrEmpty(keyword)) return false;
    var lowerKeyword = keyword.ToLower();
    
    if (Node.UserMessage.Content?.ToLower().Contains(lowerKeyword) == true)
        return true;
    if (Node.ReplyMessage?.Content?.ToLower().Contains(lowerKeyword) == true)
        return true;
    return false;
}
```

---

### 步骤2：修改 TreeVisualizationVM.cs

**改动内容**：添加搜索方法和导航方法

```csharp
private string _searchKeyword = string.Empty;
private List<TreeNodeVM> _matchedNodes = new();
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
/// 当前匹配索引（用于显示"第X个/共Y个"）
/// </summary>
public int CurrentMatchIndex => _currentMatchIndex + 1;

/// <summary>
/// 匹配节点总数
/// </summary>
public int MatchCount => _matchedNodes.Count;

/// <summary>
/// 搜索节点并高亮匹配结果
/// </summary>
/// <param name="keyword">搜索关键词</param>
public void SearchNodes(string keyword)
{
    _matchedNodes.Clear();
    _currentMatchIndex = -1;
    
    if (string.IsNullOrEmpty(keyword) || RootNode == null)
    {
        ClearAllMatches(RootNode);
        CanvasPropertyChanged?.Invoke();
        return;
    }
    
    FindMatches(RootNode, keyword);
    CanvasPropertyChanged?.Invoke();
    
    if (_matchedNodes.Count > 0)
        NavigateToMatch(0);
}

/// <summary>
/// 跳转到下一个匹配节点
/// </summary>
public void NavigateNext()
{
    if (_matchedNodes.Count == 0) return;
    _currentMatchIndex = (_currentMatchIndex + 1) % _matchedNodes.Count;
    SelectedNode = _matchedNodes[_currentMatchIndex];
}

/// <summary>
/// 跳转到上一个匹配节点
/// </summary>
public void NavigatePrevious()
{
    if (_matchedNodes.Count == 0) return;
    _currentMatchIndex = (_currentMatchIndex - 1 + _matchedNodes.Count) % _matchedNodes.Count;
    SelectedNode = _matchedNodes[_currentMatchIndex];
}

private void FindMatches(TreeNodeVM node, string keyword)
{
    node.IsMatched = node.ContainsKeyword(keyword);
    if (node.IsMatched)
        _matchedNodes.Add(node);
    
    foreach (var child in node.Children)
        FindMatches(child, keyword);
}

private void ClearAllMatches(TreeNodeVM? node)
{
    if (node == null) return;
    node.IsMatched = false;
    foreach (var child in node.Children)
        ClearAllMatches(child);
}
```

---

### 步骤3：修改 TreeVisualizationView.xaml

**改动内容**：在树可视化区域上方添加搜索栏

```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto"/>
        <RowDefinition Height="*"/>
    </Grid.RowDefinitions>
    
    <!-- 搜索栏 -->
    <StackPanel Grid.Row="0" Orientation="Horizontal" Margin="5">
        <TextBox Width="150" Text="{Binding SearchKeyword, UpdateSourceTrigger=PropertyChanged}"
                 KeyDown="SearchBox_KeyDown" Margin="0,0,5,0"/>
        <Button Content="上一个" Command="{Binding NavigatePreviousCommand}" Width="50" Margin="2,0"/>
        <Button Content="下一个" Command="{Binding NavigateNextCommand}" Width="50" Margin="2,0"/>
        <TextBlock Text="{Binding MatchInfo}" VerticalAlignment="Center" Margin="5,0,0,0"/>
    </StackPanel>
    
    <!-- 树画布 -->
    <ScrollViewer Grid.Row="1" ...>
        ...
    </ScrollViewer>
</Grid>
```

---

### 步骤4：修改 TreeVisualizationView.xaml.cs

**改动内容**：修改绘制逻辑，高亮匹配节点

```csharp
private Brush GetNodeBackground(TreeNodeVM node)
{
    if (node.IsMatched)
        return new SolidColorBrush(Color.FromRgb(255, 255, 150)); // 黄色高亮
    if (node.HasReply)
        return new SolidColorBrush(Color.FromRgb(212, 237, 218));
    return new SolidColorBrush(Color.FromRgb(220, 230, 240));
}

private Brush GetNodeBorderBrush(TreeNodeVM node)
{
    if (node.IsMatched)
        return Brushes.Orange; // 橙色边框
    if (node.HasReply)
        return Brushes.Green;
    return Brushes.Gray;
}
```

---

## 四、文件清单

### 修改文件（4个）

| 文件路径 | 改动量 | 说明 |
|---------|--------|------|
| `ViewModels/TreeNodeVM.cs` | +15行 | 添加IsMatched、ContainsKeyword |
| `ViewModels/TreeVisualizationVM.cs` | +40行 | 添加搜索和导航方法 |
| `Views/TreeVisualizationView.xaml` | +15行 | 添加搜索栏UI |
| `Views/TreeVisualizationView.xaml.cs` | +5行 | 修改高亮逻辑 |

---

## 五、数据流程

### 搜索流程
```
用户输入"你好"
    ↓
SearchKeyword 属性变化
    ↓
SearchNodes("你好") 被调用
    ↓
遍历所有节点，检查ContainsKeyword
    ↓
设置 IsMatched = true/false
    ↓
重绘画布，匹配节点显示黄色背景
    ↓
选中第一个匹配节点
```

### 导航流程
```
用户点击"下一个"
    ↓
NavigateNext() 被调用
    ↓
_currentMatchIndex++
    ↓
SelectedNode = _matchedNodes[_currentMatchIndex]
    ↓
视图滚动到选中节点
```

---

## 六、颜色编码规则（更新）

| 节点状态 | 边框颜色 | 背景颜色 |
|---------|---------|---------|
| 匹配搜索 | Orange | #FFFF96（黄色） |
| 无AI回复 | Gray | #DCE6F0（浅蓝） |
| 有AI回复 | Green | #D4EDDA（浅绿） |
| 选中节点 | Blue | #FFF3CD（浅黄） |

---

## 七、注意事项

1. **不改动原有核心逻辑**：只在现有方法中添加功能
2. **保持MVVM模式**：搜索逻辑放在ViewModel中
3. **代码注释规范**：public方法需要注释
4. **文件长度控制**：每个文件不超过100行
5. **搜索性能**：使用简单的字符串包含匹配，不区分大小写

---

## 八、实现顺序

1. ✅ 修改 `TreeNodeVM.cs` - 添加IsMatched属性和ContainsKeyword方法
2. ✅ 修改 `TreeVisualizationVM.cs` - 添加搜索和导航方法
3. ✅ 修改 `TreeVisualizationView.xaml` - 添加搜索栏UI
4. ✅ 修改 `TreeVisualizationView.xaml.cs` - 修改高亮逻辑
5. ✅ 测试搜索功能

---

## 九、效果预览

```
┌─────────────────────────────────────────┐
│ [🔍 你好] [上一个] [下一个] 第1个/共3个  │
├─────────────────────────────────────────┤
│                                         │
│     ┌──────────────┐                    │
│     │ 你好，请帮我... │ ← 黄色高亮       │
│     └──────────────┘                    │
│           │                             │
│     ┌──────────────┐                    │
│     │ 你好！我是...  │ ← 黄色高亮        │
│     └──────────────┘                    │
│                                         │
└─────────────────────────────────────────┘
```
