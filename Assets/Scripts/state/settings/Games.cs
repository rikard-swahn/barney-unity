using System.Collections.Generic;

namespace net.gubbi.goofy.state.settings {
    public class Games {
        private static readonly int MAX_GAMES = 3;
        
        public List<GameSlot> Slots {get; private set;}
        public int? CurrentGame {get; set;}        

        public Games() {
            Slots = new List<GameSlot>();
            for (int i = 0; i < MAX_GAMES; i++) {
                Slots.Add(new GameSlot());
            }
        }

        public Games(GamesDto games) {
            Slots = new List<GameSlot>();
            foreach (var slot in games.slots) {
                Slots.Add(new GameSlot(slot));
            }
        }
    }
}