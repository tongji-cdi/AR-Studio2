using System.Collections;
using UnityEngine;

namespace MText
{
    public class Typewriter : MonoBehaviour
    {
        [SerializeField] Modular3DText modular3DText = null;
        [TextArea]
        public string text = "Typewriter text";
        [Tooltip("Minimum and maximum possible speed.")]
        public Vector2 speed = Vector2.one;
        [SerializeField] AudioClip typeSound = null;
        [Tooltip("Minimum and maximum possible volume.")]
        [SerializeField] Vector2 volume = Vector2.one;
        [SerializeField] AudioSource audioSource = null;

        [SerializeField] string typingSymbol = null;
        [SerializeField] bool startAutomatically = true;
        [SerializeField] float startDelay = 0;

        void Start()
        {
            if (startAutomatically)
                StartTyping();
        }

        //call this
        public void StartTyping()
        {
            StopAllCoroutines();
            StartCoroutine(TypingRoutine());
        }

        IEnumerator TypingRoutine()
        {
            yield return null;
            yield return new WaitForSeconds(startDelay);
            if (modular3DText)
            {
                for (int i = 0; i <= text.Length; i++)
                {
                    modular3DText.Text = (text.Substring(0, i) + typingSymbol);


                    yield return null;
                    yield return new WaitForSeconds(Random.Range(speed.x, speed.y));

                    if (audioSource && typeSound)
                    {
                        audioSource.pitch = Random.Range(0.9f, 1.1f);
                        audioSource.PlayOneShot(typeSound, Random.Range(volume.x, volume.y));
                    }
                }
            }
            else
            {
                Debug.Log("<color=red>No text object is selected on typewriter.</color> :" + gameObject.name, gameObject);
            }
        }
    }
}