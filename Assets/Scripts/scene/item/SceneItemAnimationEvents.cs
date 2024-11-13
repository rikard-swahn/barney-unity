using UnityEngine;

namespace net.gubbi.goofy.scene.item {
    public class SceneItemAnimationEvents : MonoBehaviour {

        public void setSceneItemActive (string id) {
            SceneItemUtil.setItemActive (id, true);
        }

        public void setSceneItemInactive (string id) {
            SceneItemUtil.setItemActive (id, false);
        }

    }
}