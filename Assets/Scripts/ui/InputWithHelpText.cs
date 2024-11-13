using net.gubbi.goofy.extensions;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui {
    public class InputWithHelpText : MonoBehaviour {

        public Text helpText;
        public InputField inputField;

        private void Update() {
            helpText.enabled = !inputField.isFocused && inputField.text.isNullOrEmpty();
        }
    }
}