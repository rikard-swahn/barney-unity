using UnityEngine;

namespace net.gubbi.goofy.util {
    public class MouseUtil {
        
        public static bool isHovering() {
            return SystemInfo.deviceType != DeviceType.Handheld 
                   || Input.GetMouseButton(0) 
                   || Input.GetMouseButtonDown(0)
                   || Input.GetMouseButtonUp(0);
        }        
        
        public static bool isLeftButtonDown() {
            return Input.GetMouseButton(0) || Input.GetMouseButtonDown(0);
        }

    }
}