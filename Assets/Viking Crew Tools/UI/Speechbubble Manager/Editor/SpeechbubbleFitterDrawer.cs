using UnityEditor;

namespace VikingCrewTools {
    [CustomEditor(typeof(RatioLayoutFitter))]
    public class SpeechbubbleFitterDrawer : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            serializedObject.Update();
            base.DrawDefaultInspector();
            serializedObject.ApplyModifiedProperties();
        }
    }
}