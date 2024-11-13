using net.gubbi.goofy.scene.action;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui.phone {
    public class Phone : MonoBehaviour {

        public Animator disc;
        public UIItemSceneActionHandler actionHandler;
        public LoadScene loadScene;
        
        private Text phoneNumberText;        

        private static readonly string NUMBER_PREFIX = "555-";
        private static readonly string NUMBER_PROPERTY = "Number";

        private void Awake() {
            phoneNumberText = GameObject.Find(GameObjects.PHONE_NUMBER_TEXT).GetComponent<Text>();
        }

        private void Start() {
            phoneNumberText.text = GameState.Instance.StateData.getSceneProperty(gameObjectName(), NUMBER_PROPERTY, NUMBER_PREFIX);         
        }

        public void dial(int digit) {
            phoneNumberText.text += digit;
            GameState.Instance.StateData.setSceneProperty(gameObjectName(), NUMBER_PROPERTY, phoneNumberText.text);            
            disc.SetTrigger("Dial_" + digit);                       
            actionHandler.doAction();
        }

        public void exit() {
            GameState.Instance.StateData.removeSeneProperty(gameObjectName(), NUMBER_PROPERTY);
            loadScene.load();
        }
   
        private string gameObjectName() {
            return gameObject.name;
        }        
    }
}