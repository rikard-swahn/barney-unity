using System;
using net.gubbi.goofy.extensions;
using UnityEngine;

namespace net.gubbi.goofy.ui.menu.main {
    public class CurtainMenu : Menu {

        public RectTransform curtainLeft;
        public RectTransform curtainRight;
        public RectTransform titleLeft;
        public RectTransform titleRight;
        public RectTransform titleRight2;
        public RectTransform player;        
        public float curtainTranslateX;
        public GameObject pressToPlay;

        public Menu rootMenu;

        public void doStart() {
            setInteractable(false);
            blinkText(5, () => hide(rootMenu.show));
        }

        public override void hide(Action onComplete = null) {
            translateX(-curtainTranslateX, 1f, null, curtainLeft, titleLeft);            
            translateX(curtainTranslateX, 1f, null, curtainRight, titleRight2);
            translateToY(-213, 1f, null, player);

            this.delayedAction(() => { translateX(curtainTranslateX, 1f, onComplete, titleRight); },
                0.15f);            
        }

        private void blinkText(int times, Action onComplete) {
            if (times <= 0) {
                onComplete();
                return;
            }
            
            this.delayedAction(() => {
                    pressToPlay.SetActive(!pressToPlay.activeInHierarchy);
                    blinkText(times - 1, onComplete);                                        
                },
                0.1f);            
        }
    }
}