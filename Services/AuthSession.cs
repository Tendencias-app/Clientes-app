namespace CarsATTM_CustomerApp.Services;

public sealed class AuthSession
{
    public string? Jwt { get; private set; }

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(Jwt);

    public void SignIn(string jwt)
    {
        if (string.IsNullOrWhiteSpace(jwt))
        {
            throw new ArgumentException("El JWT no puede estar vacío.", nameof(jwt));
        }

        Jwt = jwt;
    }

    public void SignOut() => Jwt = null;
}
