using System.Collections.Generic;
using UnityEngine;

namespace net.gubbi.goofy.util {
    
    public class PlatformUtil {
     
        private static List<RuntimePlatform> desktopPlatforms = new List<RuntimePlatform> {
            RuntimePlatform.WindowsPlayer, RuntimePlatform.OSXPlayer, RuntimePlatform.LinuxPlayer
        };

        public static bool isDesktopPlayer() {
            return desktopPlatforms.Contains(Application.platform);
        }        
    }
}