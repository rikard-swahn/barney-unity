using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace net.gubbi.goofy.character
{
    public class CharacterSceneHandler : MonoBehaviour {

        public bool defaultCurScene;
        public bool setToCurrentScene;
        public bool defaultActive = true;
        public bool defaultVisible = true;
        public bool deactivateOnStart;

        private InitiableMonoBehaviour[] initiableScripts;

        private void Awake() {
            initiableScripts = GetComponentsInChildren<InitiableMonoBehaviour>();
        }

        private void Start() {
            if (deactivateOnStart) {
                setCharacterActiveState(false);
            }

            refreshCharacterScene ();
            initScriptsIfObjectActive();
        }		

        public void setCharacterSceneToCurrent() {
            string currentScene = SceneManager.GetActiveScene ().name;
            setCharacterSceneState (currentScene);
            refreshCharacterScene ();
        }			

        public void setCharacterScene(string scene) {
            setCharacterSceneState (scene);
            refreshCharacterScene ();
        }	

        public void setCharacterActiveState(bool active)  {
            string characterKey = gameObject.name;
            GameState.Instance.StateData.setCharacterActive(characterKey, active);
        }

        public void setCharacterActive(bool active)  {
            setCharacterActiveState (active);
            refreshCharacterScene ();
        }

        public void setCharacterVisibleState(bool visible)  {
            string characterKey = gameObject.name;
            GameState.Instance.StateData.setCharacterVisible(characterKey, visible);
        }

        public void setCharacterVisible(bool visible)  {
            setCharacterVisibleState (visible);
            refreshCharacterScene ();
        }

        public bool isCharacterInCurrentScene() {
            return gameObject.activeInHierarchy;
        }

        public bool isCharacterInScene(string scene) {
            return scene.Equals(getCharacterScene ());
        }

        private string getCharacterScene() {
            string characterKey = gameObject.name;
            return GameState.Instance.StateData.getCharacterScene (characterKey);
        }
			
        private void setCharacterSceneState(string scene) {			
            string characterKey = gameObject.name;
            GameState.Instance.StateData.changeCharacterScene (characterKey, scene);
        }
						
        private void refreshCharacterScene () {			
            string characterScene = getCharacterScene();
            string currentScene = SceneManager.GetActiveScene ().name;
            string characterKey = gameObject.name;

            if (characterScene == null && defaultCurScene || setToCurrentScene && !currentScene.Equals (characterScene)) {
                setCharacterSceneState (currentScene);
                characterScene = currentScene;
            }
            
            if (GameState.Instance.StateData.getCharacterActive(characterKey) == null) {
                setCharacterActiveState(defaultActive);
            }            

            bool inCurrentScene = currentScene.Equals (characterScene) && GameState.Instance.StateData.isCharacterActive(characterKey);

            bool wasActive = gameObject.activeInHierarchy;
            gameObject.SetActive(inCurrentScene);          
            //Debug.Log(gameObject.name + " set to active = " + inCurrentScene);
            
            if (!wasActive) {
                initScriptsIfObjectActive();
            }            

            if (GameState.Instance.StateData.getCharacterVisible(characterKey) == null) {
                setCharacterVisibleState(defaultVisible);
            }

            GameItemUtil.setItemVisible(gameObject, GameState.Instance.StateData.isCharacterVisible(characterKey));
        }
        //TODO: This thing should probably not have been done.
        private void initScriptsIfObjectActive() {
            if (gameObject.activeInHierarchy) {
                foreach (var script in initiableScripts) {
                    script.init();
                }
            }
        }
    }
}