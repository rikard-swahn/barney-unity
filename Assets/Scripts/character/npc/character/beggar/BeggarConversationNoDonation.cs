using net.gubbi.goofy.say.conversation;
using net.gubbi.goofy.util;

namespace net.gubbi.goofy.character.npc.character.grandma.conversation {

    public class BeggarConversationNoDonation : Conversation {
        public override void initConversations () {
            base.initConversations ();

            RootConversation =
                root (ROOT_ROOT_ID).options (     
                    playerOption("Talk to you later.").build(),
                    option(
                        availableIf(delegate { return flagsSet(Flags.BEGGAR_STORY); })
                            .playerOption("Do you have any other hobbies than creating games?").build()
                    ).options(
                        characterOption("It's not a hobby, it's my life!").options(
                            playerOption("Oh, yeah, sorry.").options(
                                characterOption("But did write a book with movie ideas.|I have lot's of great ideas, let me tell you about them!").options(
                                    playerOption("Sorry, I don't really have time for that.").options(                                
                                        ancestorOption(ROOT_ROOT_ID)
                                    )                                                                
                                )
                            )  
                        )
                    ),                    
                    playerOption("How did you end up like this?").options(          
                        characterOption("beggarStoryNoDonation")
                            .options(
                                playerOption("Sorry, I've got better things to do.").options(
                                    characterOption("Well bless you anyway sir.").build()
                                ),                                
                                playerOption("Yeah, sure buddy. I'm on it.").options(
                                    characterOption("Thank you sir! It means a lot!").build()
                                )
                            )                                                                                                
                    )
                );
        }
    }

}