using System;
using net.gubbi.goofy.character;
using net.gubbi.goofy.item;
using UnityEngine;

namespace net.gubbi.goofy.scene.item.action {

    public class GoToAction : SingleSceneItemAction {

        public Waypoint waypoint;
        public CharacterMove characterMove;
        public bool backwards;
        public float speedFactor = 1f;
        
        private Vector2 goTo;
        private float speedBefore;
        private bool backwardsBefore;

        protected override void Awake () {
            base.Awake ();
            goTo = waypoint.gameObject.transform.position;
        }

        protected override void doAction (ItemType selectedItem) {
            speedBefore = characterMove.speedFactor;       
            backwardsBefore = characterMove.walkBackwards;       
            
            characterMove.walkBackwards = backwards;
            if (!float.IsNaN(speedFactor) && speedFactor != 0f) {
                characterMove.speedFactor = speedFactor;
            }

            Action<bool> onCompleteWrapper = delegate { afterAction(); };
            characterMove.SetDestination (goTo, onCompleteWrapper);
        }

        protected override void afterAction() {
            characterMove.walkBackwards = backwardsBefore;
            characterMove.speedFactor = speedBefore;

            base.afterAction();
        }
    }

}