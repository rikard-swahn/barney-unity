using System;

namespace net.gubbi.goofy.extensions {
    public static class StringExtensions {
                
        public static bool isNullOrEmpty(this string str) {
            return str == null || str.Equals ("");
        }
        
        public static string[] split(this string str, string delim) {
            return str.Split (new[]{delim}, StringSplitOptions.None);			
        }

        public static string splitAndFirst (this string str, string delim) {
            return str.Contains (delim) ? str.Substring (0, str.IndexOf (delim)) : str;
        }

    }
}