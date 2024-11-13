using System;
using net.gubbi.goofy.character;
using net.gubbi.goofy.events;
using net.gubbi.goofy.item;
using net.gubbi.goofy.item.inventory;
using net.gubbi.goofy.scene.item;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene.action.handler {
    public class UITriggerSceneActionHandler : SceneActionHandler  {

        private SceneItem sceneItem;
        private CharacterFacade characterFacade;
        private Inventory inventory;

        private void Awake () {
            sceneItem = GetComponent<SceneItem>();
            characterFacade = GameObject.Find(GameObjects.PLAYER).GetComponent<CharacterFacade>();
            inventory = GameObject.Find(GameObjects.INVENTORY).GetComponent<Inventory>();
        }

        public override void doAction (ItemType selectedItem) {
            Action afterSceneItemAction = delegate {
                GameState.Instance.StateData.PlayerState.clearActionAndTarget();
                inventory.deselectItem();
                EventManager.Instance.raise(new GameEvents.UIActionEndEvent());
            };
            
            EventManager.Instance.raise(new GameEvents.UIActionStartEvent());
            GameState.Instance.StateData.PlayerState.SceneActionTargetName = gameObject.name;
            characterFacade.navStop();
            sceneItem.doAction(selectedItem, afterSceneItemAction);
        }

        public override string getLabelPrefix(ItemType selectedItem) {
            throw new NotImplementedException();
        }
        
    }
}