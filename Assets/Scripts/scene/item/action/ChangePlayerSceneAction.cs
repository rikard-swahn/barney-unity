using System.Linq;
using net.gubbi.goofy.character;
using net.gubbi.goofy.filter;
using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.load;
using net.gubbi.goofy.scene.load.transition;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene.item.action {

    public class ChangePlayerSceneAction : SingleSceneItemAction {

        public string goToScene;
        public Vector2 entryPosition;
        public Vector2 entryDirection;
        public Transitions.Type outTransition;
        public Transitions.Type inTransition;

        private CharacterMove playerMove;

        protected override void Awake () {
            base.Awake ();
            playerMove = GameObject.Find(GameObjects.PLAYER).GetComponent<CharacterMove>();
            
            if (conditions.All(c => !c.contains<ItemSelectedFilter>())) {
                ItemSelectedFilter nothingSelected = ScriptableObject.CreateInstance<ItemSelectedFilter>();
                nothingSelected.item = ItemType.EMPTY;
                conditions.Add(nothingSelected);                
            }                        
        }     

        protected override void doAction (ItemType selectedItem) {
            if (!goToScene.Equals("")) {
                afterAction ();
            }
        }

        protected override void afterAction() {

            if (playerMove != null) {
                if (!Vector2.zero.Equals(entryPosition)) {
                    playerMove.setPositionState(new Vector2(entryPosition.x, entryPosition.y));
                }
                if (!Vector2.zero.Equals(entryDirection)) {
                    playerMove.setDirectionState(new Vector2(entryDirection.x, entryDirection.y));
                }
            }

            base.afterAction ();
            SceneLoader.Instance.changeInGameScene (goToScene, Transitions.get(outTransition), Transitions.get(inTransition));
        }
    }

}