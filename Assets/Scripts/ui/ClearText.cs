using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui.menu.ingame {

    public class ClearText : MonoBehaviour {
        private void Awake () {
            Text text = GetComponentInParent<Text>();
            text.text = "";
        }
    }

}