using System;

namespace net.gubbi.goofy.extensions {
    public static class FloatExtensions {
        
        public static bool looseEquals(this float f1, float f2, float epsilon) {
            return Math.Abs(f1 - f2) < epsilon;
        }
        
    }
}