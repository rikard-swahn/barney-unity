using UnityEngine;

namespace net.gubbi.goofy.ui.menu.ingame {
    public class TextStyle : MonoBehaviour {

        public Color mainColor;
        public Color outlineColor;

        public Color getMainColor() {
            return mainColor;
        }

        public Color getOutlineColor() {
            return outlineColor;
        }

    }
}