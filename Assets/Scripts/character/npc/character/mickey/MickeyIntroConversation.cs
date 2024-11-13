using net.gubbi.goofy.say.conversation;
using net.gubbi.goofy.util;

namespace net.gubbi.goofy.character.npc.character.mickey {

    public class MickeyIntroConversation : Conversation {

        private static readonly string CONVERSATION_ID = "MICHAEL_INTRO";

        public override void initConversations () {
            base.initConversations ();

            RootConversation =
                root (ROOT_ROOT_ID).options (
                    playerOption("Yes, hello?").options(
                        characterOption("Hello Barney, this is Michael.").options(
                            playerOption("Oh hello. Everything fine with you?").options(
                                characterOption("Yes, sure sure.|So, tomorrow is Donalds birthday!|What have you bought him?").options(
                                    playerOption("Uh... I am still working on that.").options(                            
                                        characterOption("I guess your budget is a bit tight, being unemployed and all?").options(
                                            playerOption("No, no problem, I just haven't decided what to get yet... What did you get?").options(
                                                characterOption("I bought two tickets for the Mystery Cruise. For him and me.").options(
                                                    playerOption("Oh? Just the two of you?").options(
                                                        characterOption("Yeah. I will be so nice to get some time alone with my best friend.|He is so cute.").options(
                                                            playerOption("Look, I have to go now.").options(
                                                                option(
                                                                    optionValue().
                                                                        characterOption("Oh, OK. Bye then")
                                                                        .afterSay(delegate {
                                                                            setFlag(Flags.AFTER_INTRO_PHONE);
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