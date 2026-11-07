using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;

namespace CarsATTM_CustomerApp.Services;

public sealed class LoginService(HttpClient httpClient, IOptions<ApiOptions> options) : ILoginService
{
    private readonly string loginEndpoint = options.Value.LoginEndpoint;

    public async Task<string?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        //mock temporal
        await Task.Delay(2000, cancellationToken);

        return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOjEyMzQ1Njc4OSwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.WQzROFJ7I251UmKwPS1ayRrS02OlHqvt1nng-Q3PQdI";


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
    public string BaseUrl { get; set; } = "https://localhost:5001/";
    public string LoginEndpoint { get; set; } = "api/auth/login";
}
