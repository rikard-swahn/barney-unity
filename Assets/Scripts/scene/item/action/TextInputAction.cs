using net.gubbi.goofy.filter;
using net.gubbi.goofy.item;
using UnityEngine.UI;

namespace net.gubbi.goofy.scene.item.action {
    public class TextInputAction : SingleSceneItemAction {

        public InputField inputField;
        
        protected override void doAction (ItemType selectedItem) {
            afterAction();
        }        
        
        public override FilterContext getFilterContext(ItemType selectedItem) {
            FilterContext ctx = base.getFilterContext(selectedItem);

            return ctx.mergeProperties(
                FilterContext.builder()
                    .property(FilterContext.STRING, inputField.text)
                    .build()
            );
        }        
        
    }
}