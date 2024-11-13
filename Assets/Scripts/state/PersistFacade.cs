﻿using System;
using System.Collections.Generic;
using net.gubbi.goofy.events;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.scene.load.transition;
using net.gubbi.goofy.state.settings;
 using net.gubbi.goofy.unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace net.gubbi.goofy.state {
    public class PersistFacade : MonoBehaviour {

        public bool autoSave = true;

        private void Awake() {
            EventManager.Instance.addListener<GameEvents.SceneChangeInCurrentGameInitEvent>(sceneChangeInGameHandler);
            EventManager.Instance.addListener<GameEvents.GameTimeEvent>(gameTimeHandler);
        }

        //TODO: This method does not belong in this class. Separate time/statistics script should do this.
        private void gameTimeHandler(GameEvents.GameTimeEvent e) {
            if (SettingsState.Instance.getCurrentGame() != null && !SettingsState.Instance.getCurrentGameSlot().PartComplete) {
                GameState.Instance.addGameTime(e.deltaTime);

                if (SettingsState.Instance.getCurrentGameSlot().AutoSaveEnabled) {
                    GameState.Instance.addGameTimeForStartedGame(e.deltaTime);
                }
            }
        }

        private void sceneChangeInGameHandler(GameEvents.SceneChangeInCurrentGameInitEvent e) {
            GameState.Instance.StateData.Scene = e.NextScene;
            HandleAutoSave();
        }

        private void OnApplicationQuit() {
            Debug.Log("OnApplicationQuit");
            HandleAutoSave();
        }

        private void OnApplicationPause(bool paused) {
            if (paused) {
                Debug.Log("OnApplicationPause");
                HandleAutoSave();
            }
        }

        public void HandleAutoSave() {
            if (autoSave && SettingsState.Instance.getCurrentGameSlot().AutoSaveEnabled) {
                save();
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void BeforeSceneLoad() {
            try {
                SettingsState.Instance.load();
            }
            catch (Exception e) {
                Debug.LogError("Error loading settings. Using default settings. Message: " + e.Message);
            }            

            if (GameState.Instance.StateData.Scene.isNullOrEmpty()) {
                GameState.Instance.StateData.Scene = SceneManager.GetActiveScene().name;
            }
        }

        public static void load(int index, SceneTransition outTransition, SceneTransition inTransition, Action<string> onError) {
            SettingsState.Instance.setCurrentGame(index);
            
            try {
                GameState.Instance.load(index, outTransition, inTransition);

                AnalyticsFacade.Instance.CustomEvent("Gubbi_Game_Loaded", new Dictionary<string, object> {
                    {"StartTime", SettingsState.Instance.getPartStartTime().ToStringStandard()},
                    {"CurrentTime", DateTime.UtcNow.ToStringStandard()},
                    {"GameTime", GameState.Instance.LoadingStateData.PartGameTimeSeconds}
                });
            }
            catch (Exception e) {
                Debug.LogError("Error loading game. Message: " + e.Message);
                AnalyticsFacade.Instance.CustomEvent("Gubbi_Game_Load_Error", new Dictionary<string, object> {
                    { "Message", e.Message},
                    { "stackTrace", e.StackTrace}
                });

                onError(e.Message);
            }
        }
        
        public void deleteGame(int index) {
            GameState.Instance.delete(index);
            SettingsState.Instance.deleteGameSlot(index);
        }        

        private void save() {      
            int index = (int)SettingsState.Instance.getCurrentGame();
            SettingsState.Instance.setSavedGameData(index);
            GameState.Instance.save (index);
        }

        public void clearCurrentGame() {
            SettingsState.Instance.clearCurrentGame();
            GameState.Instance.reset();
        }

        public void setAutoSaveEnabled(bool enabled) {
            //If autosave enabled in slot, and now it is disabled, trigger a save, to save the latest state first.
            if (SettingsState.Instance.getCurrentGameSlot().AutoSaveEnabled && !enabled) {
                HandleAutoSave();
            }
            
            int index = (int)SettingsState.Instance.getCurrentGame();
            SettingsState.Instance.setAutoSaveEnabled(index, enabled);            
        }
    }
}