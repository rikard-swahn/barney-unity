using net.gubbi.goofy.character;
using net.gubbi.goofy.character.npc.behaviour;
using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.item.action;
using net.gubbi.goofy.util;
using UnityEngine;

namespace Assets.Scripts.scene.item.action {
    public class CharacterSayAction : SingleSceneItemAction {

        public string say;
        public bool turnPlayer;
        public bool turnPlayerToCharacter;
        public Vector2 direction;
        public CharacterFacade characterFacade;

        private CharacterFacade playerFacade;
        private CharacterBehaviours behaviours;

        private new void Awake() {
            base.Awake ();

            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            playerFacade = playerGo.GetComponent<CharacterFacade>();
            
            behaviours = characterFacade.gameObject.GetComponentInChildren<CharacterBehaviours> ();
        }

        protected override void doAction(ItemType selectedItem) {
            if (behaviours != null) {
                behaviours.freeze ();
            }                        
            
            if (turnPlayer) {
                playerFacade.stopAndTurnTowards (gameObject.transform.position);
            }
            else if (turnPlayerToCharacter) {
                playerFacade.stopAndTurnTowards (characterFacade.getPosition ());                
            }

            setDirection();
            
            characterFacade.sayText (
                say,
                afterAction
            );
        }

        protected override void afterAction() {
            if (behaviours != null) {
                behaviours.unFreeze();
            }                 
            
            base.afterAction();            
        }

        private void setDirection() {
            if (!Vector2.zero.Equals(direction)) {
                characterFacade.setDirectionImmediate(direction);
            }
        }

    }
}