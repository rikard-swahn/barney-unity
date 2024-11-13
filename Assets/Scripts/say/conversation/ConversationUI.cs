using System;
using System.Collections.Generic;
using net.gubbi.goofy.events;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.game;
using net.gubbi.goofy.ui.cursor;
using net.gubbi.goofy.util;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.say.conversation {
    public class ConversationUI : MonoBehaviour {

        private IList<GameObject> options;
        private Action<int> optionCallback;
        private Image background;
        private bool conversationEnabled;
        private bool controlDisabledOverride;

        private void Awake() {
            options = gameObject.findChildrenWithTag(Tags.CONV_OPT);
            background = GetComponent<Image>();
            
            background.enabled = false;
            disableOptions();			
            EventManager.Instance.addListener<GameEvents.SceneTransitionStartedEvent>(sceneTransitionStartedHandler);
            EventManager.Instance.addListener<GameEvents.SceneTransitionCompletedEvent>(sceneTransitionCompletedHandler);                        
        }
        
        private void sceneTransitionStartedHandler(GameEvents.SceneTransitionStartedEvent e) {
            controlDisabledOverride = true;
        }
        private void sceneTransitionCompletedHandler(GameEvents.SceneTransitionCompletedEvent e) {
            controlDisabledOverride = false;
        }        

        public void onSelectOption(int optionIndex) {				
            optionCallback(optionIndex);
        }

        private void Update() {
            if (conversationEnabled && !controlDisabledOverride) {
                EventManager.Instance.raise (new GameEvents.CursorEvent(CursorType.NORMAL));
            }
        }

        public void enableConversation(IList<string> texts, Action<int> optionCallback) {
            if (texts.Count == 0) {
                throw new ArgumentException("No options given!");
            }

            if (texts.Count > options.Count) {
                Debug.LogError("More options given than UI text elements!");
            }

            conversationEnabled = true;
            background.enabled = true;
            InteractionState.Instance.setState(InteractionState.StateType.SCENE_CANVAS);
            this.optionCallback = optionCallback;

            for(int i = 0; i < options.Count; i++) {
                if (texts.Count > i) {
                    enableOption (i, texts[i]);
                } else {
                    disableOption (i);
                }
            }
        }

        public void disableConversation() {            
            if (conversationEnabled) {
                conversationEnabled = false;
                background.enabled = false;
                InteractionState.Instance.removeState(InteractionState.StateType.SCENE_CANVAS);
                disableOptions();
            }            
        }

        private void disableOptions() {
            for (int i = 0; i < options.Count; i++) {
                disableOption(i);
            }
        }

        private void disableOption(int option) {
            options [option].GetComponentInChildren<Text>().text = "";            
            options[option].gameObject.SetActive(false);		
        }

        private void enableOption(int option, string text) {
            options [option].GetComponentInChildren<Text>().text = "> " + text;
            options[option].gameObject.SetActive(true);
        }
			
    }
}