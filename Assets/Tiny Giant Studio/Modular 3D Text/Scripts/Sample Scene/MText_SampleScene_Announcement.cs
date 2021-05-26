using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MText
{
    public class MText_SampleScene_Announcement : MonoBehaviour
    {
        [SerializeField] string announcement = null;
        [SerializeField] Modular3DText modular3DText = null;
        [SerializeField] Animator animator = null;
        [SerializeField] ParticleSystem myParticleSystem = null;

        // Start is called before the first frame update
        void Start()
        {
            animator.SetTrigger("Open");
            myParticleSystem.Play();
            Invoke("UpdateText",1.5f);
        }

        void UpdateText()
        {
            modular3DText.UpdateText(announcement);
        }

    }
}