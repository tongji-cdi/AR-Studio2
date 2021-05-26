/// Created by Ferdowsur Asif @ Tiny Giant Studio

using System.Collections;
using UnityEngine.Events;
using UnityEngine;

namespace MText
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular 3D Text/Input field")]
    public class Mtext_UI_InputField : MonoBehaviour
    {

        [Tooltip("if not in a list")]
        public bool autoFocusOnGameStart = true;
        public bool interactable = true;

        [SerializeField] 
        private int maxCharacter = 20;
        [SerializeField] 
        private string typingSymbol = "|";

        [SerializeField]
        private string _text = string.Empty;
        public string Text
        {
            get { return _text; }
            set { _text = value; UpdateText(true); }
        }

        public string placeHolderText = "Enter Text...";

        public Modular3DText textComponent = null;
        public Renderer background = null;

        public bool enterKeyEndsInput = true;

        public Material placeHolderTextMat = null;

        public Material inFocusTextMat = null;
        public Material inFocusBackgroundMat = null;

        public Material outOfFocusTextMat = null;
        public Material outOfFocusBackgroundMat = null;

        public Material disabledTextMat = null;
        public Material disabledBackgroundMat = null;

        private Material currentTextMaterial = null;

        [SerializeField] 
        private AudioClip typeSound = null;
        [SerializeField] 
        private AudioSource audioSource = null;

        public UnityEvent onInput = null;
        public UnityEvent onBackspace = null;
        public UnityEvent onInputEnd = null;


        #region remember inspector layout
#if UNITY_EDITOR
        [HideInInspector] public bool showMainSettings = true;
        [HideInInspector] public bool showStyleSettings = false;
        [HideInInspector] public bool showAudioSettings = false;
        [HideInInspector] public bool showUnityEventSettings = false;
#endif
        #endregion remember inspector layout

        public string test;

        private void Awake()
        {
            if (!MText_Utilities.GetParentList(transform))
                Focus(autoFocusOnGameStart);
        }


        private void Update()
        {
            foreach (char c in Input.inputString)
            {
                if (c == '\b') // has backspace/delete been pressed?
                {
                    if (_text.Length != 0)
                    {
                        _text = _text.Substring(0, _text.Length - 1);
                        UpdateText(true);
                        onBackspace.Invoke();
                    }
                }
                else if (((c == '\n') || (c == '\r')) && enterKeyEndsInput) // enter/return
                {
                    InputComplete();
                }
                else
                {
                    if (_text.Length < maxCharacter)
                    {
                        _text += c;
                        UpdateText(true);
                        onInput.Invoke();
                    }
                }
            }
        }

        public void InputComplete()
        {
            onInputEnd.Invoke();
            this.enabled = false;
        }

        public void UpdateText()
        {
            UpdateText(false);
        }
        public void UpdateText(string newText)
        {
            _text = newText;
            UpdateText(false);
        }
        public void UpdateText(int newTextInt)
        {
            _text = newTextInt.ToString();
            UpdateText(false);
        }
        public void UpdateText(float newTextFloat)
        {
            _text = newTextFloat.ToString();
            UpdateText(false);
        }

        public void UpdateText(bool sound)
        {
            if (!textComponent)
                return;

            TouchScreenKeyboard.Open(_text);


            if (!string.IsNullOrEmpty(_text))
            {
                textComponent.Material = currentTextMaterial;
                textComponent.UpdateText(string.Concat(_text, typingSymbol));
            }
            else
            {
                textComponent.Material = placeHolderTextMat;
                textComponent.UpdateText(placeHolderText);
            }

            if (typeSound && sound && audioSource)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(typeSound);
            }
        }

        /// <summary>
        /// Sets the text to empty
        /// </summary>
        public void EmptyText()
        {
            _text = string.Empty;
            UpdateText(false);
        }

        public void Select()
        {
            Focus(true);

            transform.parent?.GetComponent<MText_UI_List>()?.SelectItem(transform.GetSiblingIndex());
        }

        //coroutine to delay a single frame to avoid pressing "enter" key in one list to apply to another UI just getting enabled
        public void Focus(bool enable)
        {
            StartCoroutine(FocusRoutine(enable));
        }

        //coroutine to delay a single frame to avoid pressing "enter" key in one list to apply to another UI just getting enabled
        IEnumerator FocusRoutine(bool enable)
        {
            yield return null;
            FocusFunction(enable);
        }

        public void Focus(bool enable, bool delay)
        {
            if (!delay)
                FocusFunction(enable);
            else
                FocusRoutine(enable);
        }

        void FocusFunction(bool enable)
        {
            if (interactable)
            {
                this.enabled = enable;

                if (enable)
                    SelectedVisual();
                else
                    UnselectedVisual();
            }
            else
            {
                DisableVisual();
            }

            UpdateText(false);
        }


        public void Interactable()
        {
            Focus(false, false);
        }
        public void Uninteractable()
        {
            this.enabled = false;

            DisableVisual();
            UpdateText(false);
        }





        #region Visuals
        private void SelectedVisual()
        {
            //item1 = applyStyleFromParent
            var applySelectedItemMaterial = ApplySelectedStyleFromParent();

            //apply parent list mat
            if (applySelectedItemMaterial.Item1)
                UpdateMaterials(applySelectedItemMaterial.Item2.selectedItemFontMaterial, applySelectedItemMaterial.Item2.selectedItemBackgroundMaterial);
            //apply self mat
            else
                UpdateMaterials(inFocusTextMat, inFocusBackgroundMat);
        }

        private void UnselectedVisual()
        {
            //item1 = applyStyleFromParent
            var applyNormalStyle = ApplyNormalStyleFromParent();

            //apply parent list mat
            if (applyNormalStyle.Item1)
                UpdateMaterials(applyNormalStyle.Item2.normalItemFontMaterial, applyNormalStyle.Item2.normalItemBackgroundMaterial);
            //apply self mat
            else
                UpdateMaterials(outOfFocusTextMat, outOfFocusBackgroundMat);
        }

        public void DisableVisual()
        {
            //item1 = applyStyleFromParent
            var applySelectedItemMaterial = ApplySelectedStyleFromParent();

            //apply parent list mat
            if (applySelectedItemMaterial.Item1)
                UpdateMaterials(applySelectedItemMaterial.Item2.disabledItemFontMaterial, applySelectedItemMaterial.Item2.disabledItemBackgroundMaterial);
            //apply self mat
            else
                UpdateMaterials(disabledTextMat, disabledBackgroundMat);
        }

        private void UpdateMaterials(Material textMat, Material backgroundMat)
        {
            if (textComponent)
                textComponent.Material = textMat;
            if (background)
                background.material = backgroundMat;

            currentTextMaterial = textMat;
        }

        private MText_UI_List GetParentList()
        {
            return MText_Utilities.GetParentList(transform);
        }

        public (bool, MText_UI_List) ApplyNormalStyleFromParent()
        {
            MText_UI_List list = GetParentList();
            if (list)
            {
                if (list.controlChildVisuals && list.customNormalItemVisual)
                {
                    return (true, list);
                }
            }
            //don't apply from list
            return (false, list);
        }
        public (bool, MText_UI_List) ApplySelectedStyleFromParent()
        {
            //get style from parent list
            MText_UI_List list = GetParentList();
            if (list)
            {
                if (list.controlChildVisuals && list.customSelectedItemVisual)
                {
                    return (true, list);
                }
            }
            //don't apply from list
            return (false, list);
        }
        public (bool, MText_UI_List) ApplyDisabledStyleFromParent()
        {
            MText_UI_List list = GetParentList();

            if (list)
            {
                if (list.controlChildVisuals && list.customDisabledItemVisual)
                    return (true, list);
            }
            return (false, list);
        }
        #endregion Visual
    }
}
