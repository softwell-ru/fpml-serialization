using System.Collections.Concurrent;
using System.Xml;
using System.Xml.Serialization;

namespace SoftWell.Fpml.Serialization.Xml;

public class XmlSerializer<TBaseType> : ISerializer<TBaseType>
{
    private readonly XmlSerializerFactory _xmlSerializerFactory = new();

    private readonly IReadOnlyDictionary<string, Type> _typesMapping;

    private readonly ConcurrentDictionary<Type, XmlSerializer> _serializers = new();

    public XmlSerializer(IXmlSerializationOptions<TBaseType> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _typesMapping = new Dictionary<string, Type>(
            EnumerateNamedTypes(options),
            StringComparer.Ordinal);
    }

    public TBaseType Deserialize(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

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

    public Stream Serialize(TBaseType obj, SerializationOptions options)
    {
        ArgumentNullException.ThrowIfNull(obj);
        ArgumentNullException.ThrowIfNull(options);

        var serializer = GetSerializerByType(obj.GetType());
        var stream = new MemoryStream();
        var streamWriter = XmlWriter.Create(stream, new()
        {
            Encoding = options.Encoding,
            Indent = options.PrettyPrint
        });

        serializer.Serialize(streamWriter, obj);
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
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

    private static IEnumerable<KeyValuePair<string, Type>> EnumerateNamedTypes(IXmlSerializationOptions<TBaseType> options)
    {
        var types = typeof(TBaseType).Assembly.GetTypes()
            .Concat(options.KnownAssemblies?.SelectMany(x => x.GetTypes()) ?? Enumerable.Empty<Type>())
            .Concat(options.KnownTypes ?? Enumerable.Empty<Type>())
            .Where(x => x.IsSubclassOf(typeof(TBaseType)))
            .Distinct()
            .ToList();

        foreach (var t in types)
        {
            var attr = t.GetCustomAttributes(typeof(XmlRootAttribute), false).FirstOrDefault();
            if (attr is XmlRootAttribute xmlRoot)
            {
                yield return new KeyValuePair<string, Type>(xmlRoot.ElementName, t);
            }
        }
    }
}
