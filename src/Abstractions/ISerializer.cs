namespace SoftWell.Fpml.Serialization;

public interface ISerializer<TBaseType>
{
    TBaseType Deserialize(Stream stream);

    void Serialize(Stream stream, TBaseType obj, SerializationOptions options);
}
