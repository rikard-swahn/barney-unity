namespace net.gubbi.goofy.item.inventory.action {
	
    public class ItemTypeCombo {
        private ItemType item1;
        private ItemType item2;

        public ItemTypeCombo (ItemType item1, ItemType item2) {
            this.item1 = item1;
            this.item2 = item2;
        }
			
        public override bool Equals (object obj) {
            if (obj == null)
                return false;
            if (ReferenceEquals (this, obj))
                return true;
            if (obj.GetType () != typeof(ItemTypeCombo))
                return false;
            ItemTypeCombo other = (ItemTypeCombo)obj;
            return item1 == other.item1 && item2 == other.item2 || item1 == other.item2 && item2 == other.item1;
        }		

        public override int GetHashCode () {
            unchecked {
                return item1.GetHashCode () ^ item2.GetHashCode ();
            }
        }		
    }

}