using UnityEngine;
using UnityEditor;

namespace MText
{
    [CustomEditor(typeof(Mtext_UI_InputField))]
    public class Mtext_UI_InputFieldEditor : Editor
    {
        public MText_Settings settings;

        float defaultSmallHorizontalFieldSize = 72.5f;
        float defaultNormalltHorizontalFieldSize = 100;
        float defaultLargeHorizontalFieldSize = 120f;
        float defaultExtraLargeHorizontalFieldSize = 150f;


        Mtext_UI_InputField myTarget;
        SerializedObject soTarget;

        SerializedProperty autoFocusOnGameStart;
        SerializedProperty interactable;

        SerializedProperty maxCharacter;
        SerializedProperty typingSymbol;
        SerializedProperty enterKeyEndsInput;


        SerializedProperty textComponent;
        SerializedProperty background;

        SerializedProperty text;
        SerializedProperty placeHolderText;

        SerializedProperty placeHolderTextMat;

        SerializedProperty inFocusTextMat;
        SerializedProperty inFocusBackgroundMat;

        SerializedProperty outOfFocusTextMat;
        SerializedProperty outOfFocusBackgroundMat;

        SerializedProperty disabledTextMat;
        SerializedProperty disabledBackgroundMat;

        SerializedProperty typeSound;
        SerializedProperty audioSource;

        SerializedProperty onInput;
        SerializedProperty onBackspace;
        SerializedProperty onInputEnd;

        bool showMainSettings = true;
        bool showStyleSettings = false;
        bool showAudioSettings = false;
        bool showUnityEventSettings = false;

        enum FieldSize
        {
            small,
            normal,
            large,
            extraLarge
        }


        void OnEnable()
        {
            myTarget = (Mtext_UI_InputField)target;
            soTarget = new SerializedObject(target);

            autoFocusOnGameStart = soTarget.FindProperty("autoFocusOnGameStart");
            interactable = soTarget.FindProperty("interactable");

            maxCharacter = soTarget.FindProperty("maxCharacter");
            typingSymbol = soTarget.FindProperty("typingSymbol");
            enterKeyEndsInput = soTarget.FindProperty("enterKeyEndsInput");

            textComponent = soTarget.FindProperty("textComponent");
            background = soTarget.FindProperty("background");

            text = soTarget.FindProperty("_text");
            placeHolderText = soTarget.FindProperty("placeHolderText");


            placeHolderTextMat = soTarget.FindProperty("placeHolderTextMat");

            inFocusTextMat = soTarget.FindProperty("inFocusTextMat");
            inFocusBackgroundMat = soTarget.FindProperty("inFocusBackgroundMat");

            outOfFocusTextMat = soTarget.FindProperty("outOfFocusTextMat");
            outOfFocusBackgroundMat = soTarget.FindProperty("outOfFocusBackgroundMat");

            disabledTextMat = soTarget.FindProperty("disabledTextMat");
            disabledBackgroundMat = soTarget.FindProperty("disabledBackgroundMat");


            disabledBackgroundMat = soTarget.FindProperty("disabledBackgroundMat");

            typeSound = soTarget.FindProperty("typeSound");
            audioSource = soTarget.FindProperty("audioSource");

            onInput = soTarget.FindProperty("onInput");
            onBackspace = soTarget.FindProperty("onBackspace");
            onInputEnd = soTarget.FindProperty("onInputEnd");


            showMainSettings = myTarget.showMainSettings;
            showStyleSettings = myTarget.showStyleSettings;
            showAudioSettings = myTarget.showAudioSettings;
            showUnityEventSettings = myTarget.showUnityEventSettings;
        }

        public override void OnInspectorGUI()
        {
            soTarget.Update();
            EditorGUI.BeginChangeCheck();

            GUILayout.Space(10);
            MainSettings();
            StyleSettings();
            AudioSettings();
            UnityEventsSettings();

            SaveInspectorLayoutSettings();

            if (EditorGUI.EndChangeCheck())
            {
                myTarget.UpdateText();
                soTarget.ApplyModifiedProperties();
                ApplyModifiedValuesToGraphics();
                EditorUtility.SetDirty(myTarget);
            }
        }

