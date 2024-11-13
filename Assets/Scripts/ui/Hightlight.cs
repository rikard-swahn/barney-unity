using net.gubbi.goofy.extensions;
using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public class Hightlight : MonoBehaviour {

        public Image highlight;
        public Color textNormalColor;
        public Color textHighlightColor;
        public bool highlighted;

        private Text[] texts;

        private void Awake() {
            texts = GetComponentsInChildren<Text>();
            setHighlighted(highlighted);
        }

        public void setHighlighted(bool highlighted) {
            highlight.enabled = highlighted;

            if (highlighted) {
                texts.ForEach(t => t.color = textHighlightColor);
            }
            else {
                texts.ForEach(t => t.color = textNormalColor);
            }
        }
    }
}