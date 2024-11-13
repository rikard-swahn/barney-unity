using System;

namespace net.gubbi.goofy.state.settings {
    public class GameSlot {

        public bool Empty {get; set;}
        public bool GameStateExist {get; set;}
        public string Description {get; set;}
        public bool AutoSaveEnabled {get; set;}
        public int CurrentPart {get; set;}
        public bool PartEnding {get; set;}
        public DateTime SaveTime {get; set;}
        public float GameStateVersion {get; set;}
        public bool PartComplete {get; set;}
        public DateTime PartStartTime {get; set;}

        public GameSlot() {
            Empty = true;
        }

        public GameSlot(GameSlotDto dto) {
            Empty = dto.empty;
            GameStateExist = dto.gameStateExist;
            Description = dto.description;
            AutoSaveEnabled = dto.autoSaveEnabled;
            SaveTime = dto.saveTime;
            GameStateVersion = dto.gameStateVersion;
            CurrentPart = dto.currentPart;
            PartComplete = dto.partComplete;
            PartStartTime = dto.partStartTime;
        }
    }
}