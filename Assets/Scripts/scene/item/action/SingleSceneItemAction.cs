using System;
using System.Collections;
using Mgl;
using net.gubbi.goofy.audio;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.filter;
using net.gubbi.goofy.item;
using net.gubbi.goofy.player;
using net.gubbi.goofy.ui;
using net.gubbi.goofy.util;
using scene.item.action;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.scene.item.action {

    public abstract class SingleSceneItemAction : SceneItemAction {

        public string actionText;
        public float actionTextTime;
        public GameObject target;
        public bool skipOnResume;
        public string labelPrefix;
        public Filter skipCondition;
        public DelayedAudioClip sfxBefore;
        public DelayedAudioClip sfxAfter;
        public MusicClip musicBefore;
        public bool stopMusic;
        public bool stopSfx;

        protected Text text;	
        private bool actionTextSet;
        private Transform textPosTransform;
        private SceneInputHandler sceneInput;
        private Action onComplete;
        private IEnumerator actionTextRoutine;
        private SfxPlayer sfxPlayer;
        private MusicPlayer musicPlayer;
        private CanvasTextPosition canvasTextPos;

        protected virtual void Awake () {
            actionTextSet = !actionText.isNullOrEmpty();
            sceneInput = GameObject.Find(GameObjects.PLAYER).GetComponent<SceneInputHandler>();
            if (target == null) {
                target = gameObject;
            }            

            if (actionTextSet) {
                GameObject textGo = GameObject.Find (GameObjects.ACTION_TEXT);
                text = textGo.GetComponent<Text>();
                textPosTransform = target.findChildWithTag (Tags.TEXT_POS).transform;
                canvasTextPos = textGo.GetComponent<CanvasTextPosition>();
            }
            
            sfxPlayer = GameObject.Find (GameObjects.SFX_PLAYER).GetComponent<SfxPlayer>();           
            musicPlayer = GameObject.Find (GameObjects.MUSIC_PLAYER).GetComponent<MusicPlayer>();
        }

        public override bool filter(ItemType selectedItem) {
            FilterContext ctx = getFilterContext(selectedItem);
            return conditionsMatches(ctx);
        }

        public override void start(ItemType selectedItem, Action onComplete) {
            doFullAction(selectedItem, onComplete);
        }

        public void doFullAction(ItemType selectedItem, Action onComplete) {
            if (skip()) {
                onComplete();
                return;
            }
            
            this.onComplete = onComplete;
            beforeAction();
            doAction(selectedItem);
        }

        public override void resume(ItemType selectedItem, Action onComplete) {
            if (skip() || skipOnResume) {
                onComplete();
                return;
            }

            this.onComplete = onComplete;
            beforeAction();
            doAction(selectedItem);
        }

        public virtual FilterContext getFilterContext(ItemType selectedItem) {
            return FilterContext
                .builder()
                .property(FilterContext.SELECTED_ITEM, selectedItem)
                .build();
        }

        public virtual bool conditionsMatches(FilterContext ctx) {
            foreach (var condition in conditions) {
                if (!condition.matches(ctx)) {
                    return false;
                }
            }

            return true;
        }

        private void beforeAction () {
            sceneInput.setControlEnabled(false, getControlById());

            if (actionTextSet) {
                canvasTextPos.Pos = textPosTransform.position;
                text.text = I18n.Instance.__(actionText);
                actionTextRoutine = waitAndClearActionText();
                StartCoroutine(actionTextRoutine);
            }
            
            if (stopSfx) {
                sfxPlayer.stop();
            }
            
            sfxPlayer.play(sfxBefore);            

            if (stopMusic) {
                musicPlayer.fadeOutStop();
            }
            else {
                musicPlayer.playDelayed(musicBefore);                    
            }            
        }

        protected abstract void doAction(ItemType selectedItem);

        protected virtual void afterAction() {            
            sfxPlayer.play(sfxAfter);                        
            sceneInput.setControlEnabled(true, getControlById());
            onComplete();
        }

        private IEnumerator waitAndClearActionText () {
            yield return new WaitForSeconds(actionTextTime);
            text.text = "";
        }
        
        public override string getLabelPrefix() {
            if (!labelPrefix.isNullOrEmpty()) {
                return I18n.Instance.__(labelPrefix);
            }

            return base.getLabelPrefix();
        }
        
        private bool skip() {
            return skipCondition != null && skipCondition.matches();
        }        
                
    }

}