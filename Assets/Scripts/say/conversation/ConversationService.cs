using System;
using System.Collections.Generic;
using System.Linq;
using Mgl;
using net.gubbi.goofy.events;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.item;
using net.gubbi.goofy.item.inventory;
using net.gubbi.goofy.player;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using unity;
using UnityEngine;
using Random = System.Random;

namespace net.gubbi.goofy.say.conversation {
    public class ConversationService : InitiableMonoBehaviour {

        public Conversation conversation;

        private ConversationUI ui;
        private SceneInputHandler inputHandler;
        private Say say;
        private GameObject playerGo;
        private Random random = new Random();
        private Inventory inventory;
        private static readonly string CONTROL_BY_ID = "conversation";
        private static readonly string CUSTOM_OPT_PREFIX = "+";

        private void Awake() {
            this.ui = GameObject.Find (GameObjects.CONVERSATION).GetComponent<ConversationUI>();
            this.inventory = GameObject.Find (GameObjects.INVENTORY).GetComponent<Inventory>();
            this.playerGo = GameObject.Find(GameObjects.PLAYER);
            this.inputHandler = playerGo.GetComponent<SceneInputHandler> ();
            this.say = GameObject.Find(GameObjects.SPEECH_BUBBLE_CONTAINER).GetComponent<Say>();		
        }

        private void Start() {
            if (conversation == null) {
                conversation = GetComponents<Conversation>().FirstOrDefault(c => c.enabled);
            }
        }

        public override void init() {                        
            ConversationState convState = GameState.Instance.StateData.ConversationState;
            if (convState != null) {
                if (convState.CharacterKey.Equals (gameObject.name)) {
                    beginInternal (GameState.Instance.StateData.SelectedItem, convState.PositionId, convState.AtChildren);
                }
            }
        }

        public void begin () {
            this.begin (inventory.SelectedItem);
        }

        public void begin (ItemType selectedItem) {
            beginInternal (selectedItem, null, false);
        }
        private void beginInternal (ItemType selectedItem, string positionId, bool atChildren) {
            EventManager.Instance.raise(new GameEvents.ConversationStartEvent());   
            inputHandler.setControlEnabled (false, CONTROL_BY_ID);
            
            GameState.Instance.StateData.setConversationCharacter (gameObject.name);                        

            conversation.initConversations();
            ConversationOption rootOption = selectConversation (selectedItem);

            if (!positionId.isNullOrEmpty ()) {
                ConversationOption opt = rootOption.gotoPosition (positionId);

                if (opt == null) {
                    converse (rootOption);
                }
                else {
                    if (atChildren) {
                        converse(opt);
                    }
                    else {
                        chooseOption (opt);    
                    }                    
                }
                
            } else {
                converse (rootOption);
            }
        }

        private void converse (ConversationOption option) {
            if (!option.Transient) {
                GameState.Instance.StateData.setConversationPosition(option.getPositionId());
                GameState.Instance.StateData.setConversationAtChildren(true);    
            }
            
            option.refreshChildren();            
            IList<ConversationOption> availableChildren = option.getChildren()
                .Where (c => c.isAvailable())
                .ToList ();
            
            if (availableChildren.Count == 0) {
                endConversation ();
                return;
            }
            
            if (availableChildren.Count == 1) {
                chooseOption(availableChildren.ElementAt (0));
                return;
            }

            IList<ConversationOption> characterOptions = availableChildren
                .Where (c => !c.Value.SelfOption)
                .ToList ();

            if (characterOptions.Count > 0) {
                chooseOption(characterOptions.ElementAt (random.Next (0, characterOptions.Count)));
                return;
            }

            List<string> texts = new List<string>(availableChildren.Select (getOptionText()));
            texts.Reverse();
            Action<int> onOptionSelected = delegate(int nextChosenChild) {
                int reversedIndex = texts.Count - 1 - nextChosenChild;
                chooseOption(availableChildren.ElementAt (reversedIndex));
            };
            ui.enableConversation (texts, onOptionSelected);
        }

        private static Func<ConversationOption, string> getOptionText() {

            
            return c => {
                string text = c.Value.SayText;
                
                string customOptKey = CUSTOM_OPT_PREFIX + text;
                string customOpt = I18n.Instance.__(customOptKey);
                if (!customOpt.Equals(customOptKey)) {
                    return customOpt;
                }

                text = I18n.Instance.__(text);
                return text.splitAndFirst(I18nUtil.LINE_DELIM);
            };
        }

        private void chooseOption(ConversationOption option) {
            if (!option.Transient) {
                GameState.Instance.StateData.setConversationPosition(option.getPositionId());
                GameState.Instance.StateData.setConversationAtChildren(false);
            }

            option.setUsed();

            ui.disableConversation ();
            if (option.isReference ()) {
                converse (option.resolve ());
            } 
            else if (option.isDelayed()) {
                option.onDelayedResult(delegate { sayText(option);});        
            }            
            else {
                sayText (option);
            }	
        }

        private ConversationOption selectConversation (ItemType selectedItem) {
            if (selectedItem != ItemType.EMPTY ) {
                if (conversation.ItemConversations.ContainsKey (selectedItem)) {
                    return conversation.ItemConversations [selectedItem];
                } else {
                    return conversation.InvalidItemConversation;
                }
            }		

            return conversation.RootConversation;
        }

        private void sayText (ConversationOption option) {						
            Action sayCompleteCallback = delegate {
                if(option.Value.AfterSayCallback != null) {
                    option.Value.AfterSayCallback(option, delegate { converse (option); });
                }
                else {
                    //After saying text, show next options for this option
                    converse (option);
                }
            };		

            string text = option.Value.SayText;

            if (option.Value.SayArgs != null && option.Value.SayArgs.Length > 0) {
                var i18NArgs = option.Value.SayArgs.Select(a => I18n.Instance.__(a.ToString())).ToArray();
                text = I18n.Instance.__(text, i18NArgs);
            }
            else {
                text = I18n.Instance.__(text);
            }
            
            ItemType item = inventory.SelectedItem;

            if (inventory.getItem(item) != null) {
                var itemLabel = inventory.getItem(item).Type.getLabel();
                text = text.Replace("{thisItem}", I18n.Instance.__("this" + itemLabel));
            }

            if (option.Value.BeforeSayCallback != null) {
                option.Value.BeforeSayCallback ();
            }

            GameObject sayer = option.Value.SelfOption ? this.playerGo : gameObject;
            say.sayRaw (sayer, text, sayCompleteCallback);

        }

        private void endConversation () {
            GameState.Instance.StateData.clearConversationState ();            
            inputHandler.setControlEnabled (true, CONTROL_BY_ID);
            inventory.deselectItem();
            EventManager.Instance.raise(new GameEvents.ConversationEndEvent());            
        }

    }
}