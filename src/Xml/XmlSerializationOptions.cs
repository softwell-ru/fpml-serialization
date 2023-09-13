using System.Reflection;

namespace SoftWell.Fpml.Serialization.Xml;

public class XmlSerializationOptions<TBaseType> : IXmlSerializationOptions<TBaseType>
{
    public IEnumerable<Assembly>? KnownAssemblies { get; set; }

    public IEnumerable<Type>? KnownTypes { get; set; }
}