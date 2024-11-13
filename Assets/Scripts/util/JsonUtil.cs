using System;
using FullSerializer;

namespace net.gubbi.goofy.util {
    public class JsonUtil {
        private static readonly fsSerializer _serializer = new fsSerializer();
        
        private static volatile JsonUtil instance;
        private static object syncRoot = new Object();
        private JsonUtil() {
            _serializer.Config.CustomDateTimeFormatString = DateTimeUtil.DATE_FORMAT;
        }
        public static JsonUtil Instance {
            get {
                if (instance == null) {
                    lock (syncRoot) {
                        if (instance == null) {
                            instance = new JsonUtil();
                        }
                    }
                }
                return instance;
            }
        }

        public string Serialize<T>(T value) {
            fsData data;
            _serializer.TrySerialize(typeof(T), value, out data).AssertSuccessWithoutWarnings();
            return fsJsonPrinter.CompressedJson(data);
        }

        public T Deserialize<T>(string json) {
            fsData data = fsJsonParser.Parse(json);
            object deserialized = null;
            _serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();
            return (T)deserialized;
        }
        
    }
}