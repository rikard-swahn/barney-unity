using net.gubbi.goofy.events;
using net.gubbi.goofy.util;

namespace ui.text {
    public class LineBreakService {
        
        private static LineBreakService _instance;
        private LineBreakStrategy _lineBreakStrategy;
        private const string JP_LOCALE = "jp";

        public static LineBreakService Instance {
            get
            {
                if (_instance == null)
                {
                    _instance = new LineBreakService();
                    _instance.init();
                    
                    EventManager.Instance.addListener<GameEvents.LocaleChangedEvent>(_ => _instance.init());
                }

                return _instance;
            }
        }

        private void init() {
            _lineBreakStrategy = GetLineBreakStrategy();
        }

        public string breakString(string str, int maxLineWidth) {
            return _lineBreakStrategy.breakString(str, maxLineWidth);
        }

        private LineBreakStrategy GetLineBreakStrategy() {
            var locale = I18nManager.GetLanguage();
            
            if (locale.Equals(JP_LOCALE)) {
                return new JapaneseLineBreakStrategy();
            }
            
            return new LatinLineBreakStrategy();
        }
    }
}