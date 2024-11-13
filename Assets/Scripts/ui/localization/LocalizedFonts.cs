using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using net.gubbi.goofy.events;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.util;
using TMPro;
using ui;
using UnityEngine;
using UnityEngine.UI; 

namespace net.gubbi.goofy.ui {
    public class LocalizedFonts : MonoBehaviour {

        public LocalizedFontReplacements fontReplacements;
        public bool printUsedCharacters;

        private readonly Dictionary<Text, OriginalFont> _defaultTextFonts = new Dictionary<Text, OriginalFont>();
        private readonly Dictionary<TextMeshProUGUI, OriginalTmpFont> _defaultTmpFonts = new Dictionary<TextMeshProUGUI, OriginalTmpFont>();
        private readonly Dictionary<TextButton, OriginalFont> _defaultTextButtonFonts = new Dictionary<TextButton, OriginalFont>();
        private readonly Dictionary<string, Dictionary<Font, FontOverride>> _fontReplacement = new Dictionary<string, Dictionary<Font, FontOverride>>();
        private readonly Dictionary<string, Dictionary<TMP_FontAsset, TmpFontOverride>> _tmpFontReplacement = new Dictionary<string, Dictionary<TMP_FontAsset, TmpFontOverride>>();
        private Text[] _texts;
        private TextMeshProUGUI[] _tmps;
        private TextButton[] _textButtons;
        
        private void Awake() {
            fontReplacements.replacements.ForEach(r => AddFontMapping(r.locale, r.original, r.fontOverride));
            fontReplacements.tmpReplacements.ForEach(r => AddFontMapping(r.locale, r.original, r.fontOverride));

            _texts = GetComponentsInChildren<Text>(true);
            foreach (Text text in _texts) {
                _defaultTextFonts[text] = new OriginalFont(text.font, text.fontStyle, text.fontSize);
                if (text.font == null) {
                    Debug.LogError("Missing font for " + text.name);
                }
            }    
            
            _tmps = GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (TextMeshProUGUI text in _tmps) {
                _defaultTmpFonts[text] = new OriginalTmpFont(text.font, text.fontStyle, text.fontSize);
                if (text.font == null) {
                    Debug.LogError("Missing font for " + text.name);
                }                
            }            
            
            _textButtons = GetComponentsInChildren<TextButton>(true);
            foreach (TextButton textButton in _textButtons) {
                _defaultTextButtonFonts[textButton] = new OriginalFont(textButton.font, textButton.fontStyle);
            }
            
            EventManager.Instance.addListener<GameEvents.LocaleChangedEvent>(localeChangedHandler);
        }

        private void Start() {
            if (printUsedCharacters) {
                Debug.Log("Characters used in scene: " + GetUsedCharacters());
            }

            if (false) {
                Debug.Log("to do:" + new string("マイケルのドニーとのクルーズ行きを阻止せよマイケルに犯罪の罪を着せろクルーズのチケットを手に入れ、ドニーにプレゼントせよやるべきことリスト:".Distinct().ToArray()));    
            }
            
            SetUITexts();
        }

        private void localeChangedHandler(GameEvents.LocaleChangedEvent e) {
            SetUITexts();
        }

        private void SetUITexts() {
            var locale = I18nManager.GetLanguage();
            
            _texts.ForEach(t => {
                if (_fontReplacement.ContainsKey(locale) && _defaultTextFonts[t].font != null && _fontReplacement[locale].ContainsKey(_defaultTextFonts[t].font)) {
                    var replacement = _fontReplacement[locale][_defaultTextFonts[t].font];
                    t.font = replacement.font;
                    t.fontStyle = replacement.fontStyle;
                    t.fontSize = Mathf.RoundToInt(_defaultTextFonts[t].size * replacement.sizeFactor);
                }
                else {
                    t.font = _defaultTextFonts[t].font;
                    t.fontStyle = _defaultTextFonts[t].style;
                    t.fontSize = _defaultTextFonts[t].size;
                }
                
            });       
                        
            _tmps.ForEach(t => {
                if (_tmpFontReplacement.ContainsKey(locale) && _defaultTmpFonts[t].font != null && _tmpFontReplacement[locale].ContainsKey(_defaultTmpFonts[t].font)) {
                    var replacement = _tmpFontReplacement[locale][_defaultTmpFonts[t].font];
                    t.font = replacement.font;
                    t.fontStyle = replacement.style;
                    t.fontSize = _defaultTmpFonts[t].size * replacement.sizeFactor;
                }
                else {
                    t.font = _defaultTmpFonts[t].font;
                    t.fontStyle = _defaultTmpFonts[t].style;
                    t.fontSize = _defaultTmpFonts[t].size;
                }
  
            });       
            
            _textButtons.ForEach(t => {
                if (_fontReplacement.ContainsKey(locale) && _fontReplacement[locale].ContainsKey(_defaultTextButtonFonts[t].font)) {
                    var replacement = _fontReplacement[locale][_defaultTextButtonFonts[t].font];
                    t.font = replacement.font;
                    t.fontStyle = replacement.fontStyle;
                }
                else {
                    t.font = _defaultTextButtonFonts[t].font;
                    t.fontStyle = _defaultTextButtonFonts[t].style;
                }
                
            });
            
        }

        private void AddFontMapping(string locale, Font originalFont, FontOverride replaceWith) {
            var localeFontReplacements = _fontReplacement.PutIfAbsent(locale, new Dictionary<Font, FontOverride>());
            localeFontReplacements[originalFont] = replaceWith;
        }

        private void AddFontMapping(string locale, TMP_FontAsset originalFont, TmpFontOverride replaceWith) {
            var localeFontReplacements = _tmpFontReplacement.PutIfAbsent(locale, new Dictionary<TMP_FontAsset, TmpFontOverride>());
            localeFontReplacements[originalFont] = replaceWith;
        }

        private string GetUsedCharacters() {
            string str = ""; 
            
            _texts.ForEach(t => {
                str += Regex.Replace(t.text, @"\n", "");
            });

            _tmps.ForEach(t => {
                str += Regex.Replace(t.text, @"\n", "");
            });

            return new string(str.Distinct().ToArray());
        }
    }

    public class OriginalFont {
        public Font font;
        public FontStyle style;
        public int size;

        public OriginalFont(Font font, FontStyle style, int size) {
            this.font = font;
            this.style = style;
            this.size = size;
        }

        public OriginalFont(Font font, FontStyle style) {
            this.font = font;
            this.style = style;
        }
    }
    
    public class OriginalTmpFont {
        public TMP_FontAsset font;
        public FontStyles style;

        public float size;

        public OriginalTmpFont(TMP_FontAsset font, FontStyles style, float size) {
            this.font = font;
            this.style = style;
            this.size = size;
        }
    }

}