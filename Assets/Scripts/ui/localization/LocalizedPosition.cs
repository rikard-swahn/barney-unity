using System;
using System.Linq;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.ui {
    public class LocalizedPosition : MonoBehaviour {

        public LocalizedPositionEntry[] positions;

        private void Start() {
            var locale = I18nManager.GetLanguage();
            var pos = positions.First(p => p.locale.Equals(locale));

            var rectTransform = GetComponent<RectTransform>();
            rectTransform.anchoredPosition = pos.position;
        }
    }

    [Serializable]
    public class LocalizedPositionEntry {
        public Vector2 position;
        public string locale;
    }
}