namespace ui.text {
    public class JapaneseLineBreakStrategy : LineBreakStrategy {

        public string breakString(string str, int maxLineWidth) {
            var words = Mikan.Mikan.Split(str);
            
            var strWithBreaks = "";
            var line = "";

            foreach (var word in words) {
                
                //If trying to add a word (to a non-empty line) that makes line too long, break into new line before adding word
                if (line.Length > 0 && line.Length + word.Length > maxLineWidth) {
                    strWithBreaks += line + "\n";
                    line = "";
                }
                
                line += word;
            }

            strWithBreaks += line;

            return strWithBreaks;            
        }
    }
}