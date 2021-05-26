using UnityEngine;

namespace MText
{
    [CreateAssetMenu(menuName = "Modular 3d Text/Settings")]
    public class MText_Settings : ScriptableObject
    {
        [HideInInspector] public string selectedTab = "Getting Started";

        public Color thirdBackgroundColor = new Color(0.9f, 0.9f, 0.9f);
        public Color thirdPropertyFieldColor = Color.white;


        public float smallHorizontalFieldSize = 72.5f;
        public float normalltHorizontalFieldSize = 100;
        public float largeHorizontalFieldSize = 132.5f;
        public float extraLargeHorizontalFieldSize = 150f;


        //default text settings
        public MText_Font defaultFont = null;
        public Vector3 defaultTextSize = new Vector3(8, 8, 2);
        public Material defaultTextMaterial = null;
        public Material defaultBackgroundMaterial = null;
        public bool autoCreateInEditorMode = true;
        public bool autoCreateInPlayMode = true;


        


        [HideInInspector] public int vertexDensity = 1; //default 1
        [HideInInspector] public float sizeXY = 1; //default 1
        [HideInInspector] public float sizeZ = 1; //default 1
        [HideInInspector] public float smoothingAngle = 30; //default 30

        [HideInInspector] public MeshExportStyle meshExportStyle = MeshExportStyle.exportAsObj;

        //[HideInInspector] public bool createLogTextFile = false;
        //[HideInInspector] public bool createConsoleLogs = false;


        public enum MeshExportStyle
        {
            exportAsObj,
            exportAsMeshAsset
        }



        #region Font creation
        //font creation settings
        public enum CharInputStyle
        {
            CharacterRange,
            UnicodeRange,
            CustomCharacters,
            UnicodeSequence
            //CharacterSet
        }

        public CharInputStyle charInputStyle;


        public char startChar = '!'; //default '!'
        public char endChar = '~'; //default '~'
        public string startUnicode = "0021"; //default
        [HideInInspector] public string endUnicode = "007E"; //default 

        [HideInInspector]
        [TextArea(10, 99)]
        public string customCharacters = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~"; //default 

        [HideInInspector]
        [TextArea(10, 99)]
        public string unicodeSequence = "\\u0021-\\u007E"; //default     
        #endregion
    }
}
