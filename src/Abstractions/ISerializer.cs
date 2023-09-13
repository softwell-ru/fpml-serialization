namespace SoftWell.Fpml.Serialization;

public interface ISerializer<TBaseType>
{
    TBaseType Deserialize(Stream stream);

    Stream Serialize(TBaseType obj, SerializationOptions options);
}
