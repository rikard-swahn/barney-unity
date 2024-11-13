using UnityEngine;

namespace ui {
    public abstract class HandheldEnable : MonoBehaviour {

        public bool forHandheld;
        
        protected virtual void Awake() {
            bool isHandheld = SystemInfo.deviceType == DeviceType.Handheld;
            enabled = (forHandheld == isHandheld);
        }        
        
    }
}