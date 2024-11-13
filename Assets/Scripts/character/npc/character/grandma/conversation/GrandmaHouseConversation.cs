using net.gubbi.goofy.say.conversation;
using net.gubbi.goofy.util;

namespace net.gubbi.goofy.character.npc.character.grandma.conversation {

    public class GrandmaHouseConversation : Conversation {

        private static readonly string CONVERSATION_ID = "GRANDMA_HOUSE";

        public override void initConversations () {
            base.initConversations ();	

            ConversationOption[] afterHelloOptions = getAfterHelloOptions();

            RootConversation = 
                root (ROOT_ROOT_ID).options (
                    playerOption("Hello Grandma!").options(
                        option(
                            availableIf(delegate {
                                return flagsSet(Flags.GRANDMA_ANGRY);
                            }).characterOption ("I have nothing to say to you.").build()						
                        ).build(),
                        option(
                            availableIf(delegate {
                                return flagsSet(Flags.GRANDMA_ABANDONED);
                            }).characterOption ("Hello Barney...|Why did you just leave me behind?").build()
                        ).options(
                            playerOption ("I'm sorry Grandma.|Something urgent came up.").options(
                                characterOption ("Oh, ok, I understand.|Don't worry about it.", opt => {
                                    removeFlag(Flags.GRANDMA_ABANDONED);
                                }).options(
                                    afterHelloOptions
                                )
                            )
                        ),
                        option(
                            availableIf(delegate {
                                return flagsNotSet (Flags.GRANDMA_ABANDONED, Flags.GRANDMA_ANGRY);
                            }).characterOption ("Oh hello Barney!").build()
                        ).options (
                            afterHelloOptions                           
                        )
                    )
                );
        }

        private ConversationOption[] getAfterHelloOptions() {
            return new [] {
                playerOption ("See you later.").options (
                    characterOption ("Ok, bye!").build ()
                ),    
                option(                                
                        availableIf(delegate { return flagsNotSet(Flags.GRANDMA_INVITED); }).playerOption("So what have you been up to lately?").build()                                
                    )                            
                    .options(                
                        characterOption("Oh, not much.|I'd love to get out of the house more and meet people.").options(
                            option (
                                availableIf (delegate {
                                    return flagsNotSet (Flags.GRANDMA_SUGGESTED_INVITE);
                                }).characterOption("Say, why don't you invite me over sometime?").afterSay(delegate { setFlag(Flags.GRANDMA_SUGGESTED_INVITE);}).build ()
                            ).options (
                                coffeeOptions()  
                            ),
                            option (
                                availableIf (delegate {
                                    return flagsSet(Flags.GRANDMA_SUGGESTED_INVITE);
                                }).characterOption("Say, why don't you invite me over again sometime?").build ()
                            ).options (
                                coffeeOptions()
                            )
                        )                
                
                    )
            };
        }

        private ConversationOption[] coffeeOptions() {
            return new[] {
                playerOption("Yeah... Look, I have to go.|But we can talk about this another time.").options(
                    characterOption("Oh, OK!").build()
                ),
                playerOption("Yes, I'll be sure to do that.").options(
                    characterOption("OK, GREAT!|I don't have any plans today, so right now is fine for me!").options(
                        playerOption("Oh, right now? I'm not sure if...").options(
                            characterOption("Yes, today!|See you soon!", opt => { setFlag(Flags.GRANDMA_INVITED); removeFlag (Flags.GRANDMA_COFFEE); }).build()
                        )
                    )
                )
            };
        }

        protected override string getConversationId () {
            return CONVERSATION_ID;
        }

    }

}