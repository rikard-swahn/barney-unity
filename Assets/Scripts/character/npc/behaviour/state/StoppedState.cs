using System;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.util;
using UnityEngine.SceneManagement;

namespace net.gubbi.goofy.character.npc.behaviour.state {
    public class StoppedState : BehaviourRunState {
        private CharacterBehaviours behaviours;

        public StoppedState(CharacterBehaviours behaviours) {
            this.behaviours = behaviours;
        }

        public BehaviourRunStateType getStateType () {
            return BehaviourRunStateType.STOPPED;
        }

        public void start (BehaviourGroupType behaviourGroup) {
            behaviours.addEventLog("Stopped  " + behaviours.getCurrentGroupType() + "-> Started " + behaviourGroup);
            behaviours.RunState = behaviours.getStartedState();
            behaviours.startBehaviour (behaviourGroup);
        }

        public void stop () {
            //Do nothing
        }

        public void freeze () {
            behaviours.addEventLog("Stopped -> Frozen Stopped , Behaviour group: " + behaviours.getCurrentGroupType());
            behaviours.RunState = behaviours.getFrozenStoppedState();
        }

        public void unFreeze () {
            throw new InvalidOperationException ("unFreeze() called in Stopped state. Scene: " 
                + SceneManager.GetActiveScene ().name + ", Time: " + DateTime.UtcNow.ToStringStandard() + ", " + behaviours.ToString());
        }

        public void resume() {
            //Do nothing
        }

        public void freezeUI () {
            //Do nothing
        }

    }
}