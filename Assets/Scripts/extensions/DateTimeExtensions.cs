using System;
using System.Globalization;
using net.gubbi.goofy.util;

namespace net.gubbi.goofy.extensions {
    public static class DateTimeExtensions {

        public static string ToStringNoCulture(this DateTime dateTime, string format) {
            return dateTime.ToString(format, DateTimeFormatInfo.InvariantInfo);
        }
        
        public static string ToStringStandard(this DateTime dateTime) {
            return dateTime.ToString(DateTimeUtil.DATE_FORMAT, DateTimeFormatInfo.InvariantInfo);
        }
    }
}