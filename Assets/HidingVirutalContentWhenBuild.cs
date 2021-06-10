using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingVirutalContentWhenBuild : MonoBehaviour
{
    public GameObject[] ObjectsToHide;


    private void Awake()
    {
        #if UNITY_ANDROID
                Debug.Log("这里安卓设备");
        #endif

        #if UNITY_IPHONE
                Debug.Log("这里苹果设备");
        #endif

        #if UNITY_STANDALONE_WIN
                Debug.Log("电脑上运行");
        #endif


        switch (Application.platform)
        {
        case RuntimePlatform.WindowsEditor:
            print("Windows");
            break;
  
        case RuntimePlatform.Android:
            print("Android");
            break;
  
        case RuntimePlatform.IPhonePlayer:
            print("Iphone");
            break;
        }
    }   

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            
    }
}
