using System.Net;
using System.Net.Http.Json;
namespace CarsATTM_CustomerApp.Services;

public sealed class LoginService(HttpClient httpClient) : ILoginService
{
    private readonly string loginEndpoint = "api/auth/login";

    public async Task<string?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        //mock temporal
        await Task.Delay(2000, cancellationToken);

        return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOjEwMDEsIm5hbWUiOiJKb2huIERvZSIsImFkbWluIjp0cnVlLCJpYXQiOjE1MTYyMzkwMjJ9.EsmaXlSsONbYNacAH1rHedh4ji6_IDBrAukVvZvYFHs";


        using var response = await httpClient.PostAsJsonAsync(loginEndpoint, request, cancellationToken);

        if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken);
        return string.IsNullOrWhiteSpace(result?.Token) ? null : result.Token;
    }
}

public sealed class ApiOptions
{
    public string LoginBaseUrl { get; set; } = "https://localhost:5001/";
    public string MultasBaseUrl { get; set; } = "https://localhost:5002/";
}
