using Mgl;
using net.gubbi.goofy.audio;
using net.gubbi.goofy.ui.menu.main;
using net.gubbi.goofy.util;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui.menu.ingame {
    public class HowToPlayLightbox : MonoBehaviour{
        
        public Menu helpMenu;
        public Sprite[] backgrounds;
        public string[] texts;
        public Image backgroundImage;
        public Text text;
        public GameObject previousButton;
        public GameObject nextButton;
        public AudioClip sfxClick;
        
        private int currentStep;
        private SfxPlayer sfxPlayer;

        private void Awake() {
            sfxPlayer = GameObject.Find (GameObjects.SFX_PLAYER).GetComponent<SfxPlayer>();
        }

        public void show() {
            gameObject.SetActive(true);
            currentStep = 0;
            refresh();
        }

        public void previous() {
            currentStep--;
            refresh();
            playClickSfx();
        }

        public void next() {
            currentStep++;
            refresh();
            playClickSfx();
        }

        public void close() {
            gameObject.SetActive(false);
            helpMenu.showInstantly();
            playClickSfx();
        }

        private void refresh() {
            backgroundImage.sprite = backgrounds[currentStep];
            
            string i18nSuffix = (SystemInfo.deviceType == DeviceType.Handheld) ? "Touch" : "Desktop";
            text.text = I18n.Instance.__(texts[currentStep] + "_" + i18nSuffix);
            
            previousButton.SetActive(currentStep > 0);
            nextButton.SetActive(currentStep < 3);                        
        }
        
        private void playClickSfx() {
            sfxPlayer.playOnceMenu(sfxClick);
        }        
        
    }
}