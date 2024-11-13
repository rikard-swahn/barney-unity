using net.gubbi.goofy.say.conversation;
using net.gubbi.goofy.util;

namespace net.gubbi.goofy.character.npc.character.grandma.conversation {

    public class DonaldIntroConversation : Conversation {

        private static readonly string CONVERSATION_ID = "DONALD_INTRO";

        public override void initConversations () {
            base.initConversations ();

            RootConversation =
                root (ROOT_ROOT_ID).options (
                    playerOption("You really have fixed up your garden really nice Donnie!").options(                            
                        characterOption("Thank you Barney!|And thank you for lending me all those tools|and all the good advice.").options(
                            playerOption("Oh don't mention it, just happy to help.|So... yes, it's your birthday tomorrow. How does it feel?").options(
                                characterOption("It feels good I guess. Time flies, I'm starting to get old!").options(
                                    playerOption("Oh, nonsense!|You look better than ever.").options(
                                        characterOption("Thank you!|Well... I better...|I have some things to take care of today, so talk to you tomorrow?").options(
                                            playerOption("Yeah, of course!|Well, bye then.").options(
                                                option(
                                                    optionValue().
                                                        characterOption("Bye Barney.")
                                                        .afterSay(delegate {
                                                            setFlag(Flags.AFTER_INTRO_DONALD);
                                                        })
                                                        .build()
                                                ).build()    
                                            )
                                        )                                            
                                    )
                                )
                            )
                        )
                    )
                    
                );
        }


        protected override string getConversationId () {
            return CONVERSATION_ID;
        }

    }

}