using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagScript : MonoBehaviour
{

    public GameObject targetProjectIntro;
    // Start is called before the first frame update
    void Start()
    {
        targetProjectIntro.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenProjectIntro()
    {
        targetProjectIntro.SetActive(true);
    }
}
