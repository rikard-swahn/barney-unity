using System;
using System.Reflection;

namespace net.gubbi.goofy.util {
    public enum BodyState {
        [Attr("Standing")]
        STANDING,
        [Attr("Sitting")]
        SITTING
    }
				
    public static class BodyStates {
        public static string getKey(this BodyState type) {
            Attr attr = getAttr (type);
            return attr.Key;
        }

        public static BodyState fromKey(string key) {
            foreach (BodyState state in Enum.GetValues(typeof(BodyState))) {
                if (state.getKey ().Equals (key)) {
                    return state;
                }
            }

            throw new ArgumentException ("No match for key " + key);
        }

        private static Attr getAttr(BodyState type) {
            return (Attr)Attribute.GetCustomAttribute(forValue(type), typeof(Attr));
        }

        private static MemberInfo forValue(BodyState type) {
            return typeof(BodyState).GetField(Enum.GetName(typeof(BodyState), type));
        }
    }

    class Attr : Attribute {
        public string Key { get; private set; }

        internal Attr(string key) {
            this.Key = key;
        }
    }
}