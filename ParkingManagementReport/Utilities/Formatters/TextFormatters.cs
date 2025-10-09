using System;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ParkingManagementReport.Utilities.Formatters
{
    internal class TextFormatters
    {
        public static string ErrorStacktraceFromException(Exception exception)
        {
            if (exception == null)
                return "";

            return $"{exception.GetType()} | {exception.Message} | {exception.StackTrace}";
        }

        public static string ExtractThaiMonthFromDate(string fullDate)
        {
            if (string.IsNullOrWhiteSpace(fullDate))
                return string.Empty;

            string[] formats = {
            "yyyy-MM-dd", "yyyy/MM/dd", "dd-MM-yyyy", "dd/MM/yyyy",
            "yyyy-MM-dd HH:mm:ss", "yyyy/MM/dd HH:mm:ss",
            "dd-MM-yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss"
            };

            DateTime parsedDate;

            if (DateTime.TryParseExact(fullDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                CultureInfo thaiCulture = new CultureInfo("th-TH");
                return thaiCulture.DateTimeFormat.GetMonthName(parsedDate.Month);
            }

            return string.Empty; // return empty if cannot parse
        }

        public static string ExtractBuddhistEraFromDate(DateTime date)
        {
            var thaiCalendar = new ThaiBuddhistCalendar();
            int buddhistYear = thaiCalendar.GetYear(date);
            return $"พ.ศ. {buddhistYear}";
        }

        public static string ExtractChristianEraFromDate(DateTime date)
        {
            return $"ค.ศ. {date.Year}";
        }

        public static string RemoveSpecialCharacters(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";

            string trimmed = input.Trim();
            string cleaned = trimmed.Replace("\r", "").Replace("\n", "");

            return cleaned;
        }

        internal static string RemoveBracketFromName(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return Regex.Replace(input, @"^\[\d+\]\s*", "");
        }
    }
}
