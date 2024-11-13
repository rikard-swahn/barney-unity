using net.gubbi.goofy.editor.extensions;
using net.gubbi.goofy.scene;
using UnityEditor;
using UnityEngine;

namespace Editor.net.gubbi.goofy.editor.scene {
    
    [CustomEditor(typeof(ScrollTeleport))]
    [CanEditMultipleObjects]
    public class ScrollTeleportEditor : UnityEditor.Editor {
     
        void OnSceneGUI() {
            Undo.RecordObject(Target, "Scroll Teleport Rect Edit");
            Target.allowedRect = editRect(Target.allowedRect, Color.green, Color.yellow, Color.green);             
            Target.spawnRect = editRect(Target.spawnRect, Color.green, Color.magenta, Color.green);             
        }

        private static Rect editRect(Rect rect, Color border, Color fill, Color handles) {            
            Rect newScreenRect = rect.EditorResize(
                Handles.CubeHandleCap,
                border,
                fill,
                HandleUtility.GetHandleSize(Vector3.zero) * 0.1f,
                0.1f
            );

            Handles.color = handles;

            Vector2 pos = Handles.DoPositionHandle(rect.position, Quaternion.identity);
            if (pos != rect.position) {
                newScreenRect = new Rect(pos, rect.size);
            }
            return newScreenRect;
        }

        private ScrollTeleport Target {
            get {return target as ScrollTeleport;}
        }         
        
    }
}