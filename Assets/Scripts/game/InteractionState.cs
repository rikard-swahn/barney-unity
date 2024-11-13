using System;
using System.Collections.Generic;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.util;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

namespace net.gubbi.goofy.game {
    public class InteractionState {

        private Dictionary<StateType, int> stateDic = new Dictionary<StateType, int>();
        private List<String> eventLog = new List<string>();
        private static readonly int MAX_EVENTS = 10;
        
        private static volatile InteractionState instance;
        private static object syncRoot = new Object();
        private InteractionState() {}
        public static InteractionState Instance {
            get {
                if (instance == null) {
                    lock (syncRoot) {
                        if (instance == null) {
                            instance = new InteractionState();
                        }
                    }
                }
                return instance;
            }
        }

        public bool isMouseOverMenu() {                        
            return MouseUtil.isHovering() && RaycastUtil.mouseRaycastHitsTag(Tags.MENU_AREA);            
        }
        
        public bool hasState(StateType state) {
            return stateDic.ContainsKey(state); 
        }
        
        public bool hasOnlyState(StateType state) {
            return stateDic.ContainsKey(state) && stateDic.Count == 1; 
        }        
        
        public bool isEmpty() {
            return stateDic.Count == 0;
        }

        public void clear() {
            stateDic.Clear();
            eventLog.Clear();
        }

        public void setState(StateType type) {
            if (stateDic.ContainsKey(type)) {
                stateDic[type]++;
            }
            else {
                stateDic[type] = 1;
            }
            
            addEventLog("Set state " + type + ". Count: " + stateDic[type]);
        }

        public void removeState(StateType type) {
            if (!stateDic.ContainsKey(type)) {
                List<string> reverseLog = new List<string>(eventLog);
                reverseLog.Reverse();
                var eventLogStr = String.Join(", ", reverseLog);
                throw new ArgumentException("Tried removing state " + type + ", which has count 0. Scene:" + SceneManager.GetActiveScene ().name 
                                            + ", Time: " + DateTime.UtcNow.ToStringStandard() + ", Event Log: " + eventLogStr);
            }

            stateDic[type]--;
            
            addEventLog("Remove state " + type + ". Count: " + stateDic[type]);
            
            if (stateDic[type] <= 0) {
                stateDic.Remove(type);
            }
        }

        public enum StateType {
            SCENE_CANVAS, PAUSE_MENU
        }
        
        private void addEventLog(string log) {
            if (eventLog.Count >= MAX_EVENTS) {
                eventLog.RemoveAt(0);
            }
            
            eventLog.Add(DateTime.UtcNow.ToStringStandard() + " - " + log);
        }        
    }
}