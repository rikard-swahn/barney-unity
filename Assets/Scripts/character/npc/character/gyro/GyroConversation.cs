using System.Linq;
using net.gubbi.goofy.item;
using net.gubbi.goofy.say.conversation;
using net.gubbi.goofy.scene.item;
using net.gubbi.goofy.state;
using net.gubbi.goofy.ui.goals;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.character.gyro {

    public class GyroConversation : Conversation {

        private static readonly string CONVERSATION_ID = "ARTHUR";
        private GoalsLightbox goalsLightbox;
        private static readonly string FRAME_MICHAEL_GOAL = "FrameMickey";

        protected override void Awake() {
            base.Awake();
            goalsLightbox = GameObject.Find (GameObjects.GOALS_LIGHTBOX).GetComponent<GoalsLightbox>();
        }

        public override void initConversations () {
            base.initConversations ();

            ItemConversations[ItemType.TURD] = root().options(createTurdConversation());

            ItemConversations[ItemType.COFFEE_POT] =
                root().options(
                    playerOption(
                            delegate {
                                playerCharacterFacade.turnTowardsCamera();
                            },
                            "He looks like he's had enough for today."
                        )
                        .build()
                );

            RootConversation =
                root (ROOT_ROOT_ID).options (
                    playerOption("See you later.").build(),
                    option(
                            availableIf(delegate {
                                    return sceneItemFlag("Robot", SceneItemProperties.BROKEN) && flagsNotSet(Flags.GYRO_WORKING_FERTILIZER);
                                })
                                .playerOption("Why the sad face Gryo?")
                                .build()
                        )
                        .options(
                            characterOption("Well, my robot broke. It's 6 months of work down the drain.")
                                .options(
                                    playerOption("Yes, right. I'm sorry about.. FOR that.")
                                        .build()
                                )
                        )
                    ,
                    option (
                            showOnceTransient()
                                .availableIf (o => {
                                    return flagsSet(Flags.GYRO_WORKING_FERTILIZER);
                                })
                                .playerOption("How's it going with the fertilizer?")
                                .build()
                        )
                        .options(
                            characterOption("Well, I still have not found the missing component.")
                                .options(
                                    characterOption("As I said, I am missing some kind of organic base.|Some natural organic fertilizer, but I need something \"extra\" to it.")
                                        .options(
                                            playerOption("Ok, I'm on it!|See you later.").build()
                                        )
                                )
                        ),
                    option (
                            showOnceTransient()
                                .availableIf(delegate {
                                        return !sceneItemFlag("Robot", SceneItemProperties.BROKEN);
                                    }
                                )
                                .playerOption("Say, what are you working on there Arthur?")
                                .build ()
                        )
                        .options(
                            characterOption("A butler robot.")
                                .options(
                                    playerOption("Oh ok. I would like to talk about something else...")
                                        .options(
                                            ancestorOption(ROOT_ROOT_ID)
                                        ),
                                    playerOption("Why do you need a butler robot?")
                                        .options(
                                            characterOption("So I have more time to work on my projects.")
                                                .options(
                                                    playerOption("What projects?|More butler robots?").options(
                                                        characterOption("Don't be silly now Barney.|Do you have anything else you wanted to talk about?").options(
                                                            ancestorOption(ROOT_ROOT_ID)
                                                        )
                                                    )
                                                )
                                        )
                                )
                        ),
                    playerOption("I would like your advice on something")
                        .options(
                            characterOption("Yes?")
                                .options(
                                    playerOption("I'm good for now.")
                                        .options(
                                            ancestorOption(ROOT_ROOT_ID)
                                        ),
                                    option(
                                        availableIf(delegate {
                                                return flagsSet(Flags.FAILED_DRUG_TIP_REMEMBER) && flagsNotSet(Flags.WEED_HARVESTED);
                                            })
                                            .playerOption("Hypothetically speaking, if someone wanted to acquire illegal drugs, what would be the easiest way to do that?")
                                            .build()
                                    ).options(
                                        characterOption("*Sigh*|Why are you asking me this?").options(                                           
                                            playerOption("I'm just trying to educate myself and understand more about our society.").options(                                                    
                                                characterOption("Oh. Well, I guess people buy it on the Internet these days.").options(
                                                    playerOption("Yes, right. Thank you Arthur.").options(                                                                
                                                        ancestorOption(ROOT_ROOT_ID)
                                                    )                                                        
                                                )
                                            )
                                        )
                                        
                                    ),
                                    option(
                                        availableIf(delegate {
                                                return flagsNotSet(Flags.MICKEY_BUSTED);
                                            })
                                            .playerOption("If I wanted to get someone out of the way...|How would I go about doing that?|Hypothetically speaking, that is.")
                                            .build()
                                    ).options(
                                        characterOption("Hmm, what do you mean by 'Out of the way' Barney?").options(
                                            playerOption("I mean, just to make them disappear.").options(
                                                characterOption("Are you talking about murder Barney?").options(
                                                    playerOption("No! No, not necessarily, just make them go away for a while.").options(
                                                        characterOption("Do you mean like framing someone for a crime and putting them in prison?").options(
                                                            option(
                                                                optionValue()
                                                                    .afterSay(delegate {
                                                                        UnityUtil.endOfFrameCallback(this, delegate {
                                                                            if (!GameState.Instance.StateData.Goals.ElementAt(0).text.Equals(FRAME_MICHAEL_GOAL)) {
                                                                                GameState.Instance.StateData.setGoal(0, FRAME_MICHAEL_GOAL);
                                                                                goalsLightbox.show();                                                                            
                                                                            }                                                                            
                                                                        });                                                                           
                                                                    })
                                                                    .playerOption("Hmm, that's not a bad idea...|Thank you Arthur.")
                                                                    .build()
                                                            ).build()                                                    
                                                        )                                                    
                                                    )
                                                )
                                            )
                                        )
                                    ),
                                    option(
                                            availableIf (o => {
                                                    return flagsSet(Flags.SEED_PLANTED) && flagsNotSet(Flags.GYRO_WORKING_FERTILIZER, Flags.GYRO_FERTILIZER_DONE);
                                                })
                                                .playerOption("I planted something, and it won't grow.")
                                                .build()
                                        )
                                        .options(
                                            characterOption("Have you watered it?").options(
                                                option(
                                                        availableIf(
                                                                delegate {
                                                                    return GameState.Instance.StateData.hasSceneProperty(Items.PLANTED_SEED, SceneItemProperties.WATERED);
                                                                }
                                                            )
                                                            .playerOption("YesCasual")
                                                            .build()
                                                    )
                                                    .options(
                                                            
                                                        option(availableIf(
                                                                    delegate {
                                                                        return sceneItemFlag("Robot", SceneItemProperties.BROKEN);
                                                                    })
                                                                .characterOption("Well... I might have something for you|I have been working on a Super-fertilizer...|... and now I guess I have time to finish it|since my Robot is destroyed.")
                                                                .build()
                                                            )
                                                            .options(
                                                                playerOption("Great! When can you have it finished?")
                                                                    .options(
                                                                        characterOption("Well, I am missing one component.")
                                                                            .options(
                                                                                playerOption("And what is that?")
                                                                                    .options(
                                                                                        option(
                                                                                                optionValue()
                                                                                                    .characterOption("I am missing some kind of organic base for my formula to be complete.|Some natural organic fertilizer, but I need something \"extra\" to it.")
                                                                                                    .afterSay(delegate {
                                                                                                            setFlag(Flags.GYRO_WORKING_FERTILIZER);
                                                                                                        }
                                                                                                    )
                                                                                                    .build()
                                                                                            )
                                                                                            .options(
                                                                                                playerOption("Ok, I'm on it!|See you later.").build()
                                                                                            )
                                                                                    )
                                                                            )
                                                                    )
                                                            ),                                                            
                                                            
                                                        option(availableIf(
                                                                    delegate {
                                                                        return !sceneItemFlag("Robot", SceneItemProperties.BROKEN);
                                                                    })
                                                                .characterOption("And still nothing?")
                                                                .build()
                                                            )                                                                                                                                                                        
                                                            .options(
                                                                playerOption("It just grows so slow.")
                                                                    .options(
                                                                        characterOption("These things take time Barney.|Give it a few weeks.")
                                                                            .options(
                                                                                playerOption("What!?|That is too long.")
                                                                                    .options(
                                                                                        characterOption("Well... I might have something for you|I have been working on a Super-fertilizer...|...but I don't have time to finish it now.|I need to finish this butler robot first.|I need to get back to work now.")
                                                                                            .build()
                                                                                    )
                                                                            )
                                                                    )
                                                            )
                                                    ),
                                                option(
                                                        availableIf(
                                                                delegate {
                                                                    return !GameState.Instance.StateData.hasSceneProperty(Items.PLANTED_SEED, SceneItemProperties.WATERED);
                                                                }
                                                            )
                                                            .playerOption("No")
                                                            .build()
                                                    )
                                                    .options(
                                                        characterOption("Well, you better do that then.|I need to get back to work now.")
                                                            .build()
                                                    ),
                                                option(
                                                        availableIf(
                                                                delegate {
                                                                    return !GameState.Instance.StateData.hasSceneProperty(Items.PLANTED_SEED, SceneItemProperties.WATERED);
                                                                }
                                                            )
                                                            .playerOption("Do I need to?")
                                                            .build()
                                                    )
                                                    .options(
                                                        characterOption("Yes Barney... yes you do.|I need to get back to work now.")
                                                            .build()
                                                    )
                                            )
                                        )
                                )
                        )
                );
        }

        private ConversationOption[] createTurdConversation() {
            return new[] {
                option(
                        availableIf(delegate {
                                return flagsSet(Flags.GYRO_WORKING_FERTILIZER);
                            })
                            .playerOption("Hey, I got something here which might be|the component you are missing for your super fertilizer.")
                            .build()
                    )
                    .options(
                        option(
                            availableIf(
                                    delegate {
                                        return flagsNotSet(Flags.GYRO_FERTILIZER_TABLE_REACHED);
                                    }
                                )
                                .characterOption("Let me just get to my lab table first and I will check it out.")
                                .build()
                        ).build(),        
                        
                        option(
                            availableIf(
                                    delegate {
                                        return flagsSet(Flags.GYRO_FERTILIZER_TABLE_REACHED);
                                    }
                                )
                                .characterOption("Sweet Christmas. Where did you get that?|And why are you holding it in your hand?")
                                .build()
                        ).options(
                            playerOption("Do you want it or not?")
                                .options(
                                    option(
                                            optionValue()
                                                .characterOption("*Sigh*|I guess it might work. Let me give it a try...")
                                                .afterSay(delegate {
                                                        behaviours.setNewBehaviour(BehaviourGroupType.GYRO_COMPLETE_FERTILIZER);
                                                    }
                                                )
                                                .build()
                                        )
                                        .build()
                                )
                        )
                    ),
                option(
                        availableIf(delegate {
                                return flagsNotSet(Flags.GYRO_WORKING_FERTILIZER);
                            })
                            .playerOption("Do you want {thisItem}?")
                            .build()
                    )
                    .options(
                        getCharacterOptionsFromText(I18nUtil.options("CharacterDeclineStrong"))
                    )


            };
        }

        protected override string getConversationId () {
            return CONVERSATION_ID;
        }

    }

}