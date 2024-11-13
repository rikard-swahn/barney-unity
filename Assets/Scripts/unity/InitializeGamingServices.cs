using System;
using System.Collections.Generic;
using net.gubbi.goofy.events;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using static net.gubbi.goofy.events.GameEvents;

namespace net.gubbi.goofy.unity {
    public class InitializeGamingServices : MonoBehaviour {
        const string ENVIRONMENT = "production";

        async void Start() {
            try {
                var options = new InitializationOptions().SetEnvironmentName(ENVIRONMENT);
                await UnityServices.InitializeAsync(options);
                
                Debug.Log("Unity Gaming Services has been successfully initialized.");
                EventManager.Instance.raise(new UnityGamingServicesInitializedEvent());
                
                AnalyticsFacade.Instance.CustomEvent("Gubbi_Launch", new Dictionary<string, object> {
                    {"OS", SystemInfo.operatingSystem}
                });                
                
            }
            catch (Exception exception) {
                OnError(exception.Message);
            }
        }
        
        void OnError(string message) {
            Debug.LogError("An error occurred during Unity Gaming Services initialization: " + message);
        }        
    }
}