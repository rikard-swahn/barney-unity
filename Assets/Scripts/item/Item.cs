using System;
using System.Collections.Generic;
using Mgl;
using state.game;
using UnityEngine;

namespace net.gubbi.goofy.item {

    public class Item {

        public ItemType Type { get; private set;}
        private Dictionary<string, Property> properties;

        public Item (ItemDto itemDto) {
            Type = itemDto.type;
            properties = itemDto.properties;
        }

        public Item(ItemType type) {
            Type = type;
            properties = new Dictionary<string, Property> ();
        }

        /// <summary>
        /// Adds the properties.
        /// </summary>
        /// <returns><c>false</c>, if item should be removed as a result of the new property state after adding, <c>true</c> otherwise.</returns>
        /// <param name="otherItem">Other item.</param>
        public virtual bool addProperties(Item otherItem) {
            if (otherItem.Type != Type) {
                throw new ArgumentException ("Trying to add item of type " + otherItem.Type + " to type " + Type);
            }

            var props = otherItem.Properties.GetEnumerator ();
            while (props.MoveNext ()) {
                var pair = props.Current;
                properties[pair.Key] = getNewPropertyValue (pair);
            }

            return true;
        }

        public virtual bool hasAmountOf (Item item) {
            if (item.Type != Type) {
                return false;
            }

            //Test that this item has at least the amount of the given item
            int? otherAmount = item.getAmount ();
            if (otherAmount != null) {
                int? amount = getAmount ();
                if (amount == null) {
                    return false;
                }
                return (int)amount >= (int)otherAmount;
            }

            return true;
        }

        protected int? getAmount () {
            if(!hasProperty(ItemProperties.AMOUNT)) {
                return null;
            }

            return getProperty(ItemProperties.AMOUNT).intVal;
        }

        protected void setAmount(int amount) {
            setProperty (ItemProperties.AMOUNT, amount);
        }
        
        public float? getAge () {
            if(!hasProperty(ItemProperties.AGE_SECONDS)) {
                return null;
            }

            return getProperty(ItemProperties.AGE_SECONDS).floatVal;
        }

        public void setAge(float age) {
            setProperty (ItemProperties.AGE_SECONDS, age);
        }

        private Property getNewPropertyValue (KeyValuePair<string, Property> pair) {
            if(properties.ContainsKey(pair.Key) && properties [pair.Key].intVal != null) {
                return new Property(properties [pair.Key].getIntval() + pair.Value.getIntval());
            }

            return pair.Value;
        }

        /// <summary>
        /// Allows getting customized label for the item, using the items properties.
        /// This default implementation just uses the label of the item type.
        /// </summary>
        public virtual string getLabel() {            
            return I18n.Instance.__(Type.getLabel ());
        }

        public bool hasProperty(string key) {
            return properties.ContainsKey (key);
        }

        public Property getProperty (string key) {
            return properties[key];
        }

        public void setProperty (string key, bool value) {
            properties [key] = new Property(value);
        }
        public void setProperty (string key, int value) {
            properties [key] = new Property(value);
        }
        public void setProperty (string key, float value) {
            properties [key] = new Property(value);
        }

        public Dictionary<string, Property> Properties {
            get {
                return this.properties;
            }
        }

        public override string ToString ()
        {
            return string.Format ("[Item: Type={0}, Properties={1}]", Type, properties.toDebugString());
        }

        public static Item fromDto(ItemDto dto) {
            //TODO: Not very pretty to reference BANK_CARD here
            if (dto.type == ItemType.BANK_CARD) {
                return new CardItem(dto);
            }
            
            if (dto.type == ItemType.COFFEE_POT) {
                return new CoffeePotItem(dto);
            }
            
            return new Item (dto);
        }

        public void Update() {            
            float age = getAge() ?? 0f;
            setAge(age + Time.deltaTime);
        }
    }
}