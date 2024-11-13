using net.gubbi.goofy.events;
using UnityEngine;

public class GuiEventHandler : MonoBehaviour {

    private void OnGUI() {
        Event e = Event.current;

        if (e.type == EventType.Repaint) {
            EventManager.Instance.raise (new GameEvents.RepaintEvent());
        }

        if (e.isMouse || e.type == EventType.Repaint) {
            Vector3 mousePos = Input.mousePosition;
            EventManager.Instance.raise(new GameEvents.MouseEvent(mousePos, e));
            EventManager.Instance.raise(new GameEvents.LateMouseEvent(mousePos, e));
        }
    }

    private void Update() {
        if(Input.GetKeyUp(KeyCode.Escape)) {
            GameEvents.KeyUpEvent keyEvent = new GameEvents.KeyUpEvent(KeyCode.Escape);
            EventManager.Instance.raise(keyEvent);

            if (!keyEvent.Consumed) {
                EventManager.Instance.raise(new GameEvents.LateKeyUpEvent(KeyCode.Escape));
            }
        }
    }
}