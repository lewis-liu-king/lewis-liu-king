# ECNUChatTree 项目改进计划

## 一、项目现状分析

### 1.1 项目定位

这是一个**树形AI聊天工具**，核心特色是将对话组织成多叉树结构，允许用户在任意节点分支探索不同的对话路径。这是一个学习型项目，用于掌握C#、XAML、大模型API调用、多叉树数据结构、MVVM设计模式。

### 1.2 现有功能

| 功能模块     | 状态   | 说明             |
| -------- | ---- | -------------- |
| 多对话管理    | ✅ 已有 | 创建新聊天、切换聊天     |
| 树形对话结构   | ✅ 已有 | 多叉树，支持分支       |
| 树可视化     | ✅ 已有 | Canvas绘制节点和连接线 |
| 大模型API调用 | ✅ 已有 | 支持ECNU API     |
| 保存和加载    | ✅ 已有 | JSON格式持久化      |

### 1.3 Plan.md中待改善点

* [x] 保存和加载功能

* [ ] 配置文件自定义

* [ ] 错误信息处理功能

* [ ] 现有视图功能的完善

* [ ] 界面美化工作

***

## 二、与市面LLM应用对比分析

### 2.1 主流LLM应用功能清单

| 功能类别             | ChatGPT | Claude | Poe | 本项目 |
| ---------------- | ------- | ------ | --- | --- |
| 流式输出             | ✅       | ✅      | ✅   | ❌   |
| Markdown渲染       | ✅       | ✅      | ✅   | ❌   |
| 代码高亮             | ✅       | ✅      | ✅   | ❌   |
| 复制消息             | ✅       | ✅      | ✅   | ❌   |
| 编辑消息             | ✅       | ✅      | ✅   | ❌   |
| 重新生成             | ✅       | ✅      | ✅   | ❌   |
| 多模型切换            | ✅       | ❌      | ✅   | ❌   |
| System Prompt自定义 | ✅       | ✅      | ✅   | ❌   |
| 对话导出             | ✅       | ✅      | ✅   | ❌   |
| 历史搜索             | ✅       | ✅      | ✅   | ❌   |
| 深色模式             | ✅       | ✅      | ✅   | ❌   |
| 快捷键              | ✅       | ✅      | ✅   | ❌   |
| 树形对话             | ❌       | ❌      | ❌   | ✅   |

### 2.2 本项目独特优势

* **树形对话结构**：这是最大的差异化特点，允许用户探索不同的对话分支

* **可视化树结构**：直观展示对话路径

***

## 三、改进功能清单

### 优先级说明

* 🔴 **高优先级**：核心体验提升，建议优先实现

* 🟡 **中优先级**：重要功能，显著提升用户体验

* 🟢 **低优先级**：锦上添花，时间充裕时实现

***

### 第一阶段：基础体验完善（高优先级）

#### 1. 流式输出 🔴

**问题**：当前需要等待AI完整回复才能看到结果，体验较差
**方案**：

* 使用SSE（Server-Sent Events）或流式API

* 实时显示AI回复内容

* 添加停止生成按钮

**涉及文件**：

* `Services/OpenAIChat.cs` - 添加流式请求方法

* `ViewModels/ChatInformationVM.cs` - 处理流式更新

* `Views/ChatInformationView.xaml` - 添加停止按钮

***

#### 2. Markdown渲染 🔴

**问题**：AI回复的Markdown格式无法正确显示（代码块、列表、标题等）
**方案**：

* 引入Markdown渲染库（如Markdig）

* 支持代码块语法高亮

* 支持表格、列表等格式

**涉及文件**：

* 新增 `Controls/MarkdownTextBlock.cs`

* 修改 `Views/ChatInformationView.xaml`

***

#### 3. 配置文件自定义 🔴

**问题**：API Key等配置硬编码在代码中
**方案**：

* 创建 `appsettings.json` 配置文件

* 支持自定义API端点、模型名称、参数

* 支持多个API配置切换

**涉及文件**：

* 新增 `appsettings.json`

* 新增 `Services/ConfigurationService.cs`

* 修改 `Services/ApiConfig.cs`

***

#### 4. 错误处理优化 🔴

**问题**：错误信息不够友好，缺乏重试机制
**方案**：

* 友好的错误提示（Toast通知）

* 网络错误自动重试

* API限流处理

**涉及文件**：

* 新增 `Services/NotificationService.cs`

* 修改 `ViewModels/ChatInformationVM.cs`

***

### 第二阶段：对话功能增强（中优先级）

#### 5. 消息操作功能 🟡

**功能列表**：

* 复制消息内容

* 编辑已发送的消息（重新提问）

* 重新生成AI回复

* 删除消息节点

**涉及文件**：

* 修改 `Views/ChatInformationView.xaml`

* 修改 `ViewModels/ChatInformationVM.cs`

* 修改 `Models/ChatTreeNode.cs`

***

#### 6. System Prompt自定义 🟡

