using System;

namespace state.game {
    public class Property {
        public int? intVal;
        public bool? boolVal;
        public float? floatVal;
        public String stringVal;

        public Property(bool val) {
            boolVal = val;
        }
        public Property(int val) {
            intVal = val;
        }
        public Property(string val) {
            stringVal = val;
        }
        public Property(float val) {
            floatVal = val;
        }

        public int getIntval() {
            return (int) intVal;
        }
        public bool getBool() {
            return (bool) boolVal;
        }
        public float getFloat() {
            return (float) floatVal;
        }
    }
}