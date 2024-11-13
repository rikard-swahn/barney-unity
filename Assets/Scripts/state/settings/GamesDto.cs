using System.Collections.Generic;

namespace net.gubbi.goofy.state.settings {
    
    public class GamesDto {
        public List<GameSlotDto> slots;

        public GamesDto() { }

        public GamesDto(Games games) {
            slots = new List<GameSlotDto>();
            foreach (var slot in games.Slots) {
                slots.Add(new GameSlotDto(slot));
            }
        }
    }
}