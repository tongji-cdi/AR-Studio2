/// Created by Ferdowsur Asif @ Tiny Giant Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEditor;
using System.Text.RegularExpressions;
using System.Linq;

namespace MText
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MText_TextUpdater))]
    [AddComponentMenu("Modular 3D Text/3D Text")]

    /// <summary>
    /// The base script to draw 3D texts
    /// </summary>
    public class Modular3DText : MonoBehaviour
    {
        [FormerlySerializedAs("text")]
        [TextArea]
        [SerializeField] private string _text = string.Empty;
        public string Text
        {
            get { return _text; }
            set { _text = value; SetTextDirty(); }
        }
        private string ProcessedText => GetProcessedText();


        [SerializeField] private List<string> lineList = new List<string>();
        [SerializeField] private List<string> oldLineList = new List<string>();
        /// <summary>
        /// Contains a list of all the character gameobject created by Text
        /// </summary>
        public List<GameObject> characterObjectList = new List<GameObject>();
#if UNITY_EDITOR
        /// <summary>
        /// EDITOR ONLY!
        /// This holds all the reference for the all characters created to crosscheck if any characters are left over.
        /// This is due to unity editor not being able to delete/create without playmode on
        /// </summary>
        public List<GameObject> _allcharacterObjectList = new List<GameObject>();
#endif



        //Creation settings--------------------------------------------------------------------------------------
        [Tooltip("only prefabs need mesh to be saved")]
        public bool autoSaveMesh = false;

        //Main Settings------------------------------------------------------------------------------------------
        [FormerlySerializedAs("font")]
        [SerializeField] private MText_Font _font = null;
        public MText_Font Font
        {
            get { return _font; }
            set { _font = value; oldLineList.Clear(); SetTextDirty(); }
        }


        [FormerlySerializedAs("material")]
        [SerializeField] private Material _material;
        public Material Material
        {
            get { return _material; }
            set { _material = value; SetTextDirty(); }
        }

        [FormerlySerializedAs("fontSize")]
        [SerializeField] private Vector3 _fontSize = new Vector3(8, 8, 1);
        public Vector3 FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; SetTextDirty(); }
        }


        public int textDirection = 1; //Left to right or right to left //todo: change it to enum


        [FormerlySerializedAs("characterSpacingInput")]
        [SerializeField] private float _characterSpacing = 1;
        public float CharacterSpacing
        {
            get { return _characterSpacing; }
            set { _characterSpacing = value; SetTextDirty(); }
        }
        private float ModifiedCharacterSpacing => CharacterSpacing * 0.1f * FontSize.x;



        [FormerlySerializedAs("lineSpacingInput")]
        [SerializeField] private float _lineSpacing = 1;
        public float LineSpacing
        {
            get { return _lineSpacing; }
            set { _lineSpacing = value; SetTextDirty(); }
        }
        private float ModifiedLineSpacing => LineSpacing * 0.13f * FontSize.y;





        [FormerlySerializedAs("capitalize")]
        [SerializeField] private bool _capitalize = false;
        public bool Capitalize
        {
            get { return _capitalize; }
            set { _capitalize = value; SetTextDirty(); }
        }

        [FormerlySerializedAs("lowercase")]
        [SerializeField] private bool _lowercase = false;
        public bool LowerCase
        {
            get { return _lowercase; }
            set { _lowercase = value; SetTextDirty(); }
        }




        //Layout Settings----------------------------------------------------------------------------------------
        [FormerlySerializedAs("layoutStyle")]
        [SerializeField] private int _layoutStyle = 0;
        public int LayoutStyle
        {
            get { return _layoutStyle; }
            set { _layoutStyle = value; SetTextDirty(); }
        }


        [SerializeField]
        private TextAnchor _textAnchor;
        public TextAnchor TextAnchor
        {
            get
            {
                return _textAnchor;
            }
            set
            {
                if (value == TextAnchor.UpperLeft)
                {
                    alignTop = true;
                    alignMiddle = false;
                    alignBottom = false;

                    alignLeft = true;
                    alignCenter = false;
                    alignRight = false;
                }
                else if (value == TextAnchor.UpperCenter)
                {
                    alignTop = true;
                    alignMiddle = false;
                    alignBottom = false;

                    alignLeft = false;
                    alignCenter = true;
                    alignRight = false;
                }
                else if (value == TextAnchor.UpperRight)
                {
                    alignTop = true;
                    alignMiddle = false;
                    alignBottom = false;

                    alignLeft = false;
                    alignCenter = false;
                    alignRight = true;
                }
                else if (value == TextAnchor.MiddleLeft)
                {
                    alignTop = false;
                    alignMiddle = true;
                    alignBottom = false;

                    alignLeft = true;
                    alignCenter = false;
                    alignRight = false;
                }
                else if (value == TextAnchor.MiddleCenter)
                {
                    alignTop = false;
                    alignMiddle = true;
                    alignBottom = false;

                    alignLeft = false;
                    alignCenter = true;
                    alignRight = false;
                }
                else if (value == TextAnchor.MiddleRight)
                {
                    alignTop = false;
                    alignMiddle = true;
                    alignBottom = false;

                    alignLeft = false;
                    alignCenter = false;
                    alignRight = true;
                }
                else if (value == TextAnchor.LowerLeft)
                {
                    alignTop = false;
                    alignMiddle = false;
                    alignBottom = true;

                    alignLeft = true;
                    alignCenter = false;
                    alignRight = false;
                }
                else if (value == TextAnchor.LowerCenter)
                {
                    alignTop = false;
                    alignMiddle = false;
                    alignBottom = true;

                    alignLeft = false;
                    alignCenter = true;
                    alignRight = false;
                }
                else if (value == TextAnchor.LowerRight)
                {
                    alignTop = false;
                    alignMiddle = false;
                    alignBottom = true;

                    alignLeft = false;
                    alignCenter = false;
                    alignRight = true;
                }

                _textAnchor = value;
                SetTextDirty();
            }
        }

        //Alignment Horizontal
        public bool alignCenter = true;
        public bool alignLeft = false;
        public bool alignRight = false;

        //Alignment Vertical
        public bool alignTop = false;
        public bool alignMiddle = true;
        public bool alignBottom = false;




        //Alignment Circular
        [FormerlySerializedAs("circularAlignmentRadius")]
        [SerializeField] private float _circularAlignmentRadius = 5;
        public float CircularAlignmentRadius
        {
            get { return _circularAlignmentRadius; }
            set { _circularAlignmentRadius = value; SetTextDirty(); }
        }

        [FormerlySerializedAs("circularAlignmentSpreadAmount")]
        [SerializeField] private float _circularAlignmentSpreadAmount = 360;
        public float CircularAlignmentSpreadAmount
        {
            get { return _circularAlignmentSpreadAmount; }
            set { _circularAlignmentSpreadAmount = value; SetTextDirty(); }
        }

        [FormerlySerializedAs("circularAlignmentAngle")]
        [SerializeField] private Vector3 _circularAlignmentAngle = new Vector3(0, 0, 0);
        public Vector3 CircularAlignmentAngle
        {
            get { return _circularAlignmentAngle; }
            set { _circularAlignmentAngle = value; SetTextDirty(); }
        }



        [FormerlySerializedAs("height")]
        [SerializeField] private float _height = 2;
        public float Height
        {
            get { return _height; }
            set { _height = value; SetTextDirty(); }
        }


        [FormerlySerializedAs("length")]
        [SerializeField] private float _width = 15;
        public float Width
        {
            get { return _width; }
            set { _width = value; SetTextDirty(); }
        }
        private float adjustedWidth = 0; //adjustedForMistakes like 0 length and height
        public float depth = 1;


        //Spawn effects
        public List<MText_ModuleContainer> typingEffects = new List<MText_ModuleContainer>();
        public List<MText_ModuleContainer> deletingEffects = new List<MText_ModuleContainer>();
        public bool customDeleteAfterDuration = false;
        public float deleteAfter = 1;

        //advanced settings-----------------------------------------------------------------------------------------------
        [Tooltip("When text is updated, old characters are moved to their correct position if their position is moved by something like module.")]
        public bool repositionOldCharacters = true;
        public bool reApplyModulesToOldCharacters = false;
        //public bool activateChildObjects = true;

        [Tooltip("Pooling increases performence if you are changing lots of text when game is running.")]
        public bool pooling = false;
        public MText_Pool pool = null;

        //[Tooltip("Uses unity's Mesh.Combine method.\n" +
        //    "Unity has a limit of verticies one mesh can have which causes the bugs on large texts")]
        public bool combineMeshInEditor = false;
        public bool dontCombineInEditorAnyway = false;
        [Tooltip("There is no reason to turn this on unless you really need this for something. \nOtherwise, wasted resource on combining")]
        public bool combineMeshDuringRuntime = false;
        [Tooltip("Don't let letters show up in hierarchy in play mode. They are still there but not visible.")]
        public bool hideLettersInHierarchyInPlayMode = false;
        [Tooltip("If combine mesh is turned off")]
        public bool hideLettersInHierarchyInEditMode = false;

        [Tooltip("Breaks prefab connection while saving prefab location, can replace prefab at that location with a click")]
        public bool canBreakOutermostPrefab = false;
        //bool reconnectingPrefab = false;

        public string assetPath = string.Empty;
        [SerializeField] List<string> meshPaths = new List<string>();

        #region remember inspector layout
