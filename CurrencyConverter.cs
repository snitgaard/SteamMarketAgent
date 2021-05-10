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
                string replacement = Regex.Replace(input, @"\t|\n|\r", "");
                string noCurrency = Regex.Replace(replacement, @"pуб.", "");
                double stringAsDouble = double.Parse(noCurrency, NumberStyles.Currency);
                double convertedResult = stringAsDouble / RUS;
                return convertedResult;
            }
            if (input.Contains("€"))
            {
                string.Format(input);
                string replacement = Regex.Replace(input, @"\t|\n|\r", "");
                string noCurrency = Regex.Replace(replacement, @"€", "");
                double stringAsDouble = double.Parse(noCurrency, NumberStyles.Currency);
                double convertedResult = stringAsDouble / EUR;
                return convertedResult;
            }
            if (input.Contains("¥"))
            {
                string.Format(input);
                string replacement = Regex.Replace(input, @"\t|\n|\r", "");
                string noCurrency = Regex.Replace(replacement, @"¥", "");
                double stringAsDouble = double.Parse(noCurrency, NumberStyles.Currency);
                double convertedResult = stringAsDouble / YUAN;
                return convertedResult;
            }
            return 0;
        }
    }
}
