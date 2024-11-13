using System;
using Assets.Scripts.character.npc.behaviour;
using net.gubbi.goofy.item;
using net.gubbi.goofy.say.conversation;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;

namespace net.gubbi.goofy.character.npc.character.grandma.conversation {

    public class GrandmaGoofyHouseConversation : Conversation {

        private static readonly string CONVERSATION_ID = "GRANDMA_BARNEY_HOUSE";
        
        private Func<ConversationOption, bool> grandmaFlirt = delegate { return flagsSet(Flags.GRANDMA_FLIRT); };
        private Func<ConversationOption, bool> grandmaNoFlirt = delegate { return flagsNotSet(Flags.GRANDMA_FLIRT); };
        
        public override void initConversations () {
            base.initConversations ();
            
            RootConversation =
                root (ROOT_ROOT_ID).options (
                    option(availableIf(grandmaNoFlirt).playerOption ("Never mind.").build()).build(),
                    option(availableIf(grandmaNoFlirt).playerOption("So how's it going, grandma?").build()).options(
                        grandmaStateResponses()
                    ),
                    option(availableIf(grandmaNoFlirt).playerOption("How are you grandma?").build()).options(
                        grandmaStateResponses()
                    ),                                                            
                    option(availableIf(grandmaFlirt).playerOption("No!").build()).build(),
                    option(availableIf(grandmaFlirt).playerOption("What!?").build()).build(),
                    option(availableIf(grandmaFlirt).playerOption("What?").build()).build()
                );

            ItemConversations[ItemType.COFFEE_POT] = root().options(createCoffeeConversation());
        }

        private ConversationOption[] grandmaStateResponses() {

            return new[] {
                option (
                    availableIf (delegate {
                            return !grandmaSitting();
                        })
                        .characterOption("Please Barney, would you let me sit down first?").build()
                ).build(),
                option (
                    availableIf (delegate {
                            return grandmaSitting() && flagsNotSet(Flags.GRANDMA_COFFEE);
                        })
                        .characterOption("Oh Barney, I'm just so tired. Let me rest a while.").build()
                ).build(),
                option (
                    availableIf (delegate {
                            return grandmaSitting() && flagsSet (Flags.GRANDMA_COFFEE);
                        })
                        .characterOption("Oh it's going good Barney, I guess...").build()
                ).options(                    
                    playerOption("Is anything bothering you?").options(
                        characterOption("No...|Well, it can get a bit lonely out on the farm sometimes.|So it's nice of you inviting me over.").options(
                            playerOption("Yes, sure! It's nice to have you over!").options(                                
                                characterOption("It makes me so glad to hear that!", opt => {
                                    GameState.Instance.StateData.setFlags (Flags.GRANDMA_FLIRT);
                                    
                                    behaviours.stop();
                                    behaviours.startFrozen(BehaviourGroupType.GRANDMA_FLIRT);                                                                                        
                                }).build()
                            ),
                            playerOption("Yes, no problem.").build()
                        )
                    ),
                    playerOption("Ok, that's good to hear.").build()
                )
            };
        }
        

        private ConversationOption[] createCoffeeConversation () {
            return new[] {                
                option(
                        optionValue()
                            .availableIf(delegate { return !grandmaSitting();})
                            .playerOption("Would you like some coffee grandma?")
                            .build()                                    
                    )
                    .options(
                        characterOption("Please Barney, would you let me sit down first?").build()
                    )
                ,
                option(
                        optionValue()
                            .availableIf(delegate { return grandmaSitting() && flagsNotSet(Flags.GRANDMA_COFFEE);})
                            .playerOption("Here, have some coffee grandma.")
                            .afterSay(delegate {
                                playerSetItemActive(GameObjects.COFFEE_CUP, true);
                            })
                            .build()
                    )
                    .options(
                        playerOption("I hope you like it.")
                            .options(
                                option(
                                    availableIf(delegate {
                                            return flagsNotSet(Flags.GRANDMA_COFFEE) &&
                                                   Inventory.getItem<CoffeePotItem>(ItemType.COFFEE_POT).isCold();
                                        })
                                        .characterOption("Thank you Barney!")
                                        .afterSay(delegate {
                                            behaviours.stop();
                                            behaviours.startFrozen(BehaviourGroupType.GRANDMA_COFFE_COLD);                                                                                        
                                        })
                                        .build()
                                ).build(),
                                option(
                                    availableIf(delegate {
                                            return flagsNotSet(Flags.GRANDMA_COFFEE) &&
                                                   !Inventory.getItem<CoffeePotItem>(ItemType.COFFEE_POT).isCold();
                                        })
                                        .characterOption("Thank you Barney!")
                                        .afterSay(delegate {
                                            GameState.Instance.StateData.setCharacterAnimationBoolParams(
                                                Characters.GRANDMA_DUCK, AnimationParams.COFFEE);
                                            setAnimatorFlag(AnimationParams.COFFEE, true);
                                            setAnimatorTrigger(AnimationParams.DRINK_ONCE);
                                            GameState.Instance.StateData.setFlags(Flags.GRANDMA_COFFEE);
                                        })
                                        .build()
                                ).build()
                            )

                    ),
                option(
                        optionValue()
                            .availableIf(delegate { return flagsSet(Flags.GRANDMA_COFFEE);})
                            .playerOption("Would you like some more coffee grandma?")
                            .build()                                    
                    )
                    .options(
                        characterOption("Thank you Barney, but I don't need a refill.").build()                        
                    )                
                
            };
        }

        private bool grandmaSitting() {
            return BodyState.SITTING == characterMove.getBodyState();
        }

        protected override string getConversationId () {
            return CONVERSATION_ID;
        }

    }

}