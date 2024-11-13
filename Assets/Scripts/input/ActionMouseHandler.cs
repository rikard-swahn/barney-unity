﻿using Mgl;
 using net.gubbi.goofy.character;
 using net.gubbi.goofy.events;
 using net.gubbi.goofy.extensions;
 using net.gubbi.goofy.item;
 using net.gubbi.goofy.item.inventory;
 using net.gubbi.goofy.scene.action;
 using net.gubbi.goofy.ui.cursor;
 using net.gubbi.goofy.ui.menu.ingame;
 using net.gubbi.goofy.util;
 using PolyNav;
 using UnityEngine;
 using UnityEngine.UI;

namespace net.gubbi.goofy.input {
    public class ActionMouseHandler : MonoBehaviour, MouseHandler {

        public SceneActionHandler actionHandler;
        
        private Text itemText;
        private Text itemTextInventory;
        private LabelProvider labelProvider;
        private PolyNavAgent navAgent;
        private CharacterMove playerMove;
        private Inventory inventory;

        private void Awake () {
            if (actionHandler == null) {
                actionHandler = GetComponent<SceneActionHandler> ();    
            }            
            itemText = GameObject.Find(GameObjects.ITEM_TEXT).GetComponent<Text>();
            itemTextInventory = GameObject.Find(GameObjects.ITEM_TEXT_INVENTORY).GetComponent<Text> ();            
            labelProvider = GetComponent<LabelProvider> ();
            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            navAgent = playerGo.GetComponentInChildren<PolyNavAgent>();            
            playerMove = playerGo.GetComponent<CharacterMove>();
            inventory = GameObject.Find(GameObjects.INVENTORY).GetComponent<Inventory>();
        }

        public void handleMouseOver(ItemType selectedItem) {
            if (selectedItem != ItemType.EMPTY && !playerMove.hasNavigationFinishCallback()) {
                itemTextInventory.text = I18n.Instance.__("UseWithSomething", inventory.getItem(selectedItem).getLabel());
            }
            else {
                itemTextInventory.text = "";
            }          
            
            if (navAgent.abortNextTarget() && navAgent.HasFinishCallback()) {
                EventManager.Instance.raise(new GameEvents.CursorEvent(CursorType.NORMAL));
                itemText.text = getItemLabel();      
            }
            else {
                EventManager.Instance.raise(new GameEvents.CursorEvent(CursorType.ACTION));
                itemText.text = getLabel (selectedItem);        
            }
        }

        public void handleLeftClick(Vector3 mousePos, ItemType selectedItem) {		
            actionHandler.doAction (selectedItem);			
        }

        private string getItemLabel() {
            return labelProvider != null ? labelProvider.getLabel () : "";            
        }

        private string getLabel(ItemType selectedItem) {                        
            if (labelProvider == null) {
                return "";
            }            
            
            string itemLabel = labelProvider.getLabel ();
            
            if (selectedItem != ItemType.EMPTY) {
                return itemLabel;                
            } else {
                string prefix = actionHandler.getLabelPrefix(selectedItem);
                
                if (!prefix.isNullOrEmpty()) {
                    return prefix + " " + itemLabel;
                }
                
                return itemLabel;
            }                        
        }
    }
}