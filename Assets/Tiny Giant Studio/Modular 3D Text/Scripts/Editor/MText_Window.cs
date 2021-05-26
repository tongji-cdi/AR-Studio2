/// Created by Ferdowsur Asif @ Tiny Giant Studio
using UnityEngine;
using UnityEditor;
using MText.FontCreation;
using System.Collections.Generic;
using System.Linq;
using MText.EditorHelper;

namespace MText
{
    public class MText_Window : EditorWindow
    {
        SerializedObject soTarget;

        public MText_Settings settings;
        private string selectedTab = "Getting Started";
        //private bool neverStartedBefore = true;

        GUIStyle tabStyle;

        void OnEnable()
        {
            if (settings && soTarget == null)
            {
                soTarget = new SerializedObject(settings);
                VerifyReferences();
            }
        }

        [MenuItem("Tools/Tiny Giant Studio/Modular 3D Text", false, 100)]
        public static void ShowWindow()
        {
            if (Application.isPlaying)
            {
                Debug.Log("");
                Application.Quit();
            }

            EditorWindow editorWindow = GetWindow(typeof(MText_Window), false, "Modular 3D Text");
            editorWindow.minSize = new Vector2(400, 650);
            editorWindow.Show();
        }


        void VerifyReferences()
        {
            if(!settings)
                settings = MText_FindResource.VerifySettings(settings);
        }


        void OnGUI()
        {
            if (settings && soTarget == null)
            {
                soTarget = new SerializedObject(settings);
            }

            GenerateStyle();
            EditorGUI.BeginChangeCheck();

            GUILayout.Space(5);
            EditorGUILayout.BeginVertical("Box");

            Tabs();
            DrawUILine(Color.gray);

            GUILayout.Space(10);

            //if (selectedTab == "Customization")
            //    Preference();
            if (selectedTab == "Feedback")
                Feedback();
            else if (selectedTab == "Font Creation")
                FontCreation();
            else
                GeneralInformation();


            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndVertical();

            GUILayout.Space(5f);
            DrawUILine(Color.grey, 1, 2);
            ProductInformation();
            GUILayout.Space(5f);


            if (EditorGUI.EndChangeCheck())
            {
                if (soTarget != null) soTarget.ApplyModifiedProperties();
                EditorUtility.SetDirty(settings);
            }
        }

        void GenerateStyle()
        {
            if (tabStyle == null)
            {
                tabStyle = new GUIStyle(GUI.skin.button);
                tabStyle.margin = new RectOffset(0, 0, tabStyle.margin.top, tabStyle.margin.bottom);
            }
        }
        bool LeftButton(GUIContent content)
        {
            bool clicked = false;
            Rect rect = GUILayoutUtility.GetRect(20, 20);
            GUI.BeginGroup(rect);
            if (GUI.Button(new Rect(0, 0, rect.width + tabStyle.border.right, rect.height), content, tabStyle))
                clicked = true;

            GUI.EndGroup();
            return clicked;
        }
        bool MidButton(GUIContent content)
        {
            bool clicked = false;
            Rect rect = GUILayoutUtility.GetRect(20, 20);
            GUI.BeginGroup(rect);
            if (GUI.Button(new Rect(-tabStyle.border.left, 0, rect.width + tabStyle.border.left + tabStyle.border.right, rect.height), content, tabStyle))
                clicked = true;
            GUI.EndGroup();
            return clicked;
        }
        bool RightButton(GUIContent content)
        {
            bool clicked = false;
            Rect rect = GUILayoutUtility.GetRect(20, 20);
            GUI.BeginGroup(rect);
            if (GUI.Button(new Rect(-tabStyle.border.left, 0, rect.width + tabStyle.border.left, rect.height), content, tabStyle))
                clicked = true;
            GUI.EndGroup();
            return clicked;
        }

