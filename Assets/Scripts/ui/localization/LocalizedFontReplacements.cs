using System;
using TMPro;
using UnityEngine;

namespace net.gubbi.goofy.ui {
    
    [CreateAssetMenu(menuName = "Font/Replacements")]
    public class LocalizedFontReplacements : ScriptableObject {
        public FontReplacement[] replacements;
        public TmpFontReplacement[] tmpReplacements;
    }
    
    [Serializable]
    public class FontReplacement {
        public string locale;
        public Font original;
        public FontOverride fontOverride;
    }    
        
    [Serializable]
    public class TmpFontReplacement {
        public string locale;
        public TMP_FontAsset original;
        public TmpFontOverride fontOverride;
    }    
    
    [Serializable]
    public class FontOverride {
        public Font font;
        public FontStyle fontStyle;
        public float sizeFactor = 1;
    }     
    
    [Serializable]
    public class TmpFontOverride {
        public TMP_FontAsset font;
        public FontStyles style;
        public float sizeFactor = 1;
    }    
}