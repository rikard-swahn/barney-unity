using System;
using Mgl;

namespace net.gubbi.goofy.item {
    public class CardItem : Item {

        public CardItem (int amount) : base(ItemType.BANK_CARD) {			
            setAmount(amount);
        }

        public CardItem(ItemDto itemDto) : base(itemDto) {}

        public override string getLabel() {
            int? amount = getAmount ();

            if (amount != null) {
                return I18n.Instance.__("BankCardItem", amount);	
            }

            throw new Exception ("Cash must have an amount!");
        }
    }
}