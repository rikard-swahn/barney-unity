using System;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using Object = System.Object;

namespace net.gubbi.goofy.unity {
    public class AnalyticsFacade  {
        
        //Singleton pattern ---------------------
        private static volatile AnalyticsFacade instance;
        private static readonly object syncRoot = new Object();
        private AnalyticsFacade() {}
        public static AnalyticsFacade Instance {
            get {
                if (instance == null) {
                    lock (syncRoot) {
                        if (instance == null) {
                            instance = new AnalyticsFacade();
                        }
                    }
                }
                return instance;
            }
        }        
        //---------------------------------------        

        public void CustomEvent(string eventName, IDictionary<string, object> eventParams = null) {

            try {
                if (eventParams != null) {
                    AnalyticsService.Instance.CustomData(eventName, eventParams);
                }
                else {
                    AnalyticsService.Instance.CustomData(eventName);
                }
                
            }
            catch (Exception exception) {
                Debug.LogError("An error occurred during AnalyticsService.CustomEvent: " + exception.Message);
            }
        }

    }
}