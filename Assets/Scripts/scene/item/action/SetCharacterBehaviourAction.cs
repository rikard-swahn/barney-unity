using net.gubbi.goofy.character.npc.behaviour;
using net.gubbi.goofy.item;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene.item.action {

    public class SetCharacterBehaviourAction : SingleSceneItemAction {

        public string characterKey;
        public BehaviourGroupType behaviourGroup;

        protected override void doAction (ItemType selectedItem) {
            GameObject character = GameObject.Find (characterKey);
            CharacterBehaviours behaviours = character != null ? character.GetComponentInChildren<CharacterBehaviours>() : null; 
            
            if (behaviours != null) {
                behaviours.start(behaviourGroup);
            }
            else {
                GameState.Instance.StateData.setCharacterBehaviourType(characterKey, behaviourGroup);
            }

            afterAction ();
        }


    }

}