using System.Collections.Generic;
using net.gubbi.goofy.filter;
using net.gubbi.goofy.item;
using net.gubbi.goofy.item.inventory;
using net.gubbi.goofy.scene.item.action;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene.action {
    public class InstantSceneActionTrigger : MonoBehaviour {

        public string triggerForTag;
        public List<Filter> conditions;

        private Inventory inventory;
        private SingleSceneItemAction action;

        private void Awake () {			
            inventory = GameObject.Find(GameObjects.INVENTORY).GetComponent<Inventory>();
            action = GetComponent<SingleSceneItemAction>();
        }

        void OnTriggerEnter2D(Collider2D other) {
            ItemType selectedItem = inventory.SelectedItem;

            FilterContext ctx = getFilterContext(selectedItem);
            if (!conditionsMatches(ctx)) {
                return;
            }

            
            if (other.CompareTag (triggerForTag) && action.filter(selectedItem)) {
                action.start(selectedItem, delegate {});
            }
        }

        private bool conditionsMatches(FilterContext ctx) {
            foreach (var condition in conditions) {
                if (!condition.matches(ctx)) {
                    return false;
                }
            }

            return true;
        }

        private FilterContext getFilterContext(ItemType selectedItem) {
            return FilterContext
                .builder()
                .property(FilterContext.SELECTED_ITEM, selectedItem)
                .build();
        }
    }
}