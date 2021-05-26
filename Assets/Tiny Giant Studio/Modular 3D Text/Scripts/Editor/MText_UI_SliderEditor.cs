using UnityEngine;
using UnityEditor;

namespace MText
{
    [CustomEditor(typeof(MText_UI_Slider))]
    public class MText_UI_SliderEditor : Editor
    {
        public MText_Settings settings;

        float defaultSmallHorizontalFieldSize = 72.5f;
        float defaultNormalltHorizontalFieldSize = 100;
        float defaultLargeHorizontalFieldSize = 120f;
        float defaultExtraLargeHorizontalFieldSize = 150f;


        MText_UI_Slider myTarget;
        SerializedObject soTarget;

        SerializedProperty autoFocusOnGameStart;
        SerializedProperty interactable;

        SerializedProperty minValue;
        SerializedProperty maxValue;
        SerializedProperty handle;
        SerializedProperty background;
        SerializedProperty backgroundSize;

        SerializedProperty keyControl;
        SerializedProperty keyStep;
        SerializedProperty scrollUp;
        SerializedProperty scrollDown;


        SerializedProperty handleGraphic;
        SerializedProperty progressBar;
        SerializedProperty selectedHandleMat;
        SerializedProperty unSelectedHandleMat;
        SerializedProperty clickedHandleMat;
        SerializedProperty disabledHandleMat;

        SerializedProperty useEvents;
        SerializedProperty onValueChanged;
        SerializedProperty sliderDragEnded;

        SerializedProperty useValueRangeEvents;
        SerializedProperty usePercentage;
        SerializedProperty valueRangeEvents;

        float value;

        bool showKeyboardSettings = false;
        bool showVisualSettings = false;
        bool showEventsSettings = false;
        bool showValueRangeSettings = false;

        string[] directionOptions = new[] { "Left to Right", "Right to Left" };
        enum FieldSize
        {
            small,
            normal,
            large,
            extraLarge
        }

        

        void OnEnable()
        {
            myTarget = (MText_UI_Slider)target;
            soTarget = new SerializedObject(target);

            autoFocusOnGameStart = soTarget.FindProperty("autoFocusOnGameStart");
            interactable = soTarget.FindProperty("interactable");

            minValue = soTarget.FindProperty("minValue");
            maxValue = soTarget.FindProperty("maxValue");
            handle = soTarget.FindProperty("handle");
            background = soTarget.FindProperty("background");
            backgroundSize = soTarget.FindProperty("backgroundSize");

            keyControl = soTarget.FindProperty("keyControl");
            keyStep = soTarget.FindProperty("keyStep");
            scrollUp = soTarget.FindProperty("scrollUp");
            scrollDown = soTarget.FindProperty("scrollDown");

            onValueChanged = soTarget.FindProperty("onValueChanged");
            sliderDragEnded = soTarget.FindProperty("sliderDragEnded");
            useEvents = soTarget.FindProperty("useEvents");

            handleGraphic = soTarget.FindProperty("handleGraphic");
            progressBar = soTarget.FindProperty("progressBar");
            selectedHandleMat = soTarget.FindProperty("selectedHandleMat");
            unSelectedHandleMat = soTarget.FindProperty("unSelectedHandleMat");
            clickedHandleMat = soTarget.FindProperty("clickedHandleMat");
            disabledHandleMat = soTarget.FindProperty("disabledHandleMat");

            useValueRangeEvents = soTarget.FindProperty("useValueRangeEvents");
            usePercentage = soTarget.FindProperty("usePercentage");
            valueRangeEvents = soTarget.FindProperty("valueRangeEvents");


            showKeyboardSettings = myTarget.showKeyboardSettings;
            showVisualSettings = myTarget.showVisualSettings;
            showEventsSettings = myTarget.showEventsSettings;
            showValueRangeSettings = myTarget.showValueRangeSettings;
        }

        public override void OnInspectorGUI()
        {
            soTarget.Update();
            EditorGUI.BeginChangeCheck();

            MainSettings();
            GUILayout.Space(10);
            VisualSettings();
            KeyboardSettings();
            EventSettings();
            ValueRanges();

            SaveInspectorLayoutSettings();

            if (EditorGUI.EndChangeCheck())
            {
                soTarget.ApplyModifiedProperties();
                ApplyModifiedValuesToHandleAndBackground();
                EditorUtility.SetDirty(myTarget);
            }
        }

