using net.gubbi.goofy.util;

namespace net.gubbi.goofy.character.npc.behaviour.state {
    public class FrozenStoppedState : BehaviourRunState {
        private CharacterBehaviours behaviours;

        public FrozenStoppedState(CharacterBehaviours behaviours) {
            this.behaviours = behaviours;
        }

        public BehaviourRunStateType getStateType () {
            return BehaviourRunStateType.FROZEN_STOPPED;
        }

        public void start (BehaviourGroupType behaviourGroup) {
            behaviours.addEventLog("Frozen Stopped  " + behaviours.getCurrentGroupType() + "-> Frozen Started " + behaviourGroup);
            behaviours.RunState = behaviours.getFrozenStartedState ();
            behaviours.setNewBehaviour(behaviourGroup);
        }

        public void stop () {
            behaviours.addEventLog("Frozen Stopped " + behaviours.getCurrentGroupType() + " -> Stopped (via stop)");
            behaviours.stopBehaviour ();
            behaviours.RunState = behaviours.getStoppedState();
        }

        public void freeze () {
            //Do nothing
        }

        public void unFreeze () {
            behaviours.addEventLog("Frozen Stopped " + behaviours.getCurrentGroupType() + " -> Stopped (via unFreeze)");
            behaviours.RunState = this.behaviours.getStoppedState();
            behaviours.resume ();
        }

        public void resume() {
            //Do nothing
        }

        public void freezeUI () {
            //Do nothing
        }

    }
}