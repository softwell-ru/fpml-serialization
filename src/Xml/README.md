# SoftWell.Fpml.Serialization.Xml

Пакеты для сериализации fpml в xml


## Использование

Хотя и предполагается, что для каждой схемы будет создан свой пакет, можно использовать и напрямую.
Xml-сериализаторам нужно знать, в какие классы десериализовать xml тэги. 
Для этого нам надо знать, какие есть типы, наследующиеся от базового.

По умолчанию мы смотрим все типы из сборки с базовым, но, если есть какие-то еще типы в других сборках, можно передать список этих типов и\или список сборок в конструктор:


```c#
ISerializer<Document> serializer = new XmlSerializer<Document>(new XmlSerializationOptions<Document>
{
    KnownAssemblies = GetMyCustomAssemblies(),
    KnownTypes = GetMyCustomTypes()
});

```

или при регистрации зависимостей:

```c#
services.AddXmlSerialization<Document, ISerializer<Document>, XmlSerializer<Document>>(
    opts => 
    {
        opts.KnownAssemblies = GetMyCustomAssemblies();
        opts.KnownTypes = GetMyCustomTypes();
    });
```