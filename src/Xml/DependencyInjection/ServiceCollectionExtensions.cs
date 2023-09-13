using SoftWell.Fpml.Serialization;
using SoftWell.Fpml.Serialization.Xml;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddXmlSerialization<TBaseType, TSerializer, TSerializerImplementation>(
        this IServiceCollection services,
        Action<IXmlSerializationOptions<TBaseType>>? configure = null)
            where TSerializer : class, ISerializer<TBaseType>
            where TSerializerImplementation : class, TSerializer
    {
        ArgumentNullException.ThrowIfNull(services);

        var options = new XmlSerializationOptions<TBaseType>();
        configure?.Invoke(options);

        return services
            .AddSingleton<IXmlSerializationOptions<TBaseType>>(options)
            .AddSingleton<TSerializer, TSerializerImplementation>();
    }
}