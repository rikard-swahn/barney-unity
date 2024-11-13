using net.gubbi.goofy.audio;
using net.gubbi.goofy.events;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.ui.menu.main {
    public class MenuScreen : MonoBehaviour {
        
        public AudioClip music;
        
        private MusicPlayer musicPlayer;
        
        protected virtual void Awake() {
            EventManager.Instance.addListener<GameEvents.MusicFinishedEvent>(musicFinishedHandler);
        }        

        private void Start() {
            musicPlayer = GameObject.Find(GameObjects.MUSIC_PLAYER).GetComponent<MusicPlayer>();
            playMusic();
        }
     
        private void musicFinishedHandler(GameEvents.MusicFinishedEvent e) {
            playMusic();
        }        
        
        private void playMusic() {
            musicPlayer.playLooping(music);
        }        
        
    }
}