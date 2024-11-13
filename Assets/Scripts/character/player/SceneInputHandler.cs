using System;
using System.Collections.Generic;
using net.gubbi.goofy.character;
using net.gubbi.goofy.events;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.game;
using net.gubbi.goofy.input;
using net.gubbi.goofy.item.inventory;
using net.gubbi.goofy.scene.action;
using net.gubbi.goofy.state;
using net.gubbi.goofy.ui.cursor;
using net.gubbi.goofy.util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace net.gubbi.goofy.player {

    public class SceneInputHandler : MonoBehaviour {
        public bool FreeWalk = true;

        private bool controlEnabled = true;
        private bool controlDisabledOverride;
        private Inventory inventory;        
        private HashSet<string> disabledBy = new HashSet<string>();
        private MouseHandler sceneMouseHandler;
        private CharacterMove playerMove;
        
        private void Awake () {
            inventory = GameObject.Find(GameObjects.INVENTORY).GetComponent<Inventory>();
            playerMove = GetComponent<CharacterMove>();            
            
            EventManager.Instance.addListener<GameEvents.MouseEvent>(handleMouseEvent);
            EventManager.Instance.addListenerFirst<GameEvents.SceneChangeInCurrentGameInitEvent>(handleSceneChange);
            EventManager.Instance.addListener<GameEvents.SceneTransitionStartedEvent>(sceneTransitionStartedHandler);
            EventManager.Instance.addListener<GameEvents.SceneTransitionCompletedEvent>(sceneTransitionCompletedHandler);
            sceneMouseHandler = new SceneMouseHandler(playerMove, FreeWalk);
        }

        private void sceneTransitionStartedHandler(GameEvents.SceneTransitionStartedEvent e) {
            controlDisabledOverride = true;

            if (controlEnabled) {
                EventManager.Instance.raise(new GameEvents.PlayerControlDisabledEvent());
            }
        }

        private void sceneTransitionCompletedHandler(GameEvents.SceneTransitionCompletedEvent e) {
            controlDisabledOverride = false;
            
            if (controlEnabled) {
                EventManager.Instance.raise(new GameEvents.PlayerControlEnabledEvent());
            }            
        }

        private void Start() {
            string sceneActionTargetName = GameState.Instance.StateData.PlayerState.SceneActionTargetName;
            Vector2? dest = GameState.Instance.StateData.PlayerState.SceneTarget;
                       
            UnityUtil.endOfFrameCallback(this, delegate { lateStart(sceneActionTargetName, dest); });
        }

        private void lateStart(string sceneActionTargetName, Vector2? dest) {            
            if (sceneActionTargetName != null) {
                GameObject target = GameObject.Find(sceneActionTargetName);
                if (target == null) {
                    throw new Exception("GameObject not found: " + sceneActionTargetName + " in Scene: " + SceneManager.GetActiveScene ().name);
                }
                
                SceneActionHandler handler = target.GetComponent<SceneActionHandler>();
                if (handler == null)  {
                    throw new Exception("SceneActionHandler not found on GameObject: " + sceneActionTargetName + " in Scene: " + SceneManager.GetActiveScene ().name);
                }

                handler.doAction(inventory.SelectedItem);
            }
            else if (dest != null) {
                playerMove.SetDestination((Vector2)dest);                                    
            }
        }

        private void handleSceneChange(GameEvents.SceneChangeInCurrentGameInitEvent e) {
            GameState.Instance.StateData.PlayerState.clearActionAndTarget();
        }

        private void Update() {
            if (controlDisabledOverride || (mouseOverUnpausedScene() && !controlEnabled)) {
                EventManager.Instance.raise(new GameEvents.CursorEvent(CursorType.DISABLED));
            }        
        }

        private void handleMouseEvent(GameEvents.MouseEvent e) {
            if (!controlDisabledOverride && mouseOverUnpausedScene() && controlEnabled) {

                MouseHandler mouseHandler = getMouseHandler (e);

                if (e.MouseButton == 0) {
                    GameState.Instance.StateData.PlayerState.SceneTarget = null;                                    
                    mouseHandler.handleLeftClick (e.MousePos, inventory.SelectedItem);
                }

                if (controlEnabled) {
                    mouseHandler.handleMouseOver(inventory.SelectedItem);
                }
            }
        }

        private static bool mouseOverUnpausedScene() {
            return !Pause.Instance.isPaused() && InteractionState.Instance.isEmpty() && !InteractionState.Instance.isMouseOverMenu();
        }

        public void setControlEnabled(bool controlEnabled, string byId) {
            if (!controlEnabled) {
                disabledBy.Add(byId);
            }
            else {
                disabledBy.Remove(byId);
            }

            bool oldEnabled = this.controlEnabled;
            this.controlEnabled = disabledBy.Count == 0;
            
            bool controlEnabledChanged = this.controlEnabled != oldEnabled;

            if (controlEnabledChanged && !controlDisabledOverride) {
                if (controlEnabled) {
                    EventManager.Instance.raise(new GameEvents.PlayerControlEnabledEvent());
                }
                else {
                    EventManager.Instance.raise(new GameEvents.PlayerControlDisabledEvent());
                }
            }
        }

        public bool isControlEnabled() {
            return controlEnabled && !controlDisabledOverride;
        }

        private MouseHandler getMouseHandler(GameEvents.MouseEvent e) {            
            if (!MouseUtil.isHovering()) {
                return sceneMouseHandler;
            }
            
            MouseHandler handler = getSceneItemMouseHandler(e.MousePos);

            if (handler == null) {
                handler = sceneMouseHandler;
            }

            return handler;
        }

        private MouseHandler getSceneItemMouseHandler (Vector3 pos) {
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);

            MouseHandler topMouseHandler = null;
            Renderer topAllRenderer = null;

            foreach (RaycastHit2D hit in hits) {
                if (hit.collider != null) {					
                    GameObject go = hit.collider.gameObject;
                    MouseHandler handler = go.GetComponent<MouseHandler>();
                    if (handler != null) {
                        Renderer renderer = go.getTopActiveRenderer ();

                        if (renderer == null && topAllRenderer == null) {
                            topMouseHandler = handler;
                        }
                        else if(
                            renderer != null && 
                            (
                                topAllRenderer == null
                                || renderer.sortingLayerID > topAllRenderer.sortingLayerID
                                || renderer.sortingLayerID == topAllRenderer.sortingLayerID && renderer.sortingOrder > topAllRenderer.sortingOrder
                            )						
                        ) {
                            topAllRenderer = renderer;
                            topMouseHandler = handler;
                        }
                        
                    }
                }
            }

            return topMouseHandler;
        }
			
    }

}