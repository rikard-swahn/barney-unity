using net.gubbi.goofy.player;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene.scenes.cruiseship {
    public class TheEndStop : MonoBehaviour {

        public string TheEndTag;
        
        private SceneInputHandler sceneInput;
        private static readonly string CONTROL_BY_ID = "TheEndButton";

        private void Awake() {
            sceneInput = GameObject.Find(GameObjects.PLAYER).GetComponent<SceneInputHandler>();
            sceneInput.setControlEnabled (false, CONTROL_BY_ID);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag(TheEndTag)) {                
                other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                sceneInput.setControlEnabled (true, CONTROL_BY_ID);
            }            
        }
    }
}