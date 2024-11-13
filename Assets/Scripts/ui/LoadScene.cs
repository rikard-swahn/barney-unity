using net.gubbi.goofy.character;
using net.gubbi.goofy.scene.load;
using net.gubbi.goofy.scene.load.transition;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.ui {
    public class LoadScene : MonoBehaviour {
        
        public string scene;
        public Vector2 scenePosition;
        public Vector2 sceneDirection;
        public Transitions.Type outTransition;
        public Transitions.Type inTransition;         

        private CharacterMove playerMove;

        private void Awake() {
            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            if (playerGo != null) {
                playerMove = playerGo.GetComponent<CharacterMove>();
            }
        }        
        
        public void load() {
            if (playerMove != null) {
                if (!Vector2.zero.Equals(scenePosition)) {
                    playerMove.setPositionState(new Vector2(scenePosition.x, scenePosition.y));
                }
                if (!Vector2.zero.Equals(sceneDirection)) {
                    playerMove.setDirectionState(new Vector2(sceneDirection.x, sceneDirection.y));
                }
            }

            SceneLoader.Instance.changeInGameScene(scene, Transitions.get(outTransition), Transitions.get(inTransition));
        }        
        
    }
}