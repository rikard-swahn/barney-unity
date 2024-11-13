namespace state {
    
    public class InventoryStateDto {
        public int? itemsOffset;
        
        public InventoryStateDto() {            
        }

        public InventoryStateDto(InventoryState state) {
            itemsOffset = state.ItemsOffset;
        }        
    }
}