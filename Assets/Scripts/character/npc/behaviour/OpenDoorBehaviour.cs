using System;
using net.gubbi.goofy.player;
using net.gubbi.goofy.scene.door;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class OpenDoorBehaviour : CharacterBehaviour {

        public GameObject doorGameObj;
        public bool actionAnimation;
        
        private Door door;
        private TimedAnimatorFlag timedAnimatorFlag;

        private new void Awake() {
            base.Awake ();
            door = doorGameObj.GetComponent<Door> ();
            timedAnimatorFlag = GetComponentInParent<TimedAnimatorFlag>();
            skipOnResume = false;
        }

        protected override void doBehaviour (Action onCompleteBehaviour) {            
            if (actionAnimation && !door.isOpen()) {
                Action afterAnim = delegate {
                    door.setOpen(true);
                    onCompleteBehaviour();
                };  
                timedAnimatorFlag.setFlag (afterAnim, AnimationParams.DO_ACTION, Constants.DEFAULT_ACTION_TIME);
            }
            else {
                door.setOpen(true);
                onCompleteBehaviour ();    
            }
        }
               
        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            door.setOpenState(true);
            return true;
        }

    }
}