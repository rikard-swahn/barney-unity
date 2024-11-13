using System.Collections.Generic;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.extensions {
    public static class GameObjectExtensions {
        
        public static GameObject findChildWithTag(this GameObject parent, string tag) {			
            foreach(Transform child in parent.transform){
                if (child.CompareTag (tag)) {
                    return child.gameObject;
                }
            }		

            return null;
        }

        public static List<GameObject> findChildrenWithTag(this GameObject parent, string tag) {			
            List<GameObject> children = new List<GameObject> ();

            foreach(Transform child in parent.transform){
                if (child.CompareTag (tag)) {
                    children.Add (child.gameObject);                    
                }
                children.AddRange(findChildrenWithTag(child.gameObject, tag));
            }		

            return children;
        }
        
        public static IList<T> getComponentsInChildrenWithTag<T>(this GameObject parent, string tag) where T : Component{			
            IList<T> components = new List<T> ();

            IList<T> allComponents = parent.GetComponentsInChildren<T>();            
            foreach(T component in allComponents){
                if (component.CompareTag (tag)) {
                    components.Add (component);
                }
            }		

            return components;
        }

        public static Renderer getTopActiveRenderer(this GameObject go) {
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>();

            Renderer top = null;

            foreach (Renderer renderer in renderers) {

                if (renderer.enabled && renderer.gameObject.activeInHierarchy					
                    && (
                        top == null
                        || renderer.sortingLayerID > top.sortingLayerID
                        || renderer.sortingLayerID == top.sortingLayerID && renderer.sortingOrder > top.sortingOrder
                    )) {

                    top = renderer;
                }
            }

            return top;
        }

        public static GameObject getAncestorWithTag(this GameObject obj, string tag) {
            if (obj.CompareTag(tag)) {
                return obj;
            }

            if (obj.transform.parent == null) {
                return null;
            }

            return obj.transform.parent.gameObject.getAncestorWithTag(tag);
        }

        public static bool isMouseOverChildWithTag(this GameObject obj, string tag) {
            return MouseUtil.isHovering() && RaycastUtil.childWithTagHitByMouseRaycast(tag, obj);
        }
    }
}