using System;
using System.Collections.Generic;
using System.Linq;
using net.gubbi.goofy.audio;
using net.gubbi.goofy.character;
using net.gubbi.goofy.character.npc.behaviour;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.item;
using net.gubbi.goofy.item.inventory;
using net.gubbi.goofy.player;
using net.gubbi.goofy.scene.item;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.say.conversation {

    public class Conversation : MonoBehaviour {

        public bool disableForHandheld;
        public bool disableForDesktop;

        protected Inventory Inventory { get; private set;}
        protected CharacterFacade playerCharacterFacade;
        protected CharacterMove characterMove;
        protected CharacterMove playerMove;
        protected CharacterBehaviours behaviours;
        private ConversationOption rootConversation;
        protected IDictionary<ItemType, ConversationOption> itemConversations;
        protected ConversationOption invalidItemConversation;
        protected Animator[] animators;
        protected TimedAnimatorFlag timedAnimatorFlag;
        protected SfxPlayer sfxPlayer;
        
        protected static readonly string ROOT_ROOT_ID = "ROOT";

        protected virtual void Awake() {
            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            Inventory = GameObject.Find (GameObjects.INVENTORY).GetComponent<Inventory>();
            playerCharacterFacade = playerGo.GetComponent<CharacterFacade>();
            characterMove = GetComponent<CharacterMove>();
            behaviours = GetComponentInChildren<CharacterBehaviours>();
            animators = GetComponentsInChildren<Animator>();
            timedAnimatorFlag = playerGo.GetComponent<TimedAnimatorFlag>();
            playerMove = playerGo.GetComponent<CharacterMove>();
            sfxPlayer = GameObject.Find (GameObjects.SFX_PLAYER).GetComponent<SfxPlayer>();
            
            bool isHandheld = SystemInfo.deviceType == DeviceType.Handheld;
            enabled = isHandheld && !disableForHandheld || !isHandheld && !disableForDesktop;
        }

        public virtual void initConversations () {
            rootConversation = ConversationOption.value(ConversationOptionValue.EMPTY).build();
            itemConversations = new Dictionary<ItemType, ConversationOption>();

            invalidItemConversation = root ().options (
                playerOption("Do you want {thisItem}?").options(
                    getCharacterOptionsFromText(I18nUtil.options("CharacterDecline"))
                )
            );

            ItemConversations[ItemType.BANK_CARD] =
                root().options(
                    playerOption(
                            delegate {
                                playerCharacterFacade.turnTowardsCamera();
                            },
                            "Why would I give that away?"
                        )
                        .build()
                );

            itemConversations[ItemType.TURD] = root().options(
                    playerOption("Do you want {thisItem}?").options(
                        getCharacterOptionsFromText(I18nUtil.options("CharacterDeclineStrong"))
                    )
                );

        }

        public virtual ConversationOption[] getOptionsToCheckLengthFor() {
            return new[]{RootConversation};
        }

        protected void setAnimatorFlag(string flag, bool set) {            
            animators.ForEach(a => a.SetBool(flag, set));
        }

        protected void setAnimatorTrigger(string trigger) {            
            animators.ForEach(a => a.SetTrigger(trigger));
        }

        protected ConversationOptionValue.Builder optionValue() {
            return ConversationOptionValue.builder ();
        }

        protected ConversationOption.Builder root() {
            return ConversationOption.value (ConversationOptionValue.EMPTY);
        }

        protected ConversationOption.Builder root(string rootId) {
            return ConversationOption.value (id(rootId).build());
        }

        protected ConversationOption.Builder playerOption(string text) {
            return ConversationOption.value (ConversationOptionValue.builder().playerOption (text).build());
        }

        protected ConversationOption.Builder playerOption(Action beforeSay, string text) {
            return ConversationOption.value (
                ConversationOptionValue.builder ().playerOption (text).beforeSay(beforeSay).build()
            );				
        }

        protected ConversationOption.Builder playerOption(string text, Action<ConversationOption> afterSay) {
            return ConversationOption.value (
                ConversationOptionValue.builder ().playerOption (text).afterSay(afterSay).build()
            );				
        }

        protected ConversationOption.Builder playerOption(string text, Action<ConversationOption, Action> afterSay) {
            return ConversationOption.value (
                ConversationOptionValue.builder ().playerOption (text).afterSayDelayed(afterSay).build()
            );				
        }

        protected ConversationOption.Builder characterOption(string text, Action<ConversationOption> afterSay) {
            return ConversationOption.value (ConversationOptionValue.builder().characterOption (text).afterSay(afterSay).build());
        }
        
        protected ConversationOption.Builder characterOption(string text, Action<ConversationOption, Action> afterSay) {
            return ConversationOption.value (ConversationOptionValue.builder().characterOption (text).afterSayDelayed(afterSay).build());
        }

        protected ConversationOption.Builder characterOption(string text, params object[] args) {
            return ConversationOption.value (ConversationOptionValue.builder().characterOption (text, args).build());
        }


        protected ConversationOptionValue.Builder availableIf(Func<ConversationOption, bool> isAvailableCallback) {
            return ConversationOptionValue.builder ()
                .availableIf (isAvailableCallback);
        }

        protected ConversationOptionValue.Builder showOnceTransient() {
            return ConversationOptionValue.builder()
                .availableIf(option => {
                        return !option.isUsed();
                    }
                );
        }

        protected ConversationOptionValue.Builder id(string id) {
            return ConversationOptionValue.builder().id(id);			
        }

        protected ConversationOptionValue.Builder ifItems(params ItemType[] requiresItems) {
            return ConversationOptionValue.builder()
                .availableIf(delegate {
                    HashSet<ItemType> invTypes = new HashSet<ItemType> (
                        GameState.Instance.StateData.PlayerItems.Values.Cast<Item>().Select (e => e.Type)
                    );

                    HashSet<ItemType> requiredItemsSet = new HashSet<ItemType> (requiresItems);

                    //Is all the requires item types in the inventory?
                    return requiredItemsSet.IsSubsetOf(invTypes);
                });
        }

        protected ConversationOptionValue.Builder ifNotCash(int amount) {
            return ifNotItems (new CardItem (amount));
        }

        protected ConversationOptionValue.Builder ifNotItems(params Item[] requiresItems) {
            return ConversationOptionValue.builder()
                .availableIf(delegate {
                    return !Inventory.hasItem(requiresItems);
                });			
        }

        protected ConversationOptionValue.Builder ifCash(int amount) {
            return ifItems (new CardItem (amount));
        }

        protected ConversationOptionValue.Builder ifItems(params Item[] requiresItems) {
            return ConversationOptionValue.builder()
                .availableIf(delegate {
                    return Inventory.hasItem(requiresItems);
                });
        }

        protected ConversationOption.Builder option(ConversationOptionValue optionValue) {
            return ConversationOption.value (optionValue);
        }

        protected ConversationOption ancestorOption(string ancestorId, bool transient = false) {
            return new AncestorIdConversationOption (ancestorId, transient);
        }

        protected ConversationOption ancestorOption(int ancestorSteps) {
            return new AncestorNConversationOption (ancestorSteps);
        }

        protected virtual string getConversationId () {
            throw new NotImplementedException ();
        }

        protected ConversationOption[] getCharacterOptionsFromText(string[] texts) {
            return texts.Select (x => characterOption (x).build ())
                .ToArray ();
        }

        protected ConversationOption[] getCharacterOptionsFromText(string[] texts, ConversationOption nextOption) {
            return texts.Select (x => characterOption (x).options(nextOption))
                .ToArray ();			
        }

        protected static bool flagsSet(params string[] flags) {
            return GameState.Instance.StateData.flagsSet (flags); 
        }

        protected static bool flagsNotSet(params string[] flags) {
            return GameState.Instance.StateData.flagsNotSet (flags); 
        }

        protected static bool characterActive(string characterKey) {
            return GameState.Instance.StateData.isCharacterActive(characterKey);
        }

        protected static bool sceneItemFlag(string itemKey, string itemProperty) {
            return GameState.Instance.StateData.hasSceneProperty(itemKey, itemProperty) &&
                   GameState.Instance.StateData.getSceneProperty(itemKey, itemProperty).getBool();
        }

        protected static void removeFlag(string flag) {
            GameState.Instance.StateData.removeFlags(flag);
        }

        protected static void setFlag(string flag) {			
            GameState.Instance.StateData.setFlags (flag);
        }
        
        protected void playerSetItemActive(string itemKey, bool active, bool mirrorOnVertical = false) {            
            playerMove.MirrorOnVertical = mirrorOnVertical;
            standardPlayerAction(delegate {
                playerMove.MirrorOnVertical = false;
            });
            SceneItemUtil.setItemActive(itemKey, active);
        }
        
        protected void playerActivateItemDelayed(string itemKey, Action afterCallback, bool mirrorOnVertical = false) {            
            playerMove.MirrorOnVertical = mirrorOnVertical;
            standardPlayerAction(delegate {
                playerMove.MirrorOnVertical = false;
                afterCallback();
                SceneItemUtil.setItemActive(itemKey, true);
            });
            
            SceneItemUtil.setItemActiveTransient(itemKey, true);            
        } 
        
        protected void playerDeactivateItemDelayedAfterAnimation(string itemKey, Action afterAnimation, bool mirrorOnVertical = false) {
            playerMove.MirrorOnVertical = mirrorOnVertical;
            standardPlayerAction(delegate {
                playerMove.MirrorOnVertical = false;
                SceneItemUtil.setItemActive(itemKey, false);
                afterAnimation();
            });            
        }
                
        protected void standardPlayerAction(Action callback = null) {
            timedAnimatorFlag.setFlag(callback, AnimationParams.DO_ACTION, Constants.DEFAULT_ACTION_TIME);
        }                
			
        public ConversationOption RootConversation {
            get {
                return this.rootConversation;
            }
            set {
                this.rootConversation = value;
            }
        }

        public IDictionary<ItemType, ConversationOption> ItemConversations {
            get {
                return this.itemConversations;
            }
        }

        public ConversationOption InvalidItemConversation {
            get {
                return this.invalidItemConversation;
            }
        }
    }



}