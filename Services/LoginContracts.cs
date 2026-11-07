namespace CarsATTM_CustomerApp.Services;

public sealed record LoginRequest(string UserName, string Password);

public sealed record LoginResponse(string Token);
