using UnityEngine;

namespace net.gubbi.goofy.state {

    public struct Vector2Dto {
        public float x;
        public float y;

        public Vector2Dto(float x, float y) {
            this.x = x;
            this.y = y;
        }

        public static Vector2Dto? fromVector2(Vector2? v) {
            return v != null ? new Vector2Dto(((Vector2) v).x, ((Vector2) v).y) : (Vector2Dto?) null;
        }

        public static Vector2? toVector2(Vector2Dto? v) {
            return v != null ? new Vector2(((Vector2Dto) v).x, ((Vector2Dto) v).y) : (Vector2?) null; 
        }
    }
}