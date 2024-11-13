using System;
using Mgl;
using net.gubbi.goofy.audio;
using net.gubbi.goofy.item;
using net.gubbi.goofy.item.inventory;
using net.gubbi.goofy.scene.action;
using net.gubbi.goofy.scene.item;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene.scenes.map {
    public class MapItemSceneActionHandler : SceneActionHandler  {

        public int pos;
        public SceneItem sceneItem;
        public AudioClip sfxClick;
        
        private MapRoom mapRoom;
        private Inventory inventory;
        private SfxPlayer sfxPlayer;

        private void Awake () {
            mapRoom = GameObject.FindWithTag(Tags.ROOM).GetComponent<MapRoom>();
            inventory = GameObject.Find(GameObjects.INVENTORY).GetComponent<Inventory>();
            sfxPlayer = GameObject.Find (GameObjects.SFX_PLAYER).GetComponent<SfxPlayer>();
        }

        public override void doAction (ItemType selectedItem) {
            sfxPlayer.playOnce(sfxClick);
            
            GameState.Instance.StateData.PlayerState.SceneActionTargetName = gameObject.name;
            mapRoom.goTo (pos, onDestinationReachedCallback);
        }

        public override string getLabelPrefix(ItemType selectedItem) {
            return I18n.Instance.__("MapGoTo");
        }        
        
        private void onDestinationReachedCallback() {
            Action clearTarget = delegate {
                GameState.Instance.StateData.PlayerState.clearActionAndTarget();
                inventory.deselectItem();
            };

            sceneItem.doAction (ItemType.EMPTY, clearTarget);
        }

    }
}