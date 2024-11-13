using net.gubbi.goofy.scene.action;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.scene.scenes.mickey.safe {
    public class Safe : MonoBehaviour {

        public int codeLength;
        public UIItemSceneActionHandler actionHandler;

        private Text safeCodeText;
        private static readonly string CODE_PROPERTY = "Code";

        private void Awake() {
            safeCodeText = GameObject.Find(GameObjects.SAFE_CODE_TEXT).GetComponent<Text>();
        }

        private void Start() {
            safeCodeText.text = GameState.Instance.StateData.getSceneProperty(gameObjectName(), CODE_PROPERTY, "");
        }

        public void enterDigit(int digit) {
            setCodeText(safeCodeText.text + digit);
            if (safeCodeText.text.Length == codeLength) {                
                actionHandler.doAction();
            }
        }

        private void setCodeText(string text) {
            safeCodeText.text = text;
            GameState.Instance.StateData.setSceneProperty(gameObjectName(), CODE_PROPERTY, text);            
        }

        private string gameObjectName() {
            return gameObject.name;
        }           
    }
}