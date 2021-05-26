/// Created by Ferdowsur Asif @ Tiny Giant Studios

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MText
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular 3D Text/Horizontal Selector")]
    public class MText_UI_HorizontalSelector : MonoBehaviour
    {
        /// <summary>
        /// The 3D text used to show the current value
        /// </summary>
        [Tooltip("The 3D text used to show the current value")]
        public Modular3DText text;
        [Space]

        /// <summary>
        /// If keyboard control is enabled, selected = you can control via selected
        /// <para>This value will be controlled by list, if it is in one</para>
        /// </summary>
        [Tooltip("If keyboard control is enabled, selected = you can control via selected. \nThis value will be controlled by list, if it is in one")]
        public bool selected = false;

        /// <summary>
        /// Can this be interacted with. If disabled, can't be selected in list
        /// </summary>
        [Tooltip("If keyboard control is enabled, selected = you can control via selected\nOr selected/deselected in a List")]
        public bool interactable = true;

        [Space]
        /// <summary>
        /// Available options for horizontal selector
        /// </summary>
        [Tooltip("Available options for horizontal selector")]
        public List<string> options = new List<string>
        (
            new string[] { "Option 1", "Option 2", "Option 3" }
        );

        [SerializeField]
        private int value;
        public int Value
        {
            get { return value; }
            set
            {
                this.value = value;
                UpdateText();
                onValueChangedEvent.Invoke();
            }
        }

        [Space]

        public bool keyboardControl = true;
        public KeyCode increaseKey = KeyCode.LeftArrow;
        public KeyCode decreaseKey = KeyCode.RightArrow;

        [Header("Audio")]
        public AudioClip valueChangeSoundEffect;
        public AudioSource audioSource;

        [Header("Events")]
        public UnityEvent onValueChangedEvent;
        public UnityEvent onSelectEvent;




        private void Awake()
        {
            if (selected && keyboardControl) this.enabled = true;
            else this.enabled = false;
        }
        private void Update()
        {
            if (!keyboardControl)
            {
                this.enabled = false;
                return;
            }

            if (Input.GetKeyDown(increaseKey))
            {
                Decrease();
            }
            else if (Input.GetKeyDown(decreaseKey))
            {
                Increase();
            }
        }

        /// <summary>
        /// Increases the selected number. 
        /// <para>If the number is greater/equal(>=) than the options count, sets it to 0</para>
        /// </summary>
        public void Increase()
        {
            value++;
            if (value >= options.Count)
                value = 0;

            UpdateText();
            onValueChangedEvent.Invoke();

            if (audioSource && valueChangeSoundEffect)
                audioSource.PlayOneShot(valueChangeSoundEffect);
        }

        /// <summary>
        /// Decreases the selected number. 
        /// <para>If the number is less than zero, sets it to max</para>
        /// </summary>
        public void Decrease()
        {
            value--;
            if (value < 0)
                value = options.Count - 1;

            UpdateText();
            onValueChangedEvent.Invoke();

            if (audioSource && valueChangeSoundEffect)
                audioSource.PlayOneShot(valueChangeSoundEffect);
        }




        /// <summary>
        /// Updates current text to match the currently selected value
        /// </summary>
        public void UpdateText()
        {
            if (options.Count == 0 || value < 0 || options.Count <= value)
                return;

            if (text)
                text.UpdateText(options[value]);
            else
                Debug.LogError("No text is attached to Horizontal selector: " + gameObject.name, gameObject);
        }






        /// <summary>
        /// Selects/Deselects the component
        /// </summary>
        /// <param name="Bool">true = selected, false = deselected</param>
        public void Focus(bool Bool)
        {
            selected = Bool;
            if (selected && interactable)
            {
                onSelectEvent.Invoke();
                if (keyboardControl) this.enabled = true;
            }
            else this.enabled = false;
        }
    }
}