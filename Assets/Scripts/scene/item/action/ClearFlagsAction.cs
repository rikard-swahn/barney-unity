﻿using net.gubbi.goofy.item;
 using net.gubbi.goofy.state;

namespace net.gubbi.goofy.scene.item.action {

    public class ClearFlagsAction : SingleSceneItemAction {
        public string[] flags;

        protected override void doAction (ItemType selectedItem) {
            GameState.Instance.StateData.removeFlags (flags);
            afterAction();
        }
    }

}