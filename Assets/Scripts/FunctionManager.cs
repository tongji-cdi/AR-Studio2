using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FunctionManager : MonoBehaviour
{
    [HideInInspector]
    public int userType;
    public GameObject m_LoginUI;
    public GameObject m_UserUI;
    public GameObject m_AdminUI;


    void Awake()
    {
        m_LoginUI.SetActive(true);
        m_UserUI.SetActive(false);
        m_AdminUI.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetUserType()
    {
        Toggle theActiveToggle =  m_LoginUI.GetComponent<ToggleGroup>().ActiveToggles().FirstOrDefault();
        if(theActiveToggle.name == "ToggleUser")
        {
            userType = 0;
        }else if(theActiveToggle.name == "ToggleUser")
        {
            userType = 1;
        }
    }

    public void Login()
    {
        m_LoginUI.SetActive(false);

        if(userType == 0)
        {
            m_UserUI.SetActive(true);
        }else if(userType == 1)
        {
            m_AdminUI.SetActive(true);
        }
    }

}
