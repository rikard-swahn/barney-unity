using System;
using System.Collections.Generic;
using Mgl;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.scene.load;
using net.gubbi.goofy.scene.load.transition;
using net.gubbi.goofy.state;
using net.gubbi.goofy.state.settings;
using net.gubbi.goofy.util;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui.menu.main {
    public class GamesMenu : Menu {
        
        public string[] partsStartScene;
        
        public Menu rootMenu;
        public ConfirmMenu confirmMenu;
        
        public RectTransform slot1;
        public RectTransform slot2;
        public RectTransform slot3;
        public RectTransform backButton;
        
        public MessageMenu messageMenu;
        
        private float xDelta;
        private float backY;        

        private IList<GameObject> slots; 
        private PersistFacade _persistFacade;
        private readonly string[] slotNames = {"A", "B", "C"};
        
        private static readonly string I18N_SLOT_EMPTY = "GameSlotGameEmpty";        
        private static readonly string I18N_WATCH_AGAIN = "GameSlotWatchAgain";        
        private static readonly string I18N_INTRO_PARTS = "GameSlotIntroParts";        
        private static readonly string I18N_INTRO = "GameSlotIntro";
        private static readonly string I18N_ENDING_PARTS = "GameSlotEndingParts";
        private static readonly string I18N_ENDING = "GameSlotEnding";
        private static readonly string I18N_PART = "GameSlotPart";
        private static readonly string I18N_SCENE_PARTS = "GameSlotSceneParts";

        private int numParts;

        protected override void Awake() {    
            slots = gameObject.findChildrenWithTag(Tags.GAME_SLOT);
            _persistFacade = GameObject.FindGameObjectWithTag(Tags.ROOM).GetComponent<PersistFacade>();
            base.Awake();
            
            xDelta = slot2.anchoredPosition.x;
            backY = backButton.anchoredPosition.y;
            numParts = partsStartScene.Length;
        }
        
        public override void show() {
            base.show();
            updateSlots();
            playTweenSfx();
            Action onComplete = delegate {setInteractable(true);};
            translateToX(0, 0.5f, onComplete, slot1, slot2, slot3);            
            translateToY(-125, 0.5f, null, backButton);  
        }

        public override void hide(Action onComplete = null) {
            setInteractable(false);
            Action onCompleteWrapper = () => {
                base.hide();
                if (onComplete != null) {
                    onComplete();
                }
            };
            translateX(-xDelta, 0.5f, null, slot1, slot3);            
            translateX(xDelta, 0.5f, onCompleteWrapper, slot2);            
            translateToY(backY, 0.5f, null, backButton);                         
        }

        public void back() {
            hide(rootMenu.show);            
            playClickSfx();
        }

        public void slotSelected(int index) {
            GameSlot slot = SettingsState.Instance.StateData.Games.Slots[index];
            
            if (slot.Empty) {
                //Slot is empty, start a new game
                newGame(index, 1);
            }
            else if (!slot.GameStateExist) {
                //Slot is selected before, but there is no save game, start part from beginning.
                newGame(index, slot.CurrentPart);
            }
            //If a part is completed, and there is a next part
            else if (slot.PartComplete && slot.CurrentPart < numParts) {
                //Start next part
                SettingsState.Instance.deleteGameSlot(index);
                newGame(index, slot.CurrentPart + 1);
            }         
            //If a part is completed (and there is no next part, ref above)
            //or a part is not completed (ref above)
            else {                
                continueGame(index);
            }
            playClickSfx();
        }
        
        private void newGame(int index, int part) {
            SettingsState.Instance.setCurrentGame(index);
            SettingsState.Instance.setCurrentPart(part);                                    
            SettingsState.Instance.markSlotNotEmpty();                                    
            GameState.Instance.StateData.Scene = partsStartScene[part - 1];
            SceneLoader.Instance.loadScene (GameState.Instance.StateData.Scene, Transitions.get(Transitions.Type.CIRCLE_OUT), Transitions.get(Transitions.Type.CIRCLE_IN));
        }
        
        public void delete(int index) {
            Action onConfirm = delegate {
                _persistFacade.deleteGame(index);
                updateSlots();                
            };

            string detailText = I18n.Instance.__("DeleteGameConfirmText", slotNames[index]);
            confirmMenu.show(detailText, onConfirm);            
            playClickSfx();
        }            

        private void continueGame(int index) {            
            PersistFacade.load(index, Transitions.get(Transitions.Type.CIRCLE_OUT), Transitions.get(Transitions.Type.CIRCLE_IN), onLoadGameError);
        }

        private void onLoadGameError(string message) {
            string detailText = I18n.Instance.__("LoadGameError");
            messageMenu.show(detailText);  
        }

        private void updateSlots() {
            List<GameSlot> slotState = SettingsState.Instance.StateData.Games.Slots;
            
            for (int i = 0; i < slots.Count; i++) {
                GameSlot slot = slotState[i];
                Image deleteImage = slots[i].findChildWithTag(Tags.DELETE).GetComponent<Image>();
                Text description = slots[i].findChildWithTag(Tags.DESCRIPTION).GetComponent<Text>();
                                
                deleteImage.enabled = !slot.Empty;
                
                if (slot.Empty) {
                    deleteImage.enabled = false;
                    description.text = I18n.Instance.__(I18N_SLOT_EMPTY);
                }
                else if (!slot.GameStateExist) {                    
                    if (numParts > 1) {
                        description.text = I18n.Instance.__(I18N_INTRO_PARTS, slot.CurrentPart);
                    }
                    else {
                        description.text = I18n.Instance.__(I18N_INTRO);
                    }
                }
                else if (slot.PartComplete && slot.CurrentPart < numParts) {                    
                    description.text = I18n.Instance.__(I18N_PART, slot.CurrentPart + 1);
                }
                else if (slot.PartComplete) {                    
                    description.text = numParts > 1
                        ? I18n.Instance.__(I18N_ENDING_PARTS, slot.CurrentPart)
                        : I18n.Instance.__(I18N_ENDING);
                    
                    description.text += " " + I18n.Instance.__(I18N_WATCH_AGAIN);
                }                
                else if (slot.PartEnding) {
                    description.text = numParts > 1
                        ? I18n.Instance.__(I18N_ENDING_PARTS, slot.CurrentPart)
                        : I18n.Instance.__(I18N_ENDING);                                                            
                }
                else {

                    string sceneDesctiption = I18n.Instance.__(slot.Description);
                    
                    description.text = numParts > 1
                        ? I18n.Instance.__(I18N_SCENE_PARTS, slot.CurrentPart, sceneDesctiption)
                        : sceneDesctiption;
                }
            }
        }        
    }
}