        void EventSettings()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 0;

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(useEvents, GUIContent.none, GUILayout.MaxWidth(25));
            showEventsSettings = EditorGUILayout.Foldout(showEventsSettings, "Unit Events", true, EditorStyles.foldout);
            GUILayout.EndHorizontal();

            if (showEventsSettings)
            {
                DrawUILine(Color.grey, 1, 2);
                EditorGUILayout.PropertyField(onValueChanged);
                EditorGUILayout.PropertyField(sliderDragEnded);

                GUILayout.Space(5);
            }
            if (!Selection.activeTransform)
            {
                showEventsSettings = false;
            }
            GUILayout.EndVertical();
        }

        void MainSettings()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 0;

            DrawUILine(Color.grey, 1, 2);
            HorizontalField(autoFocusOnGameStart, "Auto Focus", "Selects on Awake() \nSelected items can be controlled by keyboard.\nIf it's in a list, this is controlled by list", FieldSize.small);
            HorizontalField(interactable, "Interactable", "", FieldSize.small);
            DrawUILine(Color.grey, 1, 2);

            GUILayout.Space(5);

            EditorGUILayout.PropertyField(minValue);
            EditorGUILayout.PropertyField(maxValue);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Value", GUILayout.MaxWidth(100));
            value = EditorGUILayout.Slider(myTarget.Value, myTarget.minValue, myTarget.maxValue);
            GUILayout.EndHorizontal();

            GUILayout.Space(2.5f);
            DrawUILine(Color.grey, 1, 2);
            GUILayout.Space(2.5f);

            EditorGUILayout.PropertyField(handle);
            EditorGUILayout.PropertyField(progressBar);

            EditorGUILayout.PropertyField(background);
            EditorGUILayout.PropertyField(backgroundSize);

            myTarget.directionChoice = EditorGUILayout.Popup(myTarget.directionChoice, directionOptions);

            //if (!myTarget.handle)
            //{
            //    EditorGUILayout.HelpBox("A slider handle script is required. Even if it's disabled", MessageType.Warning);
            //}