        void MainSettings()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 1;
            GUIContent content = new GUIContent("Main Settings");
            showMainSettings = EditorGUILayout.Foldout(showMainSettings, content, true, EditorStyles.foldout);
            if (showMainSettings)
            {
                EditorGUI.indentLevel = 0;

                DrawUILine(Color.grey, 1, 2);
                if (!MText_Utilities.GetParentList(myTarget.transform))
                    HorizontalField(autoFocusOnGameStart, "Auto Focus", "This focuses the element on game start. Focused = you can type to give input.");
                HorizontalField(interactable, "Interactable");
                DrawUILine(Color.grey, 1, 2);

                HorizontalField(text, "Text");
                HorizontalField(placeHolderText, "Placeholder");
                HorizontalField(maxCharacter, "Max Character");
                HorizontalField(typingSymbol, "Typing Symbol");
                HorizontalField(enterKeyEndsInput, "Enter Key Ends Input", "", FieldSize.extraLarge);
                DrawUILine(Color.grey, 1, 2);

                if (!myTarget.textComponent)
                    EditorGUILayout.HelpBox("Text Component is required", MessageType.Error);
                HorizontalField(textComponent, "Text Component", "Reference to the 3D Text where input will be shown");
                HorizontalField(background, "Background");
            }
            if (!Selection.activeTransform)
            {
                showMainSettings = false;
            }
            GUILayout.EndVertical();
        }
        void StyleSettings()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 1;
            GUIContent content = new GUIContent("Style Settings");
            showStyleSettings = EditorGUILayout.Foldout(showStyleSettings, content, true, EditorStyles.foldout);
            if (showStyleSettings)
            {
                EditorGUI.indentLevel = 0;

                DrawUILine(Color.grey, 1, 2);

                EditorGUILayout.LabelField("Font Metarial");
                EditorGUI.indentLevel = 1;
                HorizontalField(placeHolderTextMat, "Placeholder");
                HorizontalField(inFocusTextMat, "In Focus");
                HorizontalField(outOfFocusTextMat, "Out of Focus");
                HorizontalField(disabledTextMat, "Disabled");

                EditorGUI.indentLevel = 0;
                EditorGUILayout.LabelField("Background Metarial");
                EditorGUI.indentLevel = 1;
                HorizontalField(inFocusBackgroundMat, "In Focus");
                HorizontalField(outOfFocusBackgroundMat, "Out of Focus");
                HorizontalField(disabledBackgroundMat, "Disabled");
            }
            if (!Selection.activeTransform)
            {
                showStyleSettings = false;
            }
            GUILayout.EndVertical();
        }
        void AudioSettings()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 1;
            GUIContent content = new GUIContent("Audio Settings");
            showAudioSettings = EditorGUILayout.Foldout(showAudioSettings, content, true, EditorStyles.foldout);
            if (showAudioSettings)
            {
                EditorGUI.indentLevel = 0;

                DrawUILine(Color.grey, 1, 2);

                HorizontalField(typeSound, "Type Sound");
                HorizontalField(audioSource, "Audio Source");
            }
            if (!Selection.activeTransform)
            {
                showAudioSettings = false;
            }
            GUILayout.EndVertical();
        }

        void UnityEventsSettings()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 1;
            GUIContent content = new GUIContent("Unity Events");
            showUnityEventSettings = EditorGUILayout.Foldout(showUnityEventSettings, content, true, EditorStyles.foldout);
            if (showUnityEventSettings)
            {
                EditorGUILayout.PropertyField(onInput);
                EditorGUILayout.PropertyField(onBackspace);
                EditorGUILayout.PropertyField(onInputEnd);
            }
            if (!Selection.activeTransform)
            {
                showUnityEventSettings = false;
            }
            GUILayout.EndVertical();
        }

        void ApplyModifiedValuesToGraphics()
        {
            if (!myTarget.interactable)
                myTarget.Uninteractable();
            else
                myTarget.Interactable();
        }



        private void SaveInspectorLayoutSettings()
        {
            myTarget.showMainSettings = showMainSettings;
            myTarget.showStyleSettings = showStyleSettings;
            myTarget.showAudioSettings = showAudioSettings;
            myTarget.showUnityEventSettings = showUnityEventSettings;
        }



        void HorizontalField(SerializedProperty property, string label, string tooltip = "", FieldSize fieldSize = FieldSize.normal)
        {
            float myMaxWidth = 100;
            //not to self: it's ternary operator not tarnary operator. Stop mistyping
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