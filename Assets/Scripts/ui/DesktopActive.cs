using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.ui {
    public class DesktopActive : MonoBehaviour {

        public bool forDesktop;
        
        private void Awake() {
            gameObject.SetActive(forDesktop == PlatformUtil.isDesktopPlayer());            
        }        
        
    }
}