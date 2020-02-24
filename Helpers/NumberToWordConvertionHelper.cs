using System;
using System.Linq;

namespace Nutava.Test.NumberToWord.Helpers
{
    public static class NumberToWordConvertionHelper
    {
        /// <summary>
        /// Convers input number string to its equivalent english word.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>equivalent word for the input number</returns>
        public static string ConvertNumberToWords(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input));
            }
            string result = string.Empty;
            try
            {
                bool exit = false;
                double number = Convert.ToDouble(input);
                input = number.ToString();
                if (number > 0)
                {
                    int numDigits = input.Length;
                    int position = 0;

                    // get unit/place for the number.
                    string unit = $" { DataHelper.DigitsToUnitMap.FirstOrDefault(x => x.Value.Contains(numDigits)).Key} ";
                   
                    GetWords(input, ref result, ref exit, numDigits, ref position);

                    //Split the number string based on position and recursively get the words for the numbers from the respective maps until exit.
                    if (!exit)
                    {
                        if (input.Substring(0, position) != "0" && input.Substring(position) != "0")
                            result = $"{ConvertNumberToWords(input.Substring(0, position))}{unit}{ ConvertNumberToWords(input.Substring(position))}";

                        else
                            result = $"{ConvertNumberToWords(input.Substring(0, position))}{ConvertNumberToWords(input.Substring(position))}";
                    }
                    if (result.Trim().Equals(unit.Trim())) result = " ";
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return result.Trim();
        }
        
        /// <summary>
        /// Get words for input number string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="result"></param>
        /// <param name="exit"></param>
        /// <param name="numDigits"></param>
        /// <param name="position"></param>
        private static void GetWords(string input, ref string result, ref bool exit, int numDigits, ref int position)
        {
             //If num of digits is 1, get the words directly from the ones map.
            if (numDigits == 1)
            {
                DataHelper.OnesToWordsMap.TryGetValue(Convert.ToInt32(input), out result);
                exit = true;
            }
            // If num of digits is 2, check for double digit map and get the word, if not found split the digits and get the values form respective maps.
            else if (numDigits == 2)
            {
                if (!DataHelper.TensToWordsMap.TryGetValue(Convert.ToInt32(input), out result))
                {
                    if (DataHelper.TensToWordsMap.TryGetValue(Convert.ToInt32(input.Substring(0, 1)), out result))
                        result += " " + DataHelper.OnesToWordsMap[Convert.ToInt32(input.Substring(1, 1))];
                }
                exit = true;
            }
            //If num of digits is 3 or higher get the position from DigitsToUnitMap to split the string accordingly. 
            else if (DataHelper.DigitsToUnitMap["Hundred"].Any(x => x == numDigits))
                position = (numDigits % 3) + 1;
            else if (DataHelper.DigitsToUnitMap["Thousand"].Any(x => x == numDigits))
                position = (numDigits % 4) + 1;
            else if (DataHelper.DigitsToUnitMap["Million"].Any(x => x == numDigits))
                position = (numDigits % 7) + 1;
            else if (DataHelper.DigitsToUnitMap["Billion"].Any(x => x == numDigits))
                position = (numDigits % 10) + 1;
            else
                exit = true;
        }
    }
}
