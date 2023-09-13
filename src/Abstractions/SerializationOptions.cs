using System.Text;

namespace SoftWell.Fpml.Serialization;

public record SerializationOptions
{
    public Encoding Encoding { get; init; } = Encoding.UTF8;

    public bool PrettyPrint { get; init; }
}