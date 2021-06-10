using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MText;

public class LikeBtnScript : MonoBehaviour
{

    //public GameObject m_numbers;
    public Modular3DText m_numbers;
    MeshRenderer m_MeshRenderer;

    int m_int;
    bool isON = false;


    Color normalColor = new Color(1f,1f,1f, 0.5960785f);
    Color onColor = new Color(0.9716981f, 0.5374823f, 0f, 0.5960785f);

    // Start is called before the first frame update
    void Start()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
        m_MeshRenderer.material.color = normalColor;
        //m_numbers.Material.color = normalColor; 

        m_int = int.Parse(m_numbers.Text);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickEventHandler()
    {
        if(isON == false)
        {
            isON = true;
            m_MeshRenderer.material.color = onColor;
            //m_numbers.Material.color = onColor;


            m_numbers.Text = (m_int + 1).ToString();
        }else
        {
            isON = false;
            m_MeshRenderer.material.color = normalColor;
            //m_numbers.Material.color = normalColor;

            m_numbers.Text = m_int.ToString();
        }
    }
}
