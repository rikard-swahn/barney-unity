using net.gubbi.goofy.item;
using net.gubbi.goofy.say.conversation;
using net.gubbi.goofy.scene.item;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.character.hobby {

    public class HobbyClerkConversation : Conversation {

        public AudioClip sfxPutCounter;
		
        private static readonly string CONVERSATION_ID = "HOBBY";
        //Option ids
        private static readonly string RECEIPT_MAIN = "RECEIPT.MAIN";
        private static readonly string PLANE_MAIN = "PLANE.MAIN";
        private static readonly string MODEM_MAIN = "MODEM.MAIN";

        private static readonly int PLANE_COST = 100;
        private static readonly int MODEM_COST = 250;

        public sealed override void initConversations () {
            base.initConversations ();

            ItemConversations[ItemType.MODEL_PLANE] = root().options(createPlaneConversation());				
            ItemConversations[ItemType.RECEIPT] = root().options(createReceiptConversation());
            ItemConversations[ItemType.MODEM] = root().options(createModemConversation());
            
            RootConversation = root (ROOT_ROOT_ID).options (
                createAlarmConversation(),
                playerOption ("I'll just continue browsing.").options (
                    characterOption ("Sure thing.").build ()
                ),
                createModemConversation (),
                createPlaneConversation (),
                option (showOnceTransient ().playerOption ("It's a nice store you have here.").build ()).options (
                    characterOption ("Thank you.").options (
                        characterOption ("So, what can I help you with today?").options (
                            ancestorOption (ROOT_ROOT_ID)
                        )

                    )
                )
            );
        }

        public override ConversationOption[] getOptionsToCheckLengthFor() {
            return new[]{RootConversation, ItemConversations[ItemType.MODEL_PLANE], ItemConversations[ItemType.RECEIPT], ItemConversations[ItemType.MODEM]};
        }

        private ConversationOption createPlaneConversation () {
            return option(
                ifItems(ItemType.MODEL_PLANE)
                    .playerOption ("About this model aircraft...")
                    .afterSayDelayed((opt, afterCallback) => {
                        sfxPlayer.playOnce(sfxPutCounter);
                        playerActivateItemDelayed(Items.MODEL_PLANE_COUNTER, afterCallback, true);
                    })
                    .build()).options (
                option(id(PLANE_MAIN).characterOption ("Yes?").build()).options (
                    option(
                            optionValue()
                                .playerOption ("Never mind.")
                                .afterSayDelayed((opt, afterCallback) => {                                    
                                    playerDeactivateItemDelayedAfterAnimation(Items.MODEL_PLANE_COUNTER, afterCallback, true);
                                })
                                .build()
                        )
                        .options(
                            ancestorOption(ROOT_ROOT_ID)
                        ),
                    playerOption ("I'd like to buy it.").options (
                        characterOption ("Ok! That will be ${0}", PLANE_COST).options (
                            playerOption ("Oh, I don't have that much.", 
                                    (opt, afterCallback) => {
                                        playerDeactivateItemDelayedAfterAnimation(Items.MODEL_PLANE_COUNTER, afterCallback, true);
                                    })
                                .build()                                                           
                        )
                    ),
                    option (showOnceTransient ().playerOption ("How much is it?").build ()).options (
                        characterOption ("It's ${0}", PLANE_COST).options (
                            ancestorOption (PLANE_MAIN)
                        )                            
                    ),
                    playerOption ("I'd like to return it.").options (
                        characterOption ("Do you have your receipt with you?").options (
                            option (                                    
                                    ifItems (ItemType.RECEIPT)
                                        .playerOption ("Yes, right here.")
                                        .afterSay(delegate {
                                            standardPlayerAction();
                                            returnPlane();
                                        })
                                        .build ())
                                .build(),
                            playerOption ("No. I think I forgot it at home.").options (
                                characterOption ("You better go home and get it then", (opt, afterCallback) => {
                                        playerDeactivateItemDelayedAfterAnimation(Items.MODEL_PLANE_COUNTER, afterCallback, true);
                                    })
                                    .build()                                                             
                            ),
                            playerOption ("No, I think I might have lost it.|Is it really necessary?").options (
                                characterOption ("Yes, I'm afraid it is...|Please come back when you have it.", (opt, afterCallback) => {
                                        playerDeactivateItemDelayedAfterAnimation(Items.MODEL_PLANE_COUNTER, afterCallback, true);
                                    })
                                    .build()
                            )
                        )
                    )                         
                )                               
            );
        }

        private ConversationOption createAlarmConversation() {
            return option(
                    availableIf(delegate { return flagsSet(Flags.HOBBY_ALARM); })
                        .characterOption("Are you trying to leave without paying for something?")
                        .afterSay(delegate { removeFlag(Flags.HOBBY_ALARM);})
                        .build()
                )
                .options(
                    playerOption("Heavens, no. I wasn't leaving.|I just walked too close to the exit.").options(
                        option(
                            optionValue()
                                .characterOption("Just give it to me.")
                                .afterSayDelayed((opt, afterCallback) => {

                                    if (Inventory.hasItem(ItemType.MODEL_PLANE)) {
                                        Inventory.removeItem(ItemType.MODEL_PLANE);
                                        GameState.Instance.StateData.setSceneProperty(Items.MODEL_PLANE, SceneItemProperties.STOLEN, true);
                                        sfxPlayer.playOnce(sfxPutCounter);
                                        playerActivateItemDelayed(Items.MODEL_PLANE_COUNTER, afterCallback, true);
                                    }
                                    else {
                                        Inventory.removeItem(ItemType.MODEM);
                                        GameState.Instance.StateData.setSceneProperty (Items.MODEM, SceneItemProperties.STOLEN, true);
                                        sfxPlayer.playOnce(sfxPutCounter);
                                        playerActivateItemDelayed(Items.MODEM_COUNTER, afterCallback, true);
                                    }

                                    
                                    behaviours.startFrozen(BehaviourGroupType.HOBBY_CLERK_RETURN_STOLEN);
                                    
                                })
                                .build()
                        ).build()
                    )
                );
        }

        private ConversationOption createModemConversation () {
            return option(
                ifItems(ItemType.MODEM)
                    .playerOption ("About this modem...")
                    .afterSayDelayed((opt, afterCallback) => {
                        sfxPlayer.playOnce(sfxPutCounter);
                        playerActivateItemDelayed(Items.MODEM_COUNTER, afterCallback, true);
                    })
                    .build()).options (
                option(id(MODEM_MAIN).characterOption ("Yes...?").build()).options (                    
                    playerOption ("Never mind.", (opt, afterCallback) => {                                   
                            playerDeactivateItemDelayedAfterAnimation(Items.MODEM_COUNTER, afterCallback, true);
                        })
                        .options(
                            ancestorOption(ROOT_ROOT_ID)
                        ),          
                    
                    option(
                        showOnceTransient()
                            .playerOption("Tell me more about it.")
                            .build()
                    ).options(
                        characterOption("Sure!|This is a state of the art 3200 baud, 28.8k modem based on the V.34 standard...").options(
                            playerOption("Ok ok, that is great and all, but will I be able to read my emails with it?").options(
                                characterOption("Uh, yes, sure.").options(
                                    playerOption("Great!", (opt, afterCallback) => { 
                                            playerDeactivateItemDelayedAfterAnimation(Items.MODEM_COUNTER, afterCallback, true);
                                        })                                    
                                        .build ()
                                )
                            )
                        )
                    ),
                    option(
                        availableIf(delegate {
                            return !Inventory.hasItemWithFlag(ItemType.MODEM, ItemProperties.PURCHASED);
                        }).playerOption ("I'd like to buy it.").build()).options (
                        characterOption ("Ok! That will be ${0}", MODEM_COST).options (
                            option (
                                ifCash (MODEM_COST)
                                    .playerOption ("Ok! Here you go.")
                                    .afterSay (delegate {
                                        standardPlayerAction();
                                        Inventory.addItem (new CardItem (-MODEM_COST));
                                        Inventory.setItemFlag(ItemType.MODEM, ItemProperties.PURCHASED, true);
                                    })
                                    .build ()
                            ).options(
                                characterOption("Thank you.", (opt, afterCallback) => { 
                                        playerDeactivateItemDelayedAfterAnimation(Items.MODEM_COUNTER, afterCallback, true);
                                    }
                                ).build()
                            ),
                            option (
                                ifCash (MODEM_COST)
                                    .playerOption ("Oh, that is a bit expensive.|I'll have to think about it some more.")
                                    .afterSayDelayed((opt, afterCallback) => { 
                                        playerDeactivateItemDelayedAfterAnimation(Items.MODEM_COUNTER, afterCallback, true);
                                    })                                    
                                    .build ()
                            ).build (),
                            option (
                                ifNotCash (MODEM_COST)
                                    .playerOption ("Oh, I don't have that much.")
                                    .afterSayDelayed((opt, afterCallback) => {
                                        playerDeactivateItemDelayedAfterAnimation(Items.MODEM_COUNTER, afterCallback, true);
                                    })                                     
                                    .build ()
                            ).build ()
                        )
                    ),
                    option (
                        showOnceTransient ().availableIf(delegate {
                            return !Inventory.hasItemWithFlag(ItemType.MODEM, ItemProperties.PURCHASED);
                        }).playerOption ("How much is it?").build ()).options (
                        characterOption ("It's ${0}", MODEM_COST).options (
                            ancestorOption (MODEM_MAIN)
                        )
                    ),
                    playerOption ("I'd like to return it.").options (
                        characterOption ("Do you have your receipt with you?").options (
                            option(
                                availableIf(delegate {
                                    return !Inventory.hasItemWithFlag(ItemType.MODEM, ItemProperties.PURCHASED);
                                }).playerOption ("No. I think I forgot it at home.").build()
                            ).options(
                                characterOption ("You better go home and get it then", (opt, afterCallback) => {
                                        playerDeactivateItemDelayedAfterAnimation(Items.MODEM_COUNTER, afterCallback, true);
                                    })
                                    .build()
                            ),
                            option(
                                availableIf(delegate {
                                    return !Inventory.hasItemWithFlag(ItemType.MODEM, ItemProperties.PURCHASED);
                                }).playerOption ("No, I think I might have lost it.|Is it really necessary?").build()
                            ).options(
                                characterOption ("Yes, I'm afraid it is...|Please come back when you have it.", (opt, afterCallback) => {
                                        playerDeactivateItemDelayedAfterAnimation(Items.MODEM_COUNTER, afterCallback, true);
                                    })
                                    .build()
                            ),
                            option(
                                availableIf(delegate {
                                    return Inventory.hasItemWithFlag(ItemType.MODEM, ItemProperties.PURCHASED);
                                }).playerOption ("I never got one!").build()
                            ).options(
                                characterOption ("Sorry, then I can't help you.", (opt, afterCallback) => {
                                        playerDeactivateItemDelayedAfterAnimation(Items.MODEM_COUNTER, afterCallback, true);
                                    })
                                    .build()
                            )
                        )
                    )
                )
            );
        }

        private ConversationOption createReceiptConversation () {
            return playerOption("I have this receipt for a model aircraft...", opt => { standardPlayerAction(); })
                .options(
                    option(id(RECEIPT_MAIN).characterOption("Yes...?").build()).options(
                        playerOption("Never mind.").build(),
                        option(showOnceTransient().playerOption("Do you want it?").build()).options(
                            getCharacterOptionsFromText(I18nUtil.options("CharacterDecline"), ancestorOption(RECEIPT_MAIN))
                        ),
                        playerOption("Well, I'd like to return it.").options(
                            characterOption("Oh, let's see...|Do you have it with you?").options(
                                option(ifItems(ItemType.MODEL_PLANE)
                                        .playerOption("Yes, right here!")
                                        .afterSayDelayed((opt, afterCallback) => {
                                                sfxPlayer.playOnce(sfxPutCounter);
                                                playerActivateItemDelayed(Items.MODEL_PLANE_COUNTER, afterCallback, true);
                                                returnPlane();
                                            }
                                        )
                                        .build())
                                    .build(),
                                playerOption("Hmm... I think I forgot it at home actually.").options(
                                    characterOption("You better go home and get it then").build()
                                )
                            )
                        )

                    )
                );
        }

        private void returnPlane() {
            Inventory.addItem(new CardItem(PLANE_COST));
            Inventory.removeItem(ItemType.MODEL_PLANE);
            Inventory.removeItem(ItemType.RECEIPT);
            SceneItemUtil.setItemActive(Items.SOCIAL_SEC_FORM, true);
            SceneItemUtil.setItemActive(Items.PLAYER_MAILBOX_CLOSED, true);
            SceneItemUtil.setItemActive(Items.PLAYER_MAILBOX_OPEN, false);
            
            behaviours.startFrozen(BehaviourGroupType.HOBBY_CLERK_RETURN_PLANE);
        }

        protected override string getConversationId () {
            return CONVERSATION_ID;
        }

    }

}