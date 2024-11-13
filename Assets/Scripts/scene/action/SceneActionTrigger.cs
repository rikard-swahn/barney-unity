using System.Collections.Generic;
using net.gubbi.goofy.filter;
using net.gubbi.goofy.item;
using net.gubbi.goofy.item.inventory;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene.action {
    public class SceneActionTrigger : MonoBehaviour {

        public string triggerForTag;
        public List<Filter> conditions;

        private Inventory inventory;
        private SceneActionHandler actionHandler;

        private void Awake () {			
            actionHandler = GetComponent<SceneActionHandler> ();
            inventory = GameObject.Find(GameObjects.INVENTORY).GetComponent<Inventory>();
        }

        void OnTriggerEnter2D(Collider2D other) {
            ItemType selectedItem = inventory.SelectedItem;

            FilterContext ctx = getFilterContext(selectedItem);
            if (!conditionsMatches(ctx)) {
                return;
            }

            if (!other.CompareTag(triggerForTag)) {
                return;
            }

            string currentTarget = GameState.Instance.StateData.PlayerState.SceneActionTargetName;
            
            if (GameState.Instance.StateData.PlayerState.ItemActionIndex != null) {
                if (currentTarget.Equals(gameObject.name)) {
                    Debug.Log("Not starting action on gameobject " + gameObject.name + ". Action already running.");
                }
                else {
                    Debug.LogError("Not starting action on gameobject " + gameObject.name + ". Action already running on gameObject " + GameState.Instance.StateData.PlayerState.SceneActionTargetName);
                }
                return;
            }
            
            //If tag matches and the action to trigger is not already running
            if (!gameObject.name.Equals(GameState.Instance.StateData.PlayerState.SceneActionTargetName)) {
                actionHandler.doAction (selectedItem);
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