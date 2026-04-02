# 聊天管理功能增强实施计划

## 任务1：修复删除按钮灰色问题
- **优先级**：P0
- **依赖**：无
- **描述**：
  - 检查 DeleteChatCommand 的 CanExecute 方法
  - 确保命令能正确响应 SelectedChat 的变化
  - 测试删除按钮的可用性
- **成功标准**：
  - 删除按钮在选中聊天时可用，未选中时禁用
  - 按钮状态能实时更新
- **修改文件**：
  - [ChatManagementPanelVM.cs](file:///workspace/ECNUChatTree-master/TreeChat/ViewModels/ChatManagementPanelVM.cs)

## 任务2：修改对话名称生成逻辑
- **优先级**：P0
- **依赖**：无
- **描述**：
  - 修改 ChatInformationVM 中的标题生成逻辑
  - 改为使用最早的非根节点对话来生成标题
  - 确保标题生成只在第一次消息时触发
- **成功标准**：
  - 对话名称基于第一条用户消息生成
  - 后续消息不影响已生成的标题
- **修改文件**：
  - [ChatInformationVM.cs](file:///workspace/ECNUChatTree-master/TreeChat/ViewModels/ChatInformationVM.cs)

## 任务3：添加对话名称自定义修改功能
- **优先级**：P0
- **依赖**：任务4
- **描述**：
  - 在 ChatManagementPanelVM 中添加 EditChatTitleCommand
  - 实现编辑标题的方法
  - 添加保存修改后标题的逻辑
- **成功标准**：
  - 用户可以手动编辑对话名称
  - 修改后的标题正确保存和显示
- **修改文件**：
  - [ChatManagementPanelVM.cs](file:///workspace/ECNUChatTree-master/TreeChat/ViewModels/ChatManagementPanelVM.cs)

## 任务4：改进UI设计（添加三点菜单）
- **优先级**：P0
- **依赖**：任务3
- **描述**：
  - 修改 ChatManagementPanel.xaml
  - 为每个对话项添加三点菜单按钮
  - 实现上下文菜单，包含删除和编辑名称功能
  - 调整布局，确保名称左对齐，菜单右对齐
- **成功标准**：
  - 每个对话项显示为长条形，左侧是名称（左对齐），右侧是三点菜单
  - 点击三点菜单显示包含删除和编辑名称的选项
  - 菜单功能正常工作
- **修改文件**：
  - [ChatManagementPanel.xaml](file:///workspace/ECNUChatTree-master/TreeChat/Views/ChatManagementPanel.xaml)
  - [ChatManagementPanel.xaml.cs](file:///workspace/ECNUChatTree-master/TreeChat/Views/ChatManagementPanel.xaml.cs)

## 任务5：测试和验证
- **优先级**：P1
- **依赖**：任务1-4
- **描述**：
  - 测试所有新增和修改的功能
  - 确保删除按钮正常工作
  - 验证对话名称生成逻辑正确
  - 测试编辑名称功能
  - 验证UI布局和交互
- **成功标准**：
  - 所有功能正常工作
  - UI符合设计要求
  - 代码符合规范

## 技术实现细节

### 任务1：修复删除按钮灰色问题
- 在 ChatManagementPanelVM 的 SelectedChat setter 中添加命令状态更新
- 确保 DeleteChatCommand.RaiseCanExecuteChanged() 在 SelectedChat 变化时被调用

### 任务2：修改对话名称生成逻辑
- 保持现有的 `_hasGeneratedTitle` 标志
- 确保标题只在第一条消息时生成，后续消息不影响

### 任务3：添加对话名称自定义修改功能
- 添加 RelayCommand EditChatTitleCommand
- 实现 ShowEditTitleDialog 方法，使用 InputBox 或自定义对话框
- 确保修改后的标题正确保存到 ChatTree 和持久化存储

### 任务4：改进UI设计
- 使用 Grid 布局替代 TextBlock，实现左右布局
- 在右侧添加三点菜单按钮（使用 Ellipsis 字符或图标）
- 为菜单按钮添加 ContextMenu，包含删除和编辑名称选项
- 绑定菜单命令到对应的 ViewModel 命令

## 代码规范要求
- 所有 public 方法需要添加 XML 注释，说明用途和使用方法
- 保持代码风格与现有代码一致
- 尽量不改动原有代码结构，只添加必要的功能
- 每个类都需要类级别的注释说明其功能
- 代码行数控制在合理范围内，单个文件尽量不超过 100 行（包括注释）

## 预期交付物
- 修复删除按钮的可用性问题
- 对话名称基于最早的非根节点对话生成
- 支持手动编辑对话名称
- 改进的UI设计，包含三点菜单
- 所有功能的测试验证
- 代码符合规范要求