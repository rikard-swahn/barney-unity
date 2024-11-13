using System;
using net.gubbi.goofy.ui.menu.ingame;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui.menu.main {
    public class MessageLightbox : InGameLightbox {
        
        public Text text;

        private Action onOk;

        protected override void Awake() {
            base.Awake();
            gameObject.SetActive(true);
        }

        private void Start() {
            gameObject.SetActive(false);
        }

        public void show(string detailText, Action onOk = null) {
            text.text = detailText;
            this.onOk = onOk;
            show();
        }

        public void ok() {
            close();
            playClickSfx();
            
            if (onOk != null) {
                onOk();    
            }
        }
    }
}