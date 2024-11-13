using net.gubbi.goofy.extensions;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui {
    public class CanvasTextPosition : MonoBehaviour {
                
        public Vector2 Pos { set; private get; }
        private Text text;
        private CanvasPosition _canvasPosition;

        private void Awake() {
            text = GetComponent<Text>();
            _canvasPosition = GetComponent<CanvasPosition>();
        }

        private void Update() {
            if (!text.text.isNullOrEmpty()) {
                _canvasPosition.setCanvasPosFromWorldPos(Pos);
                text.enabled = true;
            }
            else {
                text.enabled = false;
            }
        }
    }
}