using Mgl;
using net.gubbi.goofy.filter;
using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.door;
using net.gubbi.goofy.scene.load.transition;
using UnityEngine;

namespace net.gubbi.goofy.scene.item.action {

    public class PassDoorAction : SingleSceneItemAction {
        
        public Transitions.Type outTransition;
        public Transitions.Type inTransition;       
        
        private Door door;

        protected override void Awake () {
            base.Awake ();
            door = GetComponent<Door> ();

            ItemSelectedFilter nothingSelected = ScriptableObject.CreateInstance<ItemSelectedFilter>();
            nothingSelected.item = ItemType.EMPTY;
            conditions.Add(nothingSelected);                            
        }

        protected override void doAction(ItemType selectedItem) {            
            door.enter (false, Transitions.get(outTransition), Transitions.get(inTransition));
        }

        public override string getLabelPrefix() {
            return I18n.Instance.__("DoorPrefix");
        }
    }

}