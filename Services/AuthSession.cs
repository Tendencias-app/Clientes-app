namespace CarsATTM_CustomerApp.Services;

public sealed class AuthSession
{
    public string? Jwt { get; private set; }
    public long? Sub { get; private set; }
    public string? Name { get; private set; }

    public event Action? Changed;

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(Jwt);

    public void SignIn(string jwt)
    {
        if (string.IsNullOrWhiteSpace(jwt))
        {
            throw new ArgumentException("El JWT no puede estar vacío.", nameof(jwt));
        }

        try
        {
            var payload = jwt.Split('.')[1];
            var payloadJson = Convert.FromBase64String(
                payload.Replace('-', '+').Replace('_', '/') + new string('=', (4 - payload.Length % 4) % 4));
            using var document = System.Text.Json.JsonDocument.Parse(payloadJson);
            var root = document.RootElement;

            Sub = root.GetProperty("sub").GetInt64();
            Name = root.GetProperty("name").GetString();
            Jwt = jwt;
        }
        catch (Exception exception) when (exception is IndexOutOfRangeException or FormatException or System.Text.Json.JsonException or KeyNotFoundException or InvalidOperationException)
        {
            throw new ArgumentException("El JWT no contiene claims válidos.", nameof(jwt), exception);
        }

        Changed?.Invoke();
    }

    public void SignOut()
    {
        Jwt = null;
        Sub = null;
        Name = null;
        Changed?.Invoke();
    }
}
