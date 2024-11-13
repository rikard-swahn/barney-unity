using System.Linq;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.ui.localization {
    public class LocaleActive : MonoBehaviour {

        public string[] locales;
        
        private void Start() {
            var locale = I18nManager.GetLanguage();
            gameObject.SetActive(locales.Contains(locale));            
        }        
        
    }
}