using System;
using Mgl;
using UnityEngine;

namespace net.gubbi.goofy.filter {

    [CreateAssetMenu(menuName = "Filters/String Equals")]
    public class StringEqualsFilter : Filter {

        public string str;
        public bool ignoreCase;

        public override bool matches(FilterContext ctx) {
            var i18nStr = I18n.Instance.__(str);
            
            StringComparison type = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            return ctx.getProperty<String>(FilterContext.STRING).Equals(i18nStr, type);
        }
    }
}