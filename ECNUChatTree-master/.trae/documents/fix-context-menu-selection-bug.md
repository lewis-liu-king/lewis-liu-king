# 修复三点菜单选中项Bug计划

## 问题描述

用户点击某个聊天项（如b）的三点按钮后，菜单中的"删除"和"编辑名称"操作作用于之前选中的项（如a），而不是用户点击的那个项（b）。

## 问题分析

### 当前代码流程

1. 用户选中聊天项 a → `SelectedChat = a`
2. 用户点击聊天项 b 的三点按钮 → 触发 `MoreButton_Click`
3. `MoreButton_Click` 只是显示 ContextMenu，**没有改变 SelectedChat**
4. 此时 `SelectedChat` 仍然是 a
5. 用户点击菜单中的"删除"或"编辑名称"
6. 命令执行的是对 `SelectedChat`（即 a）的操作

### 根本原因

`MoreButton_Click` 事件处理程序只负责显示 ContextMenu，没有将点击的项设置为选中状态。ViewModel 中的 `ExecuteDeleteChat` 和 `ExecuteEditChatTitle` 方法都是基于 `SelectedChat` 属性来操作的。

### 代码位置

- **XAML**: `/workspace/ECNUChatTree-master/TreeChat/Views/ChatManagementPanel.xaml`
  - 第37行：三点按钮的 Click 事件
  
- **Code-behind**: `/workspace/ECNUChatTree-master/TreeChat/Views/ChatManagementPanel.xaml.cs`
  - 第25-32行：`MoreButton_Click` 方法
  
- **ViewModel**: `/workspace/ECNUChatTree-master/TreeChat/ViewModels/ChatManagementPanelVM.cs`
  - 第112-127行：`ExecuteDeleteChat` 方法操作 `SelectedChat`
  - 第140-153行：`ExecuteEditChatTitle` 方法操作 `SelectedChat`

## 解决方案

在 `MoreButton_Click` 方法中，显示 ContextMenu 之前，先将点击按钮对应的聊天项设置为选中状态。

### 修改 ChatManagementPanel.xaml.cs

```csharp
private void MoreButton_Click(object sender, RoutedEventArgs e)
{
    if (sender is Button button && button.ContextMenu != null)
    {
        // 找到按钮所在的 ListBoxItem 并选中它
        var listBoxItem = FindParent<ListBoxItem>(button);
        if (listBoxItem != null)
        {
            listBoxItem.IsSelected = true;
        }
        
        button.ContextMenu.PlacementTarget = button;
        button.ContextMenu.IsOpen = true;
    }
}

private T? FindParent<T>(DependencyObject child) where T : DependencyObject
{
    DependencyObject parentObject = VisualTreeHelper.GetParent(child);
    
    if (parentObject == null) return null;
    
    if (parentObject is T parent)
        return parent;
    else
        return FindParent<T>(parentObject);
}
```

## 验证步骤

1. 运行应用程序
2. 创建多个聊天项（如 a、b、c）
3. 选中聊天项 a
4. 点击聊天项 b 的三点按钮
5. 验证 b 是否被选中（高亮）
6. 点击菜单中的"编辑名称"，验证是否弹出编辑对话框针对 b
7. 点击菜单中的"删除"，验证是否删除的是 b

## 文件变更

| 文件 | 变更内容 |
|------|---------|
| `ChatManagementPanel.xaml.cs` | 添加 `FindParent<T>` 辅助方法，修改 `MoreButton_Click` 方法 |
