using System.Collections.Generic;
using Mgl;
using net.gubbi.goofy.events;
using net.gubbi.goofy.util;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui.menu.main {
    public class LanguageMenu : Menu {

        public Menu settingsMenu;
        public Text languageText;

        private List<string> locales = new List<string>{"en", "es", "pt", "jp"};

        private void Start() {
            updateLanguageText();
        }         
        
        public void toggleLanguage() {
            var locale = I18nManager.GetLanguage();
            var newLocaleIndex = (locales.IndexOf(locale) + 1) % locales.Count;
            var newLocale = locales[newLocaleIndex];
            I18nManager.SetLanguageSetting(newLocale);
            EventManager.Instance.raise(new GameEvents.LocaleChangedEvent(newLocale));
            updateLanguageText();
            playClickSfx();
        }

        public void back() {            
            hide();
            settingsMenu.showInstantly();
            playClickSfx();
        }        

        private void updateLanguageText() {
            languageText.text = I18n.Instance.__("currentLanguage");
        }

    }
}