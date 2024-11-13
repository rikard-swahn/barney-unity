using net.gubbi.goofy.item;
using net.gubbi.goofy.item.inventory;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.filter {

    [CreateAssetMenu(menuName = "Filters/Has items")]
    public class HasItem : Filter {

        public ItemType[] items;
        
        private Inventory inventory;

        public override bool matches(FilterContext ctx) {
            if (inventory == null) {
                GameObject invGo = GameObject.Find(GameObjects.INVENTORY);
                inventory = invGo.GetComponent<Inventory>();
            }

            foreach (var item in items) {
                if (!inventory.hasItem(item)) {
                    return false;
                }
            }

            return true;
        }

    }
}