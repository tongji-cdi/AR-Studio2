using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class diamondManager : MonoBehaviour
{

    public event EventHandler getDiamondEvent; // 声明事件

    public void getDiamond()
    {
        getDiamondEvent(this, EventArgs.Empty); //广播该事件
        Destroy(gameObject); //销毁该钻石实例
        Debug.Log("get a diamond"); //调试用
    }
}
