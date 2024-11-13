using net.gubbi.goofy.say.conversation;
using net.gubbi.goofy.scene.item;
using net.gubbi.goofy.scene.load;
using net.gubbi.goofy.scene.load.transition;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.character.police {
    public class PoliceConversation : Conversation {

        private UISceneItem phoneItem;
        private static readonly string CONVERSATION_ID = "POLICE";
        private static readonly Vector2 DOWNSTAIRS_PHONE_POS = new Vector2(-2.476f, -0.205f);
        private static readonly Vector2 DOWNSTAIRS_PHONE_DIR = new Vector2(0, -1);
        private static readonly Vector2 START_POS = new Vector2(3.686f, 0);

        protected override void Awake() {
            base.Awake();
            phoneItem = GameObject.Find(GameObjects.PHONE_ZOOMED)?.GetComponent<UISceneItem>();
        }

        public override void initConversations () {
            base.initConversations ();
            phoneItem?.setInteractable(false);
            
            RootConversation =
                root (ROOT_ROOT_ID).options (
                    characterOption("Hello. This is the anonymous Drug Tip line.").options(
                        playerOption("Uhh, wrong number. Bye.", opt => {exitPhone();}).build(),
                        playerOption("Uhh, never mind. Bye.", opt => {exitPhone();}).build(),
                        option(
                            availableIf(delegate {
                                    return flagsSet(Flags.WEED_PLANTED);
                                })
                                .playerOption("Hello. I have tip for you.")
                                .build()
                        ).options(
                            characterOption("Yes?").options(
                                playerOption("Michael Fox of 1200 Banana Lane is selling marijuana|to the kids in the neighbourhood.|He keeps his stash hidden in the trunk of his car.").options(
                                    characterOption("Well thank you for this information sir, we will look in to the matter immediately.").options(
                                        option(
                                            optionValue().
                                                playerOption("Thank You. Bye.")
                                                .afterSay(delegate {
                                                    setFlag(Flags.DRUG_TIP);
                                                    SceneItemUtil.setItemActive ("PoliceCar", true);
                                                    GameState.Instance.StateData.changeCharacterScene (Characters.POLICE_MICHAEL, Scenes.MICKEY_GARDEN);
                                                    GameState.Instance.StateData.setCharacterActive(Characters.POLICE_MICHAEL, true);
                                                    GameState.Instance.StateData.setCharacterVisible(Characters.MICHAEL, true);
                                                    SceneItemUtil.setItemActive ("WeedBag", false);
                                                    SceneItemUtil.setItemActive ("MickeyCarOpen", true);
                                                    SceneItemUtil.setItemActive ("MickeyCarTrunkOpen", true);
                                                    SceneItemUtil.setItemActive ("MickeyCar", false);
                                                    SceneItemUtil.setItemActive ("MickeyCarTrunkUnlocked", false);
                                                    exitPhone();                                                    
                                                })
                                                .build()
                                        ).build()
                                    )
                                )
                            )
                        ),
                        option(
                            availableIf(delegate {
                                    return flagsNotSet(Flags.WEED_PLANTED, Flags.MICKEY_BUSTED);
                                })
                                .playerOption("Hello. I have tip for you.")
                                .build()
                        ).options(
                            characterOption("Yes?").options(
                                playerOption("Michael Fox of 1200 Banana Lane is selling drugs|to the kids in the neighbourhood.").options(
                                    characterOption("Ok. What kind of drugs are we talking about?").options(
                                        option(
                                            optionValue().
                                                playerOption("Uuh...|Never mind, maybe I was mistaken, bye!")
                                                .afterSay(delegate {
                                                    setFlag(Flags.FAILED_DRUG_TIP);
                                                    setFlag(Flags.FAILED_DRUG_TIP_REMEMBER);
                                                    exitPhone();
                                                })
                                                .build()
                                        ).build()
                                    )
                                )                            
                            )                        
                        )
                    )
                );
        }

        private void exitPhone() {
            playerMove.setPositionState(DOWNSTAIRS_PHONE_POS);
            playerMove.setDirectionState(DOWNSTAIRS_PHONE_DIR);
            
            GameState.Instance.StateData.setCharacterActive(Characters.POLICE, false);            
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