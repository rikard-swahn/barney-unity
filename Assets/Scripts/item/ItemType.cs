using System;
using System.Reflection;

namespace net.gubbi.goofy.item {
    public enum ItemType {

        [Attr("")]
        EMPTY,
        [Attr("TurdItem")]
        TURD,
        [Attr("Wrench")]
        WRENCH,
        [Attr("CoffeePotItem", true)]
        COFFEE_POT,
        [Attr("SocialSecurityFormItem")]
        SOCIAL_SEC_FORM,
        [Attr("FilledInSocialSecurityFormItem")]
        FILLED_SOCIAL_SEC_FORM,
        [Attr("PencilItem")]
        PENCIL,
        [Attr("ReceiptItem")]
        RECEIPT,
        [Attr("DildoItem")]
        DILDO,
        [Attr("ModemItem")]
        MODEM,
        [Attr("BankCardItem", true)]
        BANK_CARD,
        [Attr("WateringCanItem")]
        WATERING_CAN,
        [Attr("CannabisSeedsItem")]
        CANNABIS_SEEDS,
        [Attr("SuperFertilizerItem")]
        FERTILIZER,
        [Attr("CannabisItem")]
        CANNABIS,
        [Attr("WelfareCheckItem")]
        WELFARE_CHECK,
        [Attr("AdFlyerItem")]
        AD_FLYER,
        [Attr("CrowbarItem")]
        CROWBAR,
        [Attr("CruiseTicketsItem")]
        CRUISE_TICKETS,
        [Attr("RockItem")]
        ROCK,
        [Attr("RubberGloveItem")]
        GLOVE,
        [Attr("HairpinItem")]
        HAIRPIN,
        [Attr("CrackerItem")]
        CRACKER,
        [Attr("ModelPlaneItem")]
        MODEL_PLANE,
        [Attr("FilledWateringCanItem")]
        FILLED_WATERING_CAN ,
        [Attr("ToDoItem")]
        TO_DO_LIST        
    }

    public static class ItemTypes {
        public static string getLabel(this ItemType type) {
            Attr attr = getAttr (type);
            return attr.Label;
        }

        public static bool alwaysUseProperties(this ItemType type) {
            Attr attr = getAttr (type);
            return attr.RequiresProperties;
        }

        private static Attr getAttr(ItemType type) {
            return (Attr)Attribute.GetCustomAttribute(forValue(type), typeof(Attr));
        }

        private static MemberInfo forValue(ItemType type) {
            return typeof(ItemType).GetField(Enum.GetName(typeof(ItemType), type));
        }
    }

    class Attr : Attribute {
        public string Label { get; }
        public bool RequiresProperties { get; }

        internal Attr(string label, bool requiresProperties = false) {
            Label = label;
            RequiresProperties = requiresProperties;
        }
    }
}