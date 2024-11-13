using net.gubbi.goofy.state;
using UnityEngine;

namespace net.gubbi.goofy.scene.item {
    public class AgingItem : MonoBehaviour {

        public float agingTime;
        public string activateItem;
        public string[] requiresPropertyFlags;

        private float age;
        private SceneItem sceneItem;

        private void Awake () {
            this.sceneItem = GetComponent<SceneItem> ();

            if (GameState.Instance.StateData.hasSceneProperty (gameObjectName(), SceneItemProperties.AGE)) {
                age = GameState.Instance.StateData.getSceneProperty(gameObjectName(), SceneItemProperties.AGE).getFloat();
            }
        }

        private void Update() {
            if (sceneItem.isActive ()
                && hasPropertyFlags()) {

                age += Time.deltaTime;
                if (age >= agingTime) {
                    SceneItemUtil.setItemActive (activateItem, true);
                    SceneItemUtil.setItemActive (gameObjectName(), false);
                }

                GameState.Instance.StateData.setSceneProperty (gameObjectName(), SceneItemProperties.AGE, age);
            }
        }

        private string gameObjectName() {
            return gameObject.name;
        }

        private bool hasPropertyFlags() {
            if (requiresPropertyFlags == null) {
                return true;
            }

            foreach(var flag in requiresPropertyFlags) {
                if (!GameState.Instance.StateData.getSceneProperty(gameObjectName(), flag, false)) {
                    return false;
                }
            }

            return true;
        }

    }
}