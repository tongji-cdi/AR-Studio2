using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace MText
{
    [CustomEditor(typeof(Modular3DText))]
    public class Modular3DTextEditor : Editor
    {
        public MText_Settings settings;

        float defaultSmallHorizontalFieldSize = 72.5f;
        float defaultNormalltHorizontalFieldSize = 100;
        float defaultLargeHorizontalFieldSize = 120f;
        float defaultExtraLargeHorizontalFieldSize = 150f;


        Modular3DText myTarget;
        SerializedObject soTarget;

        SerializedProperty text;
        //SerializedProperty autoCreateInPlayMode;
        //SerializedProperty autoCreateInEditor;

        //main settings
        SerializedProperty font;
        SerializedProperty material;
        SerializedProperty fontSize;

        SerializedProperty characterSpacingInput;
        SerializedProperty lineSpacingInput;

        //positioning
        SerializedProperty height;
        SerializedProperty length;
        //SerializedProperty depth;

        //circular 
        SerializedProperty circularAlignmentRadius;
        SerializedProperty circularAlignmentSpreadAmount;
        SerializedProperty circularAlignmentAngle;

        //effects
        SerializedProperty typingEffects;
        SerializedProperty deletingEffects;
        SerializedProperty customDeleteAfterDuration;
        SerializedProperty deleteAfter;

        //advanced settings
        SerializedProperty repositionOldCharacters;
        SerializedProperty reApplyModulesToOldCharacters;
        //SerializedProperty activateChildObjects;

        SerializedProperty pool;
        SerializedProperty combineMeshInEditor;
        SerializedProperty dontCombineInEditorAnyway;
        SerializedProperty combineMeshDuringRuntime;
        SerializedProperty hideLettersInHierarchyInPlayMode;
        //SerializedProperty hideLettersInHierarchyInEditMode;
        SerializedProperty updateTextOncePerFrame;
        SerializedProperty autoSaveMesh;

        SerializedProperty canBreakOutermostPrefab;

        SerializedProperty debugLogs;

        bool showCreationettings = false;
        bool showMainSettings = true;
        bool showModuleSettings = false;
        bool showAdvancedSettings = false;

        //style
        private static GUIStyle toggleStyle = null;

        string[] layoutOptions = new[] { "Linear layout", "Circular layout" };
        //string[] textDirection = new[] { "Left to Right", "Right to Left" };
        int layoutStyle = 0;

        enum FieldSize
        {
            small,
            normal,
            large,
            extraLarge
        }



        void OnEnable()
        {
            myTarget = (Modular3DText)target;
            soTarget = new SerializedObject(target);

            text = soTarget.FindProperty("_text");

            //creation settings
            //autoCreateInPlayMode = soTarget.FindProperty("autoCreateInPlayMode");
            //autoCreateInEditor = soTarget.FindProperty("autoCreateInEditor");
            autoSaveMesh = soTarget.FindProperty("autoSaveMesh");

            //main settings
            font = soTarget.FindProperty("_font");
            material = soTarget.FindProperty("_material");
            fontSize = soTarget.FindProperty("_fontSize");

            characterSpacingInput = soTarget.FindProperty("_characterSpacing");
            lineSpacingInput = soTarget.FindProperty("_lineSpacing");

            //position settings
            height = soTarget.FindProperty("_height");
            length = soTarget.FindProperty("_width");
            //depth = soTarget.FindProperty("depth");

            //circular Layout
            circularAlignmentRadius = soTarget.FindProperty("_circularAlignmentRadius");
            circularAlignmentSpreadAmount = soTarget.FindProperty("_circularAlignmentSpreadAmount");
            circularAlignmentAngle = soTarget.FindProperty("_circularAlignmentAngle");

            //effects
            typingEffects = soTarget.FindProperty("typingEffects");
            deletingEffects = soTarget.FindProperty("deletingEffects");
            customDeleteAfterDuration = soTarget.FindProperty("customDeleteAfterDuration");
            deleteAfter = soTarget.FindProperty("deleteAfter");

            //advanced
            repositionOldCharacters = soTarget.FindProperty("repositionOldCharacters");
            reApplyModulesToOldCharacters = soTarget.FindProperty("reApplyModulesToOldCharacters");
            //activateChildObjects = soTarget.FindProperty("activateChildObjects");

            pool = soTarget.FindProperty("pooling");
            combineMeshInEditor = soTarget.FindProperty("combineMeshInEditor");
            dontCombineInEditorAnyway = soTarget.FindProperty("dontCombineInEditorAnyway");
            combineMeshDuringRuntime = soTarget.FindProperty("combineMeshDuringRuntime");
            hideLettersInHierarchyInPlayMode = soTarget.FindProperty("hideLettersInHierarchyInPlayMode");
            //hideLettersInHierarchyInEditMode = soTarget.FindProperty("hideLettersInHierarchyInEditMode");
            updateTextOncePerFrame = soTarget.FindProperty("updateTextOncePerFrame");
            

            canBreakOutermostPrefab = soTarget.FindProperty("canBreakOutermostPrefab");
            debugLogs = soTarget.FindProperty("debugLogs");

            showCreationettings = myTarget.showCreationettingsInEditor;
            showMainSettings = myTarget.showMainSettingsInEditor;
            showModuleSettings = myTarget.showModuleSettingsInEditor;
            showAdvancedSettings = myTarget.showAdvancedSettingsInEditor;

            layoutStyle = myTarget.LayoutStyle;
        }

        public override void OnInspectorGUI()
        {
            soTarget.Update();
            GenerateStyle();

            EditorGUI.BeginChangeCheck();
            WarningCheck();

            EditorGUILayout.PropertyField(text, GUIContent.none, GUILayout.Height(100));

            GUILayout.Space(5);
            MainSettings();
            GUILayout.Space(5);
            ModuleSettings();
            GUILayout.Space(5);
            AdvancedSettings();

            SaveInspectorLayoutSettings();

            if (EditorGUI.EndChangeCheck())
            {
                MText_Font font = myTarget.Font;
                soTarget.ApplyModifiedProperties();
                if (font != myTarget.Font)
                {
                    myTarget.Font = myTarget.Font; //this is to call the setter
                }

                if (layoutStyle != myTarget.LayoutStyle)
                    myTarget.LayoutStyle = layoutStyle;

                if (!PrefabUtility.IsPartOfPrefabAsset(target))
                    myTarget.UpdateText();

                //this is for people updating from old versions added 12/01/21. Will be removed after sufficient time has passed
                if (!myTarget.GetComponent<MText_TextUpdater>())
                    myTarget.gameObject.AddComponent<MText_TextUpdater>();

                EditorUtility.SetDirty(myTarget);
            }
        }

        private void SaveInspectorLayoutSettings()
        {
            myTarget.showCreationettingsInEditor = showCreationettings;
            myTarget.showMainSettingsInEditor = showMainSettings;
            myTarget.showModuleSettingsInEditor = showModuleSettings;
            myTarget.showAdvancedSettingsInEditor = showAdvancedSettings;
        }

        void GenerateStyle()
        {
            if (toggleStyle == null)
            {
                toggleStyle = new GUIStyle(GUI.skin.button);
                toggleStyle.margin = new RectOffset(0, 0, toggleStyle.margin.top, toggleStyle.margin.bottom);
            }
            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            EditorStyles.foldout.margin = new RectOffset(1, 5, 5, 5);
            EditorStyles.popup.fontSize = 11;
            EditorStyles.popup.fixedHeight = 18;
        }

        bool LeftButton(GUIContent content)
        {
            bool clicked = false;
            Rect rect = GUILayoutUtility.GetRect(20, 17);

            GUI.BeginGroup(rect);
            if (GUI.Button(new Rect(0, 0, rect.width + toggleStyle.border.right, rect.height), content, toggleStyle))
                clicked = true;

            GUI.EndGroup();
            return clicked;
        }
        bool MidButton(GUIContent content)
        {
            bool clicked = false;
            Rect rect = GUILayoutUtility.GetRect(20, 17);


            GUI.BeginGroup(rect);
            if (GUI.Button(new Rect(-toggleStyle.border.left, 0, rect.width + toggleStyle.border.left + toggleStyle.border.right, rect.height), content, toggleStyle))
                clicked = true;
            GUI.EndGroup();
            return clicked;
        }
        bool RightButton(GUIContent content)
        {
            bool clicked = false;
            Rect rect = GUILayoutUtility.GetRect(20, 17);


            GUI.BeginGroup(rect);
            if (GUI.Button(new Rect(-toggleStyle.border.left, 0, rect.width + toggleStyle.border.left, rect.height), content, toggleStyle))
                clicked = true;
            GUI.EndGroup();
            return clicked;
        }

        void WarningCheck()
        {
            EditorGUI.indentLevel = 0;
            if (!myTarget.Font) EditorGUILayout.HelpBox("No font selected", MessageType.Error);
            if (!myTarget.Material) EditorGUILayout.HelpBox("No material selected", MessageType.Error);
        }

        void Creation()
        {
            if (myTarget.assetPath != "" && myTarget.assetPath != null && !EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField(myTarget.assetPath, EditorStyles.boldLabel);
                if (GUILayout.Button("Apply to prefab"))
                {
                    myTarget.ReconnectPrefabs();
                }
            }

            if ((myTarget.assetPath != "" && myTarget.assetPath != null && !EditorApplication.isPlaying))
            {
                if (GUILayout.Button("Remove prefab connection"))
                {
                    myTarget.assetPath = "";
                }
            }
            if (PrefabUtility.IsPartOfPrefabInstance(myTarget.gameObject))
            {
                MeshSaveSettings();
            }
            DrawUILine(Color.grey, 1, 2);

        }


        void MainSettings()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 1;

            showMainSettings = EditorGUILayout.Foldout(showMainSettings, "Main Settings", true, EditorStyles.foldout);

            if (showMainSettings)
            {
                DrawUILine(Color.grey, 1, 2);
                EditorGUI.indentLevel = 0;

                GUILayout.Space(5);
                HorizontalField(font, "Font", "", FieldSize.small);

                HorizontalField(material, "Material", "", FieldSize.small);
                HorizontalField(fontSize, "Size", "", FieldSize.small);
                EditorGUILayout.PropertyField(combineMeshInEditor, new GUIContent("Single mesh", "Combine into a single mesh in Editor, edit mode\n" +
                    "There is no reason to turn this on for playmode/build unless you really need this for something. \nOtherwise, wasted resource on combining\n" +
                    "Check advanced settings for more options"));

                EditorGUI.indentLevel = 1;
                DontCombineInEditorEither();
                EditorGUI.indentLevel = 0;

                GUILayout.Space(10);

                TextStyles();
                layoutStyle = EditorGUILayout.Popup(myTarget.LayoutStyle, layoutOptions, EditorStyles.popup, GUILayout.MinWidth(45));

                if (myTarget.LayoutStyle == 0)
                {
                    LinearAlignment();
                    GUILayout.Space(5);
                    LinearPositionSettings();
                    LinearSpacing();
                }
                else CircularLayoutSettings();
            }
            if (!Selection.activeTransform)
            {
                showMainSettings = false;
            }
            GUILayout.EndVertical();
        }



        void TextStyles()
        {
            EditorGUILayout.BeginHorizontal();

            Color storeColor = GUI.backgroundColor;
            GUIContent leftToRight = EditorGUIUtility.IconContent("tab_next@2x", "t");
            GUIContent rightToLeft = EditorGUIUtility.IconContent("tab_prev@2x");

            GUI.backgroundColor = storeColor;
            if (myTarget.textDirection == 1)
                GUI.backgroundColor = new Color(0.6f, 0.6f, 0.6f);

            if (LeftButton(leftToRight))
            {
                myTarget.textDirection = 1;
                //if (myTarget.autoCreateInEditor) 
                myTarget.UpdateText();
            }


            GUI.backgroundColor = storeColor;
            if (myTarget.textDirection == -1)
                GUI.backgroundColor = new Color(0.6f, 0.6f, 0.6f);

            if (RightButton(rightToLeft))
            {
                myTarget.textDirection = -1;
                //if (myTarget.autoCreateInEditor) 
                myTarget.UpdateText();
            }
            GUI.backgroundColor = storeColor;

            GUI.backgroundColor = storeColor;
            if (myTarget.LowerCase)
                GUI.backgroundColor = new Color(0.6f, 0.6f, 0.6f);

            GUIContent smallCase = new GUIContent("ab", "lower case");
            if (LeftButton(smallCase))
            {
                myTarget.LowerCase = !myTarget.LowerCase;
                myTarget.Capitalize = false;
                myTarget.UpdateText();
                EditorUtility.SetDirty(myTarget);
            }


            if (myTarget.Capitalize)
                GUI.backgroundColor = new Color(0.6f, 0.6f, 0.6f);
            else
                GUI.backgroundColor = storeColor;

            GUIContent capitalize = new GUIContent("AB", "UPPER CASE");
            if (RightButton(capitalize))
            {
                myTarget.Capitalize = !myTarget.Capitalize;
                myTarget.LowerCase = false;
                myTarget.UpdateText();
                EditorUtility.SetDirty(myTarget);
            }

            GUI.backgroundColor = storeColor;
            EditorGUILayout.EndHorizontal();
        }


        void LinearPositionSettings()
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Height", GUILayout.MaxWidth(60));
            EditorGUILayout.PropertyField(height, GUIContent.none, GUILayout.MaxWidth(50));
            EditorGUILayout.LabelField("Length", GUILayout.MaxWidth(60));
            EditorGUILayout.PropertyField(length, GUIContent.none, GUILayout.MaxWidth(50));

            GUILayout.EndHorizontal();
        }
        void LinearSpacing()
        {
            GUILayout.Space(5);

            EditorGUILayout.LabelField("Spacing");
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("", GUILayout.MaxWidth(10));
            EditorGUILayout.LabelField("Character", GUILayout.MaxWidth(65));
            EditorGUILayout.PropertyField(characterSpacingInput, GUIContent.none, GUILayout.MaxWidth(40));
            EditorGUILayout.LabelField("Line", GUILayout.MaxWidth(55));
            EditorGUILayout.PropertyField(lineSpacingInput, GUIContent.none, GUILayout.MaxWidth(40));

            GUILayout.EndHorizontal();

            GUILayout.Space(5);
        }
        void LinearAlignment()
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();


            HorizontalAlignment();
            VerticalAlignment();

            EditorGUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        void CircularLayoutSettings()
        {
            HorizontalField(circularAlignmentRadius, "Radius", "", FieldSize.small);
            HorizontalField(circularAlignmentSpreadAmount, "Spread", "", FieldSize.small);
            HorizontalField(circularAlignmentAngle, "Angle", "", FieldSize.small);

            LinearSpacing();
        }

        void HorizontalAlignment()
        {
            EditorGUILayout.BeginHorizontal();

            Color storeColor = GUI.backgroundColor;
            GUIContent horizontallyleftIcon = EditorGUIUtility.IconContent("align_horizontally_left");
            GUIContent horizontallyCenterIcon = EditorGUIUtility.IconContent("align_horizontally_center");
            GUIContent horizontallyRightIcon = EditorGUIUtility.IconContent("align_horizontally_right");

            GUI.backgroundColor = storeColor;
            if (myTarget.alignLeft)
                GUI.backgroundColor = new Color(0.6f, 0.6f, 0.6f);

            if (LeftButton(horizontallyleftIcon))
            {
                myTarget.alignLeft = true;
                myTarget.alignRight = false;
                myTarget.alignCenter = false;
                //if (myTarget.autoCreateInEditor) 
                myTarget.UpdateText();
            }
            GUI.backgroundColor = storeColor;
            if (myTarget.alignCenter)
                GUI.backgroundColor = new Color(0.6f, 0.6f, 0.6f);

            if (MidButton(horizontallyCenterIcon))
            {
                myTarget.alignCenter = true;
                myTarget.alignRight = false;
                myTarget.alignLeft = false;
                //if (myTarget.autoCreateInEditor) 
                myTarget.UpdateText();
            }
            GUI.backgroundColor = storeColor;
            if (myTarget.alignRight)
                GUI.backgroundColor = new Color(0.6f, 0.6f, 0.6f);

            if (RightButton(horizontallyRightIcon))
            {
                myTarget.alignLeft = false;
                myTarget.alignCenter = false;
                myTarget.alignRight = true;
                //if (myTarget.autoCreateInEditor) 
                myTarget.UpdateText();
            }
            GUI.backgroundColor = storeColor;
            EditorGUILayout.EndHorizontal();
        }
        void VerticalAlignment()
        {
            Color storeColor = GUI.backgroundColor;

            EditorGUILayout.BeginHorizontal();

            GUIContent verticallyTopIcon = EditorGUIUtility.IconContent("align_vertically_top");
            GUIContent verticallyMiddleIcon = EditorGUIUtility.IconContent("align_vertically_center");
            GUIContent verticallyBottomIcon = EditorGUIUtility.IconContent("align_vertically_bottom");

            if (myTarget.alignTop)
                GUI.backgroundColor = new Color(0.6f, 0.6f, 0.6f);

            if (LeftButton(verticallyTopIcon))
            {
                myTarget.alignTop = true;
                myTarget.alignMiddle = false;
                myTarget.alignBottom = false;
                //if (myTarget.autoCreateInEditor) 
                myTarget.UpdateText();
            }

            GUI.backgroundColor = storeColor;
            if (myTarget.alignMiddle)
                GUI.backgroundColor = new Color(0.6f, 0.6f, 0.6f);

            if (MidButton(verticallyMiddleIcon))
            {
                myTarget.alignTop = false;
                myTarget.alignMiddle = true;
                myTarget.alignBottom = false;
                //if (myTarget.autoCreateInEditor) 
                myTarget.UpdateText();
            }

            GUI.backgroundColor = storeColor;
            if (myTarget.alignBottom)
                GUI.backgroundColor = new Color(0.6f, 0.6f, 0.6f);

            if (RightButton(verticallyBottomIcon))
            {
                myTarget.alignTop = false;
                myTarget.alignMiddle = false;
                myTarget.alignBottom = true;
                //if (myTarget.autoCreateInEditor) 
                myTarget.UpdateText();
            }
            EditorGUILayout.EndHorizontal();
            GUI.backgroundColor = storeColor;
        }

        void ModuleSettings()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 1;

            showModuleSettings = EditorGUILayout.Foldout(showModuleSettings, "Modules", true, EditorStyles.foldout);
            if (showModuleSettings)
            {
                EditorGUI.indentLevel = 2;

                DrawUILine(Color.grey, 1, 0);
                ModuleContainerList("Adding", myTarget.typingEffects, typingEffects);
                DrawUILine(Color.grey, 1, 0);
                GUILayout.Space(10);

                DeleteAfterDuration();

                ModuleContainerList("Deleting", myTarget.deletingEffects, deletingEffects);
                GUILayout.Space(5);
            }
            if (!Selection.activeTransform)
            {
                showModuleSettings = false;
            }
            GUILayout.EndVertical();
        }

        void DeleteAfterDuration()
        {
            string toolTip = "If set to false, text is deleted after module's max duration";

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(customDeleteAfterDuration, GUIContent.none, GUILayout.MaxWidth(15));
            if (!myTarget.customDeleteAfterDuration)
            {
                float duration = GetDeleteDuration();
                GUIContent content = new GUIContent("Delete After :" + duration, toolTip);
                EditorGUILayout.LabelField(content);
            }
            else
                HorizontalField(deleteAfter, "Delete After", "", FieldSize.small);
            EditorGUILayout.EndHorizontal();
        }

        float GetDeleteDuration()
        {
            float max = 0;
            for (int i = 0; i < myTarget.deletingEffects.Count; i++)
            {
                if (myTarget.deletingEffects[i].duration > max)
                    max = myTarget.deletingEffects[i].duration;
            }
            return max;
        }

        void AdvancedSettings()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 1;

            showAdvancedSettings = EditorGUILayout.Foldout(showAdvancedSettings, "Advanced Settings", true, EditorStyles.foldout);
            if (showAdvancedSettings)
            {
                DrawUILine(Color.grey, 1, 0);
                EditorGUI.indentLevel = 0;

                Creation();
                CombineMeshSettings();

                DrawUILine(Color.grey, 1, 0);
                EditorGUILayout.PropertyField(pool, new GUIContent("Pooling", "Pooling massively increases performance if you are making a lot of changes"));
                DrawUILine(Color.grey, 1, 0);

                EditorGUILayout.PropertyField(repositionOldCharacters, new GUIContent("Reposition old Chars", "If old text = '123' and updated new text = '1234',\nthe '123' will be moved to their correct position when entering the '4'"));
                EditorGUILayout.PropertyField(reApplyModulesToOldCharacters, new GUIContent("Re-apply modules", "If old text = old and updated new text = oldy,\ninstead of applying module to only 'y', it will apply to all chars"));
                //HorizontalFieldShortProperty(activateChildObjects, "Auto-activate ChildObjects", "", FieldSize.small);
                HorizontalField(updateTextOncePerFrame, "Update once per frame", "If the gameobject is active in hierarchy, uses coroutine to make sure the text is only updated visually once per frame instead of wasting resources if updated multiple times by a script. This is only used if the game object is active in hierarchy and it updates at the end of frame.", FieldSize.large);
                
                DrawUILine(Color.grey, 1, 0);


                GUIContent hideLetters = new GUIContent("Hide letters in Hierarchy", "Hides the game object of letters in the hierarchy. They are still there just not visible. No impact except for cleaner hierarchy.");
                EditorGUILayout.LabelField(hideLetters);
                EditorGUI.indentLevel = 1;
                HorizontalField(hideLettersInHierarchyInPlayMode, "In play mode");
                
                //Served no purpose other than confusing new users
                //HorizontalField(hideLettersInHierarchyInEditMode, "In editor mode");

                EditorGUI.indentLevel = 0;

                GUILayout.Space(5);

                if (myTarget.gameObject.GetComponent<MeshFilter>())
                {
                    if (GUILayout.Button(new GUIContent("Optimize mesh", "This causes the geometry and vertices of the combined mesh to be reordered internally in an attempt to improve vertex cache utilisation on the graphics hardware and thus rendering performance. This operation can take a few seconds or more for complex meshes.")))
                    {
                        MText_Utilities.OptimizeMesh(myTarget.gameObject.GetComponent<MeshFilter>().sharedMesh);
                    }
                }
                GUILayout.Space(5);


                if (PrefabUtility.IsPartOfPrefabInstance(myTarget.gameObject))
                {
                    if (PrefabUtility.IsOutermostPrefabInstanceRoot(myTarget.gameObject))
                    {
                        HorizontalField(canBreakOutermostPrefab, "Can break outermost Prefab", "", FieldSize.extraLarge);
                    }
                }
                else MeshSaveSettings();

                DrawUILine(Color.grey, 1, 0);
                EditorGUILayout.PropertyField(debugLogs);
            }
            if (!Selection.activeTransform)
            {
                showAdvancedSettings = false;
            }
            GUILayout.EndVertical();
            GUILayout.Space(15);
        }

        void CombineMeshSettings()
        {
            EditorGUILayout.LabelField(new GUIContent("Single mesh", "Combines character meshes" +
                "\nUses unity's Mesh.Combine method.\n" +
                "Unity has a limit of verticies one mesh can have which causes the bugs on large texts"));
            EditorGUI.indentLevel = 1;
            EditorGUILayout.PropertyField(combineMeshInEditor, new GUIContent("In Editor", "The same option as found in Main Settings"));

            DontCombineInEditorEither();

            EditorGUI.indentLevel = 1;
            EditorGUILayout.PropertyField(combineMeshDuringRuntime, new GUIContent("In Play mode", "There is no reason to turn this on unless you really need this for something. \nOtherwise, wasted resource on combining"));
            EditorGUI.indentLevel = 0;
        }

        void MeshSaveSettings()
        {
            if (myTarget.gameObject.GetComponent<MeshFilter>())
            {
                EditorGUILayout.PropertyField(autoSaveMesh);

                GUILayout.BeginHorizontal();
                if (!myTarget.autoSaveMesh)
                {
                    if (GUILayout.Button(new GUIContent("Save mesh", "PREFABS need saved meshes")))
                    {
                        myTarget.SaveMeshAsAsset(false);
                    }
                }
                if (GUILayout.Button(new GUIContent("Save mesh as", "Save a new copy of the mesh in project")))
                {
                    myTarget.SaveMeshAsAsset(true);
                }
                GUILayout.EndHorizontal();
            }
        }
        void DontCombineInEditorEither()
        {
            if (!myTarget.combineMeshInEditor && PrefabUtility.IsPartOfPrefabInstance(myTarget.gameObject))
            {
                GUIContent helpIcon = EditorGUIUtility.IconContent("console.warnicon");
                string warning = "Prefabs don't allow child objects that are part of the prefab to be deleted in Editor.\n" +
                    "If you add child objects, then apply, which adds these child objects to the prefab,\n" +
                    "When changing text again, this script can't delete the old gameobjects. Just disables them. Remember to clean them up manually if you enable this.";
                EditorGUILayout.PropertyField(dontCombineInEditorAnyway, new GUIContent("Even in Prefab", warning), GUILayout.MinWidth(150));
            }
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
        void HorizontalFieldShortProperty(SerializedProperty property, string label, string tooltip = "", FieldSize fieldSize = FieldSize.normal)
        {
            float myMaxWidth;
            //not to self: it's ternary operator not tarnary operator. Stop mistyping
            if (settings)
                myMaxWidth = fieldSize == FieldSize.small ? settings.smallHorizontalFieldSize : fieldSize == FieldSize.normal ? settings.normalltHorizontalFieldSize : fieldSize == FieldSize.large ? settings.largeHorizontalFieldSize : fieldSize == FieldSize.extraLarge ? settings.extraLargeHorizontalFieldSize : settings.normalltHorizontalFieldSize;
            else
                myMaxWidth = fieldSize == FieldSize.small ? defaultSmallHorizontalFieldSize : fieldSize == FieldSize.normal ? defaultNormalltHorizontalFieldSize : fieldSize == FieldSize.large ? defaultLargeHorizontalFieldSize : fieldSize == FieldSize.extraLarge ? defaultExtraLargeHorizontalFieldSize : settings.normalltHorizontalFieldSize;

            GUILayout.BeginHorizontal();
            GUIContent gUIContent = new GUIContent(label, tooltip);
            EditorGUILayout.LabelField(gUIContent);
            EditorGUILayout.PropertyField(property, GUIContent.none, GUILayout.MaxWidth(myMaxWidth));
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

        void ModuleContainerList(string label, List<MText_ModuleContainer> moduleContainers, SerializedProperty serializedContainer)
        {
            Color original = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f);

            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 0;

            EditorGUILayout.LabelField(label, EditorStyles.label);
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

    }
}