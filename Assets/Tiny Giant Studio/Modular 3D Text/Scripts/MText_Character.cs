using UnityEngine;

namespace MText
{
    [System.Serializable]
    public class MText_Character
    {
        public char character;
        public GameObject prefab;
        public Mesh meshPrefab;
        public float spacing = 1;
        public int glyphIndex;
    }
}
