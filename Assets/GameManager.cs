using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //使用UI相关的组件需要引用该命名空间
using System;

public class GameManager : MonoBehaviour
{
    public GameObject m_diamondPrefab;
    public GameObject m_DiamondSpacet;
    public Text m_StateText;
    public GameObject m_userUI;
    public GameObject m_popup;

    public int amount = 20; //设置宝石数量
    int counter = 0; //游戏时用来计数

    // Start is called before the first frame update
    void Start()
    {
        m_StateText.gameObject.SetActive(false);
        m_popup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        counter = amount;

        m_userUI.SetActive(false);
        m_StateText.gameObject.SetActive(true);
        // 更新计数UI
        m_StateText.text = "剩余宝石：" + counter + " 个";

        for (int i=0;i< counter; i++)
        {
            var x = UnityEngine.Random.Range(-1f,7f);
            var y = UnityEngine.Random.Range(-1.5f,0.5f);
            var z = UnityEngine.Random.Range(-3f,2.7f);

            var diamond = GameObject.Instantiate(m_diamondPrefab, m_DiamondSpacet.transform, false);
            diamond.transform.localPosition = new Vector3(x, y, z);

            diamond.GetComponent<diamondManager>().getDiamondEvent += GetOne;

        }
    }

    void GetOne(object sender, EventArgs e)
    {
        counter--;

        // 更新计数UI
        m_StateText.text = "剩余宝石：" + counter + " 个";

        if (counter == 0)
        {
            // 结束游戏
            EndGame();
        }
    }

    void EndGame()
    {
        m_StateText.gameObject.SetActive(false);
        m_popup.SetActive(true);
        m_userUI.SetActive(true);

    }
}
