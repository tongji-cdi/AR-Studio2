using UnityEngine;

namespace MText
{
    //this script exists only to update the ui list in editor. Is not relevant anywhere else
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class MText_UI_ListEditorHelper : MonoBehaviour
    {
#if UNITY_EDITOR           
        MText_UI_List List => GetComponent<MText_UI_List>();

        private void OnEnable()
        {
            if (UnityEditor.EditorApplication.isPlaying)            
                this.enabled = false;            
        }

        private void Update()
        {
            List.UpdateList();
        }
#endif
    }
}