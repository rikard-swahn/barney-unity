using net.gubbi.goofy.say.conversation;

namespace net.gubbi.goofy.character.npc.character.grandma.conversation {

    public class MickeyMowConversation : Conversation {

        private static readonly string CONVERSATION_ID = "MICHAEL_MOW";

        public override void initConversations () {
            base.initConversations ();

            RootConversation =
                root (ROOT_ROOT_ID).options (
                    playerOption("I have to go now").build(),
                    playerOption("Didn't you mow the lawn yesterday?").options(
                        characterOption("It was three days ago, Barney.").options(
                            playerOption("Oh, really? Well then it is high time I guess.").options(
                                characterOption("Did you have something particular on your mind?").options(
                                    ancestorOption(ROOT_ROOT_ID)
                                )
                            )
                        )
                    ),
                    playerOption("About your gift to Donnie...").options(
                        characterOption("The Mystery Cruise? I can't wait!|What about it?").options(
                            playerOption("Never mind.").options(
                                ancestorOption(ROOT_ROOT_ID)
                            ),
                            playerOption("Can I have a look at the tickets real quick?").options(
                                characterOption("Hmm, why Barney?").options(
                                    playerOption("No, I just wanted to see how they look I guess.").options(
                                        characterOption("I don't have them on me. I keep them locked up in my safe.").options(
                                            playerOption("Ah, ok. Never mind then.|Bye.").options(
                                                ancestorOption(ROOT_ROOT_ID)
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