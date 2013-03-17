using System;
using System.Collections.Generic;
using System.Text;

namespace Indvisible.RealisticData.Extensions
{
    public static class StringExtensions
    {
        public static string Replace(this string str, char item, Func<char> character)
        {
            var builder = new StringBuilder(str.Length);
            foreach (char c in str)
            {
                builder.Append(c == item ? character() : c);
            }

            return builder.ToString();
        }

        public static string Numerify(this string numberString)
        {
            return numberString.Replace('#', () => DataRandom.Rand.Next(10).ToString().ToCharArray()[0]);
        }

        public static string Letterify(this string letterString)
        {
            return letterString.Replace('?', () => 'a'.To('z').Rand());
        }

        public static string Bothify(this string str)
        {
            return Letterify(Numerify(str));
        }

        public static IEnumerable<char> To(this char from, char to)
        {
            for (char i = from; i <= to; i++)
            {
                yield return i;
            }
        }
    }
}