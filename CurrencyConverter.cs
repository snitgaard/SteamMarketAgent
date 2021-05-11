using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SteamMarketAgent
{
    public class CurrencyConverter
    {
        private Double RUS = 73.75;
        private Double EUR = 0.82;
        private Double YUAN = 6.41;
        // RM & TL

        public double currencyConversion(string input)
        {
            string.Format(input);
            if (input.Contains("pуб."))
            {
                string replacement = Regex.Replace(input, @"pуб.|\t|\n|\r|\--", "");
                double parsed = double.Parse(replacement.Trim(),
                   NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands,
                   CultureInfo.CurrentCulture);
                double convertedResult = parsed / RUS;
                double result = Math.Round(convertedResult, 2);
                return result;
            }
            if (input.Contains("€"))
            {
                string replacement = Regex.Replace(input, @"€|\t|\n|\r|\--", "");
                double parsed = double.Parse(replacement.Trim(),
                    NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands,
                    CultureInfo.CurrentCulture);
                double convertedResult = parsed / EUR;
                double result = Math.Round(convertedResult, 2);
                return result;
            }
            if (input.Contains("¥"))
            {
                string replacement = Regex.Replace(input, @"¥|\t|\n|\r|\--", "");
                string noComma = Regex.Replace(replacement, @"\.", ",");
                double parsed = double.Parse(noComma.Trim(),
                    NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands,
                    CultureInfo.CurrentCulture);
                double rounded = Math.Round(parsed, MidpointRounding.AwayFromZero);
                double convertedResult = rounded / YUAN;
                double result = Math.Round(convertedResult, 2);
                return result;
            }
            if (input.Contains("USD"))
            {
                string replacement = Regex.Replace(input, @"\$|USD|\t|\n|\r|", "");
                string noComma = Regex.Replace(replacement, @"\.", ",");
                double parsed = double.Parse(noComma.Trim(),
                    NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands,
                    CultureInfo.CurrentCulture);
                double result = Math.Round(parsed, 2);
                return parsed;
            }
            return 0;
        }
    }
}
