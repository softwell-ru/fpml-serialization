using System.Text;

namespace SoftWell.Fpml.Serialization;

public static class SerializerExtensions
{
    public static TBaseType DeserializeFromUtf8String<TBaseType>(
        this ISerializer<TBaseType> serializer,
        string utf8String)
    {
        return serializer.DeserializeFromString(utf8String, Encoding.UTF8);
    }

    public static TBaseType DeserializeFromString<TBaseType>(
        this ISerializer<TBaseType> serializer,
        string str,
        Encoding strEncoding)
    {
        ArgumentNullException.ThrowIfNull(serializer);
        ArgumentNullException.ThrowIfNull(str);
        ArgumentNullException.ThrowIfNull(strEncoding);

        using var stream = new MemoryStream(strEncoding.GetBytes(str));
        var res = serializer.Deserialize(stream);
        return res;
    }

    public static Task<string> SerializeToUtf8StringAsync<TBaseType>(
        this ISerializer<TBaseType> serializer,
        TBaseType obj,
        bool prettyPrint = false,
        CancellationToken ct = default)
    {
        return serializer.SerializeToStringAsync(
            obj,
            new SerializationOptions
            {
                Encoding = Encoding.UTF8,
                PrettyPrint = prettyPrint
            },
            ct);
    }

    public static async Task<string> SerializeToStringAsync<TBaseType>(
        this ISerializer<TBaseType> serializer,
        TBaseType obj,
        SerializationOptions options,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(serializer);
        ArgumentNullException.ThrowIfNull(obj);
        ArgumentNullException.ThrowIfNull(options);

        using var stream = serializer.Serialize(obj, options);

        using var reader = new StreamReader(stream, options.Encoding);

        var xml = await reader.ReadToEndAsync(ct);

        return xml;
    }
}