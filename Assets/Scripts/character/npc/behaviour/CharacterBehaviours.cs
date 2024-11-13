using System;
using System.Collections.Generic;
using net.gubbi.goofy.character.npc.behaviour.state;
using net.gubbi.goofy.events;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using unity;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class CharacterBehaviours : InitiableMonoBehaviour {
        public BehaviourGroup[] behaviourGroups;
        public BehaviourRunStateType defaultState = BehaviourRunStateType.STARTED;
        public string CharacterKey {get; private set;}

        private BehaviourRunState startedState;
        private BehaviourRunState stoppedState;
        private BehaviourRunState frozenStartedState;
        private BehaviourRunState frozenStoppedState;
        private BehaviourRunState[] states;

        private BehaviourRunState runState;
        private BehaviourGroupType currentGroup = BehaviourGroupType.EMPTY;
        private Dictionary<BehaviourGroupType, BehaviourGroup> behaviourGroupMap;
        private CharacterMove characterMove;
        private CharacterSceneHandler characterSceneHandler;

        private List<string> eventLog = new List<string>();
        private static readonly int MAX_EVENTS = 100;

        public CharacterBehaviours () {
            startedState = new StartedState(this);
            stoppedState = new StoppedState (this);
            frozenStartedState = new FrozenStartedState (this);
            frozenStoppedState = new FrozenStoppedState (this);
            states = new[]{startedState, stoppedState, frozenStartedState, frozenStoppedState};
        }		

        private void Awake() {
            characterMove = GetComponentInParent<CharacterMove>();
            CharacterKey = gameObject.getAncestorWithTag(Tags.CHARACTER).name;
            characterSceneHandler = GetComponentInParent<CharacterSceneHandler>();
            behaviourGroupMap = getBehaviourGroupMap ();
            RunState = getRunStateStartValue();          
            addEventLog("Behaviour run state at start: " + runState.GetType().Name);
        }

        private void OnEnable() {
            EventManager.Instance.addListener<GameEvents.ConversationStartEvent>(conversationStartHandler);
            EventManager.Instance.addListener<GameEvents.ConversationEndEvent>(conversationEndHandler);            
        }

        private void OnDisable() {
            EventManager.Instance.removeListener<GameEvents.ConversationStartEvent>(conversationStartHandler);
            EventManager.Instance.removeListener<GameEvents.ConversationEndEvent>(conversationEndHandler);    
        }

        public override void init() {                        
            setStartGroup();
            
            if (hasBehaviours()) {				
                EventManager.Instance.addListener<GameEvents.SceneChangeInCurrentGameEvent>(sceneChangeInGameHandler);

                if(currentGroup != BehaviourGroupType.EMPTY) {
                    behaviourGroupMap [currentGroup].init ();
                    resume ();
                }
            }

            if (currentGroup == BehaviourGroupType.EMPTY && !runState.Equals(stoppedState))  {
                addEventLog("EMPTY behaviour at start. Changing " + runState.getStateType() + " -> Stopped");
                RunState = stoppedState;
            }
            
        }

        //State action delegation ----------
        //Starts a new behaviour group
        public void start(BehaviourGroupType groupType) {
            runState.start (groupType);
        }

        public void stop () {
            runState.stop ();
        }

        public void freeze () {
            runState.freeze ();
        }

        public void unFreeze () {
            runState.unFreeze ();
        }

        private void freezeUI() {
            runState.freezeUI ();
        }
        //----------------------------------

        //Resumes current state. For internal use in state machine, do not call from outside.
        public void resume() {
            runState.resume ();
        }

        //For internal use in state machine, do not call from outside.
        public void stopBehaviour () {
            if (currentGroup != BehaviourGroupType.EMPTY) {
                behaviourGroupMap[currentGroup].stop();
            }
        }

        //For internal use in state machine, do not call from outside.
        public void startBehaviour(BehaviourGroupType newGroupType) {
            if (isStarted()) {
                stopBehaviour();
            }
            
            setNewBehaviour(newGroupType);
            doBehaviour ();
        }

        //For internal use in state machine, do not call from outside.
        public void freezeBehaviourUI () {
            if (currentGroup == BehaviourGroupType.EMPTY) {
                addEventLog("freezeBehaviourUI");
                return;
            }

            addEventLog("freezeUI");
            behaviourGroupMap [currentGroup].freezeUI();
            if (characterMove != null) {
                characterMove.stop ();
            }
        }

        //For internal use in state machine, do not call from outside.

        public void setSelectedNewGroup() {
            BehaviourGroupType nextGroup = selectCurrentGroup();

            if (canOverrideWithBehaviour(currentGroup, nextGroup)) {
                setNewBehaviour(nextGroup);
            }
        }

        public bool isStarted () {
            return runState == startedState;
        }

        public void setNewBehaviour(BehaviourGroupType newGroupType) {
            addEventLog("setNewBehaviour " + newGroupType);
            setGroup(newGroupType);
            behaviourGroupMap[currentGroup].restart();
        }

        private void setGroup (BehaviourGroupType behaviourGroupType) {
            currentGroup = behaviourGroupType;
            GameState.Instance.StateData.setCharacterBehaviourType(CharacterKey, behaviourGroupType);
        }

        //For internal use in state machine, do not call from outside.
        public BehaviourRunState RunState {
            set {
                GameState.Instance.StateData.setCharacterBehaviourRunState (CharacterKey, value.getStateType());
                runState = value;
            }
        }

        //For internal use in state machine, do not call from outside.
        public BehaviourRunState getStoppedState() {
            return stoppedState;
        }

        //For internal use in state machine, do not call from outside.
        public BehaviourRunState getStartedState () {
            return startedState;
        }

        //For internal use in state machine, do not call from outside.
        public BehaviourRunState getFrozenStartedState () {
            return frozenStartedState;
        }

        //For internal use in state machine, do not call from outside.
        public BehaviourRunState getFrozenStoppedState () {
            return frozenStoppedState;
        }

        //For internal use in state machine, do not call from outside.
        public void resumeBehaviour() {
            if (currentGroup == BehaviourGroupType.EMPTY) {
                return;
            }

            behaviourGroupMap[currentGroup].resumeBehaviour(onCompletedGroup);         
        }

        public void resumeBehaviourBackground() {
            if (currentGroup == BehaviourGroupType.EMPTY) {
                return;
            }

            behaviourGroupMap[currentGroup].resumeBehaviourBackground();                
        }

        public void startFrozen(BehaviourGroupType groupType) {
            RunState = frozenStartedState;
            addEventLog("startFrozen " + groupType);
            setNewBehaviour(groupType);
        }

        public BehaviourGroupType getCurrentGroupType() {
            return currentGroup;
        }

        public BehaviourGroup getCurrentGroup() {
            return behaviourGroupMap.ContainsKey(currentGroup)
                ? behaviourGroupMap[currentGroup]
                : null;
        }
        
        //For internal use in state machine, do not call from outside.
        private void doBehaviour() {
            if (currentGroup == BehaviourGroupType.EMPTY) {
                return;
            }

            behaviourGroupMap[currentGroup].startBehaviour(onCompletedGroup);
        }

        private void sceneChangeInGameHandler (GameEvents.SceneChangeInCurrentGameEvent e) {
            if (!isStarted ()) {
                return;
            }
            if (currentGroup == BehaviourGroupType.EMPTY) {
                return;
            }
            if (!gameObject.activeInHierarchy) {
                return;
            }

            freezeUI ();
            endOfBehaviour ();
        }

        private void conversationStartHandler(GameEvents.ConversationStartEvent e) {
            freeze();
        }

        private void conversationEndHandler(GameEvents.ConversationEndEvent e) {
            if (isFrozen()) {
                unFreeze();
            }
        }

        private void endOfBehaviour() {
            behaviourGroupMap [currentGroup].endOfBehaviour ();
        }

        private BehaviourRunState getRunStateStartValue () {
            BehaviourRunStateType stateType = getRunStateTypeStartValue ();

            foreach (BehaviourRunState state in states) {
                if (state.getStateType () == stateType) {
                    return state;
                }
            }

            throw new ArgumentException ("Missing state impl for type " + stateType);
        }

        private BehaviourRunStateType getRunStateTypeStartValue () {
            if (!hasBehaviours ()) {
                addEventLog("getRunStateTypeStartValue, no behaviours");
                return BehaviourRunStateType.STOPPED;
            }

            BehaviourRunStateType? state = GameState.Instance.StateData.getCharacterBehaviourRunState (CharacterKey);
            if (state != null) {
                addEventLog("getRunStateTypeStartValue, state: " + state);
                return (BehaviourRunStateType)state;
            }

            addEventLog("getRunStateTypeStartValue, using defaultState: " + defaultState);
            return defaultState;	
        }

        private void onCompletedGroup (bool stop) {
            if (stop) {
                addEventLog("Stopped, onCompletedGroup, stop=true");
                stopAndClearBehaviour();
                return;
            }

            if (!characterSceneHandler.isCharacterInCurrentScene()) {
                addEventLog("Stopped, onCompletedGroup, not in scene");
                RunState = stoppedState;
                return;
            }

            BehaviourGroupType nextGroup = selectCurrentGroup();

            if (nextGroup != BehaviourGroupType.EMPTY) {
                addEventLog("startBehaviour " + nextGroup);
                startBehaviour(nextGroup);
            }
            else {
                stopAndClearBehaviour();
                addEventLog("Stopped, onCompletedGroup, no next behaviour");
            }
        }

        private void stopAndClearBehaviour() {
            RunState = stoppedState;
            GameState.Instance.StateData.setCharacterBehaviourRunState(CharacterKey, BehaviourRunStateType.STOPPED);
            setGroup(BehaviourGroupType.EMPTY);
        }

        private Dictionary<BehaviourGroupType, BehaviourGroup> getBehaviourGroupMap () {
            Dictionary<BehaviourGroupType, BehaviourGroup> behaviourMap = new Dictionary<BehaviourGroupType, BehaviourGroup> ();

            foreach(BehaviourGroup behaviour in behaviourGroups) {
                behaviourMap.Add (behaviour.behaviourGroupType, behaviour);
            }

            return behaviourMap;
        }

        private void setStartGroup() {
            BehaviourGroupType currentSelected = selectCurrentGroup();
            BehaviourGroupType? storedGroupNullable = GameState.Instance.StateData.getCharacterBehaviourType(CharacterKey);

            if (storedGroupNullable != null) {
                BehaviourGroupType storedGroup = (BehaviourGroupType) storedGroupNullable;
                if (canOverrideWithBehaviour(storedGroup, currentSelected)) {
                    setNewBehaviour(currentSelected);
                    return;
                }

                addEventLog("setStartGroup, stored: " + storedGroup);
                setGroup(storedGroup);
                return;
            }

            if (currentSelected != BehaviourGroupType.EMPTY) {
                addEventLog("setStartGroup, selected: " + currentSelected);
                setGroup(currentSelected);
            }
        }

        private bool canOverrideWithBehaviour(BehaviourGroupType current, BehaviourGroupType newBehaviour) {
            return (current == BehaviourGroupType.EMPTY || behaviourGroupMap[current].isAutoBehaviour)
                   && newBehaviour != current && newBehaviour != BehaviourGroupType.EMPTY;
        }

        private BehaviourGroupType selectCurrentGroup() {
            foreach (var group in behaviourGroups) {
                if ((group.condition == null || group.condition.matches()) && group.isAutoBehaviour) {
                    return group.behaviourGroupType;
                }
            }

            return BehaviourGroupType.EMPTY;
        }

        private bool hasBehaviours() {
            return behaviourGroups.Length > 0;
        }

        private bool isFrozen() {
            return runState == frozenStartedState || runState == frozenStoppedState;
        }

        public override string ToString() {
            List<string> reverseLog = new List<string>(eventLog);
            reverseLog.Reverse();
            var eventLogStr = String.Join(", ", reverseLog);
            return "Character: " + CharacterKey + ", " + getBehaviourStr() +  ", Event Log: (" + eventLogStr + ")";
        }

        private string getBehaviourStr() {
            string behaviourName = getCurrentGroup() != null ? getCurrentGroup().getCurrentBehaviour().GetType().Name : "<empty>";
            int behaviourIndex = getCurrentGroup() != null ? getCurrentGroup().CurrentBehaviour : -1;

            return "Current behaviour: " + behaviourName + ", Current behaviour index: " + behaviourIndex 
                   + ", Current behaviour group: " + getCurrentGroupType();
        }

        public void addEventLog(string log) {
            if (eventLog.Count >= MAX_EVENTS) {
                eventLog.RemoveAt(0);
            }
            
            eventLog.Add(DateTime.UtcNow.ToStringStandard() + " - " + getBehaviourStr() + ", Log: " + log);
        }
        
    }
}