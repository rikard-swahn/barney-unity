using System;
using net.gubbi.goofy.item;
using net.gubbi.goofy.say.conversation;
using net.gubbi.goofy.scene.load;
using net.gubbi.goofy.scene.load.transition;
using net.gubbi.goofy.state.settings;
using net.gubbi.goofy.util;

namespace net.gubbi.goofy.character.npc.character.grandma.conversation {

    public class DonaldConversation : Conversation {

        private static readonly string CONVERSATION_ID = "DONNIE";
        private static readonly string CRUSIE_SHIP_TEXT_SCENE = "CruiseText";
        
        private Func<ConversationOption, bool> donaldTickets = delegate { return flagsSet(Flags.DONALD_TICKETS); };
        private Func<ConversationOption, bool> donaldNoTickets = delegate { return flagsNotSet(Flags.DONALD_TICKETS); };        

        public override void initConversations () {
            base.initConversations ();

            RootConversation =
                root (ROOT_ROOT_ID).options (
                    option(availableIf(donaldTickets).characterOption("Tickets for the Mystery Cruise!").build()).options(
                        playerOption("Yes, we leave in two weeks!").options(
                            characterOption("Oh Barney, thank you so much!|I don't know what to say!").options(
                                playerOption("You don't have to say anthing, I'm just glad you liked the gift.").options(
                                    option(
                                        optionValue()
                                            .characterOption("Of course I do! I can't wait. Thank you so much Barney!")
                                            .afterSay(delegate {
                                                SettingsState.Instance.setPartEnding();
                                                SceneLoader.Instance.changeInGameScene (CRUSIE_SHIP_TEXT_SCENE, Transitions.get(Transitions.Type.CIRCLE_OUT), Transitions.get(Transitions.Type.FADE_IN));                                                        
                                            })
                                            .build()
                                    ).build()
                                )
                            )
                        )
                    ),
                    option(availableIf(donaldNoTickets).playerOption("Hello Donnie!").build()).options(
                        option(
                            availableIf(delegate {
                                return flagsSet(Flags.MICKEY_BUSTED);
                            }).characterOption ("Oh hello Barney...").build()
                        ).options(
                            playerOption("I have to go. Bye.").build(),
                            playerOption("Why the sad face?").options(
                                characterOption("Didn't you hear? Michael has been arrested!|They say he sold drugs!").options(
                                    playerOption("Yes, who would have thought?").options(
                                        characterOption("You don't think he did it do you!?").options(
                                            playerOption("Well you never know...").options(
                                                characterOption("WHAT!? He would never do that!").options(
                                                    playerOption("No, you're probably right.").options(
                                                        getTicketOptions()
                                                    )
                                                )
                                            ),
                                            playerOption("Oh no, of course not.").options(
                                                getTicketOptions()
                                            )
                                        )
                                    )
                                )
                            )
                        ),
                        option(
                            availableIf(delegate {
                                return flagsNotSet(Flags.MICKEY_BUSTED);
                            }).characterOption("Oh hello Barney!|It's so good to see you!").build()
                        ).options(
                            playerOption("I have to go. Bye.").build(),
                            playerOption("Happy birthday!").options(
                                characterOption("Thank you Barney!|I'm so excited, Michael told me he got me something really special.|I wonder what it could be.").options(
                                    playerOption("That's nice. I have to go now, bye.").build(),
                                    playerOption("I also got you something really special.").options(
                                        characterOption("Oh, really? What is it!?").options(
                                            playerOption("Actually, I didn't bring it with me, I will be right back.|See you later.").build(),
                                            playerOption("It's... uuuh... I forgot it at home.|I will be right back!").build()
                                        )
                                    )
                                )
                            )
                        )
                    )
                );
        }

        private ConversationOption[] getTicketOptions() {
            return new[] {
                playerOption("I have to go. Bye.").build(),
                playerOption("Anyway, I got you something for your birthday.|I hope it can cheer you up a little.").options(
                    characterOption("Oh, really? What is it!?").options(
                        playerOption("Actually, I didn't bring it with me, I will be right back.|See you later.").build(),
                        option(
                            ifItems(ItemType.CRUISE_TICKETS)
                                .playerOption("I got us these tickets...")
                                .build()
                        ).options(
                            characterOption("Oh, what is this?", opt => {
                                behaviours.stop();
                                behaviours.startFrozen(BehaviourGroupType.DONALD_TICKETS);                                
                            }).build()                                                            
                        )
                    )
                )
            };
        }

        protected override string getConversationId () {
            return CONVERSATION_ID;
        }

    }

}