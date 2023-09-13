using SoftWell.Fpml.Serialization.Xml;

namespace SoftWell.Fpml.Confirmation.Serialization.Xml;

public class XmlDocumentSerializer : XmlSerializer<Document>, IDocumentSerializer
{
    public XmlDocumentSerializer(IXmlSerializationOptions<Document> options) : base(options)
    {
    }
}