        void Tabs()
        {
            Color originalBackgroundColor = GUI.backgroundColor;
            GUILayout.BeginHorizontal();

            if (selectedTab == "Getting Started")
            {
                if (settings)
                    GUI.backgroundColor = settings.thirdBackgroundColor;
                else
                    GUI.backgroundColor = Color.grey;
            }
            GUIContent gettingStarted = new GUIContent("Getting Started");
            if (LeftButton(gettingStarted))
            {
                selectedTab = "Getting Started";

                if (settings)
                    settings.selectedTab = selectedTab;
            }
            GUI.backgroundColor = originalBackgroundColor;




            if (selectedTab == "Customization")
            {
                if (settings)
                    GUI.backgroundColor = settings.thirdBackgroundColor;
                else
                    GUI.backgroundColor = Color.grey;
            }


            //GUIContent Customization = new GUIContent("Preference");
            //if (MidButton(Customization))
            //{
            //    selectedTab = "Customization";

            //    if (settings)
            //        settings.selectedTab = selectedTab;
            //}
            //GUI.backgroundColor = originalBackgroundColor;



            if (selectedTab == "Font Creation")
            {
                if (settings)
                    GUI.backgroundColor = settings.thirdBackgroundColor;
                else
                    GUI.backgroundColor = Color.grey;
            }
            GUIContent fontCreation = new GUIContent("Font Creation");
            if (MidButton(fontCreation))
            {
                selectedTab = "Font Creation";

                if (settings)
                    settings.selectedTab = selectedTab;
            }
            GUI.backgroundColor = originalBackgroundColor;

            if (selectedTab == "Feedback")
            {
                if (settings)
                    GUI.backgroundColor = settings.thirdBackgroundColor;
                else
                    GUI.backgroundColor = Color.grey;
            }
            GUIContent Feedback = new GUIContent("Feedback");
            if (RightButton(Feedback))
            {
                selectedTab = "Feedback";

                if (settings)
                    settings.selectedTab = selectedTab;
            }

            GUI.backgroundColor = originalBackgroundColor;
            GUILayout.EndHorizontal();
        }

