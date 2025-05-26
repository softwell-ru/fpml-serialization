using SoftWell.Fpml.Serialization;
using SoftWell.Fpml.Serialization.Xml;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddXmlSerialization<TBaseType>(
        this IServiceCollection services,
        Action<IXmlSerializationOptions<TBaseType>>? configure = null)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        return services.AddXmlSerialization<TBaseType, ISerializer<TBaseType>, XmlSerializer<TBaseType>>(configure);
    }

    public static IServiceCollection AddXmlSerialization<TBaseType, TSerializer, TSerializerImplementation>(
        this IServiceCollection services,
        Action<IXmlSerializationOptions<TBaseType>>? configure = null)
            where TSerializer : class, ISerializer<TBaseType>
            where TSerializerImplementation : class, TSerializer
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        var options = new XmlSerializationOptions<TBaseType>();
        configure?.Invoke(options);

        return services
            .AddSingleton<IXmlSerializationOptions<TBaseType>>(options)
            .AddSingleton<TSerializer, TSerializerImplementation>();
    }
}