using net.gubbi.goofy.item;
using net.gubbi.goofy.item.inventory;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.filter {

    [CreateAssetMenu(menuName = "Filters/Item flag")]
    public class ItemFlagFilter : Filter {

        public ItemType item;
        public string flag;
        public bool checkSet = true;
        
        private Inventory inventory;

        public override bool matches(FilterContext ctx) {
            if (inventory == null) {
                GameObject invGo = GameObject.Find(GameObjects.INVENTORY);
                inventory = invGo.GetComponent<Inventory>();
            }
                                    
            return inventory.hasItemWithFlag(this.item, flag) == checkSet;
        }
    }
}