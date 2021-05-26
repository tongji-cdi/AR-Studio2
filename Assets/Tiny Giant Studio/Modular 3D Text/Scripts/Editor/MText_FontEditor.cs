/// Created by Ferdowsur Asif @ Tiny Giant Studio
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace MText
{
    [CustomEditor(typeof(MText_Font))]
    public class MText_FontEditor : Editor
    {
        MText_Font myTarget;
        SerializedObject soTarget;

        SerializedProperty rotationFix;
        SerializedProperty positionFix;
        SerializedProperty scaleFix;
        SerializedProperty fontSet;
        SerializedProperty overwriteOldSet;
        SerializedProperty characters;
        SerializedProperty monoSpaceFont;
        SerializedProperty useUpperCaseLettersIfLowerCaseIsMissing;
        SerializedProperty emptySpaceSpacing;
        SerializedProperty characterSpacing;
        SerializedProperty fallbackFonts;

        SerializedProperty enableKerning;
        SerializedProperty kerningMultiplier;

        void OnEnable()
        {
            myTarget = (MText_Font)target;
            soTarget = new SerializedObject(target);

            rotationFix = soTarget.FindProperty("rotationFix");
            positionFix = soTarget.FindProperty("positionFix");
            scaleFix = soTarget.FindProperty("scaleFix");

            fontSet = soTarget.FindProperty("fontSet");
            overwriteOldSet = soTarget.FindProperty("overwriteOldSet");
            monoSpaceFont = soTarget.FindProperty("monoSpaceFont");
            useUpperCaseLettersIfLowerCaseIsMissing = soTarget.FindProperty("useUpperCaseLettersIfLowerCaseIsMissing");
            emptySpaceSpacing = soTarget.FindProperty("emptySpaceSpacing");
            characterSpacing = soTarget.FindProperty("characterSpacing");

            characters = soTarget.FindProperty("characters");
            fallbackFonts = soTarget.FindProperty("fallbackFonts");

            enableKerning = soTarget.FindProperty("enableKerning");
            kerningMultiplier = soTarget.FindProperty("kerningMultiplier");
        }

        public override void OnInspectorGUI()
        {
            soTarget.Update();
            EditorGUI.BeginChangeCheck();

            GetFontSet();
            GUILayout.Space(5);
            FallBackFont();
            GUILayout.Space(10);
            FixSettings();
            GUILayout.Space(20);
            CreateCharacterList();
            GUILayout.Space(20);
            KerningSettings();


            if (EditorGUI.EndChangeCheck())
            {
                soTarget.ApplyModifiedProperties();
                ApplyFontChanges();
            }
        }

       //Called after fields are changed in inspector
        void ApplyFontChanges()
        {
            List<GameObject> allObjectInScene = GetAllObjectsOnlyInScene();
            List<Modular3DText> texts = new List<Modular3DText>();
            for(int i = 0; i < allObjectInScene.Count; i++)
            {
                if (allObjectInScene[i].GetComponent<Modular3DText>())
                    texts.Add(allObjectInScene[i].GetComponent<Modular3DText>());
            }

            for (int i = 0; i < texts.Count; i++)
            {
                //if (texts[i].autoCreateInEditor)
                {
                    if(texts[i].Font == target)
                    {
                        texts[i].UpdateText();
                    }
                }
            }
        }

        List<GameObject> GetAllObjectsOnlyInScene()
        {
            List<GameObject> objectsInScene = new List<GameObject>();

            foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
            {
                if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                    objectsInScene.Add(go);
            }

            return objectsInScene;
        }


        void FixSettings()
        {
            EditorGUILayout.LabelField("Incase this specific font doesnt have proper transform");

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Rotation Fix", GUILayout.MaxWidth(70));
            EditorGUILayout.PropertyField(rotationFix, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Position Fix", GUILayout.MaxWidth(70));
            EditorGUILayout.PropertyField(positionFix, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Scale Fix", GUILayout.MaxWidth(70));
            EditorGUILayout.PropertyField(scaleFix, GUIContent.none);
            GUILayout.EndHorizontal();
        }

        void GetFontSet()
        {
            EditorGUILayout.LabelField("Font - Set object");
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.PropertyField(fontSet);
            EditorGUILayout.PropertyField(overwriteOldSet);


            if (GUILayout.Button("Create/Recreate characters"))
            {
                myTarget.UpdateCharacterList();
                EditorUtility.SetDirty(target);
            }
            GUILayout.Space(5);

            EditorGUILayout.PropertyField(monoSpaceFont);
            GUIContent useUpperCase = new GUIContent("Use UpperCase If LowerCase Is Missing", "Use UpperCase If LowerCase Is Missing");
            EditorGUILayout.PropertyField(useUpperCaseLettersIfLowerCaseIsMissing, useUpperCase);
            EditorGUILayout.PropertyField(emptySpaceSpacing);
            EditorGUILayout.PropertyField(characterSpacing);
        }

        private void FallBackFont()
        {
            //EditorGUILayout.PropertyField(fallbackFonts);
            EditorGUILayout.LabelField(new GUIContent("Fallback font", "If this font has missing characters, it will try to get the character from font"));

            for (int i = 0; i < myTarget.fallbackFonts.Count; i++)
            {
                GUILayout.BeginHorizontal();

                if (fallbackFonts.arraySize > i)
                {
                    if (myTarget.fallbackFonts[i] == myTarget)
                    {
                        Debug.LogError("Unnecessary self reference found on fallback font :" + i, myTarget);
                        myTarget.fallbackFonts.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(fallbackFonts.GetArrayElementAtIndex(i), GUIContent.none);

                        if (GUILayout.Button("-", GUILayout.MaxWidth(30)))
                        {
                            myTarget.fallbackFonts.RemoveAt(i);
                        }
                    }
                }

                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("+"))
            {
                myTarget.fallbackFonts.Add(null);
                EditorUtility.SetDirty(target);
            }
        }

        int characterCountInAPage = 25;
        int currentPage;

        void CreateCharacterList()
        {


            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Page: " + (currentPage + 1));
            if (GUILayout.Button("Previous Page"))
            {
                currentPage--;
                if (currentPage < 0)
                    currentPage = myTarget.characters.Count / characterCountInAPage;
                EditorUtility.SetDirty(target);
            }
            if (GUILayout.Button("Next Page"))
            {
                currentPage++;
                if (currentPage > myTarget.characters.Count / characterCountInAPage)
                    currentPage = 0;
                EditorUtility.SetDirty(target);
            }
            GUILayout.EndHorizontal();



            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Char -", GUILayout.MaxWidth(45));
            EditorGUILayout.LabelField("Spacing -", GUILayout.MaxWidth(65));
            EditorGUILayout.LabelField("Prefab -", GUILayout.MaxWidth(55));
            EditorGUILayout.LabelField("or Mesh Asset");

            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);



            int startingNumber = currentPage * characterCountInAPage;
            if(startingNumber>= myTarget.characters.Count - 1)            
                startingNumber = 0;
            int endingNumber = startingNumber + characterCountInAPage;
            if (endingNumber >= myTarget.characters.Count)
                endingNumber = myTarget.characters.Count;

            for (int i = startingNumber; i < endingNumber; i++)
            {
                GUILayout.BeginHorizontal();

                //if (characters.arraySize > 0)
                if (characters.arraySize > i)
                {
                    //GUILayout.BeginVertical();
                    //var texture = AssetPreview.GetAssetPreview(myTarget.characters[i].prefab);
                    //if (texture)
                    //    GUILayout.Box(texture);

                    //EditorGUILayout.PropertyField(characters.GetArrayElementAtIndex(i).FindPropertyRelative("prefab"), GUIContent.none);
                    //GUILayout.EndVertical();

                    EditorGUILayout.PropertyField(characters.GetArrayElementAtIndex(i), GUIContent.none);

                    if (GUILayout.Button("-", GUILayout.MaxWidth(30)))
                    {
                        myTarget.characters.RemoveAt(i);
                    }
                }

                GUILayout.EndHorizontal();
            }

            if (currentPage == Mathf.FloorToInt(myTarget.characters.Count / characterCountInAPage))
            {
                if (GUILayout.Button("+"))
                {
                    MText_Character character = new MText_Character();
                    myTarget.characters.Add(character);
                    EditorUtility.SetDirty(target);
                }
            }

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Page: " + (currentPage + 1));
            if (GUILayout.Button("Previous Page"))
            {
                currentPage--;
                if (currentPage < 0)
                    currentPage = myTarget.characters.Count / characterCountInAPage;
                EditorUtility.SetDirty(target);
            }
            if (GUILayout.Button("Next Page"))
            {
                currentPage++;
                if (currentPage > myTarget.characters.Count / characterCountInAPage)
                    currentPage = 0;
                EditorUtility.SetDirty(target);
            }
            GUILayout.EndHorizontal();
        }

        private void KerningSettings()
        {
            if (myTarget.kernTable.Count > 0)
            {
                EditorGUILayout.LabelField(myTarget.kernTable.Count + " kern table");
                EditorGUILayout.PropertyField(enableKerning);
                EditorGUILayout.PropertyField(kerningMultiplier);

                if(GUILayout.Button("Clear kern table"))
                {
                    myTarget.kernTable.Clear();
                }
            }
        }
    }
}
