using System;
using System.Collections.Generic;
using net.gubbi.goofy.extensions;
using UnityEditor;
using UnityEngine;

namespace net.gubbi.goofy.editor.extensions {
    public static class EditorRectExtensions {
        
        public static Rect EditorResize(this Rect rect, Handles.CapFunction capFunc, Color capCol, Color fillCol, float capSize, float snap) {
            Vector2 halfRectSize = new Vector2(rect.size.x * 0.5f, rect.size.y * 0.5f);
        
            Vector3[] rectangleCorners = { 
                new Vector2(rect.position.x - halfRectSize.x, rect.position.y - halfRectSize.y),   // Bottom Left
                new Vector2(rect.position.x + halfRectSize.x, rect.position.y - halfRectSize.y),   // Bottom Right
                new Vector2(rect.position.x + halfRectSize.x, rect.position.y + halfRectSize.y),   // Top Right
                new Vector2(rect.position.x - halfRectSize.x, rect.position.y + halfRectSize.y)    // Top Left
            }; 
            
            Handles.color = fillCol;
            Handles.DrawSolidRectangleWithOutline(rectangleCorners, new Color(fillCol.r, fillCol.g, fillCol.b, 0.25f), capCol);
        
            Vector3[] handlePoints = { 
                new Vector2(rect.position.x - halfRectSize.x, rect.position.y),   // Left
                new Vector2(rect.position.x + halfRectSize.x, rect.position.y),   // Right
                new Vector2(rect.position.x, rect.position.y + halfRectSize.y),   // Top
                new Vector2(rect.position.x, rect.position.y - halfRectSize.y)    // Bottom 
            }; 
        
            Handles.color = capCol;
        
            var newSize = rect.size;
            var newPosition = rect.position;        
            
            var leftHandle = Handles.Slider(handlePoints[0], -Vector3.right, capSize, capFunc, snap).x - handlePoints[0].x;
            var rightHandle = Handles.Slider(handlePoints[1], Vector3.right, capSize, capFunc, snap).x - handlePoints[1].x;
            var topHandle = Handles.Slider(handlePoints[2], Vector3.up, capSize, capFunc, snap).y - handlePoints[2].y;
            var bottomHandle = Handles.Slider(handlePoints[3], -Vector3.up, capSize, capFunc, snap).y - handlePoints[3].y;
        
            newSize = new Vector2(
                Mathf.Max(0.0f, newSize.x - leftHandle + rightHandle), 
                Mathf.Max(0.0f, newSize.y + topHandle - bottomHandle)
            );
        
            newPosition = new Vector2(
                newPosition.x + leftHandle * 0.5f + rightHandle * 0.5f, 
                newPosition.y + topHandle * 0.5f + bottomHandle * 0.5f
            );
                                    
            return new Rect(newPosition, newSize);            
        }


        //Snap to position
        public static Rect SnapMovement(this Rect rect, List<Rect> snapTo, Vector2 pivot, float snap) {
            if (snapTo == null) {
                return rect;
            }                   
            
            Rect rectAdjusted = rect.MoveBySizeFactor(pivot);

            foreach (var snapToRect in snapTo) {
                Rect snapRectAdjusted = snapToRect.MoveBySizeFactor(pivot);

                float? snapValue;

                if (rectAdjusted.yMax >= snapRectAdjusted.yMin && rectAdjusted.yMin <= snapRectAdjusted.yMax) {
                    if ((snapValue = SnapToValue(rectAdjusted.xMin, snapRectAdjusted.xMax, snap)) != null 
                        || (snapValue = SnapToValue(rectAdjusted.xMin, snapRectAdjusted.xMin, snap)) != null ) {
                        rectAdjusted = new Rect(new Vector2((float)snapValue, rectAdjusted.position.y), rectAdjusted.size);
                    }
                
                    if ((snapValue = SnapToValue(rectAdjusted.xMax, snapRectAdjusted.xMax, snap)) != null
                        || (snapValue = SnapToValue(rectAdjusted.xMax, snapRectAdjusted.xMin, snap)) != null) {
                        rectAdjusted = new Rect(new Vector2((float)snapValue - rectAdjusted.width, rectAdjusted.position.y), rectAdjusted.size);
                    }
                }

                if (rectAdjusted.xMax >= snapRectAdjusted.xMin && rectAdjusted.xMin <= snapRectAdjusted.xMax) {
                    if ((snapValue = SnapToValue(rectAdjusted.yMin, snapRectAdjusted.yMax, snap)) != null
                        || (snapValue = SnapToValue(rectAdjusted.yMin, snapRectAdjusted.yMin, snap)) != null) {
                        rectAdjusted = new Rect(new Vector2(rectAdjusted.position.x, (float) snapValue), rectAdjusted.size);
                    }

                    if ((snapValue = SnapToValue(rectAdjusted.yMax, snapRectAdjusted.yMax, snap)) != null
                        || (snapValue = SnapToValue(rectAdjusted.yMax, snapRectAdjusted.yMin, snap)) != null) {
                        rectAdjusted = new Rect(new Vector2(rectAdjusted.position.x, (float) snapValue - rectAdjusted.height), rectAdjusted.size);
                    }
                }

            }
            
            return rectAdjusted.MoveBySizeFactor(-pivot);
        }

        //Snap to position
        public static Rect SnapResize(this Rect rect, List<Rect> snapTo, Vector2 pivot, float snap) {
            if (snapTo == null) {
                return rect;
            }                   
            
            Rect rectAdjusted = rect.MoveBySizeFactor(pivot);

            foreach (var snapToRect in snapTo) {
                Rect snapRectAdjusted = snapToRect.MoveBySizeFactor(pivot);

                float? snapValue;
                                
                if (rectAdjusted.yMax >= snapRectAdjusted.yMin && rectAdjusted.yMin <= snapRectAdjusted.yMax) {   
                                                                                                    
                    if ((snapValue = SnapToValue(rectAdjusted.xMin, snapRectAdjusted.xMax, snap)) != null 
                        || (snapValue = SnapToValue(rectAdjusted.xMin, snapRectAdjusted.xMin, snap)) != null ) {
                        rectAdjusted.xMin = (float) snapValue;                        
                    }
                
                    if ((snapValue = SnapToValue(rectAdjusted.xMax, snapRectAdjusted.xMax, snap)) != null
                        || (snapValue = SnapToValue(rectAdjusted.xMax, snapRectAdjusted.xMin, snap)) != null) {                        
                        rectAdjusted.xMax = (float) snapValue;
                    }
                }

                if (rectAdjusted.xMax >= snapRectAdjusted.xMin && rectAdjusted.xMin <= snapRectAdjusted.xMax) {
                                        
                    if ((snapValue = SnapToValue(rectAdjusted.yMin, snapRectAdjusted.yMax, snap)) != null
                        || (snapValue = SnapToValue(rectAdjusted.yMin, snapRectAdjusted.yMin, snap)) != null) {
                        rectAdjusted.yMin = (float) snapValue;
                    }

                    if ((snapValue = SnapToValue(rectAdjusted.yMax, snapRectAdjusted.yMax, snap)) != null
                        || (snapValue = SnapToValue(rectAdjusted.yMax, snapRectAdjusted.yMin, snap)) != null) {
                        rectAdjusted.yMax = (float) snapValue;
                    }
                }
            }
            
            return rectAdjusted.MoveBySizeFactor(-pivot);
        }

        private static float? SnapToValue(float value, float snapValue, float snap) {
            if (Math.Abs(value - snapValue) < snap) {
                return snapValue;
            }
            return null;
        }
    }
}