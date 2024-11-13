using System;
using Mgl;

namespace net.gubbi.goofy.util {
    public static class I18nUtil {
        public static string LINE_DELIM = "|";
        public static string OPTION_DELIM = ">";
        private static Random random = new Random();

        public static string[] options(string str) {
            return I18n.Instance.array(str, OPTION_DELIM);
        }

        public static string randomOption(string str) {            
            string[] msgs = I18n.Instance.array(str, OPTION_DELIM);            
            return msgs[random.Next (0, msgs.Length)];
        }               

    }
}