using System.Collections.Generic;

namespace Nutava.Test.NumberToWord.Helpers
{
    public static class DataHelper
    {
        /// <summary>
        /// Maps single digits to their equivalent english words.
        /// </summary>
        public static readonly Dictionary<int, string> OnesToWordsMap = new Dictionary<int, string>()
        {
            {0,"Zero" },
            {1,"One" },
            {2,"Two" },
            {3,"Three" },
            {4,"Four" },
            {5,"Five" },
            {6,"Six" },
            {7,"Seven" },
            {8,"Eight" },
            {9,"Nine" },
        };

        /// <summary>
        /// Maps double digits to their equivalent english words.
        /// </summary>
        public static readonly Dictionary<int, string> TensToWordsMap = new Dictionary<int, string>()
        {
            {10, "Ten" },
            {11,"Eleven" },
            {12,"Tweleve" },
            {13,"Thirteen" },
            {14,"Forteen" },
            {15,"Fifteen" },
            {16,"Sixteen" },
            {17,"Seventeen" },
            {18,"Eighteen" },
            {19,"Nineteen" },
            {2,"Twenty" },
            {3,"Thirty" },
            {4,"Forty" },
            {5,"Fifty" },
            {6,"Sixty" },
            {7,"Seventy" },
            {8,"Eighty" },
            {9,"Ninty" }
        };

        /// <summary>
        /// Maps units to their equivalent english words
        /// </summary>
        public static readonly Dictionary<string, List<int>> DigitsToUnitMap = new Dictionary<string, List<int>>
        {
            {"Hundred", new List<int>{ 3 } },
            {"Thousand" , new List<int>{ 4,5,6 }},
            {"Million", new List<int>{ 7,8,9 } },
            {"Billion", new List<int>{ 10,11,12 }},
        };
    }
}
