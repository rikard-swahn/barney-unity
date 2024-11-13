using net.gubbi.goofy.say.conversation;

namespace net.gubbi.goofy.character.npc.character.grandma.conversation {

    public class MickeyHouseConversation : Conversation {

        private static readonly string CONVERSATION_ID = "MICHAEL_HOUSE";
        
        //Option ids
        private static readonly string ROOT_CHILD = "ROOT.CHILD";

        public override void initConversations () {
            base.initConversations ();

            RootConversation =
                root (ROOT_ROOT_ID).options (
                    option(id(ROOT_CHILD).characterOption ("Yes?").build()).options(
                        playerOption("I have to go now").build(),
                        playerOption("About your gift to Donnie...").options(
                            characterOption("The Mystery Cruise? I can't wait!|What about it?").options(
                                playerOption("Never mind.").options(
                                    ancestorOption(ROOT_CHILD)
                                ),
                                playerOption("Can I have a look at the tickets real quick?").options(
                                    characterOption("Hmm, why Barney?").options(
                                        playerOption("No, I just wanted to see how they look I guess.").options(
                                            characterOption("I don't have them on me. I keep them locked up in my safe.").options(
                                                playerOption("Ah, ok. Never mind then.|Bye.").options(
                                                    ancestorOption(ROOT_CHILD)
                                                )
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