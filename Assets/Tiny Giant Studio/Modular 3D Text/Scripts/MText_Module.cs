using System.Collections;
using UnityEngine;


namespace MText
{
    public abstract class MText_Module : ScriptableObject
    {
        public abstract IEnumerator ModuleRoutine(GameObject obj, float duration);
    }
}
