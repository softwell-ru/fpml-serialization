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
        if (serializer is null) throw new ArgumentNullException(nameof(serializer));
        if (str is null) throw new ArgumentNullException(nameof(str));
        if (strEncoding is null) throw new ArgumentNullException(nameof(strEncoding));

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
        if (serializer is null) throw new ArgumentNullException(nameof(serializer));
        if (obj is null) throw new ArgumentNullException(nameof(obj));
        if (options is null) throw new ArgumentNullException(nameof(options));

        using var stream = new MemoryStream();

        serializer.Serialize(stream, obj, options);
        stream.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(stream, options.Encoding);

        ct.ThrowIfCancellationRequested();
        var str = await reader.ReadToEndAsync();

        return str;
    }
}