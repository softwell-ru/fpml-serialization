using System.Reflection;
using System.Xml;

namespace SoftWell.Fpml.Serialization.Xml;

public interface IXmlSerializationOptions<TBaseType>
{
    IEnumerable<Assembly>? KnownAssemblies { get; set; }

    IEnumerable<Type>? KnownTypes { get; set; }

    Action<XmlWriterSettings>? ConfigureXmlWriterSettings { get; set; }
}
