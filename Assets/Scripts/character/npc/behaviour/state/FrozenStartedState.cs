using System;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.util;
using UnityEngine.SceneManagement;

namespace net.gubbi.goofy.character.npc.behaviour.state {
    public class FrozenStartedState : BehaviourRunState {
        private CharacterBehaviours behaviours;

        public FrozenStartedState(CharacterBehaviours behaviours) {
            this.behaviours = behaviours;
        }

        public BehaviourRunStateType getStateType () {
            return BehaviourRunStateType.FROZEN_STARTED;
        }

        public void start (BehaviourGroupType behaviourGroup) {
            throw new InvalidOperationException ("start() called in Frozen Started state. Scene: " 
                                                 + SceneManager.GetActiveScene ().name + "  new group: " + behaviourGroup
                                                 + ", Time: " + DateTime.UtcNow.ToStringStandard() + ", " + behaviours.ToString());
        }

        public void stop () {
            behaviours.addEventLog("Frozen Started " + behaviours.getCurrentGroupType() + " -> Stopped");
            behaviours.stopBehaviour ();
            behaviours.RunState = behaviours.getStoppedState();
        }

        public void freeze () {
            //Do nothing
        }

        public void unFreeze () {
            behaviours.addEventLog("Frozen Started " + behaviours.getCurrentGroupType() + "-> Started");
            behaviours.RunState = behaviours.getStartedState ();
            behaviours.setSelectedNewGroup();
            behaviours.resume ();
        }

        public void resume() {
            behaviours.resumeBehaviourBackground();
        }

        public void freezeUI () {
            //Do nothing
        }

    }
}