using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary.Core
{
    public interface IParserSettings
    {
        string BaseUrl { get; set; }

        string Prefix { get; set; }
        string PackageName { get; }
        int StartPoint { get; set; }
        int EndPoint { get; set; }
        IEnumerable<KeyValuePair<string, string>> PostData { get; set; }
    }
}
