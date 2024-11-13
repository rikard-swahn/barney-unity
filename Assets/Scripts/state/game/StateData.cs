﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using net.gubbi.goofy.character.npc.behaviour.state;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.item;
using net.gubbi.goofy.util;
using state;
using state.game;
using UnityEngine;

namespace net.gubbi.goofy.state {

    public class StateData  {
        public string Scene {get; set;}
        
        public Dictionary<string, Dictionary<string, Property>> SceneProperties {get; private set;}
        public PlayerState PlayerState {get; private set;}
        public IOrderedDictionary PlayerItems {get; private set;}
        public ItemType SelectedItem {get; set;}
        public HashSet<string> Flags {get; private set;}
        public Dictionary<string, CharacterState> CharacterState {get; private set;}
        public ConversationState ConversationState { get; private set; }
        public List<Goal> Goals { get; set;}
        public InventoryState InventoryState { get; private set; }
        public float PartGameTimeSeconds {get; set;}
        public float PartStartedGameTimeSeconds {get; set;}
        
        public bool Initialized {get; set;}

        public StateData() {
            PlayerState = new PlayerState ();
            PlayerItems = new OrderedDictionary();
            SelectedItem = ItemType.EMPTY;
            SceneProperties = new Dictionary<string, Dictionary<string, Property>> ();
            Flags = new HashSet<string>();
            CharacterState = new Dictionary<string, CharacterState> ();
            Goals = new List<Goal>();
            InventoryState = new InventoryState();
        }

        public StateData(StateDataDto dto) {
            Scene = dto.scene;
            
            PlayerState = new PlayerState(dto.playerState);
            PlayerItems = new OrderedDictionary();
            foreach(ItemDto itemDto in dto.playerItems) {                                
                addItemToPlayer (Item.fromDto(itemDto));
            }
            SelectedItem = dto.selectedItem;
			            
            SceneProperties = dto.sceneItemProperties;
            Flags = new HashSet<string> (dto.flags);

            CharacterState = new Dictionary<string, CharacterState> ();
            var entries = dto.characterState.GetEnumerator ();
            while (entries.MoveNext()) {
                var current = entries.Current;
                CharacterState.Add (current.Key, new CharacterState (current.Value));
            }

            ConversationState = dto.conversationState != null ? new ConversationState(dto.conversationState) : null;
            Goals = dto.goals;
            
            InventoryState = new InventoryState(dto.inventoryState);
            
            PartGameTimeSeconds = dto.partGameTimeSeconds;
            PartStartedGameTimeSeconds = dto.partStartedGameTimeSeconds;
            
            Initialized = true;
        }

        public bool hasSceneProperty(string itemKey, string propertyKey) {
            return SceneProperties.ContainsKey (itemKey) && SceneProperties [itemKey].ContainsKey(propertyKey);
        }

        public bool getSceneProperty(string itemKey, string propertyKey, bool defaultValue) {
            return hasSceneProperty(itemKey, propertyKey)
                ? getSceneProperty(itemKey, propertyKey).getBool()
                : defaultValue;
        }
        public string getSceneProperty(string itemKey, string propertyKey, string defaultValue) {
            return hasSceneProperty(itemKey, propertyKey)
                ? getSceneProperty(itemKey, propertyKey).stringVal
                : defaultValue;
        }

        public Property getSceneProperty(string itemKey, string propertyKey)  {
            return SceneProperties[itemKey][propertyKey];
        }

        public void setSceneProperty (string itemKey, string propertyKey, bool val) {
            addMissingSceneProperties(itemKey);
            SceneProperties [itemKey] [propertyKey] = new Property(val);
        }
        public void setSceneProperty (string itemKey, string propertyKey, float val) {
            addMissingSceneProperties(itemKey);
            SceneProperties [itemKey] [propertyKey] = new Property(val);
        }
        public void setSceneProperty (string itemKey, string propertyKey, string val) {
            addMissingSceneProperties(itemKey);
            SceneProperties [itemKey] [propertyKey] = new Property(val);
        }

        public void removeSeneProperty(string itemKey, string propertyKey) {
            if (SceneProperties.ContainsKey(itemKey) && SceneProperties[itemKey].ContainsKey(propertyKey)) {
                SceneProperties[itemKey].Remove(propertyKey);    
            }            
        }

        public void addItemToPlayer (ItemType type) {
            if (type.alwaysUseProperties ()) {
                throw new ArgumentException ("Item of type " + type + " can not be added by type, use a full Item object");
            }

            addItemToPlayer (new Item (type));
        }

