using System;
using System.Linq;
using net.gubbi.goofy.util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui {
    public class LocalizedFontSize : MonoBehaviour {

        public LocalizedFontSizeEntry[] sizes;

        private void Start() {
            var locale = I18nManager.GetLanguage();
            
            var size = sizes.FirstOrDefault(p => p.locale.Equals(locale));
            if (size != null) {
                var text = GetComponent<Text>();
                if (text != null) {
                    text.fontSize = Mathf.RoundToInt(size.size);    
                }
                
                var tmpText = GetComponent<TextMeshProUGUI>();
                if (tmpText != null) {
                    tmpText.fontSize = size.size;    
                }
            }
        }
    }

    [Serializable]
    public class LocalizedFontSizeEntry {
        public float size;
        public string locale;
    }
}