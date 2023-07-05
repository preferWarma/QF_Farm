# QF_Farm
基于QFramework框架制作的农场模拟经营游戏
实现了基本的模拟经营类3C,包括最基本的种植系统, 工具系统等, 广泛使用QFramework提供的BindableProperty进行数据监听和UI同步, 采用非完全体的架构进行开发, 以数据作为驱动开发了挑战系统, 电脑系统(类似任务系统), 升级系统, 土地系统, 背包系统, 并在此之上实践了通用存档系统来对数据进行统一的管理
部分技术表述如下:
1. 类UniRx的Bind功能(QFramework提供的BindableProperty)的使用, 实现数据的单向和双向绑定, 采用监听方式实现数据和UI的同步
2. 简单通用的背包系统, 包括数据和UI的同步, 采用Command方式统一管理背包数据的增删和交换等
3. 基于接口实现的存档系统, 支持PlayerPrefs和Json格式, 支持注册接口实现数据的统一保存和加载, 已单独拆分到工具包中, 详细文档见https://github.com/preferWarma/UnityUtilityhttps://github.com/preferWarma/UnityUtility
4. 基于DoTween实现UI的渐入渐出动画, 植物的收割动画, 环境交互效果等
