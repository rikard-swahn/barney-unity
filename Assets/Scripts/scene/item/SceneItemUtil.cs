using net.gubbi.goofy.state;
using UnityEngine;

namespace net.gubbi.goofy.scene.item {
    public class SceneItemUtil {

        public static void setItemActive(string itemKey, bool active) {
            GameState.Instance.StateData.setSceneProperty (itemKey, SceneItemProperties.ACTIVE, active);

            GameObject item = GameObject.Find (itemKey);
            if(item != null) {
                item.GetComponent<SceneItem>().setActive(active);
            }
        }
        
        public static void setItemActiveTransient(string itemKey, bool active) {
            GameObject item = GameObject.Find (itemKey);
            if(item != null) {
                item.GetComponent<SceneItem>().setUiActive(active);
            }
        }
			
        public static void setItemActive(GameObject item, bool active) {
            GameState.Instance.StateData.setSceneProperty (item.name, SceneItemProperties.ACTIVE, active);
            item.GetComponent<SceneItem>().setActive(active);
        }
			
    }
}