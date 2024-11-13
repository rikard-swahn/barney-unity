using net.gubbi.goofy.ui.goals;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.item.inventory.action {

    public class GoalsAction : InventoryAction {
        
        private GoalsLightbox goalsLightbox;
        
        protected override void Awake() {
            base.Awake();
            item1 = ItemType.TO_DO_LIST;
            item2 = ItemType.EMPTY;
            goalsLightbox = GameObject.Find (GameObjects.GOALS_LIGHTBOX).GetComponent<GoalsLightbox>();
        }

        protected override void mainAction () {
            goalsLightbox.show();
        }

    }

}