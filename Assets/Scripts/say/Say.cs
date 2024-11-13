using System;
using System.Collections;
using Mgl;
using net.gubbi.goofy.events;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.game;
using net.gubbi.goofy.player;
using net.gubbi.goofy.ui.cursor;
using net.gubbi.goofy.ui.menu.ingame;
using net.gubbi.goofy.util;
using UnityEngine;
using VikingCrewTools;

namespace net.gubbi.goofy.say {
    public class Say : MonoBehaviour {

        private SceneInputHandler sceneInput;
        private Animator[] animators;
        private int currentLine;
        private string[] textLines;
        private bool talking;
        private bool talkingStarted;
        private bool sayTimed;
        private float? sayTimer;
        private Action sayCompleteCallback;
        private Action sayAbortedCallback;
        private GameObject sayer;
        private Transform textPosTransform;
        private bool sayNextOnClick;
        private bool doCallbackOnOverride;
        private IEnumerator waitAndSayRoutine;
        private IEnumerator nextFrameSay;
        private static readonly string CONTROL_BY_ID = "say";
        private bool controlDisabledOverride;
        private SpeechbubbleBehaviour bubble;
        private TextStyle textStyle;

        private void Awake () {
            sceneInput = GameObject.Find(GameObjects.PLAYER).GetComponent<SceneInputHandler>();
            EventManager.Instance.addListener<GameEvents.LateMouseEvent>(handleMouseEvent);
            EventManager.Instance.addListener<GameEvents.SceneTransitionStartedEvent>(sceneTransitionStartedHandler);
            EventManager.Instance.addListener<GameEvents.SceneTransitionCompletedEvent>(sceneTransitionCompletedHandler);            
        }

        private void sceneTransitionStartedHandler(GameEvents.SceneTransitionStartedEvent e) {
            controlDisabledOverride = true;
        }
        private void sceneTransitionCompletedHandler(GameEvents.SceneTransitionCompletedEvent e) {
            controlDisabledOverride = false;
        }

        private void LateUpdate() {
            if (talking) {
                updateAnimator();
                
                if (sayNextInputEnabled()) {
                    EventManager.Instance.raise (new GameEvents.CursorEvent(CursorType.ACTION));   
                }                
            }
        }

        public void sayRaw(GameObject sayer, string text, Action sayCompleteCallback) {
            if(talking || nextFrameSay != null) {
                Action previousAbortedCallback = this.sayAbortedCallback;
                bool previousCallbackOnOverride = this.doCallbackOnOverride;                
                stopTalking (false);
                
                setSayer (sayer);                        
                sayInternal (text, sayCompleteCallback, false, null, true, true, false, sayCompleteCallback);                
                
                if (previousAbortedCallback != null && previousCallbackOnOverride) {
                    previousAbortedCallback ();
                }                            
            }
            else {
                setSayer(sayer);
                sayInternal(text, sayCompleteCallback, false, null, true, true, false, sayCompleteCallback);
            }

        }

        public void say(GameObject sayer, string text, Action sayCompleteCallback) {
            text = I18n.Instance.__(text);
            sayRaw(sayer, text, sayCompleteCallback);
        }

        public void sayBackgroundRaw(GameObject sayer, string text, Action sayCompleteCallback, float sayTimer, bool doCallbackOnOverride, Action sayAbortedCallback) {
            if (talking || nextFrameSay != null) {
                sayAbortedCallback();
                return;
            }

            setSayer (sayer);            
            sayInternal (text, sayCompleteCallback, true, sayTimer, false, false, doCallbackOnOverride, sayAbortedCallback);            
        }

        public void sayBackground(GameObject sayer, string text, Action sayCompleteCallback, float sayTimer, bool doCallbackOnOverride, Action sayAbortedCallback) {
            text = I18n.Instance.__(text);
            sayBackgroundRaw(sayer, text, sayCompleteCallback, sayTimer, doCallbackOnOverride, sayAbortedCallback);
        }