#if UNITY_EDITOR
        [HideInInspector] public bool showCreationettingsInEditor = false;
        [HideInInspector] public bool showMainSettingsInEditor = true;
        [HideInInspector] public bool showModuleSettingsInEditor = false;
        [HideInInspector] public bool showAdvancedSettingsInEditor = false;

        [Tooltip("This is Editor Only.\nEven if it's turned on by accident in build, it won't print.\nNote: This will spam console logs.")]
        [SerializeField] public bool debugLogs = false;
#endif
        #endregion remember inspector layout



        //data
        int charInOneLine;
        float x, y, z; //TODO: z 
        bool createChilds;

        public bool updateTextOncePerFrame = true;
        private bool runningRoutine = false;


        private void OnEnable()
        {
            runningRoutine = false;
        }

        /// <summary>
        /// Marks the text as dirty, needs to be cleaned up/Updated
        /// </summary>
        private void SetTextDirty()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                UpdateText();
            else
            {
                if (gameObject.activeInHierarchy && updateTextOncePerFrame)
                {
                    if (!runningRoutine)
                    {
                        runningRoutine = true;
                        StartCoroutine(UpdateRoutine());
                    }
                }
                else
                {
                    UpdateText();
                }
            }
#else
                if (gameObject.activeInHierarchy && updateTextOncePerFrame)
                {
                    if (!runningRoutine)
                    {
                        runningRoutine = true;
                        StartCoroutine(UpdateRoutine());
                    }
                }
                else
                {
                    UpdateText();
                }
#endif
        }

        /// <summary>
        /// The purpose of this coroutine is to make sure that texts aren't updated too many times in a single frame. But the downside is, it makes the text update after the end of the frame
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateRoutine()
        {
            yield return new WaitForEndOfFrame();
            UpdateText();
            runningRoutine = false;
        }



        private string GetProcessedText()
        {
            if (Capitalize)
                return Text.ToUpper();
            if (LowerCase)
                return Text.ToLower();

            return Text;
        }




        public void UpdateText(string newText)
        {
            if (!Font)
            {
                Debug.Log("No font assigned on " + gameObject.name, gameObject);
                return;
            }

            Text = newText;
            UpdateText();
        }
        public void UpdateText(float number)
        {
            if (!Font)
            {
                Debug.Log("No font assigned on " + gameObject.name, gameObject);
                return;
            }

            Text = number.ToString();
            UpdateText();
        }
        public void UpdateText(int number)
        {
            if (!Font)
            {
                Debug.Log("No font assigned on " + gameObject.name, gameObject);
                return;
            }

            Text = number.ToString();
            UpdateText();
        }

        [ContextMenu("Update")]
        public void UpdateText()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                EditorApplication.delayCall += () => UpdateTextBase();
            else
                UpdateTextBase();
#else
            UpdateTextBase();
