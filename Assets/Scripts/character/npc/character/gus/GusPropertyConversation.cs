using net.gubbi.goofy.say.conversation;

namespace net.gubbi.goofy.character.npc.character.grandma.conversation {

    public class GusPropertyConversation : Conversation {
		
        public override void initConversations () {
            base.initConversations ();

            RootConversation =
                root (ROOT_ROOT_ID).options (
                    playerOption("Never mind.").build(),                    
                    playerOption("Can I borrow your watering can please?").options(
                        characterOption("No, I need it to water the garden.").options(
                            playerOption("But I don't see much watering going on though?").options(
                                characterOption("I will. I just need to rest a little more first.").build()
                            )
                        )
                    ),
                    playerOption("What are you eating there Bobby?").options(
                        characterOption("Just some crackers.|I'm trying to rest here Barney.|Leave me alone.").build()
                    ),                    
                    playerOption("So how's it going Bobby?").options(
                        characterOption("I'm trying to rest here Barney.|Leave me alone.").build()
                    )
                );
        }
			
			
    }

}