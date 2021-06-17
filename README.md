## 场景说明

示例项目中的几个场景具备不同的功能，各组同学可以按照需求拷贝相关脚本内容用于自己的项目。

### 1. CDI_UI_01

- billboard 公告牌（UI面板永远朝向MainCamera）
  - 指示线（LineRenderer）
- 3D字体、交互控件：Modular 3D Text
- World Space UI 及交互事件
- Textured Map 开关（方便真机调试）

### 2. CDI_Script_02

- 3D对象交互（与3D模型交互）
- Prefab实例化（通过代码在场景中生成3D内容）
- C# 事件

### 3. GongfangSceneNew_03

- 物体持续旋转
- 视频视图：在空间中播放视频内容

### 4. CDI_Mask_04

- 透明对象作为遮罩：通过这种方式模拟真实的物体遮挡效果



## 透明遮挡效果

> 示例场景：CDI_Mask_04

1.导入 Package: [TransparentMask.unitypackage](https://github.com/tongji-cdi/AR-Studio2/blob/main/ForTutorial/TransparentMask.unitypackage)

2.在 *Project* 面板中点击 *UniversalRenderPipelineAsset* （可以通过搜索找到），在Inspector中按照下图进行设置。

![](https://github.com/tongji-cdi/AR-Studio2/blob/main/ForTutorial/UniversalRenderPipelineAsset.png)

3.在 *Project* 面板中点击 *UniversalRenderPipelineAsset_Renderer* （可以通过搜索找到），在Inspector中按照下图进行设置。

![](https://github.com/tongji-cdi/AR-Studio2/blob/main/ForTutorial/UniversalRenderPipelineAsset_Renderer.png)

4.添加透明遮罩。 新建方块或者其他模型，将 Material 设置为 *Shdaer Graphs_unlit* ,并将其Layer设置为*Mask* （第一次操作需要点击 add layer 新建）。可以查看名为*MaskWall*的Prefabs 的设置。

![](https://github.com/tongji-cdi/AR-Studio2/blob/main/ForTutorial/Mask.png)

5. 查看效果，该物体便具体透明遮挡的功能。


---
有任何问题请联系助教.
