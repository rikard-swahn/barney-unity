using net.gubbi.goofy.events;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.util;
using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public class TextButton : MonoBehaviour {
        public Font font;
        public FontStyle fontStyle = FontStyle.Normal;
        public bool changeFontOnHover;
        public string buttonTag;
        
        private Text text;
        private Font originalFont;                
        private FontStyle originalFontStyle;                
        private Button button;
        
        private void Awake() {
            text = GetComponentInChildren<Text>();
            button = GetComponent<Button>();
        }

        private void Start() {
            originalFont = text.font;
            originalFontStyle = text.fontStyle;
            EventManager.Instance.addListener<GameEvents.MouseEvent>(handleMouseEvent);
        }

        private void handleMouseEvent(GameEvents.MouseEvent e) {            
            if (gameObject.isMouseOverChildWithTag(buttonTag)) {
                if (e.buttonOneUp()) {                    
                    button.onClick.Invoke();
                    resetFont();
                }                
                else if (changeFontOnHover || MouseUtil.isLeftButtonDown()) {
                    changeFont();    
                }
            }
            else {
                resetFont();
            }
        }
        
        public void changeFont() {
            text.font = font;
            text.fontStyle = fontStyle;
        }

        public void resetFont() {
            text.font = originalFont;
            text.fontStyle = originalFontStyle;
        }        

    }
}