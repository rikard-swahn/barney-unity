using System;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui.menu.main {
    public class ConfirmMenu : Menu {
        
        public Text text;

        private Action onPositive;
        private Action onNegative;
        
        public void show(string detailText, Action onPositive = null, Action onNegative = null) {
            text.text = detailText;
            this.onPositive = onPositive;
            this.onNegative = onNegative;
            show();
        }

        public void confirm() {
            if (onPositive != null) {
                onPositive();    
            }
            
            hide();
            playClickSfx();
        }

        public void cancel() {
            if (onNegative != null) {
                onNegative();    
            }
            
            hide();
            playClickSfx();
        }
    }
}