        private void setSayer(GameObject sayer) {
            this.sayer = sayer;
            textPosTransform = sayer.findChildWithTag (Tags.TEXT_POS).transform;
            textStyle = sayer.GetComponentInChildren<TextStyle> ();
        }

        private void sayInternal(string text, Action sayCompleteCallback, bool sayTimed, float? sayTimer, bool disableInput, bool sayNextOnClick, bool doCallbackOnOverride, Action sayAbortedCallback) {            
            this.doCallbackOnOverride = doCallbackOnOverride;
            this.sayTimed = sayTimed;
            this.sayTimer = sayTimer;
            this.sayNextOnClick = sayNextOnClick;
            if (disableInput) {
                sceneInput.setControlEnabled (false, CONTROL_BY_ID);
            }
            this.sayCompleteCallback = sayCompleteCallback;
            this.sayAbortedCallback = sayAbortedCallback;
            animators = sayer.GetComponentsInChildren<Animator> ();
            currentLine = 0;
            textLines = text.split (I18nUtil.LINE_DELIM);

            if (sayNextOnClick) {
                talkingStarted = true;
                InteractionState.Instance.setState(InteractionState.StateType.SCENE_CANVAS);   
            }

            speechBubbleNextFrame();

            if (this.sayTimed) {
                waitAndSayRoutine = waitAndSayNext ();
                StartCoroutine (waitAndSayRoutine);
            }
        }

        private IEnumerator waitAndSayNext() {
            yield return new WaitForSeconds((float)sayTimer);
            waitAndSayRoutine = null;
            doSayNext ();

            if (talking) {
                waitAndSayRoutine = waitAndSayNext ();
                StartCoroutine (waitAndSayRoutine);
            }
        }

        private void handleMouseEvent(GameEvents.MouseEvent e) {
            if (e.MouseButton != null) {
                if (sayNextInputEnabled() && nextFrameSay == null) {                    
                    nextFrameSay = UnityUtil.endOfFrameCallback(this, delegate {
                        nextFrameSay = null;
                        doSayNext();
                    });                                        
                }
            }
        }

        private void speechBubbleNextFrame() {
            nextFrameSay = UnityUtil.endOfFrameCallback(this, delegate {
                nextFrameSay = null;
                talking = true;
                addSpeechBubble();
            });
        }

        private void doSayNext() {
            currentLine++;
            if (currentLine > textLines.Length - 1) {
                stopTalking (true);
            }
            
            addSpeechBubble();
        }

        public void stopTalking(bool doCallback) {
            if (talkingStarted && sayNextOnClick) {
                InteractionState.Instance.removeState(InteractionState.StateType.SCENE_CANVAS);
            }

            talkingStarted = false;
            talking = false;            
            updateAnimator ();            
            addSpeechBubble();

            if (waitAndSayRoutine != null) {
                StopCoroutine (waitAndSayRoutine);
                waitAndSayRoutine = null;
            }
            
            if (nextFrameSay != null) {
                StopCoroutine (nextFrameSay);
                nextFrameSay = null;
            }

            sceneInput.setControlEnabled (true, CONTROL_BY_ID);

            if (sayCompleteCallback != null && doCallback) {
                sayCompleteCallback ();
            }
        }

        private void addSpeechBubble() {
            if (bubble != null) {
                bubble.deactivate();
            }
            
            if (talking) {
                bubble = SpeechbubbleManager.Instance.AddSpeechbubble(textPosTransform, textLines[currentLine]);
                bubble.text.color = textStyle.getMainColor();
            }
        }

        private void updateAnimator () {            
            animators.ForEach(a => a.SetBool(AnimationParams.TALKING, talking));
        }

        private bool sayNextInputEnabled() {
            return !InteractionState.Instance.isMouseOverMenu()
                   && InteractionState.Instance.hasOnlyState(InteractionState.StateType.SCENE_CANVAS)
                   && talking
                   && sayNextOnClick
                   && !controlDisabledOverride;
        }

        public GameObject Sayer {
            get { return sayer; }
        }
    }
}