#endif
        }

        private void UpdateTextBase()
        {
#if UNITY_EDITOR
            ///in case of something like build is started the exact frame after update text is called, 
            ///the delayed call calls to update text when the scene doesn't exist(?) and gives a null reference just once. Has mo impact. Just looks ugly
            if (!this)
                return;
#endif

            if (!Font)
                return;

            FixInvalidInputs(); //checks for mistakes like 0 length/height

            if (LayoutStyle == 0)
            {
                charInOneLine = CharacterInOneLineUpdate();
                if (charInOneLine < 1) charInOneLine = 1;
            }
            x = 0;

            createChilds = ShouldItCreateChild();

            SplitStuff();

            int newCharStartsFrom = 0;

            if (createChilds)
            {
                //text had combined mesh before
                if (GetComponent<MeshRenderer>())
                {
                    newCharStartsFrom = 0;
                    DestroyMeshRenderAndMeshFilter();
                }
                else
                {
                    newCharStartsFrom = CompareNewTextWithOld();
                }
            }

            oldLineList = new List<string>(lineList);
            DeleteReplacedChars(newCharStartsFrom);
            if (LayoutStyle == 0)
                GetPositionAtStart();

            CheckIfPoolExistsAndRequired();

            //linear
            if (LayoutStyle == 0)
            {
                if (repositionOldCharacters)
                {
                    PositionOldChars(newCharStartsFrom);
                }
                CreateNewChars(newCharStartsFrom);
            }
            else
            {
                CircularListProcessOldChars(newCharStartsFrom);
                CreateNewCharsForCircularList(newCharStartsFrom);
            }


#if UNITY_EDITOR
            if (debugLogs)
            {
                Debug.Log("Processed text is \n<color=green>" + ProcessedText + "</color>", gameObject);
            }
#endif

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                if (LayoutStyle == 1)
                    EditorApplication.delayCall += () => CircularPositioning();
                if (!createChilds)
                    EditorApplication.delayCall += () => CombineMeshes();
            }
            else
            {
                if (LayoutStyle == 1)
                    CircularPositioning();
                if (!createChilds)
                    CombineMeshes();
            }
#else
            if (LayoutStyle == 1) CircularPositioning();            
            if (!createChilds) CombineMeshes();
#endif



#if UNITY_EDITOR
            EditorApplication.delayCall += () => CheckLeftOversInEditorAndCleanUp();
#endif
        }




#if UNITY_EDITOR
        /// <summary>
        /// One user reported that there were few instances in Editor mode, where there were left over texts with hide flag enabled. 
        /// This can be due to destroy immediate not working for some reason. Since I was unable to replicate it,
        /// Just doing a blanket check on all objects created by the text.
        /// </summary>
        void CheckLeftOversInEditorAndCleanUp()
        {
            for (int i = _allcharacterObjectList.Count - 1; i >= 0; i--)
            {
                if (_allcharacterObjectList[i] == null)
                {
                    _allcharacterObjectList.Remove(_allcharacterObjectList[i]);
                    continue;
                }

                if (characterObjectList.Contains(_allcharacterObjectList[i]))
                {
                    EditorApplication.delayCall += () =>
                    {
                        try { DestroyImmediate(_allcharacterObjectList[i]); }
                        catch { }
                    };
                }
            }

            if (!hideLettersInHierarchyInEditMode && !Application.isPlaying)
            {

                if (this)
                {
                    foreach (Transform child in transform)
                    {
                        child.hideFlags = HideFlags.None;
                    }
                }
            }

        }
#endif

        void DestroyMeshRenderAndMeshFilter()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                EditorApplication.delayCall += () => DestroyImmediate(GetComponent<MeshRenderer>());
                EditorApplication.delayCall += () => DestroyImmediate(GetComponent<MeshFilter>());
            }
            else
            {
                Destroy(GetComponent<MeshRenderer>());
                Destroy(GetComponent<MeshFilter>());
            }
#else
            Destroy(GetComponent<MeshRenderer>());
            Destroy(GetComponent<MeshFilter>());
#endif
        }


        private bool ShouldItCreateChild()
        {
            bool createChilds = false;

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                if (!combineMeshInEditor)
                {
                    if (!this)
                        return true;

                    if (!PrefabUtility.IsPartOfPrefabInstance(gameObject) || (PrefabUtility.IsPartOfPrefabInstance(gameObject) && dontCombineInEditorAnyway))
                    {
                        createChilds = true;
                    }
                    else if (canBreakOutermostPrefab && PrefabBreakable())
                    {
                        RemovePrefabConnection();
                        createChilds = true;
                    }
                }
            }
            else if (!combineMeshDuringRuntime)
            {
                createChilds = true;
            }
#else
            if (!combineMeshDuringRuntime)
            {
                createChilds = true;
            }
#endif
            return createChilds;
        }

        void CheckIfPoolExistsAndRequired()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                return;
            }
