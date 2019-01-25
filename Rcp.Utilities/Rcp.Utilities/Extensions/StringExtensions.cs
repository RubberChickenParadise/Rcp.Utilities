using System;
using System.Globalization;
using System.Text;

namespace Rcp.Utilities.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Checks if the string is null or empty
        /// </summary>
        /// <param name="sVar">String Variable being checked</param>
        /// <returns>Returns if the string is Null or not.</returns>
        public static bool IsNullOrEmpty(this string sVar)
        {
            return string.IsNullOrEmpty(sVar);
        }

        /// <summary>
        /// Compares source string to specified value and ignores case.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string source, string value)
        {
            return string.Equals(source, value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns an Invariant Culture Formatted DateTime from a string
        /// </summary>
        /// <param name="source"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime FormatAsDate(this string source, string format)
        {
            var provider = CultureInfo.InvariantCulture;

            return DateTime.ParseExact(source, format, provider);
        }

        /// <summary>
        /// Converts a string to a Base64 string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToBase64(this string value)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// Converts a Base64 string to UTF8
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FromBase64(this string value)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }
    }
}