using System;
using net.gubbi.goofy.audio;
using net.gubbi.goofy.character;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.scene.item;
using net.gubbi.goofy.scene.load;
using net.gubbi.goofy.scene.load.transition;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene.door {
    public class Door : MonoBehaviour {

        public bool setMapPosition;
        public int mapPosition;        

        public Vector2 entryPosition;
        public bool keepXPosition;
        public bool keepYPosition;

        public Vector2 entryDirection;
        public bool keepDirection;

        public string goToScene;
        public Waypoint insideWaypoint;

        public AudioClip sfxOpen;
        public AudioClip sfxClose;

        public bool defaultOpen;

        private GameObject playerGo;
        private CharacterMove playerMove;
        private Animator[] doorAnimators;
        private SfxPlayer sfxPlayer;
        
        private void Awake() {
            this.doorAnimators = GetComponentsInChildren<Animator>();

            bool open = GameState.Instance.StateData.getSceneProperty(gameObject.name, SceneItemProperties.OPEN, defaultOpen);
            setOpenVisual(open);

            if (GameState.Instance.StateData.hasSceneProperty(gameObject.name, SceneItemProperties.OPEN_COMPLETE)) {
                bool openComplete = GameState.Instance.StateData.getSceneProperty(gameObject.name, SceneItemProperties.OPEN_COMPLETE).getBool();
                setOpenComplete(openComplete);                
            }

            playerGo = GameObject.Find(GameObjects.PLAYER);
            playerMove = playerGo.GetComponent<CharacterMove>();
            sfxPlayer = GameObject.Find (GameObjects.SFX_PLAYER).GetComponent<SfxPlayer>();
        }      

        public void setOpenState(bool open) {
            setStateFlag(SceneItemProperties.OPEN, open);
        }

        public void setOpen(bool open) {
            if (setOpenVisual(open) != open) {
                sfxPlayer.playOnce(open ? sfxOpen : sfxClose);    
            }            
        }

        public void setOpenComplete(bool complete) {
            setStateFlag(SceneItemProperties.OPEN_COMPLETE, complete);
            setAnimatorFlag(AnimationParams.DOOR_OPEN_COMPLETE, complete);            
        }

        public void enter(bool hideCharacter, SceneTransition outSceneTransition = null, SceneTransition inSceneTransition = null, Action onOutComplete = null) {
            if (setMapPosition) {
                GameState.Instance.StateData.PlayerState.MapPosition = mapPosition;
                GameState.Instance.StateData.PlayerState.clearPosition();
            }
            else {
                Vector3 curPos = playerGo.transform.position;
                Vector2 newPos = new Vector2();
                newPos.x = keepXPosition ? curPos.x : entryPosition.x;
                newPos.y = keepYPosition ? curPos.y : entryPosition.y;
                playerMove.setPositionState(newPos);                
            }
                                                
            Vector2 curDir = playerMove.LastDirection;
            Vector2 newDir = keepDirection
                ? new Vector2(curDir.x, curDir.y)
                : new Vector2(entryDirection.x, entryDirection.y);
            playerMove.setDirectionState(newDir);

            if (hideCharacter) {
                GameItemUtil.setItemVisible (playerGo, false);    
            }
            
            SceneLoader.Instance.changeInGameScene(goToScene, outSceneTransition, inSceneTransition, onOutComplete);
        }

        public bool isOpen() {
            return GameState.Instance.StateData.getSceneProperty(gameObject.name, SceneItemProperties.OPEN, false);
        }

        /// <summary>
        /// Set the visual state and game state of the open property of the Door.  
        /// </summary>
        /// <param name="open"></param>
        /// <returns>The previous game state of the open property.</returns>
        private bool setOpenVisual(bool open) {
            bool wasOpen = isOpen();            
            setOpenState(open);
            setAnimatorFlag(AnimationParams.DOOR_OPEN, open);
            return wasOpen;
        }

        private void setStateFlag(string flag, bool set) {
            GameState.Instance.StateData.setSceneProperty(gameObject.name, flag, set);
        }

        private void setAnimatorFlag(string flag, bool set) {
            doorAnimators.ForEach(a => a.SetBool(flag, set));
        }
    }
}