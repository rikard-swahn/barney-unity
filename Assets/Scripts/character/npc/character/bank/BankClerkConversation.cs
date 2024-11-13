using net.gubbi.goofy.item;
using net.gubbi.goofy.say.conversation;
using net.gubbi.goofy.util;

namespace net.gubbi.goofy.character.npc.character.bank {

    public class BankClerkConversation : Conversation {
		
        private static readonly string CONVERSATION_ID = "BANK_CLERK";
        private int CHECK_AMOUNT = 200;


        public sealed override void initConversations () {
            base.initConversations ();

            ItemConversations[ItemType.WELFARE_CHECK] = root().options(createCheckConversation());

            RootConversation = root (ROOT_ROOT_ID).options (
                option(
                    availableIf(delegate {
                            return characterActive(Characters.BANK_CUSTOMER);
                        })
                        .characterOption("Please wait for your turn sir. I am helping the lady.").build()
                ).build(),
                option(
                    availableIf(delegate {
                            return !characterActive(Characters.BANK_CUSTOMER);
                        })
                        .playerOption("I forgot why I came here.")
                        .build()).options(
                    characterOption("Ok... Have a nice day then.").build()
                ),
                option(
                    showOnceTransient().availableIf(delegate {
                            return !characterActive(Characters.BANK_CUSTOMER);
                        })
                        .playerOption("Hey, how's it going? ")
                        .build()).options(
                    characterOption("Excuse me sir, but can I help you with something?").options(
                        ancestorOption(ROOT_ROOT_ID)
                    )
                )
            );
        }

        private ConversationOption[] createCheckConversation() {

            return new[] {
                option(
                    availableIf(delegate { return characterActive(Characters.BANK_CUSTOMER); })
                        .playerOption("Hello. Can I just cash this check please?")
                        .build()
                ).options(
                    characterOption("Please wait for your turn sir. I am helping the lady.").build()
                ),
                
                option(
                    availableIf(delegate { return !characterActive(Characters.BANK_CUSTOMER); })
                        .playerOption("Hello. I would like to cash in this check please.")
                        .afterSay(delegate {
                            playerSetItemActive(Items.WELFARE_CHECK, true);
                            behaviours.startFrozen(BehaviourGroupType.BANK_CLERK_CHECK);
                        })                        
                        .build()
                ).options(
                    option(
                        optionValue()
                            .characterOption("Ok, let's see...")                        
                            .afterSay(delegate {
                                    Inventory.addItem(new CardItem(CHECK_AMOUNT));
                                    Inventory.removeItem(ItemType.WELFARE_CHECK);
                                }
                            )
                            .build()
                    ).build()                                        
                )                
                
                
            };
            
        }

        protected override string getConversationId () {
            return CONVERSATION_ID;
        }

    }

}