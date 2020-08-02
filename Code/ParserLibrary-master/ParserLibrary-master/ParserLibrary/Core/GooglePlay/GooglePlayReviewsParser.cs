using AngleSharp.Html.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParserLibrary.Core.GooglePlay
{
    public class GooglePlayReviewsParser : IParser<string[][]>
    {
        private static string pattern = @"""(gp:[^\""|\\]+)[^\]]+[^\""]+""([^\[]+)\\*"",\[(\d+),";
        public string[][] Parse(IHtmlDocument document)
        {
            var list  = new List<string[]>();

            RegexOptions options = RegexOptions.Multiline;

            foreach (Match m in Regex.Matches(document.Source.Text, pattern, options))
            {
                list.Add(new string[] { m.Groups[2].Value ,m.Groups[3].Value,m.Groups[1].Value});
            }

            return list.ToArray();
        }
    }
}
