using System;

namespace net.gubbi.goofy.state.settings {
    
    public class GameSlotDto {
        public bool empty;
        public bool gameStateExist;
        public string description;
        public bool autoSaveEnabled;
        public DateTime saveTime;
        public float gameStateVersion;
        public bool partComplete;
        public int currentPart;
        public DateTime partStartTime;

        public GameSlotDto() { }

        public GameSlotDto(GameSlot gameSlot) {
            empty = gameSlot.Empty;
            gameStateExist = gameSlot.GameStateExist;
            description = gameSlot.Description;
            autoSaveEnabled = gameSlot.AutoSaveEnabled;
            saveTime = gameSlot.SaveTime;
            gameStateVersion = gameSlot.GameStateVersion;
            currentPart = gameSlot.CurrentPart;
            partStartTime = gameSlot.PartStartTime;
            partComplete = gameSlot.PartComplete;
        }
    }
}