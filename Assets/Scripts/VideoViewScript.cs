using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoViewScript : MonoBehaviour
{

    public GameObject video1;
    public GameObject video2;

    public Button playBtn1;
    public Button playBtn2;

    public Button leftbtn;
    public Button rightbtn;

    int n;


    // Start is called before the first frame update
    void Start()
    {
        n = 1;
        leftbtn.interactable = false;
        rightbtn.interactable = true;

        video1.SetActive(true);
        video2.SetActive(false);

        playBtn1 = video1.GetComponentInChildren<Button>();
        playBtn2 = video2.GetComponentInChildren<Button>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeVieo()
    {
        if(n==1)
        {
            n = 2;
            video1.SetActive(false);
            video2.SetActive(true);

            leftbtn.interactable = true;
            rightbtn.interactable = false;

            playBtn2.gameObject.SetActive(true);
        }
        else
        {
            n = 1;
            video1.SetActive(true);
            video2.SetActive(false);

            leftbtn.interactable = false;
            rightbtn.interactable = true;

            playBtn1.gameObject.SetActive(true);
        }
    }

}
