using UnityEngine;

namespace net.gubbi.goofy.util {
    public class RaycastUtil {

        public static bool mouseRaycastHitsTag(string tag) {
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            foreach (RaycastHit2D hit in hits) {
                if (hit.collider != null) {
                    GameObject go = hit.collider.gameObject;
                    if (go.CompareTag(tag)) {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool childWithTagHitByMouseRaycast(string tag, GameObject parent) {
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            foreach (RaycastHit2D hit in hits) {
                if (hit.collider != null) {
                    GameObject go = hit.collider.gameObject;
                    if (go.CompareTag(tag) && go.transform.IsChildOf(parent.transform)) {
                        return true;
                    }
                }
            }

            return false;
        }
        
    }
}