using net.gubbi.goofy.audio;
using net.gubbi.goofy.util;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui {
    public class CanvasButtonSfx : MonoBehaviour {

        public AudioClip sfx;
        
        private void Awake() {
            SfxPlayer sfxPlayer = GameObject.Find (GameObjects.SFX_PLAYER).GetComponent<SfxPlayer>();
            
            Button[] buttons = GetComponentsInChildren<Button>();            
            for (int i = 0; i < buttons.Length; i++) {
                buttons[i].onClick.AddListener(() => sfxPlayer.playOnce(sfx));
            }                                   
        }
        
    }
}