using net.gubbi.goofy.character.npc.behaviour;
using net.gubbi.goofy.item;
using net.gubbi.goofy.say.conversation;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.character.bank {

    public class LadyConversation : Conversation {
		
        private static readonly string CONVERSATION_ID = "BANK_CUSTOMER";

        private CharacterBehaviours clerkBehaviours;

        protected override void Awake() {
            base.Awake();
            clerkBehaviours = GameObject.Find(Characters.BANK_CLERK)?.GetComponentInChildren<CharacterBehaviours>();
        }

        public sealed override void initConversations () {
            base.initConversations ();

            ItemConversations[ItemType.AD_FLYER] = root().options(createFlyerConversation());

            RootConversation = root (ROOT_ROOT_ID).options (
                playerOption("I'm sorry, but could you please hurry up a little?").options(
                    characterOption("Why, I never!|Who are you to tell me to hurry up?|Mind your own business!").options(
                            playerOption("Never mind.").build()
                        )
                    ),
                playerOption("May I ask what your business is here today miss?").options(
                        characterOption("Well, I don't know if it is any of your business|but I'm buying stamps.").options(
                            playerOption("It seems to be taking a while. Is there a problem?").options(
                                    characterOption("Of course not, why would there be?").options(
                                        playerOption("Never mind.").build()
                                        )
                                )
                            )
                    )
            );
        }

        private ConversationOption createFlyerConversation() {
            return playerOption("Excuse me miss, could this be of interest perhaps?")
                .options(
                    option(
                            optionValue()
                                .characterOption("What is this rubbish... let's see...")
                                .afterSay(delegate {
                                    clerkBehaviours.stop();
                                    behaviours.stop();
                                    behaviours.startFrozen(BehaviourGroupType.OLD_LADY_SHOES);
                                    clerkBehaviours.startFrozen(BehaviourGroupType.BANK_CLERK_END_STAMPS);
                                })
                                .build()
                        )
                        .build()
                );
        }

        override protected string getConversationId () {
            return CONVERSATION_ID;
        }

    }

}