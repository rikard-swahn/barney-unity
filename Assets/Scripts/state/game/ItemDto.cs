using System.Collections.Generic;
using state.game;

namespace net.gubbi.goofy.item {

    public class ItemDto {

        public Dictionary<string, Property> properties;
        public ItemType type;

        public ItemDto() { }

        public ItemDto (Item item) {
            properties = item.Properties;
            type = item.Type;
        }
        
    }
}