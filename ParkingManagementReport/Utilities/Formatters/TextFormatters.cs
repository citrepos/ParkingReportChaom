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

        #region DATE/MONTH/YEAR FORMAT
        public static string ExtractDateMonthYearWithFullMonthName(DateTime dateTime)
        {
            string extractedDateTime = dateTime.ToString("dd MMMM yyyy");

            return extractedDateTime;
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

            if (DateTime.TryParseExact(fullDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                CultureInfo thaiCulture = new CultureInfo("th-TH");
                
                return thaiCulture.DateTimeFormat.GetMonthName(parsedDate.Month);
            }

            return string.Empty; // return empty if cannot parse
        }
        #endregion
    }
}