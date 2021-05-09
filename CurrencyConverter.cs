using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SteamMarketAgent
{
    public class CurrencyConverter
    {
        private Double RUS = 73.75;
        private Double EUR = 0.82;
        private double YEN = 108.59;

        public double currencyConversion(string input)
        {
            if (input.Contains("pуб"))
            {
                input.Replace("pуб", "");
                string replacement = Regex.Replace(input, @"\t|\n|\r|\p{уб}|\r|\r", "");
                Console.WriteLine(replacement);
                double stringAsNumber = Double.Parse(replacement.Trim());
                double convertedResult = stringAsNumber / RUS;
                return convertedResult;
            }
            if (input.Contains("€"))
            {
                input.Replace("€", "");
                string replacement = Regex.Replace(input, @"\t|\n|\r", "");
                double stringAsNumber = Double.Parse(replacement.Trim());
                double convertedResult = stringAsNumber / EUR;
                return convertedResult;
            }

            if (input.Contains("¥"))
            {
                input.Replace("¥", "");
                string replacement = Regex.Replace(input, @"\t|\n|\r", "");
                double stringAsNumber = Double.Parse(replacement.Trim());
                double convertedResult = stringAsNumber / YEN;
                return convertedResult;
            }

            return 0;
        }
    }
}
