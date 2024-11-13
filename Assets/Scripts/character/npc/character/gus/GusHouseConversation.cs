using net.gubbi.goofy.say.conversation;

namespace net.gubbi.goofy.character.npc.character.grandma.conversation {

    public class GusHouseConversation : Conversation {
		
        public override void initConversations () {
            base.initConversations ();

            RootConversation =
                root (ROOT_ROOT_ID).options (
                    characterOption("I don't have time to talk now Barney!|We're having lunch.. I think.|Where is Grandma?").build()
                );
        }
			
			
    }

}