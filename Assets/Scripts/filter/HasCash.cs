using net.gubbi.goofy.item;
using net.gubbi.goofy.item.inventory;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.filter {

    [CreateAssetMenu(menuName = "Filters/Has cash")]
    public class HasCash : Filter {

        public int amount;
        
        private CardItem cardItem;
        private Inventory inventory;

        void OnEnable() {
            cardItem = new CardItem(amount);
        }

        public override bool matches(FilterContext ctx) {            
            if (inventory == null) {
                GameObject invGo = GameObject.Find(GameObjects.INVENTORY);
                inventory = invGo.GetComponent<Inventory>();
            }       
            
            return inventory.hasItem(cardItem);
        }

    }
}