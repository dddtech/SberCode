
using System.Collections.Generic;
using System.Linq;

namespace ParserLibrary.Core.GooglePlay
{
    public class GooglePlayReviewsSettings : IParserSettings
    {
        public GooglePlayReviewsSettings(string packageName)
        {
            StartPoint = 0;
            EndPoint = 0;
            var list = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("f.req", "[[[\"UsvDTd\",\"[null,null,[2,null,[100,null,null],null,[]],[\\\""+packageName+"\\\",7]]\",null,\"generic\"]]]") };
            PostData = list.AsEnumerable();
            PackageName = packageName;
        }
        public string BaseUrl { get; set; } = @"https://play.google.com/_/PlayStoreUi/data/batchexecute?hl=ru";
        public string Prefix { get; set; } = "";
        public string PackageName { get; }
        public int StartPoint { get; set; }
        public int EndPoint { get; set; }
        public IEnumerable<KeyValuePair<string, string>> PostData { get; set; }
    }
}
