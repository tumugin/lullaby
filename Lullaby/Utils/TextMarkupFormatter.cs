namespace Lullaby.Utils;

using AngleSharp;
using AngleSharp.Dom;

public class TextMarkupFormatter : IMarkupFormatter
{
    public string LiteralText(ICharacterData text) => text.Data;

    public string Comment(IComment comment) => string.Empty;

    public string Doctype(IDocumentType doctype) => string.Empty;

    public string Processing(IProcessingInstruction processing) => string.Empty;

    public string Text(ICharacterData text) => text.Data.Trim();

    public string OpenTag(IElement element, bool selfClosing)
    {
        switch (element.LocalName)
        {
            case "p":
                return "\n\n";
            case "br":
                return "\n";
            case "span":
                return " ";
        }

        return string.Empty;
    }

    public string CloseTag(IElement element, bool selfClosing) => string.Empty;
}
