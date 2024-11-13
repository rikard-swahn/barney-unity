using System;
using DigitalRuby.Tween;
using net.gubbi.goofy.extensions;
using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public class AutoHideMenu : MonoBehaviour {

        public Vector2 hideOffset;
        public float showTime;
        public float tweenHideTime;
        public float tweenShowTime;
        
        public bool startVisible;
        public float tweenShowTimeStart;
        public float showTimeStart;

        private float showTimeLeft;
        private RectTransform rectTrans;
        private Vector2 originalPos;
        private Selectable[] selectables;
        private bool tweening;

        private void Awake() {
            rectTrans = GetComponent<RectTransform>();
            originalPos = rectTrans.anchoredPosition;
            selectables = GetComponents<Selectable>();

            hide();            
        }

        private void Start() {
            if (startVisible) {
                tweenShowWithTime(tweenShowTimeStart, showTimeStart);
            }                        
        }

        private void Update() {
            if (showTimeLeft > 0) {
                showTimeLeft -= Time.deltaTime;
                showTimeLeft = Mathf.Max(showTimeLeft, 0f);
                if (showTimeLeft == 0f) {
                    tweenHide();
                }
            }
        }

        public void tweenShow() {
            tweenShowWithTime(tweenShowTime, showTime);
        }

        private void tweenShowWithTime(float tweenTime, float showTime) {            
            if (!setTweening()) {
                return;
            }                        
            
            Action<ITween<Vector3>> onCompleteWrapper = _ => {
                showWithTimer(showTime);
                clearTweening();
            };
            
            gameObject.Tween(
                null, rectTrans.anchoredPosition, originalPos, tweenTime, TweenScaleFunctions.CubicEaseIn,
                t => {
                    rectTrans.anchoredPosition = t.CurrentValue;
                }, 
                onCompleteWrapper
            );              
        }

        private void tweenHide() {
            if (!setTweening()) {
                return;
            }               
            
            setMenuEnabled(false);

            Action<ITween<Vector3>> onCompleteWrapper = _ => { clearTweening(); };
            
            gameObject.Tween(
                null, rectTrans.anchoredPosition, getTweenHideEndPos(), tweenHideTime, TweenScaleFunctions.CubicEaseIn,
                t => {
                    rectTrans.anchoredPosition = t.CurrentValue;
                }, 
                onCompleteWrapper
            );  
        }

        private bool setTweening() {
            if (tweening) {
                return false;
            }
            
            tweening = true;
            return true;
        }

        private void clearTweening() {
            tweening = false;
        }

        private void setMenuEnabled(bool enabled) {
            selectables.ForEach(s => s.interactable = enabled);
        }

        private void showWithTimer(float showForTime) {
            setMenuEnabled(true);
            showTimeLeft = showForTime;
            rectTrans.anchoredPosition = originalPos;            
        }
        
        private void hide() {
            setMenuEnabled(false);
            rectTrans.anchoredPosition = getTweenHideEndPos();            
        }

        private Vector2 getTweenHideEndPos() {
            return originalPos + hideOffset;
        }
    }
}