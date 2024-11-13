using System.Collections.Generic;
using System.Linq;
using net.gubbi.goofy.item;
using state;
using state.game;

namespace net.gubbi.goofy.state {

    public class StateDataDto {
        public string scene;
        public PlayerStateDto playerState;
        public List<ItemDto> playerItems;
        public ItemType selectedItem;
        public Dictionary<string, Dictionary<string, Property>> sceneItemProperties;
        public List<string> flags;
        public Dictionary<string, CharacterStateDto> characterState;
        public ConversationStateDto conversationState;
        public List<Goal> goals;
        public InventoryStateDto inventoryState;
        public float partGameTimeSeconds;
        public float partStartedGameTimeSeconds;

        public StateDataDto() { }

        public StateDataDto(StateData stateData) {
            scene = stateData.Scene;                        
            
            playerState = new PlayerStateDto(stateData.PlayerState);
            playerItems = new List<ItemDto>();
            foreach(Item item in stateData.PlayerItems.Values) {
                playerItems.Add(new ItemDto(item));
            }
            selectedItem = stateData.SelectedItem;

            sceneItemProperties = stateData.SceneProperties;
            flags = stateData.Flags.ToList ();

            characterState = new Dictionary<string, CharacterStateDto> ();
            var entries = stateData.CharacterState.GetEnumerator ();
            while (entries.MoveNext()) {
                var current = entries.Current;
                characterState.Add (current.Key, new CharacterStateDto (current.Value));
            }
				
            conversationState = stateData.ConversationState != null ? new ConversationStateDto(stateData.ConversationState) : null;
            goals = stateData.Goals;

            inventoryState = new InventoryStateDto(stateData.InventoryState);

            partGameTimeSeconds = stateData.PartGameTimeSeconds;
            partStartedGameTimeSeconds = stateData.PartStartedGameTimeSeconds;
        }
			
    }

}