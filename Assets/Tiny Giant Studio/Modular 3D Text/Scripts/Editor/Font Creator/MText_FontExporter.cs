using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MText.FontCreation
{
    public class MText_FontExporter
    {
        public void CreateFontFile(string prefabPath, string fontName, MText_FontCreator fontCreator)
        {
            MText_Font newFont = ScriptableObject.CreateInstance<MText_Font>();
            GameObject fontSet = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;

            newFont.UpdateCharacterList(fontSet);
            for (int i = 0; i < newFont.characters.Count; i++)
            {
                newFont.characters[i].glyphIndex = fontCreator.Index(newFont.characters[i].character);
            }

            if (fontCreator.KerningSupported())
                newFont = GetKerning(fontCreator, newFont);

            string scriptableObjectSaveLocation = EditorUtility.SaveFilePanel("Save font location", "", fontName, "asset");
            scriptableObjectSaveLocation = FileUtil.GetProjectRelativePath(scriptableObjectSaveLocation);
            AssetDatabase.CreateAsset(newFont, scriptableObjectSaveLocation);
            AssetDatabase.SaveAssets();
        }

        private MText_Font GetKerning(MText_FontCreator fontCreator, MText_Font newFont)
        {
            fontCreator.GetKerningInfo(out List<ushort> lefts, out List<ushort> rights, out List<short> offsets);

            for (int i = 0; i < lefts.Count; i++)
            {
                //kerning settings need to be redone
                //newFont.kernTable.Add(new MText_KernPairHolder(new MText_KernPair(lefts[i], rights[i]), offsets[i]));
            }

            return newFont;
        }
    }
}
