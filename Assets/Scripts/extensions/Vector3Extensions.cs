using UnityEngine;

namespace net.gubbi.goofy.extensions {
    public static class Vector3Extensions {
        
        public static Vector3 clampInside(this Vector3 pos, Rect rect) {						
            float x = rect.xMin <= rect.xMax ? Mathf.Clamp (pos.x, rect.xMin, rect.xMax) : pos.x;
            float y = rect.yMin <= rect.yMax ? Mathf.Clamp (pos.y, rect.yMin, rect.yMax) : pos.y;

            return new Vector3 (x, y, pos.z);
        }        
        
        public static bool looseEquals(this Vector3 v1, Vector3 v2, float threshold) {
            Vector3 diff = v1 - v2;
            return diff.sqrMagnitude < threshold;
        }        
    }
}