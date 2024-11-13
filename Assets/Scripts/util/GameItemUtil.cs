using net.gubbi.goofy.extensions;
using net.gubbi.goofy.scene.item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.util {
    public class GameItemUtil {

        public static void setItemVisible(GameObject gameObject, bool active) {
            gameObject.GetComponentsInChildren<Collider2D> ().ForEach(c => c.enabled = active);
            
            Transform tr = gameObject.transform.Find (GameObjects.ORDERED_RENDERING);
            if (tr != null) {				
                tr.gameObject.GetComponent<Renderer> ().enabled = active;
            }

            Renderer[] renderers = gameObject.GetComponents<Renderer> ();
            foreach(Renderer renderer in renderers) {
                renderer.enabled = active;
            }            
        }
        
        public static void setItemVisible(UISceneItem uiSceneItem, bool active) {
            uiSceneItem.GetComponentsInChildren<Collider2D> ().ForEach(c => c.enabled = active);
            uiSceneItem.GetComponentsInChildren<Image> ().ForEach(r => r.enabled = active);
            uiSceneItem.GetComponentsInChildren<Text> ().ForEach(r => r.enabled = active);
            uiSceneItem.GetComponentsInChildren<TextMeshProUGUI> ().ForEach(r => r.enabled = active);
            uiSceneItem.GetComponentsInChildren<Selectable> ().ForEach(r => r.enabled = active);
            uiSceneItem.GetComponentsInChildren<UISceneItem> ().ForEach(r => r.enabled = active);
        }

    }
}