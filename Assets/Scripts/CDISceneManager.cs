using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CDISceneManager : MonoBehaviour
{
    public GameObject TexturedMap;
    public GameObject virtualContent;
    public GameObject controlUI;
    public Text temperature;
    public Text tempInfo;
    //public Slider m_slider;




    //public Text 

    // Start is called before the first frame update
    void Awake()
    {
        // 编译版本把材质图给隐藏掉先。
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            TexturedMap.SetActive(false);
            virtualContent.SetActive(false);
        }

        // 设置空调温度调整界面
        controlUI.SetActive(false);


    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTemperatureChanged(float value)
    {
        temperature.text = value.ToString();
        tempInfo.text = value.ToString();
    }




}
