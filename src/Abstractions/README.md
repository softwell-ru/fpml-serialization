# SoftWell.Fpml.Confirmation.Serialization.Abstractions

Абстракции для сериализации fpml.

Содержит [интерфейс](./IDocumentSerializer.cs) сериалайзера и его [экстеншены](./DocumentSerializerExtensions.cs).

Сериалайзер требует generic параметр с базовым типом. Например, в случае fpml базовым типом может быть SoftWell.Fpml.Confirmation.Document, SoftWell.Fpml.Pretrade.Document и тд.