using System;
using System.Collections.Generic;
using net.gubbi.goofy.extensions;
using UnityEngine;

namespace net.gubbi.goofy.ui.cursor {

    [CreateAssetMenu(menuName = "Cursor")]
    public class CustomCursor : ScriptableObject {
        public CursorType type;
        public List<Texture2D> textures = new List<Texture2D>();
        public Vector2 hotspot;
        public float frameRate;  
        [HideInInspector]
        public float period;
        
        private List<Texture2D> scaledTextures;
        private float? scale;

        private void Awake() {
            if (textures.Count > 1 && frameRate <= 0f) {
                throw new ArgumentException("A cursor with more than one texture must have frame rate > 0");
            }

            period = textures.Count > 1 ? (textures.Count / frameRate) : 0f;
        }

        public List<Texture2D> getScaledTextures(float scale) {
            if (this.scale != scale) {
                this.scale = scale;
                rescaleTextures(scale);                
            }

            return scaledTextures;
        }

        private void rescaleTextures(float scale) {
            scaledTextures = new List<Texture2D>();            
            textures.ForEach(t => scaledTextures.Add(t.Resize(Texture2DExtensions.ImageFilterMode.Nearest, scale)));
        }
    }

}