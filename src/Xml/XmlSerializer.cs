using System.Collections.Concurrent;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace SoftWell.Fpml.Serialization.Xml;

public class XmlSerializer<TBaseType> : ISerializer<TBaseType>
{
    private readonly XmlSerializerFactory _xmlSerializerFactory = new();

    private readonly Dictionary<string, Type> _typesMapping;

    private readonly ConcurrentDictionary<Type, XmlSerializer> _serializers = new();

    public XmlSerializer(IXmlSerializationOptions<TBaseType> options)
    {
        if (options is null) throw new ArgumentNullException(nameof(options));

        _typesMapping = GetNamedTypes(options);
    }

    public TBaseType Deserialize(Stream stream)
    {
        if (stream is null) throw new ArgumentNullException(nameof(stream));

        string rootNodeName = null!;

        using var reader = new XmlTextReader(stream)
        {
            DtdProcessing = DtdProcessing.Ignore,
        };

        if (reader.MoveToContent() == XmlNodeType.Element)
        {
            rootNodeName = reader.Name;
        }

        if (string.IsNullOrWhiteSpace(rootNodeName)) throw new InvalidOperationException("Cannot get root node name");

        var serializer = GetSerializerByName(rootNodeName);
        var doc = serializer.Deserialize(reader);

        if (doc is not TBaseType res) throw new InvalidOperationException("Cannot deserialize from stream");

        return res;
    }

    public void Serialize(Stream stream, TBaseType obj, SerializationOptions options)
    {
        if (stream is null) throw new ArgumentNullException(nameof(stream));
        if (obj is null) throw new ArgumentNullException(nameof(obj));
        if (options is null) throw new ArgumentNullException(nameof(options));

        var serializer = GetSerializerByType(obj.GetType());
        using var streamWriter = XmlWriter.Create(stream, new()
        {
            Encoding = options.Encoding,
            Indent = options.PrettyPrint
        });

        serializer.Serialize(streamWriter, obj);
        streamWriter.Flush();
    }

    private XmlSerializer GetSerializerByName(string name)
    {
        if (!_typesMapping.TryGetValue(name, out var type)) throw new NotImplementedException($"Unknown name {name}");

        return GetSerializerByType(type);
    }

    private XmlSerializer GetSerializerByType(Type type)
    {
        return _serializers.GetOrAdd(
            type,
            x => _xmlSerializerFactory.CreateSerializer(type));
    }

    private static Dictionary<string, Type> GetNamedTypes(IXmlSerializationOptions<TBaseType> options)
    {
        return typeof(TBaseType).Assembly.GetTypes()
            .Concat(options.KnownAssemblies?.SelectMany(x => x.GetTypes()) ?? Enumerable.Empty<Type>())
            .Concat(options.KnownTypes ?? Enumerable.Empty<Type>())
            .Where(x => x.IsSubclassOf(typeof(TBaseType)))
            .Distinct()
            .Select(x => new
            {
                Type = x,
                RootElementName = x.GetCustomAttribute<XmlRootAttribute>(false)?.ElementName
            })
            .Where(x => x.RootElementName is not null)
            .ToDictionary(x => x.RootElementName!, x => x.Type, StringComparer.Ordinal);
    }
}
