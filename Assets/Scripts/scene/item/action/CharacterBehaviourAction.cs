using net.gubbi.goofy.character;
using net.gubbi.goofy.character.npc.behaviour;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.filter;
using net.gubbi.goofy.item;
using net.gubbi.goofy.player;
using net.gubbi.goofy.util;
using UnityEngine;

//TODO: This behaviour can be removed and instead have a ActionGroup where player does action animation and then starts a SetCharacterBehaviourAction?
namespace net.gubbi.goofy.scene.item.action.use {

    public class CharacterBehaviourAction : SingleSceneItemAction {

        public BehaviourGroup behaviourGroup;
        public bool addToScene;
        public bool turnTowardsBaseLine;

        private CharacterBehaviours behaviours;
        private CharacterSceneHandler sceneHandler;
        private CharacterFacade playerFacade;        
        private TimedAnimatorFlag timedAnimatorFlag;
        private Vector3 baseLine;
        private GameObject character;

        protected override void Awake () {
            base.Awake ();

            character = behaviourGroup.gameObject.getAncestorWithTag(Tags.CHARACTER);
            behaviours = character.GetComponentInChildren<CharacterBehaviours>();
            sceneHandler = character.GetComponentInChildren<CharacterSceneHandler> ();

            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            playerFacade = playerGo.GetComponent<CharacterFacade>();
            timedAnimatorFlag = playerGo.GetComponent<TimedAnimatorFlag>();
            baseLine = target.transform.Find(GameObjects.ORDERED_RENDERING + "/" + GameObjects.BASE_LINE).position;
        }

        public override FilterContext getFilterContext(ItemType selectedItem) {
            FilterContext ctx = base.getFilterContext(selectedItem);

            return ctx.mergeProperties(
                FilterContext.builder()
                    .property(FilterContext.CHARACTER, character)
                    .build()
            );
        }

        public override bool conditionsMatches(FilterContext ctx) {
            if (!sceneHandler.isCharacterInCurrentScene () && !addToScene) {
                return false;
            }

            return base.conditionsMatches(ctx);
        }

        protected override void doAction (ItemType selectedItem) {
            if (turnTowardsBaseLine) {
                playerFacade.turnTowards(baseLine);
            }

            timedAnimatorFlag.setFlag (afterAction, AnimationParams.DO_ACTION, Constants.DEFAULT_ACTION_TIME);
        }
        
        protected override void afterAction() {
            if (!sceneHandler.isCharacterInCurrentScene () && addToScene) {
                sceneHandler.setCharacterSceneToCurrent ();
            }
            behaviours.start(behaviourGroup.behaviourGroupType);

            base.afterAction();
        }

    }

}