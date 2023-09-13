using SoftWell.Fpml.Confirmation;
using SoftWell.Fpml.Confirmation.Serialization;
using SoftWell.Fpml.Confirmation.Serialization.Xml;
using SoftWell.Fpml.Serialization.Xml;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddXmlFpmlConfirmationSerialization(
        this IServiceCollection services,
        Action<IXmlSerializationOptions<Document>>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        return services.AddXmlSerialization<Document, IDocumentSerializer, XmlDocumentSerializer>(configure);
    }
}