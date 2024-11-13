using System;
using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.item.action;
using UnityEngine;

namespace net.gubbi.goofy.scene.action {
    public class TriggerInstantSceneActionHandler : SceneActionHandler  {

        private SingleSceneItemAction action;

        private void Awake() {
            action = GetComponent<SingleSceneItemAction>();
        }

        public override void doAction (ItemType selectedItem) {
            if (action.filter(selectedItem)) {
                action.start(selectedItem, delegate {});
            }
        }

        public override string getLabelPrefix(ItemType selectedItem) {
            throw new NotImplementedException();
        }
    }
}