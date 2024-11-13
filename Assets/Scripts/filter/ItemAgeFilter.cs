using net.gubbi.goofy.item;
using net.gubbi.goofy.item.inventory;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.filter {

    [CreateAssetMenu(menuName = "Filters/Item selected age greater than")]
    public class ItemAgeFilter : Filter {

        public float age;
        
        private Inventory inventory;

        public override bool matches(FilterContext ctx) {
            if (inventory == null) {
                GameObject invGo = GameObject.Find(GameObjects.INVENTORY);
                inventory = invGo.GetComponent<Inventory>();
            }            
            
            ItemType type = ctx.getProperty<ItemType>(FilterContext.SELECTED_ITEM);

            Item item = inventory.getItem(type);
            float? itemAge = item.getAge();

            return itemAge != null && itemAge > age;
        }
    }
}