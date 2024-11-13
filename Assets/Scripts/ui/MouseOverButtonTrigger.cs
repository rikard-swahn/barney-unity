using net.gubbi.goofy.events;
using net.gubbi.goofy.util;
using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public class MouseOverButtonTrigger : MonoBehaviour {
        
        private Button button;
        
        private void Awake() {                          
            EventManager.Instance.addListener<GameEvents.MouseEvent>(handleMouseEvent);
            button = GetComponent<Button>();
        }

        private void handleMouseEvent(GameEvents.MouseEvent e) {            
            if (isMouseOverThis()) {
                button.onClick.Invoke();
            }
        }
        
        private bool isMouseOverThis() {
            return MouseUtil.isHovering() && RaycastUtil.childWithTagHitByMouseRaycast(Tags.MENU_AREA, gameObject);
        }

    }
}