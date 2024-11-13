using net.gubbi.goofy.scene.slope;
using UnityEditor;
using UnityEngine;

namespace Editor.net.gubbi.goofy.editor.scene {
    
    [CustomEditor(typeof(SlopeOrderedRendering))]
    [CanEditMultipleObjects]
    public class SlopeOrderedRenderingEditor : UnityEditor.Editor {
        private BoxCollider2D _boxCollider;

        private void Awake() {
            _boxCollider = Target.gameObject.GetComponent<BoxCollider2D>();
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector ();
            
            float dy = Target.slopeData.rightHeight - Target.slopeData.leftHeight;
            float dx = _boxCollider.size.x;
            float angle = Mathf.Atan(dy / dx) * Mathf.Rad2Deg;
            
            GUI.enabled = false;            
            EditorGUILayout.FloatField(new GUIContent("Angle"), angle);
            GUI.enabled = true;            
        }

        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.Active)]
        static void DrawGizmo(SlopeOrderedRendering target, GizmoType gizmoType) {
            if (target.slopeData == null) {
                return;
            }       

            BoxCollider2D boxCollider = target.gameObject.GetComponent<BoxCollider2D>();
            Vector2 targetPos = target.gameObject.transform.position;
            
            Vector2 left = new Vector3(targetPos.x + boxCollider.offset.x - boxCollider.size.x / 2, targetPos.y + boxCollider.offset.y + target.slopeData.leftHeight);
            Vector2 right = new Vector3(targetPos.x + boxCollider.offset.x + boxCollider.size.x / 2, targetPos.y + boxCollider.offset.y + target.slopeData.rightHeight);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(left, right);
        }        

        private SlopeOrderedRendering Target {
            get {return target as SlopeOrderedRendering;}
        }        
         
        
    }
}