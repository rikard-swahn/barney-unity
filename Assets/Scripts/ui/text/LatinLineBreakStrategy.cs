using System.Text.RegularExpressions;

namespace ui.text {
    public class LatinLineBreakStrategy : LineBreakStrategy {
        
        private readonly Regex multipleSpaces = new Regex("[ ]{2,}", RegexOptions.None);

        public string breakString(string str, int maxLineWidth) {
            //Do not allow multiple spaces
            str = multipleSpaces.Replace(str, " ");			
			
            var words = str.Split(' ');

            var strWithBreaks = "";
            var line = "";

            foreach (var word in words) {
                
                if (line.Length > 0 && line.Length + word.Length + 1 > maxLineWidth) {
                    strWithBreaks += line + "\n";
                    line = "";
                }
                else if (line.Length > 0) {
                    line += " ";
                }

                line += word;
            }

            strWithBreaks += line;

            return strWithBreaks;            
        }
    }
}