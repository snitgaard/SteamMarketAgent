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
            if (input.Contains("pуб."))
            {
                string.Format(input);
                string replacement = Regex.Replace(input, @"\t|\n|\r|\--", "");
                string noCurrency = Regex.Replace(replacement, @"pуб.", "");
                double parsed = double.Parse(noCurrency.Trim(),
                   NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands,
                   CultureInfo.CurrentCulture);
                double convertedResult = parsed / RUS;
                return convertedResult;
            }
            if (input.Contains("€"))
            {
                string.Format(input);
                string replacement = Regex.Replace(input, @"\t|\n|\r|\--", "");
                string noCurrency = Regex.Replace(replacement, @"€", "");
                double parsed = double.Parse(noCurrency.Trim(),
                    NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands,
                    CultureInfo.CurrentCulture);
                double convertedResult = parsed / EUR;
                return convertedResult;
            }
            if (input.Contains("¥"))
            {
                string.Format(input);
                string replacement = Regex.Replace(input, @"\t|\n|\r|\--", "");
                string noComma = Regex.Replace(replacement, @"\.", ",");
                string noCurrency = Regex.Replace(noComma, @"¥", "");
                double parsed = double.Parse(noCurrency.Trim(),
                    NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands,
                    CultureInfo.CurrentCulture);
                double convertedResult = parsed / YUAN;
                return convertedResult;
            }
            if (input.Contains("USD"))
            {
                string.Format(input);
                string replacement = Regex.Replace(input, @"\t|\n|\r|", "");
                string noComma = Regex.Replace(replacement, @"\.", ",");
                string noCurrency = Regex.Replace(noComma, @"\$|USD", "");
                double parsed = double.Parse(noCurrency.Trim(),
                    NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands,
                    CultureInfo.CurrentCulture);
                return parsed;
            }
            return 0;
        }
    }
}
