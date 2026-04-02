# 修复完整三点菜单 Bug 计划

## 问题分析

**Bug 1：选中项错误**
- 点击 b 的三点按钮时，菜单操作仍然作用于 a
- 原因：点击三点按钮时没有更新 `SelectedChat`

**Bug 2：ContextMenu 绑定失效**
- ContextMenu 不在可视化树中，无法通过 `RelativeSource` 找到父元素
- 需要使用代理元素或 `PlacementTarget` 来正确绑定

**Bug 3：代码被意外删除**
- 在提交 fc9b8d4 和后续提交中，部分代码被意外删除或修改
- 需要恢复完整的功能

## 修复方案

### 任务 1：保持 ChatManagementPanelVM.cs 完整（已完成）
- [x] 恢复 cc4c816 中的完整代码
- [x] 包含 DeleteChatCommand 和 EditChatTitleCommand

### 任务 2：修复 ChatManagementPanel.xaml 中的 ContextMenu 绑定
- [x] 正确绑定 ContextMenu 的命令到 ViewModel
- [x] 使用 ProxyElement 方式

### 任务 3：修复 ChatManagementPanel.xaml.cs 中的选中逻辑
- [x] 在点击三点按钮时正确选中对应的 ChatTree
- [x] 确保 SelectedChat 正确更新

## 文件变更

| 文件 | 变更内容 |
|------|---------|
| `ChatManagementPanelVM.cs` | 已恢复完整代码 |
| `ChatManagementPanel.xaml` | 修复 ContextMenu 绑定 |
| `ChatManagementPanel.xaml.cs` | 修复选中项逻辑 |
