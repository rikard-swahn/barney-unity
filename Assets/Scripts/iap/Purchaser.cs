using System;
using System.Collections.Generic;
using System.Linq;
using net.gubbi.goofy.events;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using Object = System.Object;

namespace net.gubbi.goofy.iap {
    
    // Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
    public class Purchaser : IDetailedStoreListener {
        private IStoreController storeController;          // The Unity Purchasing system.

        private Action<PurchaseResult> resultCallback;
        private bool initStarted;
        public bool InitComplete { get; private set; }
        public InitializationFailureReason? InitFailCode { get; private set; }
                
        //Singleton pattern ---------------------
        private static volatile Purchaser instance;
        private static readonly object syncRoot = new Object();
        private Purchaser() {}
        public static Purchaser Instance {
            get {
                if (instance == null) {
                    lock (syncRoot) {
                        if (instance == null) {
                            instance = new Purchaser();
                        }
                    }
                }
                return instance;
            }
        }        
        //---------------------------------------


        public void initPurchasing(List<ProductRegistrationDto> registrationDtos) {            
            if (initStarted) {
                //Debug.Log("Purchaser initialization aldready started.");
                return;
            }
            
            Debug.Log("Store initialization started");

            initStarted = true;            
            
            // Create a builder, first passing in a suite of Unity provided stores.
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            
            // Add a product to sell / restore by way of its identifier, associating the general identifier
            // with its store-specific identifiers.
            registrationDtos.ForEach(d => {
                builder.AddProduct(d.id, d.type);
            });
            
            // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
            // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
            UnityPurchasing.Initialize(this, builder);
        }

        public bool initOk() {
            return InitComplete && InitFailCode == null;
        }
        
        public void buyProductWithId(string id, Action<PurchaseResult> resultCallback) {            
            // If Purchasing has been initialized ...
            if (initOk()) {
                // ... look up the Product reference with the general product identifier and the Purchasing 
                // system's products collection.
                Product product = storeController.products.WithID(id);
                
                // If the look up found a product for this device's store and that product is ready to be sold ... 
                if (product != null && product.availableToPurchase) {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                    // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                    // asynchronously.
                    this.resultCallback = resultCallback;
                    storeController.InitiatePurchase(product);
                }
                else {
                    // ... report the product look-up failure situation  
                    Debug.LogError("Buy product FAILED. Not purchasing product, either is not found or is not available for purchase");
                    resultCallback(new PurchaseResult(PurchaseResult.ResultType.FAIL, "ProductUnavailable", product));
                }
            }            
            else {
                // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
                // retrying initialization.
                Debug.LogError("Buy product FAILED. Not initialized.");
                resultCallback(new PurchaseResult(PurchaseResult.ResultType.FAIL, "NotInited", null));
            }
        }

        public List<Product> getPurchasableProductsWithPrefix(string prefix) {
            if (initOk()) {
                return storeController.products.all
                    .Where(p => p.availableToPurchase && p.definition.id.StartsWith(prefix)).ToList();
            }
            else {
                Debug.LogError("Get products FAILED. Not initialized.");
                return new List<Product>();
            }
        }
        
        //  
        // --- Implementation of IStoreListener methods
        //                        
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
            // Purchasing has succeeded initializing. Collect our Purchasing references.
            Debug.Log("Store initialization OK");
            
            // Overall Purchasing system, configured with products for this application.
            storeController = controller;
            // Store specific subsystem, for accessing device-specific store features.
            InitFailCode = null;
            InitComplete = true;
        }
                
        public void OnInitializeFailed(InitializationFailureReason error) {
            OnInitializeFailed(error, null);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message) {
            var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

            if (message != null)
            {
                errorMessage += $" More details: {message}";
            }

            Debug.LogError(errorMessage);
            InitFailCode = error;
            InitComplete = true;
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
            string productId = args.purchasedProduct.definition.id;
            Debug.Log(string.Format("Purchase OK. Product: '{0}'", productId));

            if (resultCallback != null) {
                // The consumable item has been successfully purchased                
                resultCallback(PurchaseResult.ok(args.purchasedProduct));
            }
            
            EventManager.Instance.raise(new GameEvents.PurchaseEvent(productId));            

            // Return a flag indicating whether this product has completely been received, or if the application needs 
            // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
            // saving purchased products to the cloud, and when that save is delayed. 
            return PurchaseProcessingResult.Complete;
        }
                
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
            // A product purchase attempt did not succeed. Check failureReason for more detail.
            Debug.Log(string.Format("Purchase FAILED. Product: '{0}', Fail reason: {1}", product.definition.storeSpecificId, failureReason));
            resultCallback(new PurchaseResult(PurchaseResult.ResultType.FAIL, failureReason.ToString(), product));
        }
        
        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            Debug.Log($"Purchase failed - Product: '{product.definition.storeSpecificId}'," +
                      $" Purchase failure reason: {failureDescription.reason}," +
                      $" Purchase failure details: {failureDescription.message}");
            resultCallback(new PurchaseResult(PurchaseResult.ResultType.FAIL, failureDescription.reason.ToString(), product));
        }        
        
        public class PurchaseResult {
            public ResultType result { get; private set; }
            public string message { get; private set; }
            public Product product { get; private set; }

            public static PurchaseResult ok(Product purchasedProduct) {
                return new PurchaseResult(ResultType.OK, null, purchasedProduct);
            }
            

            public PurchaseResult(ResultType result, string message, Product product) {
                this.result = result;
                this.message = message;
                this.product = product;
            }

            public enum ResultType {
                OK, FAIL
            }
        }        
    }

    public class ProductRegistrationDto {
        public ProductType type { get; private set; }
        public string id { get; private set; }

        public ProductRegistrationDto(string id, ProductType type) {
            this.id = id;
            this.type = type;
        }
    }


}