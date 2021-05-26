using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace MText
{
    [CustomEditor(typeof(MText_UI_Button))]
    public class MText_UI_ButtonEditor : Editor
    {
        public MText_Settings settings;

        MText_UI_Button myTarget;
        SerializedObject soTarget;

        bool showEvents = false;
        SerializedProperty onClickEvents;
        SerializedProperty whileBeingClickedEvents;
        SerializedProperty onSelectEvents;
        SerializedProperty onUnselectEvents;

        SerializedProperty interactable;
        SerializedProperty interactableByMouse;

        SerializedProperty text;
        SerializedProperty background;

        bool showStyles = false;
        SerializedProperty useStyles;

        bool showNormalStyles = false;
        SerializedProperty normalFontSize;
        SerializedProperty normalFontMaterial;
        SerializedProperty normalBackgroundMaterial;

        bool showSelectedStyles = false;
        SerializedProperty applySelectedVisual;
        SerializedProperty selectedFontSize;
        SerializedProperty selectedFontMaterial;
        SerializedProperty selectedBackgroundMaterial;

        bool showPressedStyles = false;
        SerializedProperty applyPressedVisual;
        SerializedProperty pressedFontSize;
        SerializedProperty pressedFontMaterial;
        SerializedProperty pressedBackgroundMaterial;
        SerializedProperty pressedItemReturnToSelectedVisuaAfterDelay;
        SerializedProperty pressedItemReturnToSelectedVisualTime;

        bool showDisabledStyles = false;
        SerializedProperty disabledFontSize;
        SerializedProperty disabledFontMaterial;
        SerializedProperty disabledBackgroundMaterial;

        bool showModuleSettings = false;
        SerializedProperty useModules;

        SerializedProperty unSelectModuleContainers;
        SerializedProperty applyUnSelectModuleContainers;

        SerializedProperty onSelectModuleContainers;
        SerializedProperty applyOnSelectModuleContainers;

        SerializedProperty onPressModuleContainers;
        SerializedProperty applyOnPressModuleContainers;

        SerializedProperty applyOnClickModuleContainers;
        SerializedProperty onClickModuleContainers;


        void OnEnable()
        {
            myTarget = (MText_UI_Button)target;
            soTarget = new SerializedObject(target);

            onSelectEvents = soTarget.FindProperty("onSelectEvents");
            whileBeingClickedEvents = soTarget.FindProperty("whileBeingClickedEvents");
            onUnselectEvents = soTarget.FindProperty("onUnselectEvents");
            onClickEvents = soTarget.FindProperty("onClickEvents");

            interactable = soTarget.FindProperty("interactable");
            interactableByMouse = soTarget.FindProperty("interactableByMouse");

            text = soTarget.FindProperty("text");
            background = soTarget.FindProperty("background");


            useStyles = soTarget.FindProperty("useStyles");

            normalFontSize = soTarget.FindProperty("normalFontSize");
            normalFontMaterial = soTarget.FindProperty("normalFontMaterial");
            normalBackgroundMaterial = soTarget.FindProperty("normalBackgroundMaterial");

            applySelectedVisual = soTarget.FindProperty("applySelectedVisual");
            selectedFontSize = soTarget.FindProperty("selectedFontSize");
            selectedFontMaterial = soTarget.FindProperty("selectedFontMaterial");
            selectedBackgroundMaterial = soTarget.FindProperty("selectedBackgroundMaterial");

            applyPressedVisual = soTarget.FindProperty("applyPressedVisual");
            pressedFontSize = soTarget.FindProperty("pressedFontSize");
            pressedFontMaterial = soTarget.FindProperty("pressedFontMaterial");
            pressedBackgroundMaterial = soTarget.FindProperty("pressedBackgroundMaterial");
            pressedItemReturnToSelectedVisuaAfterDelay = soTarget.FindProperty("pressedItemReturnToSelectedVisuaAfterDelay");
            pressedItemReturnToSelectedVisualTime = soTarget.FindProperty("pressedItemReturnToSelectedVisualTime");

            disabledFontSize = soTarget.FindProperty("disabledFontSize");
            disabledFontMaterial = soTarget.FindProperty("disabledFontMaterial");
            disabledBackgroundMaterial = soTarget.FindProperty("disabledBackgroundMaterial");



            useModules = soTarget.FindProperty("useModules");

            unSelectModuleContainers = soTarget.FindProperty("unSelectModuleContainers");
            applyUnSelectModuleContainers = soTarget.FindProperty("applyUnSelectModuleContainers");

            onSelectModuleContainers = soTarget.FindProperty("onSelectModuleContainers");
            applyOnSelectModuleContainers = soTarget.FindProperty("applyOnSelectModuleContainers");

            onPressModuleContainers = soTarget.FindProperty("onPressModuleContainers");
            applyOnPressModuleContainers = soTarget.FindProperty("applyOnPressModuleContainers");

            onClickModuleContainers = soTarget.FindProperty("onClickModuleContainers");
            applyOnClickModuleContainers = soTarget.FindProperty("applyOnClickModuleContainers");

            showEvents = myTarget.showEvents;
            showStyles = myTarget.showStyles;
            showNormalStyles = myTarget.showNormalStyles;
            showSelectedStyles = myTarget.showSelectedStyles;
            showPressedStyles = myTarget.showPressedStyles;
            showDisabledStyles = myTarget.showDisabledStyles;
            showModuleSettings = myTarget.showModuleSettings;
        }

        public override void OnInspectorGUI()
        {
            soTarget.Update();
            EditorGUI.BeginChangeCheck();

            GUILayout.Space(15);
            MainSettings();
            GUILayout.Space(10);

            Styles();
            GUILayout.Space(6);
            Events();
            GUILayout.Space(6);
            ModuleSettings();
            GUILayout.Space(6);

            SaveInspectorLayoutSettings();

            if (EditorGUI.EndChangeCheck())
            {
                soTarget.ApplyModifiedProperties();
                ApplyMyModifiedData();
                EditorUtility.SetDirty(myTarget);
            }
        }
        void ApplyMyModifiedData()
        {
            if (myTarget.interactable)
                myTarget.UnselectedButtonVisualUpdate();
            else
                myTarget.DisabledButtonVisualUpdate();
        }

        void Events()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 1;
            GUIContent content = new GUIContent("Unity Events",
                "On Click: \nMouse click/Touch finished or list pressed enter " +
                "\n\nWhile being Clicked: \nWhen click is pressed down" +
                "\n\nOn Select: \nMouse hover or selected in a list ready to be clicked" +
                "\n\nOn Unselect: \nMouse/Touch moved away or list unselected");
            showEvents = EditorGUILayout.Foldout(showEvents, content, true, EditorStyles.foldout);
            if (showEvents)
            {
                DrawUILine(Color.grey, 1, 2);
                GUILayout.Space(5);

                EditorGUILayout.PropertyField(onClickEvents);
                EditorGUILayout.PropertyField(whileBeingClickedEvents);
                EditorGUILayout.PropertyField(onSelectEvents);
                EditorGUILayout.PropertyField(onUnselectEvents);
            }
            if (!Selection.activeTransform)
            {
                showEvents = false;
            }
            GUILayout.EndVertical();
        }

        void MainSettings()
        {
            GUILayout.BeginVertical("Box");
            EditorGUILayout.PropertyField(text);
            EditorGUILayout.PropertyField(background);

            GUILayout.Space(5);
            DrawUILine(Color.grey);
            GUILayout.Space(5);

            EditorGUILayout.PropertyField(interactable);
            if (myTarget.interactable)
            {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(interactableByMouse, new GUIContent("By mouse/touch"));
            }
            GUILayout.EndVertical();
        }

        void Styles()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 0;

            GUILayout.BeginHorizontal();
            GUIContent content = new GUIContent("Styles", "");
            EditorGUILayout.PropertyField(useStyles, GUIContent.none, GUILayout.MaxWidth(25));
            showStyles = EditorGUILayout.Foldout(showStyles, content, true, EditorStyles.foldout);
            GUILayout.EndHorizontal();

            if (showStyles)
            {
                DrawUILine(Color.grey, 1, 2);
                EditorGUI.indentLevel = 0;
                GUILayout.Space(5);

                Color originalBackgroundColor = GUI.backgroundColor;
                if (settings) GUI.backgroundColor = settings.thirdBackgroundColor;
                else GUI.backgroundColor = new Color(.9f, .9f, .9f);

                NormalStyle();
                OnSelectStyle();
                PressedStye();
                DisabledtStyle();

                GUI.backgroundColor = originalBackgroundColor;
            }
            if (!Selection.activeTransform)
            {
                showSelectedStyles = false;
            }
            GUILayout.EndVertical();
        }
        void NormalStyle()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 1;
            GUIContent content = new GUIContent("Normal - Style", "When nothing else is happening");
            showNormalStyles = EditorGUILayout.Foldout(showNormalStyles, "Normal - Style", true, EditorStyles.foldout);
            if (showNormalStyles)
            {
                DrawUILine(Color.grey, 1, 2);
                EditorGUI.indentLevel = 0;

                Color originalBackgroundColor = GUI.backgroundColor;
                if (settings) GUI.backgroundColor = settings.thirdPropertyFieldColor;
                else GUI.backgroundColor = new Color(.9f, .9f, .9f);

                GUILayout.Space(5);

                if (myTarget.ApplyNormalStyle().Item1)
                {
                    EditorGUILayout.HelpBox("Normal style visuals are being overwritten by parent list", MessageType.Info);
                    GUILayout.Space(5);
                }

                HorizontalFieldShort(normalFontSize, "Font Size");
                HorizontalField(normalFontMaterial, "Font Material");
                HorizontalFieldLong(normalBackgroundMaterial, "Background Material");

                GUI.backgroundColor = originalBackgroundColor;
            }
            if (!Selection.activeTransform)
            {
                showNormalStyles = false;
            }
            GUILayout.EndVertical();
        }
        void OnSelectStyle()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 0;

            GUILayout.BeginHorizontal();
            GUIContent content = new GUIContent("On select - Style", "Mouse hover or selected in a list ready to be clicked");
            EditorGUILayout.PropertyField(applySelectedVisual, GUIContent.none, GUILayout.MaxWidth(25));
            showSelectedStyles = EditorGUILayout.Foldout(showSelectedStyles, content, true, EditorStyles.foldout);
            GUILayout.EndHorizontal();

            if (showSelectedStyles)
            {
                DrawUILine(Color.grey, 1, 2);
                EditorGUI.indentLevel = 0;
                GUILayout.Space(5);
                Color originalBackgroundColor = GUI.backgroundColor;
                if (settings) GUI.backgroundColor = settings.thirdPropertyFieldColor;
                else GUI.backgroundColor = new Color(.9f, .9f, .9f);
                if (myTarget.ApplyOnSelectStyle().Item1)
                {
                    EditorGUILayout.HelpBox("On select style visuals are being overwritten by parent list", MessageType.Info);
                    GUILayout.Space(5);
                }

                HorizontalFieldShort(selectedFontSize, "Font Size");
                HorizontalField(selectedFontMaterial, "Font Material");
                HorizontalFieldLong(selectedBackgroundMaterial, "Background Material");
                GUI.backgroundColor = originalBackgroundColor;
            }
            if (!Selection.activeTransform)
            {
                showSelectedStyles = false;
            }
            GUILayout.EndVertical();
        }
        void PressedStye()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 0;

            GUILayout.BeginHorizontal();
            GUIContent content = new GUIContent("Pressed - Style", "When click is pressed down");
            EditorGUILayout.PropertyField(applyPressedVisual, GUIContent.none, GUILayout.MaxWidth(25));
            showPressedStyles = EditorGUILayout.Foldout(showPressedStyles, content, true, EditorStyles.foldout);
            GUILayout.EndHorizontal();

            if (showPressedStyles)
            {
                DrawUILine(Color.grey, 1, 2);
                EditorGUI.indentLevel = 0;
                GUILayout.Space(5);
                Color originalBackgroundColor = GUI.backgroundColor;
                if (settings) GUI.backgroundColor = settings.thirdPropertyFieldColor;
                else GUI.backgroundColor = new Color(.9f, .9f, .9f);
                if (myTarget.ApplyPressedStyle().Item1)
                {
                    EditorGUILayout.HelpBox("Pressed style visuals are being overwritten by parent list", MessageType.Info);
                    GUILayout.Space(5);
                }

                HorizontalFieldShort(pressedFontSize, "Font Size");
                HorizontalField(pressedFontMaterial, "Font Material");
                HorizontalFieldLong(pressedBackgroundMaterial, "Background Material");

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(pressedItemReturnToSelectedVisuaAfterDelay, GUIContent.none, GUILayout.MaxWidth(15));
                EditorGUILayout.LabelField("Return to 'Selected' Visual");
                GUILayout.EndHorizontal();
                if (myTarget.pressedItemReturnToSelectedVisuaAfterDelay)
                {
                    EditorGUI.indentLevel = 3;
                    HorizontalFieldShort(pressedItemReturnToSelectedVisualTime, "Delay");
                }
                GUI.backgroundColor = originalBackgroundColor;
            }
            if (!Selection.activeTransform)
            {
                showPressedStyles = false;
            }
            GUILayout.EndVertical();
        }
        void DisabledtStyle()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 1;
            GUIContent content = new GUIContent("Disabled - Style", "Button isn't interactable");
            showDisabledStyles = EditorGUILayout.Foldout(showDisabledStyles, content, true, EditorStyles.foldout);
            if (showDisabledStyles)
            {
                DrawUILine(Color.grey, 1, 2);
                EditorGUI.indentLevel = 0;
                GUILayout.Space(5);
                Color originalBackgroundColor = GUI.backgroundColor;
                if (settings) GUI.backgroundColor = settings.thirdPropertyFieldColor;
                else GUI.backgroundColor = new Color(.9f, .9f, .9f);
                if (!myTarget.ApplyDisabledStyle().Item1)
                {
                    EditorGUILayout.HelpBox("Disabled style visuals are being overwritten by parent list", MessageType.Info);
                    GUILayout.Space(5);
                }

                HorizontalFieldShort(disabledFontSize, "Font Size");
                HorizontalField(disabledFontMaterial, "Font Material");
                HorizontalFieldLong(disabledBackgroundMaterial, "Background Material");
                GUI.backgroundColor = originalBackgroundColor;
            }
            if (!Selection.activeTransform)
            {
                showDisabledStyles = false;
            }
            GUILayout.EndVertical();
        }

        void ModuleSettings()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 0;

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(useModules, GUIContent.none, GUILayout.MaxWidth(25));
            showModuleSettings = EditorGUILayout.Foldout(showModuleSettings, "Modules", true, EditorStyles.foldout);
            GUILayout.EndHorizontal();

            if (showModuleSettings)
            {
                DrawUILine(Color.grey, 1, 2);
                ModuleContainerList(applyOnClickModuleContainers, "On Click", "", myTarget.onClickModuleContainers, onClickModuleContainers);
                ModuleContainerList(applyOnPressModuleContainers, "While being clicked", "", myTarget.onPressModuleContainers, onPressModuleContainers);
                ModuleContainerList(applyOnSelectModuleContainers, "On Select modules", "", myTarget.onSelectModuleContainers, onSelectModuleContainers);
                ModuleContainerList(applyUnSelectModuleContainers, "On Un-Select modules", "", myTarget.unSelectModuleContainers, unSelectModuleContainers);
            }
            if (!Selection.activeTransform)
            {
                showModuleSettings = false;
            }
            GUILayout.EndVertical();
        }

        void HorizontalField(SerializedProperty property, string label)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.MaxWidth(100));
            EditorGUILayout.PropertyField(property, GUIContent.none);
            GUILayout.EndHorizontal();
        }
        void HorizontalFieldShort(SerializedProperty property, string label)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.MaxWidth(72.5f));
            EditorGUILayout.PropertyField(property, GUIContent.none);
            GUILayout.EndHorizontal();
        }
        void HorizontalFieldLong(SerializedProperty property, string label)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.MaxWidth(132.5f));
            EditorGUILayout.PropertyField(property, GUIContent.none);
            GUILayout.EndHorizontal();
        }

        void ModuleContainerList(SerializedProperty moduleBoolProperty, string label, string toolTip, List<MText_ModuleContainer> moduleContainers, SerializedProperty serializedContainer)
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
                myTarget.EmptyEffect(moduleContainers);
            }

            GUI.contentColor = originalContent;

            GUILayout.EndVertical();
            GUI.backgroundColor = original;
        }


        private void SaveInspectorLayoutSettings()
        {
            myTarget.showEvents = showEvents;
            myTarget.showStyles = showStyles;
            myTarget.showNormalStyles = showNormalStyles;
            myTarget.showSelectedStyles = showSelectedStyles;
            myTarget.showPressedStyles = showPressedStyles;
            myTarget.showDisabledStyles = showDisabledStyles;
            myTarget.showModuleSettings = showModuleSettings;
        }



        void DrawUILine(Color color, int thickness = 1, int padding = 0)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }
    }
}