            GUILayout.EndVertical();
        }

        void KeyboardSettings()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 0;

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(keyControl, GUIContent.none, GUILayout.MaxWidth(25));
            showKeyboardSettings = EditorGUILayout.Foldout(showKeyboardSettings, "Keyboard Control", true, EditorStyles.foldout);
            GUILayout.EndHorizontal();

            if (showKeyboardSettings)
            {
                DrawUILine(Color.grey, 1, 2);

                EditorGUILayout.PropertyField(keyStep);
                EditorGUILayout.PropertyField(scrollUp);
                EditorGUILayout.PropertyField(scrollDown);

                GUILayout.Space(5);
            }
            if (!Selection.activeTransform)
            {
                showKeyboardSettings = false;
            }
            GUILayout.EndVertical();
            GUILayout.Space(10);
        }

        void VisualSettings()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 1;

            showVisualSettings = EditorGUILayout.Foldout(showVisualSettings, "Visuals", true, EditorStyles.foldout);

            if (showVisualSettings)
            {
                DrawUILine(Color.grey, 1, 2);
                EditorGUI.indentLevel = 0;

                EditorGUILayout.PropertyField(handleGraphic);
                EditorGUILayout.PropertyField(selectedHandleMat);
                EditorGUILayout.PropertyField(unSelectedHandleMat);
                EditorGUILayout.PropertyField(clickedHandleMat);
                EditorGUILayout.PropertyField(disabledHandleMat);

                GUILayout.Space(5);
            }
            if (!Selection.activeTransform)
            {
                showVisualSettings = false;
            }
            GUILayout.EndVertical();
            GUILayout.Space(10);
        }
        void ValueRanges()
        {
            GUILayout.Space(10);
            GUIContent tabName = new GUIContent("Value Range Events", "Stuff to happen when slider value enters a specific range.\nChecks value in top to down order. If you have two ranges that can be fulfilled at the same time, the first one gets called");

            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 0;

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(useValueRangeEvents, GUIContent.none, GUILayout.MaxWidth(25));
            showValueRangeSettings = EditorGUILayout.Foldout(showValueRangeSettings, tabName, true, EditorStyles.foldout);
            GUILayout.EndHorizontal();

            Color color = GUI.backgroundColor;

            if (showValueRangeSettings)
            {
                if (settings)
                    GUI.backgroundColor = settings.thirdBackgroundColor;
                else
                    GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f);

                DrawUILine(Color.grey, 1, 2);
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(usePercentage);
                for (int i = 0; i < myTarget.valueRangeEvents.Count; i++)
                {

                    GUILayout.BeginVertical("Box");
                    EditorGUI.indentLevel = 0;
                    GUILayout.Space(5);

                    EditorGUILayout.PropertyField(valueRangeEvents.GetArrayElementAtIndex(i).FindPropertyRelative("min"), new GUIContent("Minimum value"));
                    EditorGUILayout.PropertyField(valueRangeEvents.GetArrayElementAtIndex(i).FindPropertyRelative("max"), new GUIContent("Maximum value"));

                    if (myTarget.usePercentage && (myTarget.valueRangeEvents[i].min > 100 || myTarget.valueRangeEvents[i].max > 100))
                        EditorGUILayout.HelpBox("A range value is greater than 100 percant. Uncheck Use Percentage if you want to use direct values", MessageType.Warning);

                    if (myTarget.valueRangeEvents[i].min > myTarget.valueRangeEvents[i].max)
                        EditorGUILayout.HelpBox("Minimum value is greater than maximum", MessageType.Warning);


                    GUILayout.Space(2);

                    EditorGUILayout.PropertyField(valueRangeEvents.GetArrayElementAtIndex(i).FindPropertyRelative("icon"));
                    EditorGUILayout.PropertyField(valueRangeEvents.GetArrayElementAtIndex(i).FindPropertyRelative("oneTimeEvents"));
                    EditorGUILayout.PropertyField(valueRangeEvents.GetArrayElementAtIndex(i).FindPropertyRelative("repeatEvents"), new GUIContent("Gets called everytime slider value is changed at this range", ""));

                    GUI.contentColor = Color.black;
                    if (GUILayout.Button("X", GUILayout.MaxHeight(20)))
                    {
                        myTarget.valueRangeEvents.RemoveAt(i);
                    }
                    GUILayout.Space(5);

                    GUILayout.EndVertical();

                    GUILayout.Space(10);
                }

                if (GUILayout.Button("+", GUILayout.MinHeight(20)))
                {
                    myTarget.NewValueRange();
                }
            }
            GUI.backgroundColor = color;

            if (!Selection.activeTransform)
            {
                showValueRangeSettings = false;
            }
            GUILayout.EndVertical();
        }




        void ApplyModifiedValuesToHandleAndBackground()
        {
            if (value != myTarget.Value)
                myTarget.Value = value;

            if (myTarget.background)
                myTarget.background.localScale = new Vector3(myTarget.backgroundSize, myTarget.background.localScale.y, myTarget.background.localScale.z);

            if (myTarget)
                myTarget.UpdateGraphic();

            if (myTarget.interactable)
                myTarget.UnSelectedVisual();
            else
                myTarget.DisabledVisual();
        }

        void SaveInspectorLayoutSettings()
        {
            myTarget.showKeyboardSettings = showKeyboardSettings;
            myTarget.showVisualSettings = showVisualSettings;
            myTarget.showEventsSettings = showEventsSettings;
            myTarget.showValueRangeSettings = showValueRangeSettings;
        }


        void HorizontalField(SerializedProperty property, string label, string tooltip = "", FieldSize fieldSize = FieldSize.normal)
        {
            float myMaxWidth = 100;
            //not to self: it's ternary operator not tarnary operator. Stop mistaking
            if (settings)
                myMaxWidth = fieldSize == FieldSize.small ? settings.smallHorizontalFieldSize : fieldSize == FieldSize.normal ? settings.normalltHorizontalFieldSize : fieldSize == FieldSize.large ? settings.largeHorizontalFieldSize : fieldSize == FieldSize.extraLarge ? settings.extraLargeHorizontalFieldSize : settings.normalltHorizontalFieldSize;
            else
                myMaxWidth = fieldSize == FieldSize.small ? defaultSmallHorizontalFieldSize : fieldSize == FieldSize.normal ? defaultNormalltHorizontalFieldSize : fieldSize == FieldSize.large ? defaultLargeHorizontalFieldSize : fieldSize == FieldSize.extraLarge ? defaultExtraLargeHorizontalFieldSize : settings.normalltHorizontalFieldSize;

            GUILayout.BeginHorizontal();
            GUIContent gUIContent = new GUIContent(label, tooltip);
            EditorGUILayout.LabelField(gUIContent, GUILayout.MaxWidth(myMaxWidth));
            EditorGUILayout.PropertyField(property, GUIContent.none);
            GUILayout.EndHorizontal();
        }

        void DrawUILine(Color color, int thickness = 1, int padding = 2)
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