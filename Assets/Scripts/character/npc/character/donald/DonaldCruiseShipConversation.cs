using System;
using net.gubbi.goofy.say.conversation;
using net.gubbi.goofy.scene.load;
using net.gubbi.goofy.scene.load.transition;

namespace net.gubbi.goofy.character.npc.character.grandma.conversation {

    public class DonaldCruiseShipConversation : Conversation {

        private static readonly string CONVERSATION_ID = "DONALD_CRUISE_SHIP";
        private static readonly string CRUSIE_SCENE = "Cruise";

        public override void initConversations () {
            base.initConversations ();

            RootConversation =
                root(ROOT_ROOT_ID).options(
                    playerOption("Barney: I can't believe we are finally here!").options(
                        option(
                            optionValue()
                                .characterOption("Donnie: I know, it's just starting to dawn on me...")
                                .afterSay(delegate {                                    
                                    SceneLoader.Instance.changeInGameScene (CRUSIE_SCENE, Transitions.get(Transitions.Type.CIRCLE_OUT_NO_MUSIC_FADE), Transitions.get(Transitions.Type.CIRCLE_IN));
                                })
                                .build()
                        ).build()
                    )
                );
        
        }

        protected override string getConversationId () {
            return CONVERSATION_ID;
        }

    }

}