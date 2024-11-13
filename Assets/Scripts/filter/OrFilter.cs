using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace net.gubbi.goofy.filter {

    [CreateAssetMenu(menuName = "Filters/OR")]
    public class OrFilter : Filter {
        public List<Filter> filters;
        
        public override bool matches(FilterContext ctx) {
            foreach (var filter in filters) {
                if (filter.matches(ctx)) {
                    return true;
                }
            }
            
            return false;
        }
        
        public override bool contains<T>() {
            return filters.Any(c => c.contains<T>());
        }                
    }
}