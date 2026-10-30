// Сервер у процесі тестів. Дані зберігаються лише в пам'яті процесу,
// тому фабриці нічого налаштовувати.
public sealed class ApiFactory : WebApplicationFactory<Program>;
