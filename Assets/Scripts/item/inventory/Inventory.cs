using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using Mgl;
using net.gubbi.goofy.character;
using net.gubbi.goofy.events;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.game;
using net.gubbi.goofy.item.inventory.action;
using net.gubbi.goofy.player;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.item.inventory {
    public class Inventory : MonoBehaviour {

        public Image[] slotImages;
        public Image[] slotContainerImages;
        public Sprite selectedSlotContainer;        
        public bool disabled;
        public GameObject uiObject;               
        public ItemType SelectedItem {get; private set;}
        public GameObject shiftLeftButton;
        public GameObject shiftRightButton;
        
        private bool inputEnabled = true;
        private Sprite unselectedSlotContainer;        
        private int itemsOffset;
        private Text itemText;
        private Text itemTextInventory;
        private Actions actions;
        private ItemType mouseOverItem = ItemType.EMPTY;
        private int? mouseOverSlot;
        private CharacterFacade playerFacade;
        private ItemSprites itemSprites;
        private PlayerMove playerMove;        

        private void Awake () {            
            setSelectedItem(GameState.Instance.StateData.SelectedItem);
            itemText = GameObject.Find(GameObjects.ITEM_TEXT).GetComponent<Text> ();
            itemTextInventory = GameObject.Find(GameObjects.ITEM_TEXT_INVENTORY).GetComponent<Text> ();
            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            playerFacade = playerGo.GetComponent<CharacterFacade>();
            playerMove = playerGo.GetComponent<PlayerMove>();
            actions = GetComponentInChildren<Actions> ();
            itemSprites = GetComponentInChildren<ItemSprites> ();
            EventManager.Instance.addListener<GameEvents.PlayerControlEnabledEvent>(playerControlEnabledHandler);
            EventManager.Instance.addListener<GameEvents.PlayerControlDisabledEvent>(playerControlDisabledHandler);
            EventManager.Instance.addListener<GameEvents.MouseEvent>(handleMouseEvent);
            updateUiObjectActive();
            unselectedSlotContainer = slotContainerImages[0].sprite;

            if (disabled) {
                deselectItem();
            }
        }

        private void Start() {
            itemsOffset = getStartItemsOffset();
            refreshSlots();
        }

        private void Update() {            
            foreach (Item item in GameState.Instance.StateData.PlayerItems.Values) {
                item.Update();
            }
        }

        private int getStartItemsOffset() {
            int? offset = GameState.Instance.StateData.InventoryState.ItemsOffset;
            if (offset != null) {
                return (int) offset;
            }
            
            return 0;
        }

        private void handleMouseEvent(GameEvents.MouseEvent e) {
            if (InteractionState.Instance.isMouseOverMenu()) {
                clearItemTextInventory();
                itemText.text = "";
            }
            
            if (!isInputEnabled()) {
                return;
            }               
            
            //Right click
            if (e.MouseButton == 1) {
                deselectItem ();
            }

            if (gameObject.isMouseOverChildWithTag(Tags.MENU_AREA)) {
                refreshItemText ();
            }
        }

        public void shiftSelectedLeft() {          
            itemsOffset--;
            setItemsOffset(Mathf.Max (0, itemsOffset));
            refreshSlots();
        }

        public void shiftSelectedRight() {
            itemsOffset++;					
            limitOffsetToMax ();
            refreshSlots();
        }

        public void selectItem(int slot) {
            ItemType clickedItem = getItemForSlot (slot);
            if (clickedItem != ItemType.EMPTY) {

                playerMove.abortNavigation();                

                if (SelectedItem != ItemType.EMPTY && clickedItem != SelectedItem) {       
					
                    InventoryAction invAction = actions.getAction (SelectedItem, clickedItem);
                    playerFacade.stopAndTurnTowardsCamera ();
                    if (invAction != null) {
                        invAction.doAction ();
                        deselectItem ();
                    } else {
                        playerFacade.sayTextTowardsCamera (I18nUtil.randomOption("FailedAction"));
                        deselectItem ();
                    }

                }
                else if (SelectedItem != ItemType.EMPTY) {
                    //Clicked on already selected item
                    deselectItem();
                }
                else {
                    InventoryAction invAction = actions.getAction (ItemType.EMPTY, clickedItem);
                    if (invAction != null) {
                        //There is an action attached to a single item, which is run on clicking one item,
                        //and not a combination of two items
                        invAction.doAction ();
                    }
                    else {
                        setSelectedItem(clickedItem);    
                    }
                    
                    
                }
            }
            
            refreshSlots();
        }

        public void deselectItem() {
            setSelectedItem(ItemType.EMPTY);
            refreshSlots();
        }

        public void handleMouseEnterItem(int slot) {
            mouseOverSlot = slot;
            refreshMouseOverItem();
        }

        public void handleMouseExitItem() {
            mouseOverSlot = null;
            refreshMouseOverItem();
        }

        public void addItem (Item item) {
            bool hadItem = hasItem (item.Type);

            if (!GameState.Instance.StateData.addItemToPlayer (item)) {
                removeItem (item.Type);
            } else {
                if (!hadItem) {
                    setItemsOffset(getMaxItemsOffset ());
                }
                refreshSlots();
            }
        }

        public void addItem (ItemType type) {
            bool hadItem = hasItem (type);
            
            if (!hadItem) {
                GameState.Instance.StateData.addItemToPlayer (type);
                setItemsOffset(getMaxItemsOffset ());
                refreshSlots();
            }            
        }

        public void removeItem(ItemType type) {
            GameState.Instance.StateData.removeItemFromPlayer (type);

            if (SelectedItem == type) {
                setSelectedItem(ItemType.EMPTY);
            }

            limitOffsetToMax ();

            refreshSlots();
        }

        public bool hasItem(params Item[] items) {
            foreach(Item item in items) {
                Item invItem = GameState.Instance.StateData.getItem(item.Type);
                if(invItem == null) {
                    return false;
                }

                if(!invItem.hasAmountOf(item)) {
                    return false;
                }
            }

            return true;			
        }

        public bool hasItem(params ItemType[] items) {
            foreach(ItemType item in items) {
                Item invItem = GameState.Instance.StateData.getItem(item);
                if(invItem == null) {
                    return false;
                }
            }

            return true;	
        }

        public Item getItem(ItemType item) {
            return GameState.Instance.StateData.getItem(item);
        }

        public T getItem<T>(ItemType item) where T : Item {
            return (T)GameState.Instance.StateData.getItem(item);
        }
        
        public bool hasItemWithFlag(ItemType itemType, string flag) {
            Item item = GameState.Instance.StateData.getItem(itemType);

            if (item == null) {
                return false;
            }

            if (!item.hasProperty(flag)) {
                return false;
            }

            return item.getProperty(flag).getBool();
        }

        private void setItemsOffset(int offset) {
            GameState.Instance.StateData.InventoryState.ItemsOffset = offset;
            itemsOffset = offset;
        }

        private void limitOffsetToMax () {
            setItemsOffset(Mathf.Min (getMaxItemsOffset(), itemsOffset));
        }

        private void refreshItemText () {                  
            itemText.text = "";
            
            if (mouseOverItem != ItemType.EMPTY) {
                if (playerMove.hasNavigationFinishCallback() || SelectedItem == ItemType.EMPTY || SelectedItem == mouseOverItem) {
                    itemTextInventory.text = ((Item)GameState.Instance.StateData.PlayerItems [mouseOverItem]).getLabel ();
                }                
                else  {
                    itemTextInventory.text = I18n.Instance.__("UseWith", getItem(SelectedItem).getLabel(), getItem(mouseOverItem).getLabel());  
                } 
            } else { //Mouse not over inv item

                if (playerMove.hasNavigationFinishCallback() || SelectedItem == ItemType.EMPTY) {
                    clearItemTextInventory ();
                }                
                else {
                    itemTextInventory.text = I18n.Instance.__("UseWithSomething", getItem(SelectedItem).getLabel());
                }
            }
        }

        private void refreshSlots() {
            if (disabled) {
                return;
            }
            
            refreshSlotImages();
            refreshMouseOverItem();
            refreshArrowButtons();
        }

        private void refreshArrowButtons() {
            shiftLeftButton.SetActive(itemsOffset > 0);
            shiftRightButton.SetActive(itemsOffset < getMaxItemsOffset());
        }

        private void refreshMouseOverItem() {
            mouseOverItem = (mouseOverSlot != null) ? getItemForSlot((int)mouseOverSlot) : ItemType.EMPTY;
        }

        private void refreshSlotImages() {
            for (int slot = 0; slot < slotImages.Length; slot++) {
                ItemType slotItem = getItemForSlot (slot);

                if (slotItem != ItemType.EMPTY) {
                    slotImages[slot].sprite = itemSprites.getSprite(slotItem);
                    slotImages[slot].enabled = true;
                } else {
                    slotImages[slot].sprite = null;
                    slotImages[slot].enabled = false;
                }

                if (SelectedItem == slotItem && SelectedItem != ItemType.EMPTY) {
                    slotContainerImages[slot].sprite = selectedSlotContainer;
                }
                else {
                    slotContainerImages[slot].sprite = unselectedSlotContainer;
                }
            }
        }

        private ItemType getItemForSlot(int slot) {
            int slotItemIndex = itemsOffset + slot;

            IOrderedDictionary playerItems = GameState.Instance.StateData.PlayerItems;

            return slotItemIndex < playerItems.Count ?
                (ItemType)playerItems.Cast<DictionaryEntry> ().ElementAt (slotItemIndex).Key :
                ItemType.EMPTY;
        }

        private void playerControlEnabledHandler(GameEvents.PlayerControlEnabledEvent e) {
            inputEnabled = true;
            updateUiObjectActive();
        }

        private void playerControlDisabledHandler(GameEvents.PlayerControlDisabledEvent e) {
            clearItemTextInventory();
            inputEnabled = false;
            updateUiObjectActive();
        }

        private int getMaxItemsOffset () {
            int numItems = GameState.Instance.StateData.PlayerItems.Count;
            int maxItemsOffset = numItems - slotImages.Length;
            return Mathf.Max (0, maxItemsOffset);
        }

        private void setSelectedItem(ItemType selected) {
            SelectedItem = selected;
            GameState.Instance.StateData.SelectedItem = selected;
        }

        public void setItemFlag(ItemType type, string flag, bool value) {
            if (!hasItem(type)) {
                throw new ArgumentException("Player does not have item " + type);
            }

            getItem(type).setProperty(flag, value);
        }

        private bool isInputEnabled() {
            return InteractionState.Instance.isEmpty() && inputEnabled;
        }

        private void updateUiObjectActive() {
            uiObject.SetActive(!disabled && inputEnabled);
        }

        private void clearItemTextInventory() {
            itemTextInventory.text = "";
        }
    }
}