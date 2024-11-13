using Mgl;
using net.gubbi.goofy.character;
using net.gubbi.goofy.events;
using net.gubbi.goofy.item;
using net.gubbi.goofy.item.inventory;
using net.gubbi.goofy.state;
using net.gubbi.goofy.ui.cursor;
using net.gubbi.goofy.util;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.input {
    public class SceneMouseHandler : MouseHandler {
        
        private readonly CharacterMove playerMove;
        private readonly bool freeWalk;
        private readonly Text itemText;
        private readonly Text itemTextInventory;
        
        private Inventory inventory;

        public SceneMouseHandler(CharacterMove playerMove, bool freeWalk) {
            this.playerMove = playerMove;
            this.freeWalk = freeWalk;
            itemText = GameObject.Find(GameObjects.ITEM_TEXT).GetComponent<Text> ();
            itemTextInventory = GameObject.Find(GameObjects.ITEM_TEXT_INVENTORY).GetComponent<Text> ();
            inventory = GameObject.Find(GameObjects.INVENTORY).GetComponent<Inventory>();
        }

        public void handleLeftClick(Vector3 mousePos, ItemType selectedItem) {
            //TODO: clear taget on nav complete?
            if (selectedItem == ItemType.EMPTY && freeWalk) {
                Vector3 dest = Camera.main.ScreenToWorldPoint (mousePos);
                playerMove.SetDestination (dest);
                GameState.Instance.StateData.PlayerState.SceneTarget = new Vector2(dest.x, dest.y);                
            }
            else if (freeWalk) {                
                playerMove.abortNavigation();                
            }            
            
            inventory.deselectItem();
        }

        public void handleMouseOver(ItemType selectedItem) {            
            EventManager.Instance.raise(new GameEvents.CursorEvent(CursorType.NORMAL));

            itemText.text = "";

            if (selectedItem != ItemType.EMPTY && !playerMove.hasNavigationFinishCallback()) {
                itemTextInventory.text = I18n.Instance.__("UseWithSomething", inventory.getItem(selectedItem).getLabel());
            }
            else {
                itemTextInventory.text = "";
            }
        }
    }
}