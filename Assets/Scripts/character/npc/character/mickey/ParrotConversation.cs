using System.Collections.Generic;
using net.gubbi.goofy.item;
using net.gubbi.goofy.say.conversation;
using net.gubbi.goofy.scene.action;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.character.gyro {

    public class ParrotConversation : Conversation {

        public AudioClip squawkNeutral;
        public AudioClip squawkNegative;
        public SceneActionHandler crackerActionHandler;

        private static readonly string CONVERSATION_ID = "PARROT";
        private static readonly string HELLO_PLAYER_OPTIONS = "ROOT_PLAYER_OPTIONS";
        
        public override void initConversations () {
            base.initConversations ();
            
            itemConversations = new Dictionary<ItemType, ConversationOption>();
            
            ItemConversations[ItemType.BANK_CARD] =
                root().options(
                    playerOption(
                            delegate {
                                playerCharacterFacade.turnTowardsCamera();
                            },
                            "He would probably just chew it up."
                        )
                        .build()
                );            
            
            ItemConversations[ItemType.CRACKER] = root().options(createCrackerConversation());
        
            
            
            invalidItemConversation = root ().options (
                playerOption("Do you want {thisItem}?").options(                    
                    option(
                            optionValue().characterOption("Squawk...")
                                .beforeSay(delegate { sfxPlayer.playOnce(squawkNegative); }).build()
                        )
                        .options(
                            playerOption("I think that is a \"No\"").build()
                        )                    
                )
            );            

            RootConversation =
                root (ROOT_ROOT_ID).options (                    
                    playerOption("Goodbye").options(
                        parrotGoodbye()                                                
                    ),                                  
                    playerOption("Polly wants a cracker?")                         
                        .options(                                
                            option(
                                optionValue().characterOption("SQUAWK! CRACKER!|SQUAAWK!!")
                                    .beforeSay(delegate { sfxPlayer.playOnce(squawkNeutral); })
                                    .build()
                            ).options(     
                                option(
                                    availableIf(delegate { return flagsNotSet(Flags.PARROT_CRACKER); })
                                        .playerOption("Well, that sure got his attention.").build()
                                ).options(                                    
                                    ancestorOption(ROOT_ROOT_ID)
                                ),                                
                                option(
                                    availableIf(delegate { return flagsSet(Flags.PARROT_CRACKER); })
                                        .playerOption("Settle down. I don't have any more.").build()
                                ).options(                                    
                                    ancestorOption(ROOT_ROOT_ID)
                                )
                            )
                        ),                                          
                    playerOption("Hello").options(
                        option(
                            availableIf(delegate { return flagsNotSet(Flags.PARROT_CRACKER); })
                                .playerOption("He does not seem very talkative.")
                                .build()
                        ).options(ancestorOption(ROOT_ROOT_ID)),                            
                        option(availableIf(delegate { return flagsSet(Flags.PARROT_CRACKER); }).id(HELLO_PLAYER_OPTIONS)
                            .characterOption("SQUAWK! HELLO HELLO!").build()).options(
                            playerOption("Goodbye").options(
                                parrotGoodbye()
                            ),
                            option(availableIf(delegate { return flagsSet(Flags.SAFE_VISITED); }).playerOption("\"Safe code\"").build()).options(
                                characterOption("SafeHint").options(
                                    option(
                                        optionValue().characterOption("ParrotSquawkIdle")
                                            .beforeSay(delegate { sfxPlayer.playOnce(squawkNeutral); })
                                            .build()
                                    ).options(
                                        playerOption("Hmm.").options(
                                            ancestorOption(HELLO_PLAYER_OPTIONS)                                    
                                        )                                        
                                    )                                    

                                )
                            ),
                            playerOption("Can you say: \"BARNEY\"?").options(
                                characterOption("BARNEY!|BARNEY MORON!|SQUAWK!").options(
                                    playerOption("Never mind...").options(ancestorOption(HELLO_PLAYER_OPTIONS))
                                )
                            )                            
                        )
                    )                                                                                                                      
                );
        }

        private ConversationOption parrotGoodbye() {
            return option(optionValue().availableIf(delegate { return flagsSet(Flags.PARROT_CRACKER); })
                    .characterOption("SQUAWK! GOODBYE!").beforeSay(delegate { sfxPlayer.playOnce(squawkNeutral); })
                    .build())
                .build();
        }

        private ConversationOption createCrackerConversation () {
            return option(ifItems(ItemType.CRACKER).playerOption("Do you want {thisItem}?")
                .build()).options(
                option(optionValue().characterOption("SQUAWK! SQUAWK!").beforeSay(delegate { sfxPlayer.playOnce(squawkNeutral);  }).build())
                    .options(
                        playerOption("I think that is a \"Yes\".", opt => {                            
                            crackerActionHandler.doAction(ItemType.CRACKER);                            
                        }).build()
                    ));
        }        

        protected override string getConversationId () {
            return CONVERSATION_ID;
        }

    }

}