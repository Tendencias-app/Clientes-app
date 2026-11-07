namespace CarsATTM_CustomerApp.Services;

public interface ILoginService
{
    Task<string?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
