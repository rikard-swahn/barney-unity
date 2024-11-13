using net.gubbi.goofy.character.npc.behaviour;
using net.gubbi.goofy.filter;
using net.gubbi.goofy.item;
using UnityEngine;

namespace net.gubbi.goofy.scene.item.action {

    public class StopCharacterBehaviourAction : SingleSceneItemAction {

        public GameObject character;
        private CharacterBehaviours behaviours;

        protected override void Awake () {
            base.Awake ();
            behaviours = character.GetComponentInChildren<CharacterBehaviours>();
        }
        
        public override FilterContext getFilterContext(ItemType selectedItem) {
            FilterContext ctx = base.getFilterContext(selectedItem);

            return ctx.mergeProperties(
                FilterContext.builder()
                    .property(FilterContext.CHARACTER, character)
                    .build()
            );
        }        

        protected override void doAction (ItemType selectedItem) {
            behaviours.stop();
            afterAction ();
        }


    }

}