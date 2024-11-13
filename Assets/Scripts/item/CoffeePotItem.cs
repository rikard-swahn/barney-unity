using Mgl;

namespace net.gubbi.goofy.item {
    public class CoffeePotItem : Item {
        
        private float COFFEE_COLD_AGE = 30f;
        
        public CoffeePotItem () : base(ItemType.COFFEE_POT) {}
        public CoffeePotItem(ItemDto itemDto) : base(itemDto) {}
                
        public override string getLabel() {
            return I18n.Instance.__(isCold() ? "ColdCoffeePotItem" : "HotCoffeePotItem");	
        }

        public bool isCold() {
            return getAge() != null && getAge() > COFFEE_COLD_AGE;
        }

    }
}