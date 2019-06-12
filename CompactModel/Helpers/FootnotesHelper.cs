using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CompactModel.Helpers
{
    internal static class FootnotesHelper
    {
        public static string Parse(uint number)
        {
            return new string(number.ToString(CultureInfo.InvariantCulture)
                .Select(c => footnotes[c]).ToArray());
        }

        private static Dictionary<char, char> footnotes
            = new Dictionary<char, char>()
            {
                { '0', '\u2080' },
                { '1', '\u2081' },
                { '2', '\u2082' },
                { '3', '\u2083' },
                { '4', '\u2084' },
                { '5', '\u2085' },
                { '6', '\u2086' },
                { '7', '\u2087' },
                { '8', '\u2088' },
                { '9', '\u2089' }
            };
    }
}
