using System;
using System.Text.RegularExpressions;

namespace SteamMarketAgent
{
    public class CurrencyConverter
    {
        private Double RUS = 73.75;
        private Double EUR = 0.82;
        private Double YUAN = 6.41;
        private Double ZL = 3.76;
        private Double GBP = 0.71;
        private Double TL = 8.50;
        private Double CDN = 1.21;
        private Double BR = 5.30;
        private Double UKH = 27.75;
        private Double CHF = 0.91;
        private Double AUD = 1.29;
        private Double HKD = 7.77;
        private Double RM = 4.13;

        public double currencyConversion(string input)
        {

            if (input.Contains("RM"))
            {
                string replacement = Regex.Replace(input, @"RM|\t|\n|\r|\--", "");
                double parsed = double.Parse(replacement.Trim());
                double convertedResult = parsed / RM;
                double result = Math.Round(convertedResult, 2);
                return result;
            }

            if (input.Contains("HK$"))
            {
                string replacement = Regex.Replace(input, @"HK\$|\t|\n|\r|\--", "");
                double parsed = double.Parse(replacement.Trim());
                double convertedResult = parsed / HKD;
                double result = Math.Round(convertedResult, 2);
                return result;
            }
            if (input.Contains("AUD"))
            {
                string replacement = Regex.Replace(input, @"A\$|\t|\n|\r|\--", "");
                double parsed = double.Parse(replacement.Trim());
                double convertedResult = parsed / AUD;
                double result = Math.Round(convertedResult, 2);
                return result;
            }
            if (input.Contains("CHF"))
            {
                string replacement = Regex.Replace(input, @"CHF|\t|\n|\r|\--", "");
                double parsed = double.Parse(replacement.Trim());
                double convertedResult = parsed / CHF;
                double result = Math.Round(convertedResult, 2);
                return result;
            }
            if (input.Contains("₴"))
            {
                string replacement = Regex.Replace(input, @"₴|\t|\n|\r|\--|\p{Zs}", "");
                double parsed = double.Parse(replacement.Trim());
                double convertedResult = parsed / UKH;
                double result = Math.Round(convertedResult, 2);
                return result;
            }
            if (input.Contains("R$"))
            {
                string replacement = Regex.Replace(input, @"R\$|\t|\n|\r|\--", "");
                double parsed = double.Parse(replacement.Trim());
                double convertedResult = parsed / BR;
                double result = Math.Round(convertedResult, 2);
                return result;
            }
            if (input.Contains("CDN$"))
            {
                string replacement = Regex.Replace(input, @"CDN\$|\t|\n|\r|\--", "");
                double parsed = double.Parse(replacement.Trim());
                double convertedResult = parsed / CDN;
                double result = Math.Round(convertedResult, 2);
                return result;
            }
            if (input.Contains("TL"))
            {
                string replacement = Regex.Replace(input, @"TL|\t|\n|\r|\--", "");
                double parsed = double.Parse(replacement.Trim());
                double convertedResult = parsed / TL;
                double result = Math.Round(convertedResult, 2);
                return result;
            }

            if (input.Contains("zł"))
            {
                string replacement = Regex.Replace(input, @"zł|\t|\n|\r|--|\p{Zs}", "");
                double parsed = double.Parse(replacement.Trim());
                double convertedResult = parsed / ZL;
                double result = Math.Round(convertedResult, 2);
                return result;
            }

            if (input.Contains("£"))
            {
                string replacement = Regex.Replace(input, @"£|\t|\n|\r|\--", "");
                double parsed = double.Parse(replacement.Trim());
                double convertedResult = parsed / GBP;
                double result = Math.Round(convertedResult, 2);
                return result;
            }

            if (input.Contains("pуб."))
            {
                string replacement = Regex.Replace(input, @"pуб.|\t|\n|\r|\--", "");
                double parsed = double.Parse(replacement.Trim());
                double convertedResult = parsed / RUS;
                double result = Math.Round(convertedResult, 2);
                return result;
            }
            if (input.Contains("€"))
            {
                string replacement = Regex.Replace(input, @"€|\t|\n|\r|\--", "");
                string noComma = Regex.Replace(replacement, @"\.", ",");
                double parsed = double.Parse(noComma.Trim());
                double convertedResult = parsed / EUR;
                double result = Math.Round(convertedResult, 2);
                return result;
            }
            if (input.Contains("¥"))
            {
                string replacement = Regex.Replace(input, @"¥|\t|\n|\r|\--", "");
                string noComma = Regex.Replace(replacement, @"\,", ".");
                Math.Round(Convert.ToDecimal(noComma));
                double parsed = double.Parse(noComma.Trim());
                double rounded = Math.Round(parsed, MidpointRounding.AwayFromZero);
                double convertedResult = rounded / YUAN;
                double result = Math.Round(convertedResult, 2);
                return result;
            }
            if (input.Contains("USD"))
            {
                string replacement = Regex.Replace(input, @"\$|USD|\t|\n|\r|", "");
                string noComma = Regex.Replace(replacement, @"\.", ",");
                double parsed = double.Parse(noComma.Trim());
                double result = Math.Round(parsed, 2);
                return parsed;
            }
            return 0;
        }
    }
}
