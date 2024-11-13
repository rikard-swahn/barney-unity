using net.gubbi.goofy.item;
using UnityEngine;

namespace net.gubbi.goofy.scene.action {
    public abstract class SceneActionHandler : MonoBehaviour {

        public abstract void doAction (ItemType selectedItem);
        public abstract string getLabelPrefix (ItemType selectedItem);        
    }
}