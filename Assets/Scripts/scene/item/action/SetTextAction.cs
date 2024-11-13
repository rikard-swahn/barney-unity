using net.gubbi.goofy.item;
using UnityEngine.UI;

namespace net.gubbi.goofy.scene.item.action {
    public class SetTextAction : SingleSceneItemAction {

        public Text textField;
        public string value;

        protected override void doAction (ItemType selectedItem) {
            textField.text = value;
            afterAction();
        }

    }
}