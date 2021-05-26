using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace MText
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular 3D Text/Buttons")]
    public class MText_UI_Button : MonoBehaviour
    {
        public MText_Settings settings = null;


        public UnityEvent onClickEvents = new UnityEvent();
        public UnityEvent whileBeingClickedEvents = null;
        public UnityEvent onSelectEvents = null;
        public UnityEvent onUnselectEvents = null;

        public bool interactable = true;
        [Tooltip("Mouse or touch can select this")]
        public bool interactableByMouse = true;

        public Modular3DText text;
        public Renderer background;

        public bool useModules = true;
        public bool useStyles = true;

        public Vector3 normalFontSize = new Vector3(8, 8, 1);
        public Material normalFontMaterial = null;
        public Material normalBackgroundMaterial = null;
        public bool applyUnSelectModuleContainers = true;
        public List<MText_ModuleContainer> unSelectModuleContainers = new List<MText_ModuleContainer>();

        public bool applySelectedVisual = true;
        public Vector3 selectedFontSize = new Vector3(8.2f, 8.2f, 8.2f);
        public Material selectedFontMaterial = null;
        public Material selectedBackgroundMaterial = null;
        public bool applyOnSelectModuleContainers = true;
        public List<MText_ModuleContainer> onSelectModuleContainers = new List<MText_ModuleContainer>();

        public bool applyPressedVisual = true;
        public Vector3 pressedFontSize = new Vector3(8.5f, 8.5f, 8.5f);
        public Material pressedFontMaterial = null;
        public Material pressedBackgroundMaterial = null;
        public bool pressedItemReturnToSelectedVisuaAfterDelay = true;
        public float pressedItemReturnToSelectedVisualTime = 0.15f;
        float returnToselectedTime = 0;

        public bool applyOnPressModuleContainers = true;
        public List<MText_ModuleContainer> onPressModuleContainers = new List<MText_ModuleContainer>();

        public bool applyOnClickModuleContainers = true;
        public List<MText_ModuleContainer> onClickModuleContainers = new List<MText_ModuleContainer>();

        public Vector3 disabledFontSize = new Vector3(8, 8, 8);
        public Material disabledFontMaterial = null;
        public Material disabledBackgroundMaterial = null;

        public bool selectedVisual = false;

        #region remember inspector layout
#if UNITY_EDITOR
        [HideInInspector] public bool showEvents = false;
        [HideInInspector] public bool showStyles = false;
        [HideInInspector] public bool showNormalStyles = false;
        [HideInInspector] public bool showSelectedStyles = false;
        [HideInInspector] public bool showPressedStyles = false;
        [HideInInspector] public bool showDisabledStyles = false;
        [HideInInspector] public bool showModuleSettings = false;
#endif
        #endregion remember inspector layout




        private void Awake()
        {
            this.enabled = false;
        }

        private void Update()
        {
            Animation();
        }

        private void Animation()
        {
            if (Time.time > returnToselectedTime)
            {
                if (selectedVisual) SelectedButtonVisualUpdate();
                else UnselectedButtonVisualUpdate();

                this.enabled = false;
            }
        }

        //call this to select a button
        public void SelectButton()
        {
            MText_UI_List parentList = MText_Utilities.GetParentList(transform);
            if (parentList)
            {
                int childNumber = transform.GetSiblingIndex();
                //parentList.UnselectEverythingExceptSelected();
                if (parentList.selectedItem != childNumber)
                    parentList.UnselectItem(parentList.selectedItem);

                parentList.SelectItem(childNumber);
            }

            SelectedButtonVisualUpdate();
            SelectedButtonModuleUpdate();
            onSelectEvents.Invoke();
        }
        public void SelectedButtonVisualUpdate()
        {
            selectedVisual = true;
            var applyOnSelectStyle = ApplyOnSelectStyle();

            if (applyOnSelectStyle.Item1)
            {
                ApplyeStyle(applyOnSelectStyle.Item3.selectedItemFontSize, applyOnSelectStyle.Item3.selectedItemFontMaterial, applyOnSelectStyle.Item3.selectedItemBackgroundMaterial);
            }
            else if (applyOnSelectStyle.Item2)
            {
                ApplyeStyle(selectedFontSize, selectedFontMaterial, selectedBackgroundMaterial);
            }
        }
        public void SelectedButtonModuleUpdate()
        {
            (bool, bool, MText_UI_List) applyModules = ApplyOnSelectModule();

            //list modules
            if (applyModules.Item1)            
                CallModules(applyModules.Item3.onSelectModuleContainers);
            //self modules
            if (applyModules.Item2)
                CallModules(onSelectModuleContainers);
        }


        public void UnselectButton()
        {
            UnselectedButtonVisualUpdate();
#if UNITY_EDITOR    
            if (EditorApplication.isPlaying)
            {
                UnselectButtonModuleUpdate();
                onUnselectEvents.Invoke();
            }
#else
            UnselectButtonModuleUpdate();
            onUnselectEvents.Invoke();
#endif
        }
        public void UnselectedButtonVisualUpdate()
        {
            selectedVisual = false;

            //apply from list
            if (ApplyNormalStyle().Item1)
            {
                MText_UI_List parent = MText_Utilities.GetParentList(transform);
                ApplyeStyle(parent.normalItemFontSize, parent.normalItemFontMaterial, parent.normalItemBackgroundMaterial);
            }
            else if (ApplyNormalStyle().Item2)
            {
                ApplyeStyle(normalFontSize, normalFontMaterial, normalBackgroundMaterial);
            }
        }
        public void UnselectButtonModuleUpdate()
        {
            (bool, bool, MText_UI_List) applyModules = ApplyUnSelectModule();

            //list modules
            if (applyModules.Item1)
                CallModules(applyModules.Item3.unSelectModuleContainers);
            //self modules
            if (applyModules.Item2)
                CallModules(unSelectModuleContainers);
        }

        public void PressButton()
        {
            MText_UI_List parentList = MText_Utilities.GetParentList(transform);
            if (parentList)
            {
                parentList.PresstItem(transform.GetSiblingIndex());
            }

            PressButtonVisualUpdate();
            onClickEvents.Invoke();
            OnClickButtonModuleUpdate();
        }
        public void PressButtonDontCallList()
        {
            PressButtonVisualUpdate();
            onClickEvents.Invoke();
            OnClickButtonModuleUpdate();
        }

        //Difference between PressButtonClick() & PressButtonVisualUpdate() is that PressButtonVisualUpdate() automatically returns to selected visual
        //used by List
        public void PressButtonVisualUpdate()
        {
            //item 1 = apply parentstyle, item2 = apply selfstyle, item3 = list
            var applyPressedStyle = ApplyPressedStyle();

            if (applyPressedStyle.Item1)
            {
                ApplyeStyle(applyPressedStyle.Item3.pressedItemFontSize, applyPressedStyle.Item3.pressedItemFontMaterial, applyPressedStyle.Item3.pressedItemBackgroundMaterial);
            }
            else if (applyPressedStyle.Item2)
            {
                ApplyeStyle(pressedFontSize, pressedFontMaterial, pressedBackgroundMaterial);
                if (pressedItemReturnToSelectedVisuaAfterDelay)
                {
                    this.enabled = true;
                    returnToselectedTime = Time.time + pressedItemReturnToSelectedVisualTime;
                }
            }
        }

        //the methods above this line for pressed are called by everything except raycaster

        //Difference between PressButtonClick() & PressButtonVisualUpdate() is that PressButtonVisualUpdate() automatically returns to selected visual
        //used by Raycaster
        public void PressButtonClick()
        {
            (bool, bool, MText_UI_List) applyPressedStyle = ApplyPressedStyle();

            if (applyPressedStyle.Item1)
            {
                ApplyeStyle(applyPressedStyle.Item3.pressedItemFontSize, applyPressedStyle.Item3.pressedItemFontMaterial, applyPressedStyle.Item3.pressedItemBackgroundMaterial);
            }
            else if (applyPressedStyle.Item2)
            {
                ApplyeStyle(pressedFontSize, pressedFontMaterial, pressedBackgroundMaterial);
            }
        }
        public void ButtonBeingPressed()
        {
            whileBeingClickedEvents.Invoke();
            OnPressButtonModuleUpdate();
        }
        public void PressedButtonClickStopped()
        {
            if (selectedVisual)
            {
                onClickEvents.Invoke();
                OnClickButtonModuleUpdate();
            }

            if (pressedItemReturnToSelectedVisuaAfterDelay)
            {
                this.enabled = true;
                returnToselectedTime = Time.time + pressedItemReturnToSelectedVisualTime;
            }
            else
            {
                if (selectedVisual)                
                    SelectedButtonVisualUpdate();                
                else                
                    UnselectedButtonVisualUpdate();                
            }
        }
        
        public void OnClickButtonModuleUpdate()
        {
            (bool, bool, MText_UI_List) applyModules = ApplyOnClickModule();

            //list modules
            if (applyModules.Item1)
                CallModules(applyModules.Item3.onClickModuleContainers);
            //self modules
            if (applyModules.Item2)
                CallModules(onClickModuleContainers);
        } 
        public void OnPressButtonModuleUpdate()
        {
            (bool, bool, MText_UI_List) applyModules = ApplyOnPresstModule();

            //list modules
            if (applyModules.Item1)
                CallModules(applyModules.Item3.onPressModuleContainers);
            //self modules
            if (applyModules.Item2)
                CallModules(onPressModuleContainers);
        }

        public void DisabledButtonVisualUpdate()
        {
            //item1 : false = apply from parent & //true = apply from self. item2 = parent
            var applyDisabledStyle = ApplyDisabledStyle();

            if (applyDisabledStyle.Item1)            
                ApplyeStyle(disabledFontSize, disabledFontMaterial, disabledBackgroundMaterial);            
            else            
                ApplyeStyle(applyDisabledStyle.Item2.disabledItemFontSize, applyDisabledStyle.Item2.disabledItemFontMaterial, applyDisabledStyle.Item2.disabledItemBackgroundMaterial);            
        }


        private void ApplyeStyle(Vector3 fontSize, Material fontMat, Material backgroundMat)
        {
            if (text)
            {
                text.FontSize = fontSize;
                text.Material = fontMat;
                text.UpdateText();
            }

            if (background)
            {
                background.material = backgroundMat;
            }
        }

        public void Uninteractable()
        {
            interactable = false;
            DisabledButtonVisualUpdate();
        }
        public void Interactable()
        {
            interactable = true;
            UnselectedButtonVisualUpdate();
        }


        //public MText_UI_List GetParentList()
        //{
        //    if (transform.parent?.GetComponent<MText_UI_List>())
        //    {
        //        return transform.parent.GetComponent<MText_UI_List>();
        //    }
        //    else return null;
        //}

        //these checks are public for the editorscript only
#region Check if style should be applied from here
        //first is apply from list
        public (bool, bool) ApplyNormalStyle()
        {
            MText_UI_List list = MText_Utilities.GetParentList(transform);
            if (list)
            {
                if (list.controlChildVisuals && list.customNormalItemVisual)
                {
                    return (true, false);
                }
            }
            //don't apply from list
            return (false, true);
        }
        public (bool, bool, MText_UI_List) ApplyOnSelectStyle()
        {
            //get style from parent list
            MText_UI_List list = MText_Utilities.GetParentList(transform);
            if (list)
            {
                if (list.controlChildVisuals && list.customSelectedItemVisual)
                {
                    return (true, false, list);
                }
            }
            //get style from itself
            if (applySelectedVisual)
                return (false, true, list);

            //don't get style
            return (false, false, list);
        }
        //item1 = parent, item2 = self
        public (bool, bool, MText_UI_List) ApplyPressedStyle()
        {
            //get style from parent list
            MText_UI_List list = MText_Utilities.GetParentList(transform);
            if (list)
            {
                if (list.controlChildVisuals && list.customPressedItemVisual)                
                    return (true, false, list);                
            }
            //get style from itself
            if (applyPressedVisual)
                return (false, true, list);

            return (false, false, list);
        }
        //false = apply from parent & //true = apply from self
        public (bool, MText_UI_List) ApplyDisabledStyle()
        {
            MText_UI_List list = MText_Utilities.GetParentList(transform);
            if (list)
            {
                if (list.controlChildVisuals && list.customDisabledItemVisual)
                    return (false, list);
            }
            return (true, list);
        }
#endregion Check if style should be applied ends here



        private void CallModules(List<MText_ModuleContainer> moduleContainers)
        {
            if (moduleContainers.Count>0)
            {
                for (int i = 0; i < moduleContainers.Count; i++)
                {
                    if (moduleContainers[i].module)
                        StartCoroutine(moduleContainers[i].module.ModuleRoutine(gameObject, moduleContainers[i].duration));
                }
            }
        }

#region Check if Module should be applied from here
        public (bool,bool, MText_UI_List) ApplyUnSelectModule()
        {
            bool applySelfModules = false;
            bool applyListModule = false;
            MText_UI_List list = MText_Utilities.GetParentList(transform);
            if (list)
            {
                if (list.applyModules && list.applyOnSelectModuleContainers)
                    applyListModule = true;
                if (list.ignoreChildModules || list.ignoreChildUnSelectModuleContainers)
                    return (applyListModule, applySelfModules, list);
            }
            
            if (useModules && applyUnSelectModuleContainers)
                applySelfModules = true;

            return (applyListModule, applySelfModules,list);
        } 
        public (bool,bool, MText_UI_List) ApplyOnPresstModule()
        {
            bool applySelfModules = false;
            bool applyListModule = false;
            MText_UI_List list = MText_Utilities.GetParentList(transform);
            if (list)
            {
                if (list.applyModules && list.applyOnPressModuleContainers)
                    applyListModule = true;
                if (list.ignoreChildModules || list.ignoreChildOnPressModuleContainers)
                    return (applyListModule, applySelfModules, list);
            }
            if (useModules && applyOnPressModuleContainers)
                applySelfModules = true;

            return (applyListModule, applySelfModules, list);
        } 
        public (bool,bool, MText_UI_List) ApplyOnClickModule()
        {
            bool applySelfModules = false;
            bool applyListModule = false;
            MText_UI_List list = MText_Utilities.GetParentList(transform);
            if (list)
            {
                if (list.applyModules && list.applyOnClickModuleContainers)
                    applyListModule = true;
                if (list.ignoreChildModules || list.ignoreChildOnClickModuleContainers)
                    return (applyListModule, applySelfModules, list);
            }
            if (useModules && applyOnPressModuleContainers)
                applySelfModules = true;

            return (applyListModule, applySelfModules, list);
        } 
        public (bool,bool, MText_UI_List) ApplyOnSelectModule()
        {
            bool applySelfModules = false;
            bool applyListModule = false;
            MText_UI_List list = MText_Utilities.GetParentList(transform);
            if (list)
            {
                if (list.applyModules && list.applyOnSelectModuleContainers)
                    applyListModule = true;
                if (list.ignoreChildModules || list.ignoreChildOnSelectModuleContainers)
                    return (applyListModule, applySelfModules, list);
            }

            if (useModules && applyOnSelectModuleContainers)
                applySelfModules = true;

            return (applyListModule, applySelfModules, list);
        }
#endregion Check if Module should be applied ends here

        public void EmptyEffect(List<MText_ModuleContainer> moduleList)
        {
            MText_ModuleContainer module = new MText_ModuleContainer();
            module.duration = 0.5f;
            moduleList.Add(module);
        }

        public void LoadDefaultSettings()
        {
            if (settings)
            {
                normalBackgroundMaterial = settings.defaultBackgroundMaterial;
            }
        }
    }
}