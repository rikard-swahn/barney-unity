namespace state {
    public class InventoryState {
        public int? ItemsOffset { get; set;}

        public InventoryState() {            
        }

        public InventoryState(InventoryStateDto stateDto) {
            ItemsOffset = stateDto.itemsOffset;
        }
    }
}