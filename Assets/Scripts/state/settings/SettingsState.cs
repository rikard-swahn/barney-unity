using System;
using System.Collections.Generic;
using System.IO;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.unity;
using net.gubbi.goofy.util;
using UnityEngine;
using Object = System.Object;

namespace net.gubbi.goofy.state.settings {
    public class SettingsState {
        private static SettingsStateData stateData = new SettingsStateData();

        private static volatile SettingsState instance;
        private static readonly object syncRoot = new Object();
        private SettingsState() { }

        public static SettingsState Instance {
            get {
                if (instance == null) {
                    lock (syncRoot) {
                        if (instance == null) {
                            instance = new SettingsState();
                        }
                    }
                }
                return instance;
            }
        }

        public void save() {
            SettingsStateDataDto dto = new SettingsStateDataDto(stateData);
            string json = JsonUtil.Instance.Serialize(dto);
            using (StreamWriter streamWriter = File.CreateText (getFilename())) {
                streamWriter.Write (json);
            }
        }

        public void load() {
            string fileName = getFilename();
            if (File.Exists(fileName)) {
                try {
                    using (StreamReader streamReader = File.OpenText(fileName)) {
                        string json = streamReader.ReadToEnd();
                        var dto = JsonUtil.Instance.Deserialize<SettingsStateDataDto>(json);
                        stateData = new SettingsStateData(dto);
                    }
                }
                catch (Exception e) {
                    stateData = new SettingsStateData();
                    throw e;
                }
            }
        }

        public SettingsStateData StateData {
            get { return stateData; }
        }

        public void setCurrentGame(int index) {
            StateData.Games.CurrentGame = index;
        }

        public void setSavedGameData(int index) {
            string sceneName = GameState.Instance.StateData.Scene;            
            StateData.Games.Slots[index].Description = "Scene-" + sceneName;
            StateData.Games.Slots[index].GameStateExist = true;
            StateData.Games.Slots[index].SaveTime = DateTime.UtcNow;
            StateData.Games.Slots[index].GameStateVersion = GameState.VERSION;
            save();
        }
        
        public void setCurrentPart(int part) {
            getCurrentGameSlot().CurrentPart = part;
            getCurrentGameSlot().PartStartTime = DateTime.UtcNow;
            save();
        }        
        
        public void markSlotNotEmpty() {
            getCurrentGameSlot().Empty = false;            
            save();
        }        
        
        public void setPartEnding() {
            getCurrentGameSlot().PartEnding = true;
            save();
        }       
        
        public void setPartComplete() {
            if (!getCurrentGameSlot().PartComplete) {
                Debug.Log("part complete event");
                AnalyticsFacade.Instance.CustomEvent("Gubbi_Part_Complete", new Dictionary<string, object> {
                    {"StartTime", getPartStartTime().ToStringStandard()},
                    {"CurrentTime", DateTime.UtcNow.ToStringStandard()},
                    {"GameTime", GameState.Instance.StateData.PartGameTimeSeconds},
                    {"OS", SystemInfo.operatingSystem}
                });
            }

            getCurrentGameSlot().PartComplete = true;
            save();
        }

        public void setMusicEnabled(bool musicEnabled) {
            StateData.MusicEnabled = musicEnabled;
            save();
        }

        public void setSfxEnabled(bool sfxEnabled) {
            StateData.SfxEnabled = sfxEnabled;
            save();
        }
        
        public void setGameSpeed(int gameSpeed) {
            StateData.GameSpeed = gameSpeed;
            save();
        }
        
        public void setDonated() {
            StateData.HasDonated = true;
            save();
        }

        public void clearCurrentGame() {
            StateData.Games.CurrentGame = null;
        }

        public void deleteGameSlot(int index) {
            setSlot(index, new GameSlot());
            save();
        }

        public void setAutoSaveEnabled(int index, bool enabled) {
            StateData.Games.Slots[index].AutoSaveEnabled = enabled;
            save();
        }

        public DateTime getPartStartTime() {
            return getCurrentGameSlot().PartStartTime;
        }
        
        public GameSlot getCurrentGameSlot() {
            int index = (int)getCurrentGame();
            return StateData.Games.Slots[index];
        }

        public int? getCurrentGame() {
            if (StateData.Games.CurrentGame == null && Application.isEditor) {
                return 0;
            }
            
            return StateData.Games.CurrentGame;
        }

        private void setSlot(int index, GameSlot slot) {
            StateData.Games.Slots[index] = slot;
        }
        
        private string getFilename() {
            return Path.Combine(Application.persistentDataPath, "settings.json");                        
        }
    }
}