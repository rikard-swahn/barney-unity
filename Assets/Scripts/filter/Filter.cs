using UnityEngine;

namespace net.gubbi.goofy.filter {

    public abstract class Filter : ScriptableObject {

        public bool matches() {
            return matches(FilterContext.empty);
        }

        public abstract bool matches(FilterContext ctx);

        public virtual bool contains<T>() {
            return GetType() == typeof(T);
        }
    }
}