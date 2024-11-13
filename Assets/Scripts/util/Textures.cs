using UnityEngine;

namespace net.gubbi.goofy.util {
    public class Textures {

        public static Texture2D hole() {
            return Resources.Load("Sprites/ui/transitions/hole") as Texture2D;    
        }
        
        public static Texture2D heart() {
            return Resources.Load("Sprites/ui/transitions/heart") as Texture2D;    
        }
        
    }
}