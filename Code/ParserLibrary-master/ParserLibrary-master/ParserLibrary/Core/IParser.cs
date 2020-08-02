using AngleSharp.Html.Dom;

namespace ParserLibrary.Core
{
    public interface IParser<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}