        void GeneralInformation()
        {
            StartInfo();
            DrawUILine(Color.gray);

            EditorGUILayout.LabelField("Offline documentation is in asset directory");
            HorizontalButtonURL("Online Documentation", "https://docs.google.com/document/d/11Mb2H-b6QX79MpnHkNb-1Bcc7zm-A7wrMdZcEkis8UY");
            HorizontalButtonURL("(Unity) Forum", "https://forum.unity.com/threads/modular-3d-text-3d-texts-for-your-3d-game.821931/");

            GUILayout.BeginHorizontal();
            HorizontalButtonURL("How to make fonts (video)", "https://youtu.be/2ixgOJ_sXtI");
            HorizontalButtonURL("How to make fonts (written)", "https://docs.google.com/document/d/11Mb2H-b6QX79MpnHkNb-1Bcc7zm-A7wrMdZcEkis8UY/edit#heading=h.vw07a3ihsmyb");
            GUILayout.EndHorizontal();

            //HorizontalButtonURL("Rate the asset", "https://assetstore.unity.com/packages/3d/props/tools/modular-3d-text-159508");
        }
        void StartInfo()
        {
            EditorGUILayout.LabelField("Welcome to Modular 3D Text.");

            GUILayout.Space(5f);

            EditorGUILayout.LabelField("To create 3D UIs,", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Right click in your scene hierarchy, \n3D Objects > Modular 3D Text > Your UI ELement.", MessageType.Info);

            GUILayout.Space(5f);
        }

        //void Preference()
        //{
        //    if (settings)
        //    {
        //        GUILayout.Label("Default values", EditorStyles.boldLabel);


        //        EditorGUILayout.BeginHorizontal();
        //        EditorGUILayout.LabelField("Font", GUILayout.MaxWidth(120));
        //        EditorGUILayout.PropertyField(soTarget.FindProperty("defaultFont"), GUIContent.none);
        //        if (GUILayout.Button("Apply to scene", GUILayout.MaxWidth(100)))
        //        {
        //            ApplyDefaultFontToScene();
        //        }
        //        EditorGUILayout.EndHorizontal();
        //        GUILayout.Space(5f);

        //        EditorGUILayout.BeginHorizontal();
        //        EditorGUILayout.LabelField("Text Size", GUILayout.MaxWidth(120));
        //        EditorGUILayout.PropertyField(soTarget.FindProperty("defaultTextSize"), GUIContent.none);
        //        if (GUILayout.Button("Apply to scene", GUILayout.MaxWidth(100)))
        //        {
        //            ApplyDefaultTextSizeToScene();
        //        }
        //        EditorGUILayout.EndHorizontal();

        //        GUILayout.Space(5f);
        //        EditorGUILayout.BeginHorizontal();
        //        EditorGUILayout.LabelField("Text Material", GUILayout.MaxWidth(120));
        //        EditorGUILayout.PropertyField(soTarget.FindProperty("defaultTextMaterial"), GUIContent.none);
        //        if (GUILayout.Button("Apply to scene", GUILayout.MaxWidth(100)))
        //        {
        //            ApplyDefaultTextMaterialToScene();
        //        }
        //        EditorGUILayout.EndHorizontal();
        //        GUILayout.Space(5f);



        //        GUILayout.Space(5f);
        //        EditorGUILayout.BeginHorizontal();
        //        EditorGUILayout.LabelField("Auto Create In Editor", GUILayout.MinWidth(125));
        //        EditorGUILayout.PropertyField(soTarget.FindProperty("autoCreateInEditorMode"), GUIContent.none, GUILayout.MaxWidth(50));
        //        if (GUILayout.Button("Apply to scene", GUILayout.MaxWidth(100)))
        //        {
        //            ApplyDefaultAutoCreateInEditor();
        //        }
        //        EditorGUILayout.EndHorizontal();
        //        GUILayout.Space(5f);

        //        GUILayout.Space(5f);
        //        EditorGUILayout.BeginHorizontal();
        //        EditorGUILayout.LabelField("Auto Create In Play mode", GUILayout.MinWidth(125));
        //        EditorGUILayout.PropertyField(soTarget.FindProperty("autoCreateInPlayMode"), GUIContent.none, GUILayout.MaxWidth(50));
        //        if (GUILayout.Button("Apply to scene", GUILayout.MaxWidth(100)))
        //        {
        //            ApplyDefaultAutoCreateInPlayMode();
        //        }
        //        EditorGUILayout.EndHorizontal();
        //        GUILayout.Space(5f);



        //        EditorGUILayout.LabelField("Button", EditorStyles.boldLabel);


        //        EditorGUILayout.BeginHorizontal();

        //        EditorGUILayout.LabelField("Normal Background Material", GUILayout.MaxWidth(160));
        //        EditorGUILayout.PropertyField(soTarget.FindProperty("defaultBackgroundMaterial"), GUIContent.none);
        //        if (GUILayout.Button("Apply to scene", GUILayout.MaxWidth(100)))
        //            ApplyDefaultButtonNormalBackgroundMaterialToScene();

        //        EditorGUILayout.EndHorizontal();
        //    }

        //    EditorGUILayout.HelpBox("Under Construction. Expect a LOT more options in the future.", MessageType.Info);

        //}
        void Feedback()
        {
            HorizontalButtonURL("(Unity) Forum", "https://forum.unity.com/threads/modular-3d-text-3d-texts-for-your-3d-game.821931/");
            string forumMsg = "Join the conversation in Unity Forum.Submit any feature requests, issues you have run into. I always try my best to help with anything I can.";
            EditorGUILayout.HelpBox(forumMsg, MessageType.Info);

            GUILayout.Space(15f);
            HorizontalButtonURL("Rate the asset", "https://assetstore.unity.com/packages/3d/props/tools/modular-3d-text-159508");
            EditorGUILayout.HelpBox("As a new asset publisher without any marketing skill, reviews are primary method of getting discovered.\nPlease leave a review if you have the time and help me continuously improve the asset.", MessageType.Info);

            GUILayout.Space(10);

            EditorGUILayout.LabelField("Feel free to contact directly via mail. Always happy to help.");
            GUILayout.Label("Support: Asifno13@gmail.com", EditorStyles.boldLabel);
        }
        void ProductInformation()
        {
            GUILayout.Label("Modular 3D Text \nVersion: 2.0.2", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Created by Ferdowsur Asif @ Tiny Giant Studios.", EditorStyles.miniBoldLabel);
        }

        void ApplyDefaultFontToScene()
        {
            string notice = "Are you sure you want to apply '" + settings.defaultFont.name + "' font to every text in the scene?" +
                "You can't press Undo for this action.";
            if (EditorUtility.DisplayDialog("Confirmation", notice, "Apply", "Do not apply"))
            {
                Modular3DText[] modular3DTexts = FindObjectsOfType<Modular3DText>();
                for (int i = 0; i < modular3DTexts.Length; i++)
                {
                    modular3DTexts[i].Font = settings.defaultFont;
                    modular3DTexts[i].UpdateText();
                }
            }
        }
        void ApplyDefaultTextSizeToScene()
        {
            string notice = "Are you sure you want to apply '" + settings.defaultTextSize + "' font size to every text in the scene?" +
                "You can't press Undo for this action.";
            if (EditorUtility.DisplayDialog("Confirmation", notice, "Apply", "Do not apply"))
            {
                Modular3DText[] modular3DTexts = FindObjectsOfType<Modular3DText>();
                for (int i = 0; i < modular3DTexts.Length; i++)
                {
                    modular3DTexts[i].FontSize = settings.defaultTextSize;
                    modular3DTexts[i].UpdateText();
                }

                MText_UI_Button[] buttons = FindObjectsOfType<MText_UI_Button>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].normalFontSize = settings.defaultTextSize;
                }
            }
        }
        void ApplyDefaultTextMaterialToScene()
        {
            string notice = "Are you sure you want to apply '" + settings.defaultTextMaterial.name + "' material to every button in the scene?" +
                "You can't press Undo for this action.";
            if (EditorUtility.DisplayDialog("Confirmation", notice, "Apply", "Do not apply"))
            {
                Modular3DText[] modular3DTexts = FindObjectsOfType<Modular3DText>();
                for (int i = 0; i < modular3DTexts.Length; i++)
                {
                    modular3DTexts[i].Material = settings.defaultTextMaterial;
                    modular3DTexts[i].UpdateText();
                }

                MText_UI_Button[] buttons = FindObjectsOfType<MText_UI_Button>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].normalFontMaterial = settings.defaultTextMaterial;
                }
            }
        }
        //void ApplyDefaultAutoCreateInEditor()
        //{
        //    string notice = "Are you sure you want to apply this setting to every text in the scene?" +
        //        "You can't press Undo for this action.";
        //    if (EditorUtility.DisplayDialog("Confirmation", notice, "Apply", "Do not apply"))
        //    {
        //        Modular3DText[] modular3DTexts = FindObjectsOfType<Modular3DText>();
        //        for (int i = 0; i < modular3DTexts.Length; i++)
        //        {
        //            modular3DTexts[i].autoCreateInEditor = settings.autoCreateInEditorMode;
        //        }
        //    }
        //}
        //void ApplyDefaultAutoCreateInPlayMode()
        //{
        //    string notice = "Are you sure you want to apply this setting to every text in the scene?" +
        //        "You can't press Undo for this action.";
        //    if (EditorUtility.DisplayDialog("Confirmation", notice, "Apply", "Do not apply"))
        //    {
        //        Modular3DText[] modular3DTexts = FindObjectsOfType<Modular3DText>();
        //        for (int i = 0; i < modular3DTexts.Length; i++)
        //        {
        //            modular3DTexts[i].autoCreateInPlayMode = settings.autoCreateInPlayMode;
        //        }
        //    }
        //}
        //Button
        void ApplyDefaultButtonNormalBackgroundMaterialToScene()
        {
            string notice = "Are you sure you want to apply '" + settings.defaultBackgroundMaterial.name + "' to every text in the scene?" +
                "You can't press Undo for this action.";
            if (EditorUtility.DisplayDialog("Confirmation", notice, "Apply", "Do not apply"))
            {
                MText_UI_Button[] buttons = FindObjectsOfType<MText_UI_Button>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].normalBackgroundMaterial = settings.defaultBackgroundMaterial;
                    buttons[i].UnselectedButtonVisualUpdate();
                }
            }
        }

        void FontCreation()
        {
            Color normalColor = GUI.backgroundColor;
            GUI.backgroundColor = settings.thirdPropertyFieldColor;
            if (GUILayout.Button("Create Font", GUILayout.MinHeight(50)))
            {
                CreateFont();
            }
            GUI.backgroundColor = normalColor;
            GUILayout.Space(15);



            CharacterInput(normalColor);
            GUILayout.Space(15);
            FontSettings(normalColor);
            GUILayout.Space(15);
            MeshExportSettings();
        }

        private void MeshExportSettings()
        {
            GUILayout.BeginHorizontal();
            GUIContent exportStyle = new GUIContent("Export As", "Which way you want mesh to be saved as.\nWarning!: \nSaving as mesh asset means each character will be saved as separate assets in the folder");
            EditorGUILayout.LabelField(exportStyle, GUILayout.MaxWidth(120));
            EditorGUILayout.PropertyField(soTarget.FindProperty("meshExportStyle"), GUIContent.none);
            GUILayout.EndHorizontal();
        }


        void FontSettings(Color normalColor)
        {
            GUI.backgroundColor = settings.thirdBackgroundColor;
            GUILayout.BeginVertical("Box");
            GUILayout.Label("Model Settings:");
            DrawUILine(Color.gray);

            GUI.backgroundColor = normalColor;


            GUILayout.BeginHorizontal();
            GUIContent vertexDensity = new GUIContent("Vertex Density", "How many verticies should be used. Has very little impact other than calculation time if changed, since mesh automatically gets simplified after creation.");
            EditorGUILayout.LabelField(vertexDensity, GUILayout.MaxWidth(120));
            EditorGUILayout.PropertyField(soTarget.FindProperty("vertexDensity"), GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUIContent sizeXY = new GUIContent("Size XY", "Base font size.");
            EditorGUILayout.LabelField(sizeXY, GUILayout.MaxWidth(120));
            EditorGUILayout.PropertyField(soTarget.FindProperty("sizeXY"), GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUIContent sizeZ = new GUIContent("Size Z/Depth", "Base depth");
            EditorGUILayout.LabelField(sizeZ, GUILayout.MaxWidth(120));
            EditorGUILayout.PropertyField(soTarget.FindProperty("sizeZ"), GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUIContent smoothingAngle = new GUIContent("Smoothing Angle", "Any verticies with lower angle will be smooth.");
            EditorGUILayout.LabelField(smoothingAngle, GUILayout.MaxWidth(120));
            EditorGUILayout.PropertyField(soTarget.FindProperty("smoothingAngle"), GUIContent.none);
            GUILayout.EndHorizontal();

            EditorGUILayout.HelpBox("Keep these values to 1 unless you really need to change it. \nSmoothing angle by default is 30 degrees (Same as blender). Lower value will give a flat looking font and higher is smoother.", MessageType.Info);

            GUILayout.EndVertical();
        }
        private void CreateFont()
        {
            GameObject gameObject = new GameObject();

            bool exportAsObj = ExportAs();
            List<char> listOfChar = GetCharacterList();

            MText_FontCreator fontCreator = new MText_FontCreator();
            fontCreator.CreateFont(gameObject, listOfChar, settings.sizeXY, settings.sizeZ, settings.vertexDensity, settings.smoothingAngle, settings.defaultTextMaterial, exportAsObj);

            //if (!fontCreator.WasEverythingProcessed())
            //{
            //    //EditorUtility.DisplayDialog("")
            //}

            EditorUtility.DisplayProgressBar("Creating font", "Mesh creation started", 75 / 100);
            if (gameObject.transform.childCount > 0)
            {
                if (exportAsObj)
                {
                    MText_ObjExporter objExporter = new MText_ObjExporter();
                    string prefabPath = objExporter.DoExport(gameObject, true);
                    if (string.IsNullOrEmpty(prefabPath))
                    {
                        Debug.Log("Object save failed");
                        EditorUtility.ClearProgressBar();
                        return;
                    }
                    MText_FontExporter fontExporter = new MText_FontExporter();
                    fontExporter.CreateFontFile(prefabPath, gameObject.name, fontCreator);
                }
                else
                {
                    MText_MeshAssetExporter meshAssetExporter = new MText_MeshAssetExporter();
                    meshAssetExporter.DoExport(gameObject);
                }
            }

            EditorUtility.ClearProgressBar();
            if (Application.isPlaying) Destroy(gameObject);
            else DestroyImmediate(gameObject);
        }

        private bool ExportAs()
        {
            bool exportAsObj = true;
            if (settings.meshExportStyle != MText_Settings.MeshExportStyle.exportAsObj) exportAsObj = false;
            return exportAsObj;
        }

        private void HorizontalButtonURL(string text, string url)
        {
            if (GUILayout.Button(text, GUILayout.MinHeight(25)))
            {
                Application.OpenURL(url);
            }
        }

        void CharacterInput(Color normalColor)
        {
            GUI.backgroundColor = settings.thirdBackgroundColor;
            GUILayout.BeginVertical("Box");


            EditorGUILayout.PropertyField(soTarget.FindProperty("charInputStyle"), GUIContent.none);
            DrawUILine(Color.gray);

            GUI.backgroundColor = normalColor;



            if (settings.charInputStyle == MText_Settings.CharInputStyle.CharacterRange)
            {
                CharacterRangeInput();
            }
            else if (settings.charInputStyle == MText_Settings.CharInputStyle.UnicodeRange)
            {
                UnicodeRangeInput();
            }
            else if (settings.charInputStyle == MText_Settings.CharInputStyle.CustomCharacters)
            {
                CustomCharacters();
            }
            else if (settings.charInputStyle == MText_Settings.CharInputStyle.UnicodeSequence)
            {
                UnicodeSequence();
            }
            //else if (settings.charInputStyle == MText_Settings.CharInputStyle.CharacterSet)
            //{
            //    CharacterSet();
            //}
            if (GUILayout.Button("Debug.Log all characters"))
            {
                TestCharacterList();
            }


            //EditorGUILayout.HelpBox("Just a FYI: Having thousands of characters in a single file can cause issues.", MessageType.Info);
            GUILayout.EndVertical();
        }

        private void CharacterRangeInput()
        {
            //field
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Start", GUILayout.MaxWidth(120));
            EditorGUILayout.PropertyField(soTarget.FindProperty("startChar"), GUIContent.none);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("End", GUILayout.MaxWidth(120));
            EditorGUILayout.PropertyField(soTarget.FindProperty("endChar"), GUIContent.none);
            GUILayout.EndHorizontal();

            //info
            GUILayout.Label("Leave it to '!' & '~' for English");
            GUILayout.BeginHorizontal();
            HorizontalButtonURL("Get character list", "https://unicode-table.com/en/");
            HorizontalButtonURL("How it works", "https://youtu.be/JN_DSmdiRSI"); //to-do make a video with the unity editor
            GUILayout.EndHorizontal();
        }
        private void UnicodeRangeInput()
        {
            //field
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Start", GUILayout.MaxWidth(120));
            EditorGUILayout.PropertyField(soTarget.FindProperty("startUnicode"), GUIContent.none);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("End", GUILayout.MaxWidth(120));
            EditorGUILayout.PropertyField(soTarget.FindProperty("endUnicode"), GUIContent.none);
            GUILayout.EndHorizontal();

            //info
            GUILayout.Label("Leave it to 0021 & 007E for English.");

            GUILayout.BeginHorizontal();
            HorizontalButtonURL("Get character list", "https://unicode-table.com/en/");
            HorizontalButtonURL("How it works", "https://youtu.be/JN_DSmdiRSI"); //to-do make a video with the unity editor
            GUILayout.EndHorizontal();
        }
        private void CustomCharacters()
        {
            //field
            EditorGUILayout.PropertyField(soTarget.FindProperty("customCharacters"), GUIContent.none);
            //EditorGUILayout.TextField(soTarget.FindProperty("customCharacters"));

            //info
            GUILayout.Label("Just type the characters you want in the font.");
        }
        private void UnicodeSequence()
        {
            //field
            EditorGUILayout.PropertyField(soTarget.FindProperty("unicodeSequence"), GUIContent.none);

            //info
            GUILayout.Label("Separate codes with ',' & create ranges with '-' .Example:\n" +
                "0021-007E, 00C0");
        }



        //TODO: make this a setting that user can turn on/off to test
        void TestCharacterList()
        {
            List<char> myCharacters = GetCharacterList();
            Debug.Log("Character count: " + myCharacters.Count);
            for (int i = 0; i < myCharacters.Count; i++)
            {
                Debug.Log(myCharacters[i]);
            }
        }



        private List<char> GetCharacterFromRange(char start, char end)
        {
            MText_NewFontCharacterRange characterRange = new MText_NewFontCharacterRange();
            List<char> characterList = characterRange.RetrieveCharactersList(start, end);
            return characterList;
        }

        private char ConvertCharFromUnicode(string unicode)
        {
            string s = System.Text.RegularExpressions.Regex.Unescape("\\u" + unicode);
            s.ToCharArray();
            if (s.Length > 0)
                return s[0];
            else
                return ' ';
        }



        List<char> GetCharacterList()
        {
            List<char> myChars = new List<char>();

            if (settings.charInputStyle == MText_Settings.CharInputStyle.CharacterRange)
            {
                myChars = GetCharacterFromRange(settings.startChar, settings.endChar);
            }
            else if (settings.charInputStyle == MText_Settings.CharInputStyle.UnicodeRange)
            {
                char start = ConvertCharFromUnicode(settings.startUnicode);
                char end = ConvertCharFromUnicode(settings.endUnicode);

                myChars = GetCharacterFromRange(start, end);
            }
            else if (settings.charInputStyle == MText_Settings.CharInputStyle.CustomCharacters)
            {
                myChars = settings.customCharacters.ToCharArray().ToList();
            }
            else if (settings.charInputStyle == MText_Settings.CharInputStyle.UnicodeSequence)
            {
                MText_NewFontCharacterRange characterRange = new MText_NewFontCharacterRange();
                myChars = characterRange.RetrieveCharacterListFromUnicodeSequence(settings.unicodeSequence);
            }
            //else if (settings.charInputStyle == MText_Settings.CharInputStyle.CharacterSet)
            //{
            //    MText_NewFontCharacterRange characterRange = new MText_NewFontCharacterRange();
            //    // myChars = characterRange.RetrieveCharacterListFromUnicodeSequence(settings.unicodeSequence);
            //}
            myChars = myChars.Distinct().ToList();

            return myChars;
        }






        public static void DrawUILine(Color color, int thickness = 1, int padding = 1)
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