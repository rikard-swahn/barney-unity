using net.gubbi.goofy.audio;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.animation {
    public class SfxAnimationEvents : MonoBehaviour {
        
        private SfxPlayer sfxPlayer;
        
        private void Awake() {
            sfxPlayer = GameObject.Find (GameObjects.SFX_PLAYER).GetComponent<SfxPlayer>();
        }

        public void playSfx(AudioClip clip) {
            sfxPlayer.playOnce(clip);
        }
        
        public void playLoopingSfxIfSilence(AudioClip clip) {
            sfxPlayer.playLoopingIfSilence(clip);
        }        
        
        public void stopSfx(AudioClip clip) {
            sfxPlayer.stop(clip);
        }
        
    }
}