#endif
            if (pooling)
            {
                if (!pool)
                {
                    pool = MText_Pool.Instance;
                    if (!pool)
                        CreatePool();
                }
            }
        }
        void CreatePool()
        {
            GameObject newPool = new GameObject();
            newPool.name = "Modular 3D Text Pool";
            newPool.AddComponent<MText_Pool>();
            pool = newPool.GetComponent<MText_Pool>();
            MText_Pool.Instance = pool;
        }

        void SplitStuff()
        {
            string delimiterChars = "([ \r\n])";
            string[] wordArray = Regex.Split(ProcessedText, delimiterChars);

            //Adds . , etc to the last word instead of a separate word       
            List<string> wordList = RemoveSpacesFromWordArray(wordArray).ToList();

            //Organized words to lines
            GetLineList(wordList);
        }
        string[] RemoveSpacesFromWordArray(string[] wordArray)
        {
            List<string> wordList = new List<string>();
            foreach (string str in wordArray)
            {
                if (str != " ")
                    wordList.Add(str);
            }

            return wordList.ToArray();
        }
        void GetLineList(List<string> wordList)
        {
            lineList = new List<string>();

            if (LayoutStyle == 0)
                GetLinearLineList(wordList);
            else
                GetCircularLineList(wordList);
        }

        private void GetLinearLineList(List<string> wordList)
        {
            float totalSpacinginCurrentLine = 0;
            string newText = "";

            for (int i = 0; i < wordList.Count; i++)
            {
                float currentWordSpacing = TotalSpacingRequiredFor(wordList[i]);

                //pressed enter 
                if (wordList[i].Contains("\n"))
                {
                    lineList.Add(newText);
                    if (wordList[i] == "\n") newText = "";
                    else
                    {
                        //This shouldn't be happening. Ever. I don't remember why I added this but keeping it until further test is done
                        newText = wordList[i];
                    }
                    totalSpacinginCurrentLine = 0;
                    currentWordSpacing = 0;
                }
                //if the word can be placed in current line
                else if (totalSpacinginCurrentLine + currentWordSpacing < adjustedWidth)
                {
                    //not the first word
                    if (totalSpacinginCurrentLine != 0) newText = string.Concat(newText, " ", wordList[i]);
                    //first word in line
                    else newText = string.Concat(newText, wordList[i]);
                }

                //the word is too big to fit in one line
                else if (currentWordSpacing > adjustedWidth)
                {
                    if (Font.monoSpaceFont)
                    {
                        int alreadyAdded = 0;
                        int lineRequiredForTheWord = Mathf.CeilToInt(wordList[i].Length / charInOneLine);
                        for (int j = 0; j < lineRequiredForTheWord; j++)
                        {
                            string part = wordList[i].Substring(alreadyAdded, charInOneLine);
                            lineList.Add(part);
                            alreadyAdded += charInOneLine;
                        }

                        newText = wordList[i].Substring(alreadyAdded);
                        totalSpacinginCurrentLine = TotalSpacingRequiredFor(newText);
                    }
                    else
                    {
                        if (newText != string.Empty)
                            lineList.Add(newText);

                        char[] chars = wordList[i].ToCharArray();
                        newText = string.Empty;
                        totalSpacinginCurrentLine = 0;

                        for (int j = 0; j < chars.Length; j++)
                        {
                            float mySpacing;
                            if (j == 0) mySpacing = Font.Spacing(chars[j]) * ModifiedCharacterSpacing;
                            else mySpacing = Font.Spacing(chars[j - 1], chars[j]) * ModifiedCharacterSpacing;

                            if (totalSpacinginCurrentLine + mySpacing <= adjustedWidth)
                            {
                                totalSpacinginCurrentLine += mySpacing;
                                newText += chars[j];
                            }
                            else
                            {
                                lineList.Add(newText);

                                newText = chars[j].ToString();
                                totalSpacinginCurrentLine = mySpacing;
                            }
                        }
                    }
                    currentWordSpacing = 0;
                }
                //new line
                else
                {
                    string s = newText;
                    if (s != "") lineList.Add(s);
                    newText = "";
                    totalSpacinginCurrentLine = 0;
                    newText = wordList[i];
                }

                totalSpacinginCurrentLine += currentWordSpacing;

                //last word
                if (i == wordList.Count - 1)
                {
                    lineList.Add(newText);
                }
            }
        }
        private void GetCircularLineList(List<string> wordList)
        {
            //this is the else
            float totalSpacinginCurrentLine = 0;
            string newText = "";

            float arcLength = 2 * Mathf.PI * CircularAlignmentRadius * (CircularAlignmentSpreadAmount / 360);


            for (int i = 0; i < wordList.Count; i++)
            {
                float currentWordSpacing = TotalSpacingRequiredFor(wordList[i]);

                //pressed enter 
                if (wordList[i].Contains("\n"))
                {
                    lineList.Add(newText);
                    if (wordList[i] == "\n") newText = "";
                    else
                    {
                        //This shouldn't be happening. Ever. I don't remember why I added this but keeping it until further test is done
                        newText = wordList[i];
                    }
                    totalSpacinginCurrentLine = 0;
                    currentWordSpacing = 0;
                }
                //if the word can be placed in current line
                else if (totalSpacinginCurrentLine + currentWordSpacing < arcLength)
                {
                    //not the first word
                    if (totalSpacinginCurrentLine != 0) newText = string.Concat(newText, " ", wordList[i]);
                    //first word in line
                    else newText = string.Concat(newText, wordList[i]);
                }

                //the word is too big to fit in one line
                else if (currentWordSpacing > arcLength)
                {
                    if (Font.monoSpaceFont)
                    {
                        int alreadyAdded = 0;
                        float charInCircle = arcLength / ModifiedCharacterSpacing;
                        int lineRequiredForTheWord = Mathf.CeilToInt(wordList[i].Length / charInCircle);
                        for (int j = 0; j < lineRequiredForTheWord; j++)
                        {
                            string part = wordList[i].Substring(alreadyAdded, charInOneLine);
                            lineList.Add(part);
                            alreadyAdded += charInOneLine;
                        }

                        newText = wordList[i].Substring(alreadyAdded);
                        totalSpacinginCurrentLine = TotalSpacingRequiredFor(newText);
                    }
                    else
                    {
                        if (newText != string.Empty)
                            lineList.Add(newText);

                        char[] chars = wordList[i].ToCharArray();
                        newText = string.Empty;
                        totalSpacinginCurrentLine = 0;

                        for (int j = 0; j < chars.Length; j++)
                        {
                            float mySpacing;
                            if (j == 0) mySpacing = Font.Spacing(chars[j]) * ModifiedCharacterSpacing;
                            else mySpacing = Font.Spacing(chars[j - 1], chars[j]) * ModifiedCharacterSpacing;

                            if (totalSpacinginCurrentLine + mySpacing <= arcLength)
                            {
                                totalSpacinginCurrentLine += mySpacing;
                                newText += chars[j];
                            }
                            else
                            {
                                lineList.Add(newText);

                                newText = chars[j].ToString();
                                totalSpacinginCurrentLine = mySpacing;
                            }
                        }
                    }
                    currentWordSpacing = 0;
                }
                //new line
                else
                {
                    string s = newText;
                    if (s != "") lineList.Add(s);
                    newText = "";
                    totalSpacinginCurrentLine = 0;
                    newText = wordList[i];
                }

                totalSpacinginCurrentLine += currentWordSpacing;

                //last word
                if (i == wordList.Count - 1)
                {
                    lineList.Add(newText);
                }
            }
        }


        void FixInvalidInputs()
        {
            if (Width != 0) adjustedWidth = Width;
            else adjustedWidth = 10;

            if (LayoutStyle == 1)
            {
                adjustedWidth = 50 * (CircularAlignmentSpreadAmount / 360);
            }
        }

        int CompareNewTextWithOld()
        {
            int newCharStartsFrom = 0;

            for (int i = 0; i < lineList.Count; i++)
            {
                //new line
                if (oldLineList.Count <= i)
                    return (newCharStartsFrom);

                char[] newChars = lineList[i].ToCharArray();
                char[] oldChars = oldLineList[i].ToCharArray();

                //Empty line was added. 
                if (newChars.Length == 0 || oldChars.Length == 0)
                    return (newCharStartsFrom);

                for (int j = 0; j < newChars.Length; j++)
                {
                    //less character than before
                    if (j >= oldChars.Length)
                        return (newCharStartsFrom); //was newCharStartsFrom - 1//testing

                    //character got replaced
                    if (newChars[j] != oldChars[j])
                        return (newCharStartsFrom);

                    newCharStartsFrom++;
                }
            }
            return (newCharStartsFrom);
        }

        void DeleteReplacedChars(int startingFrom)
        {
            //TODO
            //delete them straight from characterObjectList instead of storing in toDelete
            List<GameObject> toDelete = new List<GameObject>();
            for (int i = startingFrom; i < characterObjectList.Count; i++)
            {
                if (i >= startingFrom)
                {
                    toDelete.Add(characterObjectList[i]);
                }
            }
            if (toDelete.Count > 0)
            {
                foreach (GameObject child in toDelete)
                {
                    DestroyObject(child);
                    characterObjectList.Remove(child);
                }
            }
        }
        void DeleteAllChildObjects()
        {
            if (characterObjectList.Count == 0)
                return;

            for (int i = 0; i < characterObjectList.Count; i++)
            {
                if (!characterObjectList[i])
                    return;

#if UNITY_EDITOR
                if (!EditorApplication.isPlaying)
                {
                    if (!PrefabUtility.IsPartOfAnyPrefab(characterObjectList[i]))
                    {
                        characterObjectList[i].transform.SetParent(null);
                        GameObject objToDestroy = characterObjectList[i];
                        EditorApplication.delayCall += () => DestroyImmediate(objToDestroy);
                    }
                    else characterObjectList[i].SetActive(false);
                }
                else
                {
                    Destroy(characterObjectList[i]);
                }
#else
                Destroy(characterObjectList[i]);
#endif
            }
            characterObjectList.Clear();
        }
        void DestroyObject(GameObject obj)
        {
            if (!obj)
                return;

            if (characterObjectList.Contains(obj))
                characterObjectList.Remove(obj);

#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                if (gameObject.activeInHierarchy)
                    StartCoroutine(RunTimeDestroyObjectRoutine(obj));
                else
                    RunTimeDestroyObjectOnDisabledText(obj);
            }
            else
            {
                if (PrefabUtility.IsPartOfAnyPrefab(obj))
                {
                    obj.SetActive(false);
                }
                else
                {
                    EditorApplication.delayCall += () =>
               {
                   try { DestroyImmediate(obj); }
                   catch { }
               };
                }
            }
