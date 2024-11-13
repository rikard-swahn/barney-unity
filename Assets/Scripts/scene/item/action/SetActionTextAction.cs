using System.Collections;
using Mgl;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.item;
using net.gubbi.goofy.ui;
using net.gubbi.goofy.util;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.scene.item.action {

    public class SetActionTextAction : SingleSceneItemAction {
        public string str;
        public Transform position;
        public bool timedClear = true;
        public float clearAfterSeconds = 1f;
        
        private IEnumerator actionTextRoutine;
        private CanvasTextPosition _canvasTextPos;
        
        protected override void Awake () {
            base.Awake();
            GameObject textGo = GameObject.Find (GameObjects.ACTION_TEXT);
            text = textGo.GetComponent<Text>();
            _canvasTextPos = textGo.GetComponent<CanvasTextPosition>();
        }        

        protected override void doAction (ItemType selectedItem) {
            _canvasTextPos.Pos = position.position;
            
            if (str.isNullOrEmpty()) {
                text.text = "";
            }
            else {
                text.text = I18n.Instance.__(str);

                if (timedClear) {
                    actionTextRoutine = waitAndClearActionText();
                    StartCoroutine(actionTextRoutine);
                }

            }

            afterAction();
        }
        
        private IEnumerator waitAndClearActionText () {
            yield return new WaitForSeconds(clearAfterSeconds);
            text.text = "";
        }        
    }

}