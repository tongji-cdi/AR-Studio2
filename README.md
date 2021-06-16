## 透明遮挡效果
> 示例场景：CDI_Mask_04

1. 导入 Package: [HidingVirtualContentWhenBuild.unitypackage](https://github.com/tongji-cdi/AR-Studio2/blob/main/utilities/HidingVirtualContentWhenBuild.unitypackage)
2. 在 *Project* 面板中点击 *UniversalRenderPipelineAsset* （可以通过搜索找到），在Inspector中按照下图进行设置。
![](https://github.com/tongji-cdi/AR-Studio2/blob/main/ForTutorial/UniversalRenderPipelineAsset.png)
3. 在 *Project* 面板中点击 *UniversalRenderPipelineAsset_Renderer* （可以通过搜索找到），在Inspector中按照下图进行设置。
![](https://github.com/tongji-cdi/AR-Studio2/blob/main/ForTutorial/UniversalRenderPipelineAsset_Renderer.png)
4. 添加透明遮罩。 新建方块或者其他模型，将 Material 设置为 *Shdaer Graphs_unlit* ,并将其Layer设置为*Mask* （第一次操作需要点击 add layer 新建）。可以查看名为*MaskWall*的Prefabs 的设置。
![](https://github.com/tongji-cdi/AR-Studio2/blob/main/ForTutorial/Mask.png)
5. 查看效果，该物体便具体透明遮挡的功能。


---
有任何问题请联系助教.