        public bool addItemToPlayer (Item item) {
            if (item.Type == ItemType.EMPTY) {
                throw new ArgumentException ("Empty item can not be added");
            }

            if (PlayerItems.Contains (item.Type)) {
                Item curItem = (Item)PlayerItems [item.Type];
                return curItem.addProperties (item);
            } else {
                PlayerItems.Add (item.Type, item);
                return true;
            }				
        }

        public void removeItemFromPlayer (ItemType type) {
            PlayerItems.Remove (type);
        }

        public Item getItem(ItemType type) {
            return (Item)PlayerItems [type];
        }

        public void setPlayerDirection (Vector2 direction) {
            PlayerState.setDirection (direction);
        }

        public void setPlayerPosition (Vector2 position) {
            PlayerState.setPosition (position);
        }

        public bool flagsSet(params string[] flags) {
            foreach(string flag in flags) {
                if(!flagSet(flag)) {
                    return false;
                }
            }

            return true;			
        }

        public bool flagsNotSet(params string[] flags) {
            foreach(string flag in flags) {
                if(flagSet(flag)) {
                    return false;
                }
            }

            return true;			
        }

        private bool flagSet (string flagName) {
            return Flags.Contains(flagName);
        }

        public void setFlags (params string[] flags) {
            foreach(string flag in flags) {
                Flags.Add (flag);
            }
        }

        public void removeFlags (params string[] flags) {
            foreach(string flag in flags) {
                Flags.Remove (flag);
            }
        }

        public void setGoal(int goal, string text) {
            Goals[goal] = new Goal(text);
        }
        
        public void setGoalComplete(int goal) {
            Goals[goal].complete = true;
        }

        public void setGoals(List<string> goals) {
            Goals = goals.Select(g => new Goal(g)).ToList();
        }

        public void setConversationCharacter(string characterKey) {
            ConversationState = new ConversationState (characterKey);
        }

        public void setConversationPosition(string positionId) {
            ConversationState.PositionId = positionId;
        }
        
        public void setConversationAtChildren(bool set) {
            ConversationState.AtChildren = set;
        }

        public void clearConversationState() {
            ConversationState = null;
        }

        //Character state methods ------------------
        public bool hasCharacterState(string characterKey) {
            return CharacterState.ContainsKey(characterKey);
        }

        public void setCharacterPosition (string characterKey, Vector3 position) {
            addMissingCharacterState (characterKey);
            CharacterState [characterKey].setPosition(position);
        }

        public Vector2? getCharacterPosition (string characterKey) {
            if (!hasCharacterState (characterKey)) {
                return null;
            }			

            return CharacterState [characterKey].getPosition();
        }

        public void setCharacterDirection (string characterKey, Vector2 direction) {
            addMissingCharacterState (characterKey);
            CharacterState [characterKey].setDirection(direction);
        }

        public Vector2? getCharacterDirection (string characterKey) {
            if (!hasCharacterState (characterKey)) {
                return null;
            }			

            return CharacterState [characterKey].getDirection();
        }

        public void changeCharacterScene (string characterKey, string scene) {
            //Debug.Log ("Set character " + characterKey + " scene to " + scene);

            if (hasCharacterState(characterKey)) {
                CharacterState.Remove(characterKey);
            }

            addMissingCharacterState(characterKey);

            CharacterState [characterKey].Scene = scene;
        }
        public string getCharacterScene(string characterKey) {
            if (!hasCharacterState (characterKey)) {
                return null;
            }

            return CharacterState [characterKey].Scene;
        }

        public void setCharacterBehaviourState (string characterKey, string key, int val) {
            addMissingBehaviourState (characterKey);
            CharacterState [characterKey].Behavior.Properties[key] = new Property(val);
        }
        public bool hasCharacterBehaviourProperty (string characterKey, string propertyKey) {
            return hasCharacterState (characterKey)
                   && CharacterState [characterKey].Behavior != null
                   && CharacterState [characterKey].Behavior.Properties.ContainsKey (propertyKey);
        }
        public Property getCharacterBehaviourState (string characterKey, string propertyKey)  {
            return CharacterState [characterKey].Behavior.Properties[propertyKey];
        }

