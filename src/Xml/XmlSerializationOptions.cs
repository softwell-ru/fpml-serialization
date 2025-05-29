using System.Reflection;
using System.Xml;

namespace SoftWell.Fpml.Serialization.Xml;

public class XmlSerializationOptions<TBaseType> : IXmlSerializationOptions<TBaseType>
{
    public IEnumerable<Assembly>? KnownAssemblies { get; set; }

    public IEnumerable<Type>? KnownTypes { get; set; }

    public Action<XmlWriterSettings>? ConfigureXmlWriterSettings { get; set; }
}