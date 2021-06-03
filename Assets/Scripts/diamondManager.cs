using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class diamondManager : MonoBehaviour
{

    public event EventHandler getDiamondEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getDiamond()
    {
        getDiamondEvent(this, EventArgs.Empty);
        Destroy(gameObject);
        Debug.Log("get a diamond");
    }
}
