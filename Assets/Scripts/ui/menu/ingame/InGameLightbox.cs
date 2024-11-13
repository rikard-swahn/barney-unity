using System.Collections.Generic;
using System.Linq;
using net.gubbi.goofy.audio;
using net.gubbi.goofy.events;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.game;
using net.gubbi.goofy.player;
using net.gubbi.goofy.ui.cursor;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.ui.menu.ingame {
    public abstract class InGameLightbox : MonoBehaviour {
        
        public AudioClip sfxClick;

        private List<Canvas> inGameCanvases = new List<Canvas>();
        private Canvas lightboxCanvas;
        private SceneInputHandler sceneInput;
        protected static readonly string CONTROL_BY_ID = "lightbox";
        private SfxPlayer sfxPlayer;

        protected virtual void Awake() {
            inGameCanvases = GameObject.FindGameObjectsWithTag(Tags.MAIN_CANVAS).Select(o => o.GetComponent<Canvas>()).ToList();
            lightboxCanvas = GetComponentInParent<Canvas>();
            sceneInput = GameObject.Find(GameObjects.PLAYER).GetComponent<SceneInputHandler>();
            gameObject.SetActive(false);
            lightboxCanvas.enabled = false;
            sfxPlayer = GameObject.Find (GameObjects.SFX_PLAYER).GetComponent<SfxPlayer>();
            EventManager.Instance.addListener<GameEvents.KeyUpEvent>(keyUpHandler);
        }
        
        public virtual void show() {
            Pause.Instance.pause(Constants.SOURCE_ID_GAME);
            sceneInput.setControlEnabled(false, CONTROL_BY_ID);
            InteractionState.Instance.setState(InteractionState.StateType.PAUSE_MENU);
            lightboxCanvas.enabled = true;
            EventManager.Instance.raise (new GameEvents.CursorEvent(CursorType.NORMAL));            
            setInGameCanvasesEnabled(false);
            gameObject.SetActive(true);
        }

        public virtual void close() {
            Pause.Instance.unPause(Constants.SOURCE_ID_GAME);      
            InteractionState.Instance.removeState(InteractionState.StateType.PAUSE_MENU);
            sceneInput.setControlEnabled(true, CONTROL_BY_ID);
            lightboxCanvas.enabled = false;
            setInGameCanvasesEnabled(true);
            gameObject.SetActive(false);
        }

        public virtual void back() {}

        protected void playClickSfx() {
            sfxPlayer.playOnceMenu(sfxClick);
        }        

        private void setInGameCanvasesEnabled(bool enabled) {
            foreach (var canvas in inGameCanvases) {
                canvas.enabled = enabled;
                canvas.GetComponentsInChildren<Collider2D>().ForEach(c => c.enabled = enabled);
            }
        }
        
        private void keyUpHandler(GameEvents.KeyUpEvent e) {
            if (!e.Consumed && e.KeyCode == KeyCode.Escape && gameObject.activeInHierarchy) {
                e.Consumed = true;
                back();
            }
        }        
    }
}