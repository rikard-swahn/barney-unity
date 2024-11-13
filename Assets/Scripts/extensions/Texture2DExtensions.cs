﻿using UnityEngine;

namespace net.gubbi.goofy.extensions {
    public static class Texture2DExtensions {
        
        public enum ImageFilterMode : int {
            Nearest = 0,
            Bilinear = 1,
            Average = 2
        }
        
        public static Texture2D Resize(this Texture2D pSource, ImageFilterMode pFilterMode, float pScale){
            int i;
 
            //*** Get All the source pixels
            Color[] aSourceColor = pSource.GetPixels(0);
            Vector2 vSourceSize = new Vector2(pSource.width, pSource.height);
 
            //*** Calculate New texture Size
            float xWidth = Mathf.RoundToInt((float)pSource.width * pScale);                     
            float xHeight = Mathf.RoundToInt((float)pSource.height * pScale);
            //New texture
            Texture2D oNewTex = new Texture2D((int)xWidth, (int)xHeight, TextureFormat.RGBA32, false);
#if UNITY_EDITOR
            oNewTex.alphaIsTransparency = true;
#endif              
 
            //*** Make destination array
            int xLength = (int)xWidth * (int)xHeight;
            Color[] aColor = new Color[xLength];
 
            Vector2 vPixelSize = new Vector2(vSourceSize.x / xWidth, vSourceSize.y / xHeight);
 
            //*** Loop through destination pixels and process
            Vector2 vCenter = new Vector2();
            for(i = 0; i < xLength; i++){
 
                //*** Figure out x&y
                float xX = (float)i % xWidth;
                float xY = Mathf.Floor((float)i / xWidth);
 
                //*** Calculate Center
                vCenter.x = (xX / xWidth) * (vSourceSize.x - 1);
                vCenter.y = (xY / xHeight) * (vSourceSize.y - 1);
 
                //*** Do Based on mode
                //*** Nearest neighbour (testing)
                if(pFilterMode == ImageFilterMode.Nearest){
 
                    //*** Nearest neighbour (testing)
                    vCenter.x = Mathf.Round(vCenter.x);
                    vCenter.y = Mathf.Round(vCenter.y);
 
                    //*** Calculate source index
                    int xSourceIndex = (int)((vCenter.y * vSourceSize.x) + vCenter.x);
 
                    //*** Copy Pixel
                    aColor[i] = aSourceColor[xSourceIndex];
                }
 
                //*** Bilinear
                else if(pFilterMode == ImageFilterMode.Bilinear){
 
                    //*** Get Ratios
                    float xRatioX = vCenter.x - Mathf.Floor(vCenter.x);
                    float xRatioY = vCenter.y - Mathf.Floor(vCenter.y);
 
                    //*** Get Pixel index's
                    int xIndexTL = (int)((Mathf.Floor(vCenter.y) * vSourceSize.x) + Mathf.Floor(vCenter.x));
                    int xIndexTR = (int)((Mathf.Floor(vCenter.y) * vSourceSize.x) + Mathf.Ceil(vCenter.x));
                    int xIndexBL = (int)((Mathf.Ceil(vCenter.y) * vSourceSize.x) + Mathf.Floor(vCenter.x));
                    int xIndexBR = (int)((Mathf.Ceil(vCenter.y) * vSourceSize.x) + Mathf.Ceil(vCenter.x));
 
                    //*** Calculate Color
                    aColor[i] = Color.Lerp(
                        Color.Lerp(aSourceColor[xIndexTL], aSourceColor[xIndexTR], xRatioX),
                        Color.Lerp(aSourceColor[xIndexBL], aSourceColor[xIndexBR], xRatioX),
                        xRatioY
                    );
                }
 
                //*** Average
                else if(pFilterMode == ImageFilterMode.Average){
 
                    //*** Calculate grid around point
                    int xXFrom = (int)Mathf.Max(Mathf.Floor(vCenter.x - (vPixelSize.x * 0.5f)), 0);
                    int xXTo = (int)Mathf.Min(Mathf.Ceil(vCenter.x + (vPixelSize.x * 0.5f)), vSourceSize.x);
                    int xYFrom = (int)Mathf.Max(Mathf.Floor(vCenter.y - (vPixelSize.y * 0.5f)), 0);
                    int xYTo = (int)Mathf.Min(Mathf.Ceil(vCenter.y + (vPixelSize.y * 0.5f)), vSourceSize.y);
 
                    //*** Loop and accumulate
                    Color oColorTemp = new Color();
                    float xGridCount = 0;
                    for(int iy = xYFrom; iy < xYTo; iy++){
                        for(int ix = xXFrom; ix < xXTo; ix++){
 
                            //*** Get Color
                            oColorTemp += aSourceColor[(int)(((float)iy * vSourceSize.x) + ix)];
 
                            //*** Sum
                            xGridCount++;
                        }
                    }
 
                    //*** Average Color
                    aColor[i] = oColorTemp / (float)xGridCount;
                }
            }
 
            //*** Set Pixels
            oNewTex.SetPixels(aColor);
            oNewTex.Apply();
 
            return oNewTex;
        }        
        
    }
}