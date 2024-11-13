using Mgl;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui.menu.ingame {
    public class InGameHowToPlayLightbox : InGameLightbox {
        
        public InGameLightbox helpBox;
        public Sprite[] backgrounds;
        public string[] texts;
        public Image backgroundImage;
        public Text text;
        public GameObject previousButton;
        public GameObject nextButton;
        
        private int currentStep;

        public override void show() {
            base.show();
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

        public override void close() {
            base.close();
            helpBox.show();
            playClickSfx();
        }
        
        public override void back() {
            close();
        }        

        private void refresh() {
            backgroundImage.sprite = backgrounds[currentStep];
            
            string i18nSuffix = (SystemInfo.deviceType == DeviceType.Handheld) ? "Touch" : "Desktop";
            text.text = I18n.Instance.__(texts[currentStep] + "_" + i18nSuffix);
            
            previousButton.SetActive(currentStep > 0);
            nextButton.SetActive(currentStep < 3);                        
        }
    }
}