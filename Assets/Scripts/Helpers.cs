using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TomatechGames.CodeIdiom
{
    public static class Helpers
    {
        public static string JumbleLetters(this string original)
        {
            original = original.Replace(" ", "").ToUpper();
            System.Random rng = new(original.GetHashCode());
            var charArray = original.ToCharArray();

            //Knuth shuffle
            int maxIndex = charArray.Length;
            while (maxIndex > 1)
            {
                int randomIndex = rng.Next(maxIndex--);
                (charArray[randomIndex], charArray[maxIndex]) = (charArray[maxIndex], charArray[randomIndex]);
            }

            return new(charArray);
        }



        public static string FormatToTimeInSeconds(this int seconds)
        {
            int minutes = seconds / 60;
            seconds %= 60;
            int hours = minutes / 60;
            minutes %= 60;
            int days = hours / 24;
            hours %= 24;

            string FormatNumber(int number) => (number < 10 ? "0" : "") + number;

            if (days > 0)
                return $"{days}:{FormatNumber(hours)}:{FormatNumber(minutes)}:{FormatNumber(seconds)}";
            else if (hours > 0)
                return $"{FormatNumber(hours)}:{FormatNumber(minutes)}:{FormatNumber(seconds)}";
            else if (minutes > 0)
                return $"{FormatNumber(minutes)}:{FormatNumber(seconds)}";
            else
                return $"00:{FormatNumber(seconds)}";
        }
    }
}
