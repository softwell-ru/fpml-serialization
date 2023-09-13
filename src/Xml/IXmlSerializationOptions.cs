using System.Reflection;

namespace SoftWell.Fpml.Serialization.Xml;

public interface IXmlSerializationOptions<TBaseType>
{
    IEnumerable<Assembly>? KnownAssemblies { get; set; }

    IEnumerable<Type>? KnownTypes { get; set; }
}
