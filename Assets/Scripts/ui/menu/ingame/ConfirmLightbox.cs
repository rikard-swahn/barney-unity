using System;
using net.gubbi.goofy.ui.menu.ingame;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui.menu.main {
    public class ConfirmLightbox : InGameLightbox {
        
        public Text text;

        private Action onPositive;
        private Action onNegative;

        protected override void Awake() {
            base.Awake();
            gameObject.SetActive(true);
        }

        private void Start() {
            gameObject.SetActive(false);
        }

        public void show(string detailText, Action onPositive = null, Action onNegative = null) {
            text.text = detailText;
            this.onPositive = onPositive;
            this.onNegative = onNegative;
            show();
        }

        public void confirm() {
            close();
            playClickSfx();
            
            if (onPositive != null) {
                onPositive();    
            }
        }

        public void cancel() {
            close();
            playClickSfx();
            
            if (onNegative != null) {
                onNegative();    
            }
        }
    }
}