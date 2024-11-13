using net.gubbi.goofy.events;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui.menu.ingame {

    public class EnableTextOnPlayerControl : MonoBehaviour {
        public bool clearTextOnDisabled;
        private Text text;

        private void Awake () {
            this.text = GetComponent<Text>();
            EventManager.Instance.addListener<GameEvents.PlayerControlEnabledEvent>(playerControlEnabledHandler);
            EventManager.Instance.addListener<GameEvents.PlayerControlDisabledEvent>(playerControlDisabledHandler);
        }

        private void playerControlEnabledHandler(GameEvents.PlayerControlEnabledEvent e) {
            text.enabled = true;            
        }

        private void playerControlDisabledHandler(GameEvents.PlayerControlDisabledEvent e) {
            text.enabled = false;
            if (clearTextOnDisabled) {
                text.text = "";
            }
        }

    }

}