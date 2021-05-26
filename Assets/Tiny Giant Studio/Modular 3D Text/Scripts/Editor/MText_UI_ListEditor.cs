using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace MText
{
    [CustomEditor(typeof(MText_UI_List))]
    public class MText_UI_ListEditor : Editor
    {
        public MText_Settings settings;

        float defaultSmallHorizontalFieldSize = 72.5f;
        float defaultNormalltHorizontalFieldSize = 100;
        float defaultLargeHorizontalFieldSize = 120f;
        float defaultExtraLargeHorizontalFieldSize = 150f;

        string ignoreChildUnSelectModuleLabel = "Ignore Child Modules for un-select";
        string ignoreChildOnSelectModuleLabel = "Ignore Child Modules for on-select";
        string ignoreChildOnPressModuleLabel = "Ignore Child Modules for on-Press";
        string ignoreChildOnClickModuleLabel = "Ignore Child Modules for on-Click";


        MText_UI_List myTarget;
        SerializedObject soTarget;

        SerializedProperty spacing;
        SerializedProperty lockRotation;

        SerializedProperty radius;
        SerializedProperty spread;
        SerializedProperty speed;

        SerializedProperty autoFocusOnStart;
        SerializedProperty autoFocusFirstItem;

        SerializedProperty keyboardControl;
        bool showKeyboardSettings = false;
        SerializedProperty scrollUp;
        SerializedProperty scrollDown;
        SerializedProperty pressItemKey;
        SerializedProperty audioSource;
        SerializedProperty keyScrollingSoundEffect;
        SerializedProperty itemSelectionSoundEffect;

        SerializedProperty controlChildVisuals;
        bool showChildVisualSettings = false;

        SerializedProperty customNormalItemVisual;
        bool showNormalItemSettings = false;
        SerializedProperty normalItemFontSize;
        SerializedProperty normalItemFontMaterial;
        SerializedProperty normalItemBackgroundMaterial;

        SerializedProperty customSelectedItemVisual;
        bool showSelectedItemSettings = false;
        SerializedProperty selectedItemFontSize;
        SerializedProperty selectedItemFontMaterial;
        SerializedProperty selectedItemBackgroundMaterial;
        SerializedProperty selectedItemPositionChange;
        SerializedProperty selectedItemMoveTime;

        SerializedProperty customPressedItemVisual;
        bool showPressedItemSettings = false;
        SerializedProperty pressedItemFontSize;
        SerializedProperty pressedItemFontMaterial;
        SerializedProperty pressedItemBackgroundMaterial;
        SerializedProperty pressedItemPositionChange;
        SerializedProperty pressedItemMoveTime;
        SerializedProperty pressedItemReturnToSelectedVisual;
        SerializedProperty pressedItemReturnToSelectedTime;

        SerializedProperty customDisabledItemVisual;
        bool showDisabledItemSettings = false;
        SerializedProperty disabledItemFontSize;
        SerializedProperty disabledItemFontMaterial;
        SerializedProperty disabledItemBackgroundMaterial;
        

        SerializedProperty applyModules;
        SerializedProperty ignoreChildModules;

        SerializedProperty ignoreChildUnSelectModuleContainers;
        SerializedProperty applyUnSelectModuleContainers;
        SerializedProperty unSelectModuleContainers;

        SerializedProperty ignoreChildOnSelectModuleContainers;
        SerializedProperty applyOnSelectModuleContainers;
        SerializedProperty onSelectModuleContainers;

        SerializedProperty ignoreChildOnPressModuleContainers;
        SerializedProperty applyOnPressModuleContainers;
        SerializedProperty onPressModuleContainers;

        SerializedProperty ignoreChildOnClickModuleContainers;
        SerializedProperty applyOnClickModuleContainers;
        SerializedProperty onClickModuleContainers;
        
        bool showAnimationSettings = false;
        bool showModuleSettings = false;

        string[] alignmentOptions = new[] { "Top", "Bottom", "Verticle Middle", "Left", "Right", "Horizontal Middle", "Circular", "Free" };
        string[] circularListOptions = new[] { "Centered 1", "Centered 2", "Centered 3", "Centered 4", "Spread" };

        enum FieldSize
        {
            small,
            normal,
            large,
            extraLarge
        }


        void OnEnable()
        {
            myTarget = (MText_UI_List)target;
            soTarget = new SerializedObject(target);

            spacing = soTarget.FindProperty("spacing");
            lockRotation = soTarget.FindProperty("lockRotation");
            radius = soTarget.FindProperty("radius");
            spread = soTarget.FindProperty("spread");
            speed = soTarget.FindProperty("speed");

            autoFocusOnStart = soTarget.FindProperty("autoFocusOnStart");
            autoFocusFirstItem = soTarget.FindProperty("autoFocusFirstItem");

            keyboardControl = soTarget.FindProperty("keyboardControl");

            audioSource = soTarget.FindProperty("audioSource");
            scrollUp = soTarget.FindProperty("scrollUp");
            scrollDown = soTarget.FindProperty("scrollDown");
            pressItemKey = soTarget.FindProperty("pressItemKey");

            keyScrollingSoundEffect = soTarget.FindProperty("keyScrollingSoundEffect");
            itemSelectionSoundEffect = soTarget.FindProperty("itemSelectionSoundEffect");


            controlChildVisuals = soTarget.FindProperty("controlChildVisuals");

            customNormalItemVisual = soTarget.FindProperty("customNormalItemVisual");
            normalItemFontSize = soTarget.FindProperty("normalItemFontSize");
            normalItemFontMaterial = soTarget.FindProperty("normalItemFontMaterial");
            normalItemBackgroundMaterial = soTarget.FindProperty("normalItemBackgroundMaterial");

            customSelectedItemVisual = soTarget.FindProperty("customSelectedItemVisual");
            selectedItemFontSize = soTarget.FindProperty("selectedItemFontSize");
            selectedItemFontMaterial = soTarget.FindProperty("selectedItemFontMaterial");
            selectedItemBackgroundMaterial = soTarget.FindProperty("selectedItemBackgroundMaterial");
            selectedItemPositionChange = soTarget.FindProperty("selectedItemPositionChange");
            selectedItemMoveTime = soTarget.FindProperty("selectedItemMoveTime");

            customPressedItemVisual = soTarget.FindProperty("customPressedItemVisual");
            pressedItemFontSize = soTarget.FindProperty("pressedItemFontSize");
            pressedItemFontMaterial = soTarget.FindProperty("pressedItemFontMaterial");
            pressedItemBackgroundMaterial = soTarget.FindProperty("pressedItemBackgroundMaterial");
            pressedItemPositionChange = soTarget.FindProperty("pressedItemPositionChange");
            pressedItemMoveTime = soTarget.FindProperty("pressedItemMoveTime");
            pressedItemReturnToSelectedVisual = soTarget.FindProperty("pressedItemReturnToSelectedVisual");
            pressedItemReturnToSelectedTime = soTarget.FindProperty("pressedItemReturnToSelectedTime");


            customDisabledItemVisual = soTarget.FindProperty("customDisabledItemVisual");
            disabledItemFontSize = soTarget.FindProperty("disabledItemFontSize");
            disabledItemFontMaterial = soTarget.FindProperty("disabledItemFontMaterial");
            disabledItemBackgroundMaterial = soTarget.FindProperty("disabledItemBackgroundMaterial");


            applyModules = soTarget.FindProperty("applyModules");
            ignoreChildModules = soTarget.FindProperty("ignoreChildModules");

            ignoreChildUnSelectModuleContainers = soTarget.FindProperty("ignoreChildUnSelectModuleContainers");
            applyUnSelectModuleContainers = soTarget.FindProperty("applyUnSelectModuleContainers");
            unSelectModuleContainers = soTarget.FindProperty("unSelectModuleContainers");

            ignoreChildOnSelectModuleContainers = soTarget.FindProperty("ignoreChildOnSelectModuleContainers");
            applyOnSelectModuleContainers = soTarget.FindProperty("applyOnSelectModuleContainers");
            onSelectModuleContainers = soTarget.FindProperty("onSelectModuleContainers");

            ignoreChildOnPressModuleContainers = soTarget.FindProperty("ignoreChildOnPressModuleContainers");
            applyOnPressModuleContainers = soTarget.FindProperty("applyOnPressModuleContainers");
            onPressModuleContainers = soTarget.FindProperty("onPressModuleContainers");

            ignoreChildOnClickModuleContainers = soTarget.FindProperty("ignoreChildOnClickModuleContainers");
            applyOnClickModuleContainers = soTarget.FindProperty("applyOnClickModuleContainers");
            onClickModuleContainers = soTarget.FindProperty("onClickModuleContainers");

            showKeyboardSettings = myTarget.showKeyboardSettings;
            showChildVisualSettings = myTarget.showChildVisualSettings;
            showNormalItemSettings = myTarget.showNormalItemSettings;
            showSelectedItemSettings = myTarget.showSelectedItemSettings;
            showPressedItemSettings = myTarget.showPressedItemSettings;
            showDisabledItemSettings = myTarget.showDisabledItemSettings;
            showAnimationSettings = myTarget.showAnimationSettings;
            showModuleSettings = myTarget.showModuleSettings;
        }

        public override void OnInspectorGUI()
        {
            soTarget.Update();
            EditorGUI.BeginChangeCheck();

            MainSettings();

            GUILayout.Space(10);
            KeyboardSettings();
            GUILayout.Space(5);
            VisualSettings();
            GUILayout.Space(5);
            AnimationSettings();
            GUILayout.Space(5);
            ModuleSettings();

            SaveInspectorLayoutSettings();

            if (EditorGUI.EndChangeCheck())
            {
                soTarget.ApplyModifiedProperties();

                if (!EditorApplication.isPlaying)
                    myTarget.UnselectEverything();

                EditorUtility.SetDirty(myTarget);
            }
        }

        void ModuleSettings()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 0;

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(applyModules, GUIContent.none, GUILayout.MaxWidth(25));
            showModuleSettings = EditorGUILayout.Foldout(showModuleSettings, "Modules", true, EditorStyles.foldout);
            GUILayout.EndHorizontal();
          

            if (showModuleSettings)
            {
                DrawUILine(Color.grey, 1, 2);
                EditorGUILayout.PropertyField(ignoreChildModules);

                ModuleContainerList(applyOnClickModuleContainers, "On Click", "", myTarget.onClickModuleContainers, onClickModuleContainers, ignoreChildOnClickModuleContainers, ignoreChildOnClickModuleLabel);
                ModuleContainerList(applyOnPressModuleContainers, "While being clicked", "", myTarget.onPressModuleContainers, onPressModuleContainers, ignoreChildOnPressModuleContainers, ignoreChildOnPressModuleLabel);
                ModuleContainerList(applyOnSelectModuleContainers, "On Select modules", "", myTarget.onSelectModuleContainers, onSelectModuleContainers, ignoreChildOnSelectModuleContainers, ignoreChildOnSelectModuleLabel);
                ModuleContainerList(applyUnSelectModuleContainers, "On Un-Select modules", "", myTarget.unSelectModuleContainers, unSelectModuleContainers, ignoreChildUnSelectModuleContainers, ignoreChildUnSelectModuleLabel);
            }
            if (!Selection.activeTransform)
            {
                showModuleSettings = false;
            }
            GUILayout.EndVertical();
        }

        void MainSettings()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 0;

            DrawUILine(Color.grey, 1, 2);
            AutoSelectionSettings();
            LayoutSettings();

            DrawUILine(Color.grey, 1, 2);
            GUILayout.EndVertical();
        }

        void AutoSelectionSettings()
        {
            EditorGUILayout.PropertyField(autoFocusOnStart, new GUIContent("Auto focus"));
            EditorGUILayout.PropertyField(autoFocusFirstItem, new GUIContent("Auto select first item"));
        }
        void LayoutSettings()
        {
            GUILayout.Space(15);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Alignment", GUILayout.MaxWidth(75));
            if (GUILayout.Button("<", GUILayout.MaxHeight(15)))
            {
                myTarget.alignmentChoice--;
                if (myTarget.alignmentChoice <= 0)
                {
                    myTarget.alignmentChoice = alignmentOptions.Length - 1;
                }
            }            
            myTarget.alignmentChoice = EditorGUILayout.Popup(myTarget.alignmentChoice, alignmentOptions);
            if (GUILayout.Button(">", GUILayout.MaxHeight(15)))
            {
                myTarget.alignmentChoice++;
                if (myTarget.alignmentChoice >= alignmentOptions.Length)
                {
                    myTarget.alignmentChoice = 0;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(2);

            if (myTarget.alignmentChoice != 6)
            {
                EditorGUILayout.PropertyField(spacing);
                EditorGUILayout.PropertyField(lockRotation);
            }
            else
            {
                CircularStyleSettings();
            }
        }
        void CircularStyleSettings()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Style", GUILayout.MaxWidth(75));
            if (GUILayout.Button("<", GUILayout.MaxHeight(15)))
            {
                myTarget.circularListStyle--;
                if (myTarget.circularListStyle <= 0)
                {
                    myTarget.circularListStyle = circularListOptions.Length - 1;
                }
            }
            myTarget.circularListStyle = EditorGUILayout.Popup(myTarget.circularListStyle, circularListOptions);            
            if (GUILayout.Button(">", GUILayout.MaxHeight(15)))
            {
                myTarget.circularListStyle++;
                if (myTarget.circularListStyle >= circularListOptions.Length)
                {
                    myTarget.circularListStyle = 0;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(2);
            EditorGUILayout.PropertyField(radius);
            EditorGUILayout.PropertyField(spread);
            HorizontalField(speed, "Animation speed");

            EditorGUILayout.PropertyField(lockRotation);

        }

        void KeyboardSettings()
        {
            GUILayout.BeginVertical("Box");

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(keyboardControl, GUIContent.none, GUILayout.MaxWidth(25));
            showKeyboardSettings = EditorGUILayout.Foldout(showKeyboardSettings, "Keyboard Control", true, EditorStyles.foldout);
            GUILayout.EndHorizontal();

            EditorGUI.indentLevel = 0;

            if (showKeyboardSettings)
            {
                DrawUILine(Color.grey, 1, 2);
                GUILayout.Space(5);

                EditorGUILayout.PropertyField(scrollUp);
                EditorGUILayout.PropertyField(scrollDown);
                EditorGUILayout.PropertyField(pressItemKey);
                GUILayout.Space(2.5f);
                DrawUILine(Color.grey, 1, 2);
                GUILayout.Space(2.5f);
                EditorGUILayout.PropertyField(audioSource);
                EditorGUILayout.PropertyField(keyScrollingSoundEffect);
                EditorGUILayout.PropertyField(itemSelectionSoundEffect);

                if(!myTarget.audioSource && (myTarget.keyScrollingSoundEffect || myTarget.itemSelectionSoundEffect))
                {
                    EditorGUILayout.HelpBox("Please add a audio source for the sound effects to play", MessageType.Warning);
                }
                
            }
            if (!Selection.activeTransform)
            {
                showKeyboardSettings = false;
            }
            GUILayout.EndVertical();
        }

        void VisualSettings()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 0;

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(controlChildVisuals, GUIContent.none, GUILayout.MaxWidth(25));
            showChildVisualSettings = EditorGUILayout.Foldout(showChildVisualSettings, "Control Child Button Visual", true, EditorStyles.foldout);
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel = 1;

            if (showChildVisualSettings)
            {
                DrawUILine(Color.grey, 1, 2);
                GUILayout.Space(5);

                NormalItemSettings();
                GUILayout.Space(2.5f);
                SelectedItemSettings();
                GUILayout.Space(2.5f);
                PressedItemSettings();
                GUILayout.Space(2.5f);
                DisabledItemSettings();

                GUILayout.Space(5);
            }
            if (!Selection.activeTransform)
            {
                showChildVisualSettings = false;
            }
            GUILayout.EndVertical();
        }

        private void AnimationSettings()
        {
            if (!myTarget.controlChildVisuals || !myTarget.customSelectedItemVisual || !myTarget.customPressedItemVisual)
            {
                GUILayout.BeginVertical("Box");
                EditorGUI.indentLevel = 1;

                showAnimationSettings = EditorGUILayout.Foldout(showAnimationSettings, "Animation Settings", true, EditorStyles.foldout);

                if (showAnimationSettings)
                {
                    DrawUILine(Color.grey, 1, 2);
                    EditorGUI.indentLevel = 0;
                    Color original = GUI.backgroundColor;
                    GUI.backgroundColor = settings.thirdBackgroundColor;

                    if (!myTarget.controlChildVisuals || !myTarget.customSelectedItemVisual)
                    {
                        GUILayout.BeginVertical("Box");
                        GUI.backgroundColor = settings.thirdPropertyFieldColor;
                        EditorGUILayout.LabelField("On Selection");
                        DrawUILine(Color.grey, 1, 2);
                        SelectedItemPostionChange(); 
                        GUILayout.EndVertical();
                    }
                    GUI.backgroundColor = settings.thirdBackgroundColor;
                    GUILayout.Space(5);
                    if (!myTarget.controlChildVisuals || !myTarget.customPressedItemVisual)
                    {
                        GUILayout.BeginVertical("Box");
                        GUI.backgroundColor = settings.thirdPropertyFieldColor;
                        EditorGUILayout.LabelField("On Press");
                        DrawUILine(Color.grey, 1, 2);
                        PressedItemPositionChange();
                        GUILayout.EndVertical();
                    }

                    GUI.backgroundColor = original;
                }

                GUILayout.EndVertical();
            }
        }

        void NormalItemSettings()
        {
            EditorGUI.indentLevel = 0;
            Color originalColor = GUI.backgroundColor;
            GUI.backgroundColor = settings.thirdBackgroundColor;
            GUILayout.BeginVertical("Box");

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(customNormalItemVisual, GUIContent.none, GUILayout.MaxWidth(25));
            showNormalItemSettings = EditorGUILayout.Foldout(showNormalItemSettings, "Normal", true, EditorStyles.foldout);
            GUILayout.EndHorizontal();

            if (showNormalItemSettings)
            {
                DrawUILine(Color.grey, 1, 2);
                EditorGUI.indentLevel = 0;
                GUI.backgroundColor = settings.thirdPropertyFieldColor;
                HorizontalField(normalItemFontSize, "Font Size");
                HorizontalField(normalItemFontMaterial, "Font Material");
                GUILayout.Space(5);
                HorizontalField(normalItemBackgroundMaterial, "Background Material","",FieldSize.large);
            }

            GUILayout.EndVertical();
            GUI.backgroundColor = originalColor;
        }

        void SelectedItemSettings()
        {
            EditorGUI.indentLevel = 0;
            Color originalColor = GUI.backgroundColor;
            GUI.backgroundColor = settings.thirdBackgroundColor;
            GUILayout.BeginVertical("Box");

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(customSelectedItemVisual, GUIContent.none, GUILayout.MaxWidth(25));
            showSelectedItemSettings = EditorGUILayout.Foldout(showSelectedItemSettings, "Selected", true, EditorStyles.foldout);
            GUILayout.EndHorizontal();

            if (showSelectedItemSettings)
            {
                DrawUILine(Color.grey, 1, 2);
                EditorGUI.indentLevel = 0;
                GUI.backgroundColor = settings.thirdPropertyFieldColor;
                HorizontalField(selectedItemFontSize, "Font Size");
                HorizontalField(selectedItemFontMaterial, "Font Material");
                GUILayout.Space(5);
                HorizontalField(selectedItemBackgroundMaterial, "Background Material", "", FieldSize.large);

                if (myTarget.customSelectedItemVisual)
                    SelectedItemPostionChange();
            }

            GUILayout.EndVertical();
            GUI.backgroundColor = originalColor;
        }

        void PressedItemSettings()
        {
            EditorGUI.indentLevel = 0;
            Color originalColor = GUI.backgroundColor;
            GUI.backgroundColor = settings.thirdBackgroundColor;
            GUILayout.BeginVertical("Box");

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(customPressedItemVisual, GUIContent.none, GUILayout.MaxWidth(25));
            showPressedItemSettings = EditorGUILayout.Foldout(showPressedItemSettings, "Pressed/Selected", true, EditorStyles.foldout);
            GUILayout.EndHorizontal();

            if (showPressedItemSettings)
            {
                DrawUILine(Color.grey, 1, 2);
                EditorGUI.indentLevel = 0;
                GUI.backgroundColor = settings.thirdPropertyFieldColor;
                HorizontalField(pressedItemFontSize, "Font Size");
                HorizontalField(pressedItemFontMaterial, "Font Material");
                HorizontalField(pressedItemBackgroundMaterial, "Background Material", "",FieldSize.large);

                if (myTarget.customPressedItemVisual)
                    PressedItemPositionChange();
            }
            GUILayout.EndVertical();
            GUI.backgroundColor = originalColor;
        }

        void DisabledItemSettings()
        {
            EditorGUI.indentLevel = 0;
            Color originalColor = GUI.backgroundColor;
            GUI.backgroundColor = settings.thirdBackgroundColor;
            GUILayout.BeginVertical("Box");

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(customDisabledItemVisual, GUIContent.none, GUILayout.MaxWidth(25));
            showDisabledItemSettings = EditorGUILayout.Foldout(showDisabledItemSettings, "Disabled", true, EditorStyles.foldout);
            GUILayout.EndHorizontal();

            if (showDisabledItemSettings)
            {
                DrawUILine(Color.grey, 1, 2);
                EditorGUI.indentLevel = 0;
                GUI.backgroundColor = settings.thirdPropertyFieldColor;
                HorizontalField(disabledItemFontSize, "Font Size");
                HorizontalField(disabledItemFontMaterial, "Font Material");
                HorizontalField(disabledItemBackgroundMaterial, "Background Material","",FieldSize.large);
            }
            GUILayout.EndVertical();
            GUI.backgroundColor = originalColor;
        }

        void SelectedItemPostionChange()
        {
            if (myTarget.alignmentChoice != 6)
            {
                HorizontalField(selectedItemPositionChange, "Move","",FieldSize.small);
                HorizontalField(selectedItemMoveTime, "Move time");
            }
        }
        void PressedItemPositionChange()
        {
            if (myTarget.alignmentChoice != 6 && myTarget.alignmentChoice != 7)
            {
                HorizontalField(pressedItemPositionChange, "Move","",FieldSize.small);
                HorizontalField(pressedItemMoveTime, "Move time");

                GUILayout.Space(5);
            }
            PressedItemReturnToSelectedVisualChange();
        }
        private void PressedItemReturnToSelectedVisualChange()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(pressedItemReturnToSelectedVisual, GUIContent.none, GUILayout.MaxWidth(15));
            EditorGUILayout.LabelField("Return to 'Selected' visual after delay");
            GUILayout.EndHorizontal();
            if (myTarget.pressedItemReturnToSelectedVisual)
            {
                EditorGUILayout.PropertyField(pressedItemReturnToSelectedTime, new GUIContent("Delay"));
            }
        }

        void HorizontalField(SerializedProperty property, string label, string tooltip = "", FieldSize fieldSize = FieldSize.normal)
        {
            float myMaxWidth = 100;
            //not to self: it's ternary operator not tarnary operator. Stop mistyping
            if (settings)            
                myMaxWidth = fieldSize == FieldSize.small ? settings.smallHorizontalFieldSize : fieldSize == FieldSize.normal ? settings.normalltHorizontalFieldSize : fieldSize == FieldSize.large ? settings.largeHorizontalFieldSize: fieldSize == FieldSize.extraLarge? settings.extraLargeHorizontalFieldSize : settings.normalltHorizontalFieldSize;
            else            
                myMaxWidth = fieldSize == FieldSize.small ? defaultSmallHorizontalFieldSize : fieldSize == FieldSize.normal ? defaultNormalltHorizontalFieldSize : fieldSize == FieldSize.large ? defaultLargeHorizontalFieldSize: fieldSize == FieldSize.extraLarge ? defaultExtraLargeHorizontalFieldSize : settings.normalltHorizontalFieldSize;
            
            GUILayout.BeginHorizontal();
            GUIContent gUIContent = new GUIContent(label, tooltip);
            EditorGUILayout.LabelField(gUIContent, GUILayout.MaxWidth(myMaxWidth));
            EditorGUILayout.PropertyField(property, GUIContent.none);
            GUILayout.EndHorizontal();
        }
       
        void DrawUILine(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }

        void ModuleContainerList(SerializedProperty moduleBoolProperty, string label, string toolTip, List<MText_ModuleContainer> moduleContainers, SerializedProperty serializedContainer, SerializedProperty ignoreChild, string ignoreChildLabel)
        {
            Color original = GUI.backgroundColor;
            if (settings) GUI.backgroundColor = settings.thirdBackgroundColor;
            else GUI.backgroundColor = new Color(.9f, .9f, .9f);

            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 0;

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(moduleBoolProperty, GUIContent.none, GUILayout.MaxWidth(15));
            GUIContent content = new GUIContent(label, toolTip);
            EditorGUILayout.LabelField(content);
            GUILayout.EndHorizontal();

            DrawUILine(Color.grey, 1, 2);

            GUILayout.Space(5);
            HorizontalField(ignoreChild,ignoreChildLabel,"",FieldSize.small);
            GUILayout.Space(5);

            if (moduleContainers.Count > 0)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Module", EditorStyles.miniLabel, GUILayout.MinWidth(10));
                EditorGUILayout.LabelField("Duration", EditorStyles.miniLabel, GUILayout.MaxWidth(65));
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(30));

                GUILayout.EndHorizontal();
            }

            Color originalContent = GUI.contentColor;
            GUI.backgroundColor = Color.white;
            GUI.contentColor = originalContent;

            for (int i = 0; i < moduleContainers.Count; i++)
            {

                GUILayout.BeginVertical("Box");
                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(serializedContainer.GetArrayElementAtIndex(i).FindPropertyRelative("module"), GUIContent.none, GUILayout.MinWidth(10));
                EditorGUILayout.PropertyField(serializedContainer.GetArrayElementAtIndex(i).FindPropertyRelative("duration"), GUIContent.none, GUILayout.MaxWidth(65));

                GUI.contentColor = Color.black;
                if (GUILayout.Button("X", GUILayout.MaxWidth(30)))
                {
                    moduleContainers.RemoveAt(i);
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }
            GUI.contentColor = Color.black;
            if (GUILayout.Button("Add New Module", GUILayout.MinHeight(20)))
            {
                myTarget.CreateEmptyEffect(moduleContainers);
            }

            GUI.contentColor = originalContent;

            GUILayout.EndVertical();
            GUI.backgroundColor = original;
        }
        private void SaveInspectorLayoutSettings()
        {
            myTarget.showKeyboardSettings = showKeyboardSettings;
            myTarget.showChildVisualSettings = showChildVisualSettings;
            myTarget.showNormalItemSettings = showNormalItemSettings;
            myTarget.showSelectedItemSettings = showSelectedItemSettings;
            myTarget.showPressedItemSettings = showPressedItemSettings;
            myTarget.showDisabledItemSettings = showDisabledItemSettings;
            myTarget.showAnimationSettings = showAnimationSettings;
            myTarget.showModuleSettings = showModuleSettings;
        }
    }
}
