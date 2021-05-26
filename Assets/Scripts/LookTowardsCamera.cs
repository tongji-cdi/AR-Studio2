using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowardsCamera : MonoBehaviour
{

    GameObject m_Camera;
    float rotateX, rotateZ;//记录物体角度x,z变量;


    void Start()
    {
        m_Camera = GameObject.FindGameObjectWithTag("MainCamera");
        rotateX = transform.eulerAngles.x;
        rotateZ = transform.eulerAngles.z;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (m_Camera)
        {
            Vector3 cameraPose = m_Camera.transform.position;
            transform.LookAt(cameraPose);
            transform.eulerAngles = new Vector3(rotateX, transform.eulerAngles.y, rotateZ);//还原最开始的x,z;
        }
        else
        {
            Debug.Log("Can not found camera");
        }
    }
}
