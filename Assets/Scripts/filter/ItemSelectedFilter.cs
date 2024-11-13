using net.gubbi.goofy.item;
using UnityEngine;

namespace net.gubbi.goofy.filter {

    [CreateAssetMenu(menuName = "Filters/Item selected")]
    public class ItemSelectedFilter : Filter {

        public ItemType item;
        public bool any;

        public override bool matches(FilterContext ctx) {
            return any && ctx.getProperty<ItemType>(FilterContext.SELECTED_ITEM) != ItemType.EMPTY || ctx.getProperty<ItemType>(FilterContext.SELECTED_ITEM) == item;
        }
    }
}