#else
            StartCoroutine(RunTimeDestroyObjectRoutine(obj));
#endif
        }
        IEnumerator RunTimeDestroyObjectRoutine(GameObject obj)
        {
            float maxDelay = 0;

            obj.transform.SetParent(null);
            if (obj.name != "space")
            {
                if (gameObject.activeInHierarchy)
                {
                    for (int i = 0; i < deletingEffects.Count; i++)
                    {
                        if (deletingEffects[i].module)
                        {
                            StartCoroutine(deletingEffects[i].module.ModuleRoutine(obj, deletingEffects[i].duration));
                            if (deletingEffects[i].duration > maxDelay) maxDelay = deletingEffects[i].duration;
                        }
                    }
                }
                if (!customDeleteAfterDuration)
                {
                    if (deletingEffects.Count > 0)
                    {
                        yield return new WaitForSeconds(maxDelay);
                    }
                }
                else
                    yield return new WaitForSeconds(deleteAfter);
            }

            if (pooling && pool)
                pool.returnPoolItem(obj);
            else Destroy(obj);
        }
        private void RunTimeDestroyObjectOnDisabledText(GameObject obj)
        {
            if (pooling && pool)
                pool.returnPoolItem(obj);
            else Destroy(obj);
        }


        #region Positioning
        void PositionOldChars(int startingFrom)
        {
            int lastCharacterPosition = 0;

            for (int i = 0; i < lineList.Count; i++)
            {
                if (lineList.Count > i)
                {
                    x = StartingX(lineList[i]);
                }

                char[] chars = lineList[i].ToCharArray();
                for (int j = 0; j < chars.Length; j++)
                {
                    if (lastCharacterPosition >= startingFrom)
                    {
                        break;
                    }
                    else
                    {
                        float characterIndividualSpacing;
                        if (j == 0) characterIndividualSpacing = (Font.Spacing(chars[0]));
                        else characterIndividualSpacing = (Font.Spacing(chars[j - 1], chars[j]));

                        float halfSpace = ModifiedCharacterSpacing * (characterIndividualSpacing / 2) * textDirection;

                        x += halfSpace;
                        //x += characterSpacing * (font.Spacing(chars[j]) / 2) * textDirection;

                        if (characterObjectList.Count > lastCharacterPosition)
                        {
                            PrepareCharacter(characterObjectList[lastCharacterPosition]);
#if UNITY_EDITOR
                            if (EditorApplication.isPlaying && reApplyModulesToOldCharacters)
                                ApplyEffects(characterObjectList[lastCharacterPosition]);
#else
                            if (reApplyModulesToOldCharacters)                                
                                    ApplyEffects(characterObjectList[lastCharacterPosition]);                                
#endif
                        }

                        //x += characterSpacing * (font.Spacing(chars[j]) / 2) * textDirection;
                        x += halfSpace;
                    }

                    lastCharacterPosition++;
                }

                y -= ModifiedLineSpacing;
            }
        }
        void GetPositionAtStart()
        {
            x = StartingX(lineList[0]);

            y = StartingY();
            z = 0;
        }


        float StartingX(string myString)
        {
            if (alignCenter)
            {
                if (!Font.monoSpaceFont)
                    return (-((TotalSpacingRequiredFor(myString) - (ModifiedCharacterSpacing * Font.emptySpaceSpacing)) / 2)) * textDirection;
                //return (-((TotalSpacingRequiredFor(myString) - (characterSpacing)) / 2)) * textDirection;
                else
                    return (-((myString.Length * ModifiedCharacterSpacing) / 2)) * textDirection;
            }
            else if (alignLeft)
            {
                return (-adjustedWidth / 2) * textDirection;
            }
            else
            {
                if (!Font.monoSpaceFont)
                    return ((adjustedWidth / 2) - (TotalSpacingRequiredFor(myString) - (ModifiedCharacterSpacing * Font.emptySpaceSpacing))) * textDirection;
                //return ((adjustedLength / 2) - (TotalSpacingRequiredFor(myString) - (characterSpacing))) * textDirection;
                else
                    return ((adjustedWidth / 2) - (myString.Length * ModifiedCharacterSpacing)) * textDirection;
            }
        }
        float TotalSpacingRequiredFor(string myString)
        {
            char[] chars = myString.ToCharArray();
            float totalSpace = 0;
            for (int i = 0; i < chars.Length; i++)
            {
                if (i == 0)
                    totalSpace += ModifiedCharacterSpacing * Font.Spacing(chars[0]);
                else
                    totalSpace += ModifiedCharacterSpacing * Font.Spacing(chars[i - 1], chars[i]);
            }
            totalSpace += ModifiedCharacterSpacing * Font.emptySpaceSpacing;
            return totalSpace;
        }
        float StartingY()
        {
            if (alignTop)
            {
                return (Height / 2 - (ModifiedLineSpacing / 2));
            }
            else if (alignMiddle)
            {
                if (lineList.Count == 1)
                    return 0;
                else
                    return lineList.Count / 2f * ModifiedLineSpacing - ModifiedLineSpacing / 2;
            }
            //alignbottom
            else
            {
                return (-Height / 2 + lineList.Count * ModifiedLineSpacing - (ModifiedLineSpacing / 2));
            }
        }
        int CharacterInOneLineUpdate()
        {
            return Mathf.FloorToInt(adjustedWidth / ModifiedCharacterSpacing);
        }

        #endregion Circular
        private void CircularPositioning()
        {
            float angle = 0;
            if (lineList.Count > 0)
            {
                if (lineList[0].Length == 1) angle = 0;
                else angle = (-(CircularAlignmentSpreadAmount / 2) + (CircularAlignmentSpreadAmount / lineList[0].Length) / 2) * textDirection;
            }
            int letterNumber = 0;
            int lineNumber = 0;
            float y = 0;

            for (int i = 0; i < characterObjectList.Count; i++)
            {

                if (letterNumber > lineList[lineNumber].Length - 1)
                {
                    y += ModifiedLineSpacing;

                    letterNumber = 0;
                    lineNumber++;

                    angle = 0;
                    if (lineList.Count > lineNumber)
                    {
                        if (lineList[lineNumber].Length > 1)
                            angle = (-(CircularAlignmentSpreadAmount / 2) + (CircularAlignmentSpreadAmount / lineList[lineNumber].Length) / 2) * textDirection;
                    }
                }
                letterNumber++;


                float x = Mathf.Sin(Mathf.Deg2Rad * angle) * CircularAlignmentRadius;
                float z = Mathf.Cos(Mathf.Deg2Rad * angle) * CircularAlignmentRadius;

                if (characterObjectList[i])
                {
                    characterObjectList[i].transform.localPosition = new Vector3(x, y, z);
                    characterObjectList[i].transform.localRotation = Quaternion.Euler(CircularAlignmentAngle.x, angle - CircularAlignmentAngle.y, CircularAlignmentAngle.z);
                }

                if (lineList.Count > lineNumber) //this is only to avoid error when editor is lagging. Keeping it outside a #IFEDITOR until further testsing is done
                    angle += (CircularAlignmentSpreadAmount / lineList[lineNumber].Length) * textDirection;
            }
        }
        private void CircularListProcessOldChars(int startingFrom)
        {
            int myCounter = 0;
            for (int i = 0; i < lineList.Count; i++)
            {
                foreach (char c in lineList[i])
                {
                    if (myCounter < startingFrom)
                    {
                        if (characterObjectList.Count >= myCounter)
                            break;
                        if (characterObjectList[i])
                        {
                            ApplyStyle(characterObjectList[myCounter]);
#if UNITY_EDITOR
                            if (EditorApplication.isPlaying && reApplyModulesToOldCharacters)
                                ApplyEffects(characterObjectList[myCounter]);
#else
                            if (reApplyModulesToOldCharacters)                                
                                    ApplyEffects(characterObjectList[myCounter]);                                
#endif
                        }
                    }

                    else
                        break;

                    myCounter++;
                }

            }
        }
        private void CreateNewCharsForCircularList(int startingFrom)
        {
            int myCounter = 0;
            for (int i = 0; i < lineList.Count; i++)
            {
                foreach (char c in lineList[i])
                {
                    if (myCounter >= startingFrom)
                    {
#if UNITY_EDITOR
                        if (!EditorApplication.isPlaying)
                        {
                            EditorApplication.delayCall += () => CreateCharForCircularList(c);
                        }
                        else
                        {
                            CreateCharForCircularList(c);
                        }
#else
                    CreateCharForCircularList(c);
#endif
                    }
                    myCounter++;
                }
            }
        }
        private void CreateCharForCircularList(char c)
        {
            if (!this)
                return;

            GameObject obj = MText_GetCharacterObject.GetObject(c, this);

            obj.transform.SetParent(transform);

            ApplyStyle(obj);
            AddCharacterToList(obj);
            ApplyEffects(obj);
        }

        private void CreateNewChars(int startingFrom)
        {
            int myCounter = 0;
            for (int i = 0; i < lineList.Count; i++)
            {
                if (myCounter + lineList[i].Length > startingFrom)
                {
                    x = StartingX(lineList[i]);
                    y = StartingY() - ModifiedLineSpacing * i;

                    string newString = "";
                    if (myCounter > startingFrom)
                    {
                        newString = lineList[i];
                    }
                    else
                    {
                        if (Font.monoSpaceFont)
                            x += ModifiedCharacterSpacing * (startingFrom - myCounter) * textDirection;
                        else
                        {
                            char[] previousStringChars = lineList[i].Substring(0, startingFrom - myCounter).ToCharArray();
                            //foreach (char c in previousStringChars)
                            for (int j = 0; j < previousStringChars.Length; j++)
                            {
                                if (j == 0)
                                    x += ModifiedCharacterSpacing * Font.Spacing(previousStringChars[0]) * textDirection;
                                else
                                    x += ModifiedCharacterSpacing * Font.Spacing(previousStringChars[j - 1], previousStringChars[j]) * textDirection;
                            }
                        }
                        newString = lineList[i].Substring(startingFrom - myCounter);
                    }


                    //foreach (char c in newString)
                    for (int j = 0; j < newString.Length; j++)
                    {
                        float characterIndividualSpacing;
                        if (j == 0) characterIndividualSpacing = (Font.Spacing(newString[0]));
                        else characterIndividualSpacing = (Font.Spacing(newString[j - 1], newString[j]));

                        float halfSpace = ModifiedCharacterSpacing * (characterIndividualSpacing / 2) * textDirection;

                        //x += characterSpacing * (font.Spacing(c) / 2) * textDirection;
                        x += halfSpace;
#if UNITY_EDITOR
                        if (!EditorApplication.isPlaying)
                        {
                            float X = x;
                            float Y = y;
                            float Z = z;
                            Transform tr = transform;
                            char c = newString[j];
                            EditorApplication.delayCall += () => CreateAndPrepareCharacter(c, X, Y, Z, tr);
                        }
                        else
                            CreateThisChar(newString[j]);
#else
                       //CreateThisChar(c);
                       CreateThisChar(newString[j]);
#endif
                        //x += characterSpacing * (font.Spacing(c) / 2) * textDirection;
                        x += halfSpace;
                    }
                }
                myCounter += lineList[i].Length;
            }
        }
        private void CreateThisChar(char c)
        {
            GameObject obj = MText_GetCharacterObject.GetObject(c, this);

            AddCharacterToList(obj);
            PrepareCharacter(obj);
            ApplyEffects(obj);
        }

        //positioning and creating in playmode
        private void PrepareCharacter(GameObject obj)
        {
            if (!this)
                return;

            if (obj)
            {
                obj.transform.SetParent(transform);
                obj.transform.localPosition = new Vector3(x + Font.positionFix.x, y + Font.positionFix.y, z + Font.positionFix.z);
                obj.transform.localRotation = Quaternion.Euler(Font.rotationFix.x, Font.rotationFix.y, Font.rotationFix.z);
                ApplyStyle(obj);
            }
        }
        #region Prepare Character
        //creating in editor
        void CreateAndPrepareCharacter(char c, float myX, float myY, float myZ, Transform tr)
        {
            if (!tr) return;

            GameObject obj = MText_GetCharacterObject.GetObject(c, this);
            if (obj)
            {
                AddCharacterToList(obj);

                obj.transform.SetParent(tr);
                obj.transform.localPosition = new Vector3(myX + Font.positionFix.x, myY + Font.positionFix.y, myZ + Font.positionFix.z);
                obj.transform.localRotation = Quaternion.Euler(Font.rotationFix.x, Font.rotationFix.y, Font.rotationFix.z);
                ApplyStyle(obj);
            }
        }

        void AddCharacterToList(GameObject obj) => characterObjectList.Add(obj);
        #endregion

        private void ApplyEffects(GameObject obj)
        {
            if (!gameObject.activeInHierarchy)
                return;

            if (obj.name != "space")
            {
                for (int i = 0; i < typingEffects.Count; i++)
                {
                    if (typingEffects[i].module) StartCoroutine(typingEffects[i].module.ModuleRoutine(obj, typingEffects[i].duration));
                }
            }
        }
        private void ApplyStyle(GameObject obj)
        {
            if (obj.GetComponent<MText_Letter>())
            {
                if (obj.GetComponent<MText_Letter>().model)
                {
                    obj.GetComponent<MText_Letter>().model.material = Material;
                }
            }
            if (obj.GetComponent<MeshFilter>())
            {
                if (!obj.GetComponent<MeshRenderer>())
                    obj.AddComponent<MeshRenderer>();

                obj.GetComponent<MeshRenderer>().material = Material;
            }



            obj.transform.localScale = new Vector3(FontSize.x, FontSize.y, FontSize.z / 2);

#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
                obj.layer = gameObject.layer;
            else
            {
                try
                {
                    EditorApplication.delayCall += () => SetLayer(obj);
                }
                catch
                {

                }
            }
#else
            SetLayer(obj);
#endif
        }
        private void SetLayer(GameObject obj)
        {
            if (obj)
                obj.layer = gameObject.layer;
        }

        private void AddToList(GameObject combinedMeshHolder)
        {
            characterObjectList.Add(combinedMeshHolder);
        }





        public void EmptyEffect(List<MText_ModuleContainer> moduleList)
        {
            MText_ModuleContainer module = new MText_ModuleContainer();
            module.duration = 0.5f;
            moduleList.Add(module);
        }
        public void NewEffect(List<MText_ModuleContainer> moduleList, MText_Module newModule)
        {
            MText_ModuleContainer module = new MText_ModuleContainer();
            module.duration = 0.5f;
            module.module = newModule;
            moduleList.Add(module);
        }
        public void ClearAllEffects()
        {
            typingEffects.Clear();
            deletingEffects.Clear();
        }






        #region Utility
        [ContextMenu("Combine meshes")]
        void CombineMeshes()
        {
            if (!this) //TODO
                return;

            if (!gameObject)
                return;

            Vector3 oldPos = transform.position;
            Quaternion oldRot = transform.rotation;
            Vector3 oldScale = transform.localScale;

            transform.rotation = Quaternion.identity;
            transform.position = Vector3.zero;
            transform.localScale = Vector3.one;

            if (GetComponent<MeshFilter>())
            {
                //using shared mesh also clears any copy of the text. For example, made with duplicating
                //GetComponent<MeshFilter>().sharedMesh.Clear();
                GetComponent<MeshFilter>().mesh = null;
            }


            List<List<MeshFilter>> meshFiltersUpperList = new List<List<MeshFilter>>();
            int listNumber = 0;
            List<MeshFilter> firstList = new List<MeshFilter>();
            meshFiltersUpperList.Add(firstList);

            int verteciesCount = 0;
            for (int j = 0; j < characterObjectList.Count; j++)
            {
                if (characterObjectList[j])
                {
                    if (characterObjectList[j].GetComponent<MeshFilter>())
                    {
                        if (verteciesCount + characterObjectList[j].GetComponent<MeshFilter>().sharedMesh.vertices.Length < 65535)
                        {
                            verteciesCount += characterObjectList[j].GetComponent<MeshFilter>().sharedMesh.vertices.Length;
                            meshFiltersUpperList[listNumber].Add(characterObjectList[j].GetComponent<MeshFilter>());
                        }
                        else
                        {
                            verteciesCount = 0;
                            List<MeshFilter> newList = new List<MeshFilter>();
                            meshFiltersUpperList.Add(newList);
                            listNumber++;
                            verteciesCount += characterObjectList[j].GetComponent<MeshFilter>().sharedMesh.vertices.Length;
                            meshFiltersUpperList[listNumber].Add(characterObjectList[j].GetComponent<MeshFilter>());
                        }
                    }
                }
            }

            for (int k = 0; k < meshFiltersUpperList.Count; k++)
            {
                MeshFilter[] meshFilters = meshFiltersUpperList[k].ToArray();
                CombineInstance[] combine = new CombineInstance[meshFilters.Length];

                int i = 0;
                while (i < meshFilters.Length)
                {
                    combine[i].mesh = meshFilters[i].sharedMesh;
                    combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                    i++;
                }

                if (!GetComponent<MeshFilter>())
                    gameObject.AddComponent<MeshFilter>();

                List<CombineInstance> combinedList = new List<CombineInstance>();
                for (int j = 0; j < combine.Length; j++)
                {
                    if (combine[j].mesh != null)
                        combinedList.Add(combine[j]);
                }
                combine = combinedList.ToArray();

                Mesh finalMesh = new Mesh();
                finalMesh.CombineMeshes(combine);

                if (k == 0)
                {
                    GetComponent<MeshFilter>().mesh = finalMesh;
                    if (!GetComponent<MeshRenderer>())
                        gameObject.AddComponent<MeshRenderer>();
                    GetComponent<MeshRenderer>().material = Material;
                }
                else
                {
                    GameObject combinedMeshHolder = new GameObject();
                    combinedMeshHolder.name = "Combined mesh 2";
                    combinedMeshHolder.transform.SetParent(transform);
                    combinedMeshHolder.transform.rotation = Quaternion.identity;
                    combinedMeshHolder.transform.localPosition = Vector3.zero;

                    combinedMeshHolder.AddComponent<MeshFilter>();
                    combinedMeshHolder.GetComponent<MeshFilter>().mesh = finalMesh;

                    combinedMeshHolder.AddComponent<MeshRenderer>();
                    combinedMeshHolder.GetComponent<MeshRenderer>().material = Material;
#if UNITY_EDITOR
                    EditorApplication.delayCall += () => AddToList(combinedMeshHolder);
#endif
                }
            }

            transform.position = oldPos;
            transform.rotation = oldRot;
            transform.localScale = oldScale;

            DeleteAllChildObjects();

#if UNITY_EDITOR
            if (autoSaveMesh) SaveMeshAsAsset(false);
#endif
        }

