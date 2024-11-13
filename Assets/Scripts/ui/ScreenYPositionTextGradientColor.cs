using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public class ScreenYPositionTextGradientColor : MonoBehaviour {

        public Color startColor;
        public Color endColor;
        private Text text;

        private void Awake() {
            text = GetComponent<Text>();
        }

        private void Update() {
            float yNormalized = transform.position.y / Screen.height;
            yNormalized = Mathf.Clamp01(yNormalized);

            Color gradientColor = (1 - yNormalized) * startColor + yNormalized * endColor;
            text.color = gradientColor;
        }
    }
}