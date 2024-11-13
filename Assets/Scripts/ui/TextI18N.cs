using System.Collections.Generic;
using Mgl;
using net.gubbi.goofy.events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui.menu.ingame {
    public class TextI18N : MonoBehaviour {

        private Dictionary<Text, string> i18nKeys = new Dictionary<Text,string>();
        private Dictionary<TextMeshProUGUI, string> i18nKeysTmp = new Dictionary<TextMeshProUGUI,string>();

        private void Awake() {
            EventManager.Instance.addListener<GameEvents.LocaleChangedEvent>(localeChangedHandler);
        }

        private void Start() {
            SetUITexts();
        }

        private void localeChangedHandler(GameEvents.LocaleChangedEvent e) {
            SetUITexts();
        }

        private void SetUITexts() {
            Text[] texts = GetComponentsInChildren<Text>(true);
            foreach (Text text in texts) {
                var i18nKey = i18nKeys.PutIfAbsent(text, text.text);
                text.text = I18n.Instance.__(i18nKey);
            }
            
            TextMeshProUGUI[] tmpTexts = GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (TextMeshProUGUI text in tmpTexts) {
                var i18nKey = i18nKeysTmp.PutIfAbsent(text, text.text);
                text.text = I18n.Instance.__(i18nKey);
            }
        }
    }
}