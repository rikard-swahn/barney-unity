using System.Collections.Generic;
using UnityEngine;

namespace net.gubbi.goofy.item.inventory.action {
	
    public class Actions : MonoBehaviour {

        private IDictionary<ItemTypeCombo, InventoryAction> actionsDictionary = new Dictionary<ItemTypeCombo, InventoryAction>();

        private void Start() {
            InventoryAction[] actions = GetComponents<InventoryAction> ();
            foreach(InventoryAction action in actions) {
                actionsDictionary.Add (new ItemTypeCombo (action.item1, action.item2), action);
            }
        }

        public InventoryAction getAction(ItemType item1, ItemType item2) {
            ItemTypeCombo combo = new ItemTypeCombo (item1, item2);
            return actionsDictionary.ContainsKey (combo) ? actionsDictionary [combo] : null;
        }

    }


}