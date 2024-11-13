using System;
using System.Collections.Generic;

namespace net.gubbi.goofy.extensions {
    public static class EnumerableExtensions {
        
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            foreach (T element in source) 
                action(element);
        }        
    }
}