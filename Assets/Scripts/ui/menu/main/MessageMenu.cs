using System;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui.menu.main {
    public class MessageMenu : Menu {
        
        public Text text;

        private Action onOk;
        
        public void show(string detailText, Action onOk = null) {
            text.text = detailText;
            this.onOk = onOk;
            show();
        }

        public void ok() {
            if (onOk != null) {
                onOk();    
            }
            
            hide();
            playClickSfx();
        }
    }
}