#if UNITY_EDITOR
        public bool PrefabBreakable()
        {
            if (!EditorApplication.isPlaying)
            {
                if (PrefabUtility.IsPartOfPrefabInstance(gameObject))
                {
                    if (!PrefabUtility.IsOutermostPrefabInstanceRoot(gameObject))
                        return false;
                    if (PrefabUtility.IsPartOfVariantPrefab(gameObject))
                        return false;
                }
                return true;
            }
            else
            {
                return true;
            }
        }
        public void ReconnectPrefabs()
        {
            //reconnectingPrefab = true;
            PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, assetPath, InteractionMode.AutomatedAction);
        }
        public void SaveMeshAsAsset(bool saveAsDifferent)
        {
            if (!EditorApplication.isPlaying)
            {
                bool canceledAction = false;

                //gets save path from explorer
                if (CanSavePath() || saveAsDifferent)
                {
                    canceledAction = GetPaths();
                }

                if (!canceledAction) SaveAsset();
            }
        }
        private void RemovePrefabConnection()
        {
            assetPath = AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromSource(gameObject));
            PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }

        private void SaveAsset()
        {
            if (GetComponent<MeshFilter>())
            {
                //not trying to overwrite with same mesh
                if (AssetDatabase.LoadAssetAtPath(meshPaths[0], typeof(Mesh)) == GetComponent<MeshFilter>().sharedMesh)
                {
                    //Debug.Log("<color=green>The current mesh is already the asset at selected location. No need to overwrite.</color>");
                }
                else
                {
                    AssetDatabase.CreateAsset(GetComponent<MeshFilter>().sharedMesh, meshPaths[0]);
                    AssetDatabase.SaveAssets();
                }
            }

            for (int i = 0; i < characterObjectList.Count; i++)
            {
                if (characterObjectList[i])
                {
                    if (!characterObjectList[i].GetComponent<MeshFilter>())
                        break;

                    //not trying to overwrite with same mesh
                    if (AssetDatabase.LoadAssetAtPath(meshPaths[i], typeof(Mesh)) == characterObjectList[i].GetComponent<MeshFilter>().sharedMesh)
                    {
                        //Debug.Log("<color=green>The current mesh is already the asset at selected location. No need to overwrite.</color>");
                    }
                    else
                    {
                        AssetDatabase.CreateAsset(characterObjectList[i].GetComponent<MeshFilter>().sharedMesh, meshPaths[i + 1]); //path i+1 because 0 is taken by main object
                        AssetDatabase.SaveAssets();
                    }
                }
            }
        }

        private bool CanSavePath()
        {
            bool saveAs = true;
            if (meshPaths.Count > 0)
            {
                if (string.IsNullOrEmpty(meshPaths[0]))
                {
                    saveAs = false;
                }
            }
            return saveAs;
        }
        private bool GetPaths()
        {
            meshPaths.Clear();
            for (int i = 0; i < characterObjectList.Count + 1; i++)
            {
                string meshNumber;
                if (i == 0) meshNumber = string.Empty;
                else meshNumber = "mesh " + i;

                string path = EditorUtility.SaveFilePanel("Save Separate Mesh" + i + " Asset", "Assets/", name + meshNumber, "asset");

                if (string.IsNullOrEmpty(path))
                    return true;
                else
                    path = FileUtil.GetProjectRelativePath(path);

                meshPaths.Add(path);
            }
            return false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = new Color(0, 0, 0, 1f);
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(Width, Height, depth));
        }
#endif
        #endregion Utility
    }
}