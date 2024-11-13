using System;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.util;
using UnityEngine.SceneManagement;

namespace net.gubbi.goofy.character.npc.behaviour.state {
    public class StartedState : BehaviourRunState {
        private CharacterBehaviours behaviours;

        public StartedState(CharacterBehaviours behaviours) {
            this.behaviours = behaviours;
        }

        public BehaviourRunStateType getStateType () {
            return BehaviourRunStateType.STARTED;
        }

        public void start (BehaviourGroupType behaviourGroup) {
            behaviours.addEventLog("Started " + behaviours.getCurrentGroupType() + " -> Started " + behaviourGroup);
            behaviours.startBehaviour (behaviourGroup);
        }

        public void stop () {
            behaviours.addEventLog("Started " + behaviours.getCurrentGroupType() + " -> Stopped");
            behaviours.stopBehaviour ();
            behaviours.RunState = behaviours.getStoppedState();
        }

        public void freeze () {
            behaviours.addEventLog("Started " + behaviours.getCurrentGroupType() + " -> Frozen Started");
            behaviours.stopBehaviour ();
            behaviours.RunState = behaviours.getFrozenStartedState ();
        }

        public void unFreeze () {
            throw new InvalidOperationException ("unFreeze() called in Started state. Scene: " 
                                                 + SceneManager.GetActiveScene ().name + ", Time: " + DateTime.UtcNow.ToStringStandard() + ", " + behaviours.ToString());
        }

        public void resume () {
            behaviours.addEventLog("resume() called in Started State, Behaviour group: " + behaviours.getCurrentGroupType());
            behaviours.resumeBehaviour ();
        }

        public void freezeUI () {
            behaviours.addEventLog("freezeUI() called in Started state, Behaviour group: " + behaviours.getCurrentGroupType());
            behaviours.freezeBehaviourUI ();
        }
			
    }
}