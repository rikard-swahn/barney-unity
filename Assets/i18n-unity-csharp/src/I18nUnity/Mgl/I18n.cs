using Lib.SimpleJSON;
using System;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mgl
{
    public sealed class I18n
    {
        private static JSONNode config = null;

        private static readonly I18n instance = new I18n();

        private static string _currentLocale = "en-US";

        private static string _localePath = "Locales/";

        private static bool _isLoggingMissing = false;

        static I18n()
        {
           
        }

        private I18n()
        {
        }

        public static I18n Instance
        {
            get
            {
                return instance;
            }
        }

        static void InitConfig()
        {
            string localConfigPath = _localePath + _currentLocale;
            // Read the file as one string.
            TextAsset configText = Resources.Load(localConfigPath) as TextAsset;
            config = JSON.Parse(configText.text);

            if (false) {
                Debug.Log("unique i18n chars:" + Instance.DistinctTranslationCharacters());
            }
        }

        public static string GetLocale()
        {
            return _currentLocale;
        }

        public static void SetLocale(string newLocale = null)
        {
            if (newLocale != null)
            {
                _currentLocale = newLocale;
                InitConfig();
            }
        }

        public static void SetPath(string localePath = null)
        {
            if (localePath != null)
            {
                _localePath = localePath;
                InitConfig();
            }
        }

        public static void Configure(string localePath = null, string newLocale = null, bool logMissing = true)
        {
            _isLoggingMissing = logMissing;
            SetPath(localePath);
            SetLocale(newLocale);
            InitConfig();
        }

        private string DistinctTranslationCharacters() {
            string allTranslationsStr = ""; 
            
            foreach (JSONNode val in config.Children) {
                var v = Regex.Replace(val.Value, @"{.*}|\||\n", "");
                allTranslationsStr += v;
            }

            var distinct = new string(allTranslationsStr.Distinct().ToArray());
            return distinct;
        }

        public string __(string key, params object[] args)
        {
            if (config == null)
            {
                InitConfig();
            }
            string translation = key;
            if (config[key] != null)
            {
                // if this key is a direct string
                if (config[key].Count == 0)
                {
                    translation = config[key];
                }
                else
                {
                    translation = FindSingularOrPlural(key, args);
                }
                // check if we have embeddable data
                if (args.Length > 0)
                {
                    translation = string.Format(translation, args);
                }
            }
            else if (_isLoggingMissing)
            {
                Debug.Log("Missing translation for:" + key);
            }
            return translation;
        }
        

        public string[] array(string key, string delim) {
            return __(key).Split(new[]{delim}, StringSplitOptions.None);
        }

        string FindSingularOrPlural(string key, object[] args)
        {
            JSONClass translationOptions = config[key].AsObject;
            string translation = key;
            string singPlurKey;
            // find format to try to use
            switch (GetCountAmount(args))
            {
                case 0:
                    singPlurKey = "zero";
                    break;
                case 1:
                    singPlurKey = "one";
                    break;
                default:
                    singPlurKey = "other";
                    break;
            }
            // try to use this plural/singular key
            if (translationOptions[singPlurKey] != null)
            {
                translation = translationOptions[singPlurKey];
            }
            else if (_isLoggingMissing)
            {
                Debug.Log("Missing singPlurKey:" + singPlurKey + " for:" + key);
            }
            return translation;
        }

        int GetCountAmount(object[] args)
        {
            int argOne = 0;
            // if arguments passed, try to parse first one to use as count
            if (args.Length > 0 && IsNumeric(args[0]))
            {
                argOne = Math.Abs(Convert.ToInt32(args[0]));
                if (argOne == 1 && Math.Abs(Convert.ToDouble(args[0])) != 1)
                {
                    // check if arg actually equals one
                    argOne = 2;
                }
                else if (argOne == 0 && Math.Abs(Convert.ToDouble(args[0])) != 0)
                {
                    // check if arg actually equals one
                    argOne = 2;
                }
            }
            return argOne;
        }

        bool IsNumeric(System.Object Expression)
        {
            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
                return true;

            return false;
        }
    }
}
