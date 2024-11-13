using UnityEngine;

namespace net.gubbi.goofy.ui {
    public class HandheldActive : MonoBehaviour {

        public bool forHandheld;
        
        private void Awake() {
            bool isHandheld = SystemInfo.deviceType == DeviceType.Handheld;
            gameObject.SetActive(forHandheld == isHandheld);            
        }        
        
    }
}