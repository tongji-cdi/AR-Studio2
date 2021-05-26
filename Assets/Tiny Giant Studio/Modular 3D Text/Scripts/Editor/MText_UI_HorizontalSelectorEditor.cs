using UnityEditor;

namespace MText
{
    [CustomEditor(typeof(MText_UI_HorizontalSelector))]
    public class MText_UI_HorizontalSelectorEditor : Editor
    {
        MText_UI_HorizontalSelector myTarget;
        SerializedObject soTarget;


        void OnEnable()
        {
            myTarget = (MText_UI_HorizontalSelector)target;
            soTarget = new SerializedObject(target);
        }

        public override void OnInspectorGUI()
        {
            if (myTarget.text == null)
                EditorGUILayout.HelpBox("Please reference a text",MessageType.Warning);

            EditorGUI.BeginChangeCheck();

            DrawDefaultInspector();

            if (EditorGUI.EndChangeCheck())
            {
                myTarget.UpdateText();
                soTarget.ApplyModifiedProperties();
            }
        }
    }
}