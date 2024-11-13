using System;
using System.Collections.Generic;
using System.Linq;
using net.gubbi.goofy.events;
using net.gubbi.goofy.state.settings;
using net.gubbi.goofy.util;
using UnityEngine;
using UnityEngine.Purchasing;
using Object = System.Object;
using static net.gubbi.goofy.events.GameEvents;

namespace net.gubbi.goofy.iap {
    public class DonationFacade {
        
        // General product identifiers for the consumable, non-consumable, and subscription products.
        // Use these handles in the code to reference which product to purchase. Also use these values 
        // when defining the Product Identifiers on the store. 
        public static readonly string DONATION_PRODUCT_PREFIX = "net.gubbi.goofy.donation";

        private static readonly List<String> donationIds = new List<String> {
            DONATION_PRODUCT_PREFIX + ".small",
            DONATION_PRODUCT_PREFIX + ".medium",
            DONATION_PRODUCT_PREFIX + ".large"
        }; 
        
        //Singleton pattern ---------------------
        private static volatile DonationFacade instance;
        private static readonly object syncRoot = new Object();
        private DonationFacade() {}
        public static DonationFacade Instance {
            get {
                if (instance == null) {
                    lock (syncRoot) {
                        if (instance == null) {
                            instance = new DonationFacade();
                        }
                    }
                }
                return instance;
            }
        }        
        //---------------------------------------

        public void init() {
            EventManager.Instance.addListener<UnityGamingServicesInitializedEvent>(OnUnityGamingServicesInitialized);
        }

        private void OnUnityGamingServicesInitialized(UnityGamingServicesInitializedEvent e) {
            if (PlatformUtil.isDesktopPlayer()) {
                Debug.Log("Donations are disabled on this platform");
                return;
            }
            
            EventManager.Instance.addListener<PurchaseEvent>(handlePurchaseEvent);

            List<ProductRegistrationDto> dtos = donationIds.Select(
                id => new ProductRegistrationDto(id, ProductType.Consumable)).ToList();                
            
            Purchaser.Instance.initPurchasing(dtos);
        }

        public List<Product> getDonationProducts() {
            return Purchaser.Instance.getPurchasableProductsWithPrefix(DONATION_PRODUCT_PREFIX);
        }

        private void handlePurchaseEvent(PurchaseEvent e) {            
            if (donationIds.Contains(e.productId)) {
                Debug.Log("Donation registered");
                SettingsState.Instance.setDonated();    
            }            
        }
    }
}