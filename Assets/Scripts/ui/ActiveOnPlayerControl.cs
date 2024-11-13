using net.gubbi.goofy.events;
using UnityEngine;

namespace net.gubbi.goofy.ui {
    public class ActiveOnPlayerControl : MonoBehaviour {
        
        protected void Awake() {            
            EventManager.Instance.addListener(delegate(GameEvents.PlayerControlEnabledEvent e) { setActive(true); });
            EventManager.Instance.addListener(delegate(GameEvents.PlayerControlDisabledEvent e) { setActive(false); });
        }

        private void setActive(bool active) {
            gameObject.SetActive(active);
        }
    }
}