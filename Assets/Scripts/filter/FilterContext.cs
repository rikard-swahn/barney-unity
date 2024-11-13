using System;
using System.Collections.Generic;

namespace net.gubbi.goofy.filter {
    public class FilterContext {

        public static readonly FilterContext empty = builder().build();
        public Dictionary <string, Object> Properties {get; private set; }

        public static readonly string SELECTED_ITEM = "selectedItem";
        public static readonly string CHARACTER = "character";
        public static readonly string SCENE_ITEM = "sceneItem";
        public static readonly string STRING = "string";

        public FilterContext() {
            Properties = new Dictionary<string, object> ();
        }

        public static FilterContextBuilder builder() {
            return new FilterContextBuilder();
        }

        public FilterContext mergeProperties(FilterContext ctx) {
            foreach (var property in ctx.Properties) {
                if (property.Value != null) {
                    Properties[property.Key] = property.Value;
                }
            }

            return this;
        }

        public T getProperty<T> (string key)  {
            object value = Properties [key];
            return (T) Convert.ChangeType(value, typeof(T));
        }

        public class FilterContextBuilder {
            private FilterContext ctx;

            public FilterContextBuilder() {
                ctx = new FilterContext();
            }

            public FilterContextBuilder property(string key, object value) {
                ctx.Properties[key] = value;
                return this;
            }

            public FilterContext build() {
                return ctx;
            }

        }


    }
}