        public void setCharacterBehaviourIndex (string characterKey, int behaviourIndex) {
            addMissingBehaviourState (characterKey);
            if (CharacterState[characterKey].Behavior.Index != behaviourIndex) {
                CharacterState[characterKey].Behavior.Properties = new Dictionary<string, Property>();
                CharacterState[characterKey].Behavior.Index = behaviourIndex;
            }
        }
        public int? getCharacterBehaviourIndex (string characterKey) {
            if (!hasCharacterState (characterKey) || CharacterState [characterKey].Behavior == null) {
                return null;
            }			

            return CharacterState [characterKey].Behavior.Index;
        }			
        public BehaviourRunStateType? getCharacterBehaviourRunState (string characterKey) {
            if (!hasCharacterState (characterKey) || CharacterState [characterKey].Behavior == null) {
                return null;
            }			

            return CharacterState [characterKey].Behavior.State;
        }	

        public void setCharacterBehaviourRunState (string characterKey, BehaviourRunStateType? runState) {
            addMissingBehaviourState (characterKey);

            CharacterState [characterKey].Behavior.State = runState;
        }

        public void setCharacterBehaviourType (string characterKey, BehaviourGroupType groupType) {
            //Debug.Log ("Set character '" + characterKey + "' behaviour group to " + groupType);
            addMissingBehaviourState (characterKey);
            CharacterState [characterKey].Behavior.BehaviourGroupType = groupType;
        }
        
        public BehaviourGroupType? getCharacterBehaviourType (string characterKey) {
            if (!hasCharacterState (characterKey) || CharacterState [characterKey].Behavior == null) {
                return null;
            }			

            return CharacterState [characterKey].Behavior.BehaviourGroupType;
        }

        public void setCharacterActive (string characterKey, bool active) {
            addMissingCharacterState (characterKey);
            CharacterState [characterKey].Active = active;
        }

        public bool isCharacterActive (string characterKey) {
            return hasCharacterState (characterKey) && CharacterState [characterKey].Active != null && (bool)CharacterState [characterKey].Active;
        }

        public bool? getCharacterActive (string characterKey) {
            return hasCharacterState (characterKey) ? CharacterState [characterKey].Active : (bool?)null;
        }

        public void setCharacterVisible(string characterKey, bool visible) {
            addMissingCharacterState (characterKey);
            CharacterState [characterKey].Visible = visible;
        }

        public bool isCharacterVisible(string characterKey) {
            return hasCharacterState (characterKey) && CharacterState [characterKey].Visible != null && (bool)CharacterState [characterKey].Visible;
        }

        public bool? getCharacterVisible(string characterKey) {
            return hasCharacterState (characterKey) ? CharacterState [characterKey].Visible : (bool?)null;
        }

        public string getCharacterBodyState (string characterKey) {
            if (!hasCharacterState (characterKey)) {
                return null;
            }

            return CharacterState [characterKey].BodyState;
        }
        public void setCharacterBodyState (string characterKey, string state) {
            addMissingCharacterState (characterKey);
            CharacterState [characterKey].BodyState = state;
        }
        
        public HashSet<string> getCharacterAnimationBoolParams (string characterKey) {
            if (!hasCharacterState (characterKey)) {
                return null;
            }

            return CharacterState[characterKey].AnimatorState.BoolParams;
        }

        public void setCharacterAnimationBoolParams (string characterKey, params string[] boolParams) {
            addMissingCharacterState (characterKey);
            boolParams.ForEach(p => CharacterState [characterKey].AnimatorState.BoolParams.Add(p));
        }        
        public void clearCharacterAnimationBoolParams (string characterKey, params string[] boolParams) {
            addMissingCharacterState (characterKey);
            boolParams.ForEach(p => CharacterState [characterKey].AnimatorState.BoolParams.Remove(p));
        }          
						
        private void addMissingCharacterState (string characterKey) {
            if (!hasCharacterState (characterKey)) {				
                CharacterState.Add (characterKey, new CharacterState ());			
            } 
        }
        private void addMissingBehaviourState(string characterKey) {
            addMissingCharacterState (characterKey);
            if (!hasBehaviourState(characterKey)) {
                CharacterState [characterKey].Behavior = new CharacterState.BehaviourState ();
            }
        }

        private bool hasBehaviourState(string characterKey) {
            return CharacterState [characterKey].Behavior != null;
        }
        
        private void addMissingSceneProperties(string itemKey) {
            if (!SceneProperties.ContainsKey(itemKey)) {
                SceneProperties.Add(itemKey, new Dictionary<string, Property>());
            }
        }        

    }

}