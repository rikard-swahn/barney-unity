using Mgl;
using UnityEngine;

namespace net.gubbi.goofy.util {
    public class I18nManager : MonoBehaviour {
        
        private const string DEFAULT_LANGUAGE = "en";
        private const string LANGUAGE_PREFS_KEY = "Language";
        private static string runtimeLanguage;
 
        private void Awake () {
            InitLanguage();
        }

        public static string GetLanguage() {
            return PlayerPrefs.GetString(LANGUAGE_PREFS_KEY, runtimeLanguage);
        }

        public static void SetLanguageSetting(string locale) {
            PlayerPrefs.SetString(LANGUAGE_PREFS_KEY, locale);
            SetLanguage(locale);
        }

        private void InitLanguage () {
            if (PlayerPrefs.HasKey(LANGUAGE_PREFS_KEY)) {
                SetLanguage (PlayerPrefs.GetString(LANGUAGE_PREFS_KEY));
                return;
            }
             
            //Debug.Log("Current System Language: " + Application.systemLanguage);
 
            switch (Application.systemLanguage) {
                // case SystemLanguage.Afrikaans: SetLanguage(""); break;
                //case SystemLanguage.Arabic: SetLanguage("ar"); break;
                case SystemLanguage.Basque: SetLanguage("es"); break;
                // case SystemLanguage.Belarusian: SetLanguage(""); break;
                // case SystemLanguage.Bulgarian: SetLanguage(""); break;
                case SystemLanguage.Catalan: SetLanguage("es"); break;
                //case SystemLanguage.Chinese: SetLanguage("zh"); break;
                // case SystemLanguage.Czech: SetLanguage(""); break;
                // case SystemLanguage.Danish: SetLanguage(""); break;
                // case SystemLanguage.Dutch: SetLanguage(""); break;
                case SystemLanguage.English: SetLanguage("en"); break;
                // case SystemLanguage.Estonian: SetLanguage(""); break;
                // case SystemLanguage.Faroese: SetLanguage(""); break;
                // case SystemLanguage.Finnish: SetLanguage(""); break;
                // case SystemLanguage.French: SetLanguage("fr"); break;
                //case SystemLanguage.German: SetLanguage("de"); break;
                // case SystemLanguage.Greek: SetLanguage(""); break;
                // case SystemLanguage.Hebrew: SetLanguage(""); break;
                // case SystemLanguage.Hungarian: SetLanguage(""); break;
                // case SystemLanguage.Icelandic: SetLanguage(""); break;
                // case SystemLanguage.Indonesian: SetLanguage(""); break;
                //case SystemLanguage.Italian: SetLanguage("it"); break;
                case SystemLanguage.Japanese: SetLanguage("jp"); break;
                //case SystemLanguage.Korean: SetLanguage("ko"); break;
                // case SystemLanguage.Latvian: SetLanguage(""); break;
                // case SystemLanguage.Lithuanian: SetLanguage(""); break;
                // case SystemLanguage.Norwegian: SetLanguage(""); break;
                // case SystemLanguage.Polish: SetLanguage(""); break;
                case SystemLanguage.Portuguese: SetLanguage("pt"); break;
                // case SystemLanguage.Romanian: SetLanguage(""); break;
                //case SystemLanguage.Russian: SetLanguage("ru"); break;
                // case SystemLanguage.SerboCroatian: SetLanguage(""); break;
                // case SystemLanguage.Slovak: SetLanguage(""); break;
                // case SystemLanguage.Slovenian: SetLanguage(""); break;
                case SystemLanguage.Spanish: SetLanguage("es"); break;
                // case SystemLanguage.Swedish: SetLanguage(""); break;
                //case SystemLanguage.Thai: SetLanguage("th"); break;
                //case SystemLanguage.Turkish: SetLanguage("tr"); break;
                //case SystemLanguage.Ukrainian: SetLanguage("ru"); break;
                // case SystemLanguage.Vietnamese: SetLanguage(""); break;
                //case SystemLanguage.ChineseSimplified: SetLanguage("zh-si"); break;
                //case SystemLanguage.ChineseTraditional: SetLanguage("zh-tr"); break;
                default: SetLanguage(DEFAULT_LANGUAGE); break;
            }
        }

        private static void SetLanguage (string locale) {
            //Debug.Log("Set locale " + locale);
            //PlayerPrefs.SetString(LANGUAGE_PREFS_KEY, locale);
            runtimeLanguage = locale;
            I18n.SetLocale(locale);
        }
    }
}