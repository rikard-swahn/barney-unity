using net.gubbi.goofy.item;

namespace net.gubbi.goofy.scene.item.action {
    public class ClearTextInputAction : TextInputAction {

        protected override void doAction (ItemType selectedItem) {
            inputField.text = "";
            afterAction();
        }
        
    }
}