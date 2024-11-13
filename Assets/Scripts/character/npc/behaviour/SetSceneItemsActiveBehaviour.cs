using System;
using System.Collections.Generic;
using net.gubbi.goofy.scene.item;
using net.gubbi.goofy.state;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class SetSceneItemsActiveBehaviour : CharacterBehaviour {

        public List<string> items;
        public bool active;
        public bool stateOnly;

        protected override void doBehaviour (Action onCompleteBehaviour) {
            setItemActive (items, active);
            onCompleteBehaviour ();
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            setItemActive (items, active);
            return true;
        }

        private void setItemActive(List<string> items, bool active) {
            
            foreach(string itemKey in items) {
                if (stateOnly) {
                    GameState.Instance.StateData.setSceneProperty (itemKey, SceneItemProperties.ACTIVE, active);
                }
                else {
                    SceneItemUtil.setItemActive(itemKey, active);
                }
            }			
        }

    }
}