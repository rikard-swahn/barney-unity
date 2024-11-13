using net.gubbi.goofy.filter;
using net.gubbi.goofy.item;

namespace net.gubbi.goofy.scene.item.action {

    public class ItemConditionsLookAction : LookAction {

        public SceneItem conditionsSceneItem;

        public override FilterContext getFilterContext(ItemType selectedItem) {
            FilterContext ctx = base.getFilterContext(selectedItem);

            return ctx.mergeProperties(
                FilterContext.builder()
                    .property(FilterContext.SCENE_ITEM, conditionsSceneItem)
                    .build()
            );
        }
    }

}