**问题**：当前System Prompt固定为"你是一个有帮助的AI助手"
**方案**：

* 创建对话时可自定义System Prompt

* 支持预设Prompt模板

* 支持修改已有对话的System Prompt

**涉及文件**：

* 修改 `Models/ChatTree.cs`

* 修改 `ViewModels/ChatManagementPanelVM.cs`

* 新增 `Views/SystemPromptDialog.xaml`

***

#### 7. 对话标题自动生成 🟡

**问题**：所有对话默认标题为"新对话"，难以区分
**方案**：

* 根据首次对话内容自动生成标题

* 支持手动编辑标题

* 支持重命名对话

**涉及文件**：

* 修改 `ViewModels/ChatManagementPanelVM.cs`

* 新增 `Services/TitleGenerator.cs`

***

#### 8. 对话导出功能 🟡

**方案**：

* 导出为Markdown文件

* 导出为JSON格式（完整树结构）

* 导出当前路径对话

**涉及文件**：

* 新增 `Services/ExportService.cs`

* 修改 `ViewModels/ChatManagementPanelVM.cs`

***

### 第三阶段：树形功能深化（中优先级）

#### 9. 节点操作增强 🟡

**功能列表**：

* 节点重命名（自定义节点显示名称）

* 节点删除（删除分支）

* 节点合并（合并两个分支）

* 节点标注（添加备注）

**涉及文件**：

* 修改 `Models/ChatTreeNode.cs`

* 修改 `ViewModels/TreeNodeVM.cs`

* 修改 `Views/TreeVisualizationView.xaml`

***

#### 10. 路径管理功能 🟡

**功能列表**：

* 路径命名（给分支起名）

* 路径比较（对比不同分支的对话）

* 路径导出（导出特定路径）

* 路径收藏（标记重要路径）

**涉及文件**：

* 新增 `Models/ChatPath.cs`

* 新增 `ViewModels/PathManagementVM.cs`

***

#### 11. 树可视化增强 🟡

**功能列表**：

* 节点缩略内容预览

* 节点颜色标记

* 节点搜索定位

* 树结构缩放

**涉及文件**：

* 修改 `Views/TreeVisualizationView.xaml`

* 修改 `Services/TreeLayoutService.cs`

***

### 第四阶段：用户体验优化（低优先级）

#### 12. 界面美化 🟢

**方案**：

* 使用现代UI框架（如MaterialDesignInXaml）

* 深色模式支持

* 动画效果

* 响应式布局

***

#### 13. 快捷键支持 🟢

**功能列表**：

* `Ctrl+N` 新建对话

* `Ctrl+S` 保存

* `Enter` 发送消息

* `Ctrl+Enter` 换行

***

#### 14. 历史搜索 🟢

**方案**：

* 全文搜索所有对话

* 按日期筛选

* 按关键词筛选

***

#### 15. 多模型支持 🟢

**方案**：

* 支持切换不同模型

* 支持自定义API端点

* 支持OpenAI、Claude等主流API

***

#### 16. 多语言支持 🟢

**方案**：

* 中英文界面切换

* 使用资源文件管理文本

***

## 四、推荐实现顺序

```
第一阶段（核心体验）
├── 1. 流式输出
├── 2. Markdown渲染  
├── 3. 配置文件自定义
└── 4. 错误处理优化

第二阶段（对话增强）
├── 5. 消息操作功能
├── 6. System Prompt自定义
├── 7. 对话标题自动生成
└── 8. 对话导出功能

第三阶段（树形深化）
├── 9. 节点操作增强
├── 10. 路径管理功能
└── 11. 树可视化增强

第四阶段（体验优化）
├── 12. 界面美化
├── 13. 快捷键支持
├── 14. 历史搜索
├── 15. 多模型支持
└── 16. 多语言支持
```

***

## 五、技术实现建议

### 5.1 推荐NuGet包

| 功能         | 推荐包                                       |
| ---------- | ----------------------------------------- |
| Markdown渲染 | `Markdig` + `Markdig.Wpf`                 |
| 代码高亮       | `AvalonEdit` 或 `Highlight`                |
| JSON配置     | `Microsoft.Extensions.Configuration.Json` |
| HTTP请求     | `System.Net.Http`（已有）                     |
| MVVM框架     | `CommunityToolkit.Mvvm`（可选）               |
| UI框架       | `MaterialDesignThemes`（可选）                |

### 5.2 架构建议

* 保持MVVM模式

* 服务层独立，便于测试

* 使用依赖注入（可选）

* 配置与代码分离

***

## 六、总结

本项目最大的特色是**树形对话结构**，建议在完善基础功能的同时，重点深化树形对话的独特功能，形成差异化竞争优势。

**最优先实现的4个功能**：

1. 流式输出 - 提升核心体验
2. Markdown渲染 - 支持代码展示
3. 配置文件自定义 - 安全性和灵活性
4. 错误处理优化 - 健壮性

