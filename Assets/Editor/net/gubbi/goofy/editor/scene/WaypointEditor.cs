using net.gubbi.goofy.scene;
using UnityEditor;
using UnityEngine;

namespace Editor.net.gubbi.goofy.editor.scene {
    
    [CustomEditor(typeof(Waypoint))]
    [CanEditMultipleObjects]
    public class WaypointEditor : UnityEditor.Editor {
        
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.Active)]
        static void DrawGizmo(Waypoint wp, GizmoType gizmoType) {
            Vector3 position = wp.transform.position;
            Gizmos.color = Color.green;
            Gizmos.DrawCube(position, wp.size);
        }        
    }
}