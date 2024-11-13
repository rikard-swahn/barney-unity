using net.gubbi.goofy.util;

namespace net.gubbi.goofy.character.npc.behaviour.state {
    public interface BehaviourRunState {
        BehaviourRunStateType getStateType ();

        void start (BehaviourGroupType behaviourGroup);
        void stop ();
        void freeze();
        void unFreeze();
        void resume();
        void freezeUI();
    }

    public enum BehaviourRunStateType {
        STARTED, STOPPED, FROZEN_STARTED, FROZEN_STOPPED
    }
}