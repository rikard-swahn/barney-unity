using UnityEngine;

namespace net.gubbi.goofy.extensions {
    public static class RectExtensions {        

        /// <summary>
        /// Clamps a rect inside outer rect. The rect is moved and resized as necessary.
        /// So if the rect is bigger than the boundry, it is resized to the boundry size.
        /// </summary>
        /// <returns>The clamped rect</returns>
        public static Rect clampInsideResize(this Rect rect, Rect boundryRect) {
            float width = Mathf.Min (rect.width, boundryRect.width);
            float height = Mathf.Min (rect.height, boundryRect.height);
            float x = Mathf.Clamp (rect.x, boundryRect.xMin, boundryRect.xMax - width);
            float y = Mathf.Clamp (rect.y, boundryRect.yMin, boundryRect.yMax - height);
            
            return new Rect(x, y, width, height);			
        }
        
        /// <summary>
        /// Clamps a rect inside outer rect, if possible. If the rect is bigger than the boundry
        /// the rect will be placed centered over the boundry rect.
        /// </summary>
        /// <returns>The insideRect inside rect.</returns>
        public static Rect clampInside(this Rect rect, Rect boundry) {                        
            float x = rect.width <= boundry.width ? 
                Mathf.Clamp (rect.x, boundry.xMin, boundry.xMax - rect.width) 
                : (boundry.xMin + boundry.width / 2) - rect.width / 2;
            
            float y = rect.height <= boundry.height ? 
                Mathf.Clamp (rect.y, boundry.yMin, boundry.yMax - rect.height)
                : (boundry.yMin + boundry.height / 2) - rect.height / 2; 
            
            return new Rect(x, y, rect.width, rect.height);			
        }

        public static bool IsToTheRightOf(this Rect rect, Rect otherRect) {
            return rect.xMin > otherRect.xMax;
        }
        
        public static bool IsToTheLeftOf(this Rect rect, Rect otherRect) {
            return rect.xMax < otherRect.xMin;
        }        
        
        public static bool IsAbove(this Rect rect, Rect otherRect) {
            return rect.yMin > otherRect.yMax;
        }
        
        public static bool IsBelow(this Rect rect, Rect otherRect) {
            return rect.yMax < otherRect.yMin;
        }

        public static void drawGizmo(this Rect rect, Color color) {
            Vector2 bottomLeft = new Vector2(rect.xMin, rect.yMin);
            Vector2 topLeft = new Vector2(rect.xMin, rect.yMax);
            Vector2 topRight = new Vector2(rect.xMax, rect.yMax);
            Vector2 bottomRight = new Vector2(rect.xMax, rect.yMin);
            Gizmos.color = color;
            Gizmos.DrawLine(bottomLeft, topLeft);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
        }        
        
        public static Rect MoveBySizeFactor(this Rect rect, Vector2 pivot) {
            Vector2 pivotAdjust = new Vector2(rect.size.x * pivot.x, rect.size.y * pivot.y);
            return new Rect(rect.position - pivotAdjust, rect.size);                
        }        
    }
}