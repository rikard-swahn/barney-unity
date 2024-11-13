using System;
using System.Collections.Generic;
using System.Linq;
using Mgl;
using net.gubbi.goofy.iap;
using net.gubbi.goofy.say.conversation;
using net.gubbi.goofy.state;
using net.gubbi.goofy.state.settings;
using net.gubbi.goofy.unity;
using net.gubbi.goofy.util;
using UnityEngine;
using Product = UnityEngine.Purchasing.Product;

namespace net.gubbi.goofy.character.npc.character.grandma.conversation {

    public class BeggarConversation : Conversation {
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
                        characterOption("beggarStory",
                                opt => {
                                    GameState.Instance.StateData.setFlags (Flags.BEGGAR_STORY);
                                })
                            .options(
                                playerOption("I'm sorry, maybe another time.").options(
                                    characterOption("Well bless you anyway sir.").options(
                                        ancestorOption(ROOT_ROOT_ID)
                                    )
                                ),
                                playerOption("Oh, alright!").options(
                                    donate()
                                )                            
                            )                                                                                                
                    ),
                    option(
                        availableIf(delegate { return flagsSet(Flags.BEGGAR_STORY); }).playerOption("Hey, I would like to help you out.").build()
                    ).options(
                        donate()
                    )
                    
                );
        }
        
        private ConversationOption donate() {
            var donateBuilder = characterOption("donateStart1");

            return donateBuilder.optionsCallback(
                getDynamicDonationOptions
            ).build();
        }

        private ConversationOption[] getDynamicDonationOptions() {            
            if (!Purchaser.Instance.InitComplete) {                
                AnalyticsFacade.Instance.CustomEvent("Gubbi_Purchase_Error_Init_NotInited");                
                
                return new[] {
                    characterOption(I18n.Instance.__("PurchaseError-Init-NotInited")).transient()
                        .options(
                            ancestorOption(ROOT_ROOT_ID, true)
                        )
                };
            }
            
            List<Product> products = DonationFacade.Instance.getDonationProducts();                        
            products = products.OrderBy(p => p.metadata.localizedPrice).Reverse().ToList();

            if (!Purchaser.Instance.initOk()) {
                AnalyticsFacade.Instance.CustomEvent("Gubbi_Purchase_Error_Init",
                    new Dictionary<string, object> {
                        { "FailCode", Purchaser.Instance.InitFailCode}
                    }
                );
                
                return new[] {
                    characterOption(getErrorMessageSayText("Init-" + Purchaser.Instance.InitFailCode)).transient()
                        .options(
                            ancestorOption(ROOT_ROOT_ID, true)
                        )
                };                                 
            }
            
            if (products.Count == 0) {
                AnalyticsFacade.Instance.CustomEvent("Gubbi_Purchase_Error_NoProducts");
                
                return new[] {
                    characterOption(getErrorMessageSayText("NoProducts")).transient()
                        .options(
                            ancestorOption(ROOT_ROOT_ID, true)
                        )
                };                    
            }

            ConversationOption donationResponse = 
                option(optionValue().characterOption("").delayed().build()).transient()
                    .options(ancestorOption(ROOT_ROOT_ID, true));

                        
            Action<Purchaser.PurchaseResult> purchaseResultHandler = delegate(Purchaser.PurchaseResult result) {
                string productId = result.product != null ? result.product.definition.id : null;
                Debug.Log("purchase of product " + productId + " returned status " + result.result + ", with msg: " + result.message);

                if (result.result == Purchaser.PurchaseResult.ResultType.FAIL) {
                    AnalyticsFacade.Instance.CustomEvent("Gubbi_Purchase_Error",
                        new Dictionary<string, object> {
                            { "Message", result.message}
                        }                        
                        );
                    donationResponse.Value.SayText = getErrorMessageSayText(result.message);
                }
                else {
                    AnalyticsFacade.Instance.CustomEvent("Gubbi_Purchase_Complete", new Dictionary<string, object> {
                        { "productID", productId}
                    });
                    
                    donationResponse.Value.BeforeSayCallback = () => standardPlayerAction();
                    donationResponse.Value.SayText = I18n.Instance.__("PurchaseThanks");        
                    SettingsState.Instance.setDonated();
                }
                
                donationResponse.setDelayedResultAvailable();
            };
            
            List<ConversationOption> opts = products.Select(p => {
                Action<ConversationOption> purchase = delegate {                    
                    Purchaser.Instance.buyProductWithId(p.definition.id, purchaseResultHandler);
                };
                
                return playerOption(p.metadata.localizedPriceString, purchase).transient().options(donationResponse);
            }).ToList();
            
            
            opts.Insert(0,                
                playerOption("If it won't help me!? Then, nothing!", opt => {
                    AnalyticsFacade.Instance.CustomEvent("Gubbi_Purchase_UserCancelled_Early");                    
                }).transient().options(
                    ancestorOption(ROOT_ROOT_ID)
                )
            );
            
            return new[] {
                characterOption("donateStart2", opt => { AnalyticsFacade.Instance.CustomEvent("Gubbi_Purchase_Start"); }).transient().options(
                    opts.ToArray()
                )
            };                         
        }                        

        private string getErrorMessageSayText(string code) {
            string errDetail = I18n.Instance.__("PurchaseError-" + code);
            return I18n.Instance.__("PurchaseError-Prefix", errDetail);            
        }
    }

}