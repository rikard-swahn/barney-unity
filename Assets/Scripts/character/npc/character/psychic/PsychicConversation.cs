using System;
using net.gubbi.goofy.say.conversation;
using net.gubbi.goofy.scene.item;
using net.gubbi.goofy.scene.load;
using net.gubbi.goofy.scene.load.transition;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.character.psychic {
    public class PsychicConversation : Conversation {

        private UISceneItem phoneItem;
        private static readonly string CONVERSATION_ID = "PSYCHIC";
        private static readonly Vector2 DOWNSTAIRS_PHONE_POS = new Vector2(-2.476f, -0.205f);
        private static readonly Vector2 DOWNSTAIRS_PHONE_DIR = new Vector2(0, -1);        
        private static readonly Vector2 START_POS = new Vector2(3.686f, 0);        

        private static readonly string ROOT_PLAYER_OPTIONS = "ROOT_PLAYER_OPTIONS";
        
        protected override void Awake() {
            base.Awake();
            phoneItem = GameObject.Find(GameObjects.PHONE_ZOOMED)?.GetComponent<UISceneItem>();
        }        
        
        public override void initConversations () {
            base.initConversations ();
            phoneItem?.setInteractable(false);
            
            ConversationOption fortuneDetails =
                characterOption("That is all I can see.").options(
                    playerOption("OK... hmm.").options(
                        ancestorOption(ROOT_PLAYER_OPTIONS)
                    )                            
                );            
            
            ConversationOption passwordWhat =
                characterOption("Don't distract me please!|It is...|The name of a loved one, now passed away.").options(
                    playerOption("Oh, yes! Now I remember!").options(
                        ancestorOption(ROOT_PLAYER_OPTIONS)
                    )                            
                );
            
            Func<ConversationOption, bool> pcAtLoginScreen = delegate { return flagsSet(Flags.PC_USED) && flagsNotSet(Flags.PC_LOGGED_IN); };
                
            RootConversation =
                root (ROOT_ROOT_ID).options (
                    option(id(ROOT_PLAYER_OPTIONS).characterOption("This is Madame Zaida speaking, Professional Fortune Teller.|What can I help you with today?").build()).options(
                        playerOption("That's all, bye.", opt => {exitPhone();}).build(),
                        option(showOnceTransient().availableIf(pcAtLoginScreen).playerOption("I have forgotten the password to my computer.").build()).options(
                            characterOption("Yes... I see it...|It is... it is...").options(
                                playerOption("Yes?").options(passwordWhat),
                                playerOption("What?").options(passwordWhat)
                            )
                        ),                        
                        option(showOnceTransient().playerOption("Tell me my fortune.").build()).options(
                            characterOption("As you wish...|I see... I see a betrayal.|Love will make you betray an old friend.").options(
                                playerOption("Oh. Any more details?").options(fortuneDetails),
                                playerOption("Who is this friend?").options(fortuneDetails)
                            )
                        )
                    )                    
                );
        }

        private void exitPhone() {
            playerMove.setPositionState(DOWNSTAIRS_PHONE_POS);
            playerMove.setDirectionState(DOWNSTAIRS_PHONE_DIR);
            
            GameState.Instance.StateData.setCharacterActive(Characters.PSYCHIC, false);
            GameState.Instance.StateData.setSceneProperty("ExitButton", SceneItemProperties.ACTIVATE_ON_LOAD, true);            
            characterMove.setPositionState(START_POS);
            phoneItem.setInteractable(true);
                        
            SceneLoader.Instance.changeInGameScene (Scenes.GOOFY_DOWNSTAIRS, Transitions.get(Transitions.Type.CIRCLE_OUT), Transitions.get(Transitions.Type.CIRCLE_IN));
        }        

        protected override string getConversationId () {
            return CONVERSATION_ID;
        }

    }

}