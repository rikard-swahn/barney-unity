using System;
using System.Collections.Generic;
using DigitalRuby.Tween;
using net.gubbi.goofy.audio;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.util;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui.menu.main {
    public abstract class Menu : MonoBehaviour {

        public bool defaultActive;
        public AudioClip sfxTween;
        public AudioClip sfxClick;
        
        private List<Selectable> selectables;        
        private SfxPlayer sfxPlayer;

        protected virtual void Awake() {
            selectables = new List<Selectable>(GetComponentsInChildren<Selectable>());
            
            setActive(defaultActive);
            sfxPlayer = GameObject.Find (GameObjects.SFX_PLAYER).GetComponent<SfxPlayer>();
        }

        public virtual void show() {
            setActive(true);
        }
        
        public virtual void hide(Action onComplete = null) {
            setActive(false);
        }
        
        public void showInstantly() {
            setActive(true);
        }
        
        public void hideInstantly() {
            setActive(false);
        }

        protected void playTweenSfx() {
            sfxPlayer.playOnceMenu(sfxTween);
        }
        
        protected void playClickSfx() {
            sfxPlayer.playOnceMenu(sfxClick);
        }

        private void setActive(bool active) {
            gameObject.SetActive(active);        
            setInteractable(active);
        }
        
        protected void translateX(float xDelta, float time, Action onComplete, params RectTransform[] transforms) {
            transforms.ForEach(c => {
                    Vector3 currentPos = c.anchoredPosition;
                    Vector3 end = new Vector3(currentPos.x + xDelta, currentPos.y, 0);
                    Action<ITween<Vector3>> onCompleteWrapper = _ => { if(onComplete != null) onComplete(); };
                    c.gameObject.Tween(
                        null, currentPos, end, time, TweenScaleFunctions.CubicEaseIn,
                        t => { c.anchoredPosition = t.CurrentValue; }, 
                        onCompleteWrapper
                    );
                }
            );
        }
        
        protected void translateToX(float x, float time, Action onComplete, params RectTransform[] transforms) {
            transforms.ForEach(c => {
                    Vector3 currentPos = c.anchoredPosition;      
                    Vector3 end = new Vector3(x, currentPos.y, 0);
                    Action<ITween<Vector3>> onCompleteWrapper = _ => { if(onComplete != null) onComplete(); };
                    c.gameObject.Tween(
                        null, currentPos, end, time, TweenScaleFunctions.CubicEaseIn,
                        t => { c.anchoredPosition = t.CurrentValue; }, 
                        onCompleteWrapper
                    );
                }
            );
        }        
        
        protected void translateToY(float y, float time, Action onComplete, params RectTransform[] transforms) {
            transforms.ForEach(c => {
                    Vector3 currentPos = c.anchoredPosition;      
                    Vector3 end = new Vector3(currentPos.x, y, 0);
                    Action<ITween<Vector3>> onCompleteWrapper = _ => { if(onComplete != null) onComplete(); };
                    c.gameObject.Tween(
                        null, currentPos, end, time, TweenScaleFunctions.CubicEaseIn,
                        t => { c.anchoredPosition = t.CurrentValue; }, 
                        onCompleteWrapper
                    );
                }
            );
        }
        
        protected void setInteractable(bool interactable) {
            selectables.ForEach(s => s.interactable = interactable);
        }  
        
    }
}