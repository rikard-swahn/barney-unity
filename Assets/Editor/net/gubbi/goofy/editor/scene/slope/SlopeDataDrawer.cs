using net.gubbi.goofy.scene.slope;
using UnityEditor;
using UnityEngine;

namespace Editor.net.gubbi.goofy.editor.scene.slope {
    
    [CustomPropertyDrawer(typeof(SlopeData))]
    public class SlopeDataDrawer : PropertyDrawer {
        
        private float lineHeight;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            lineHeight =  base.GetPropertyHeight(property, label);
            return (property.objectReferenceValue == null) ? lineHeight : lineHeight * 3;
        }

        public override void OnGUI (Rect rect, SerializedProperty prop, GUIContent label) {               
            EditorGUI.PropertyField(rect, prop, label, true);
            
            if (prop.objectReferenceValue == null) {
                return;
            }                        
            SerializedObject so = new SerializedObject(prop.objectReferenceValue);
            
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + lineHeight, rect.width, lineHeight), so.FindProperty("leftHeight"));                        
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + lineHeight * 2, rect.width, lineHeight), so.FindProperty("rightHeight"));

            so.ApplyModifiedProperties();
        }
    }
}