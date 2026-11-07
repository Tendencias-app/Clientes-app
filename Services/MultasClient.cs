using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CarsATTM_CustomerApp.Services;

public sealed class MultasClient(HttpClient httpClient) : IMultasClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public async Task<IReadOnlyList<MultaResponse>> ListarAsync(
        FiltroEstado estado = FiltroEstado.Todas,
        CancellationToken cancellationToken = default)
    {
        var endpoint = $"api/v1/multas?estado={Uri.EscapeDataString(estado.ToApiValue())}";
        return await GetAsync<IReadOnlyList<MultaResponse>>(endpoint, cancellationToken);
    }

    public Task<IReadOnlyList<MultaResponse>> ListarPorUsuarioAsync(
        int usuarioId,
        CancellationToken cancellationToken = default) =>
        GetAsync<IReadOnlyList<MultaResponse>>(
            $"api/v1/multas/usuarios/{usuarioId}", cancellationToken);

    public Task<IReadOnlyList<MultaResponse>> ListarPorOrdenAsync(
        int ordenId,
        CancellationToken cancellationToken = default) =>
        GetAsync<IReadOnlyList<MultaResponse>>(
            $"api/v1/multas/ordenes/{ordenId}", cancellationToken);

    public Task<MultaResponse> ObtenerAsync(
        int multaId,
        CancellationToken cancellationToken = default) =>
        GetAsync<MultaResponse>($"api/v1/multas/{multaId}", cancellationToken);

    public async Task<MultaResponse> PagarAsync(
        int multaId,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsync(
            $"api/v1/multas/{multaId}/pago", content: null, cancellationToken);

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<MultaResponse>(JsonOptions, cancellationToken)
            ?? throw new JsonException("La API devolvió una respuesta vacía al pagar la multa.");
    }

    private async Task<T> GetAsync<T>(string endpoint, CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync(endpoint, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return default!;

        return await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken)
            ?? throw new JsonException("La API devolvió una respuesta vacía.");
    }
}

public sealed record MultaResponse(
    int Id,
    int OrdenId,
    int UsuarioId,
    TipoMulta Tipo,
    decimal PrecioMulta,
    string Observacion,
    EstadoMulta Estado,
    DateTimeOffset CreadaEn,
    DateTimeOffset? FechaPago);

public enum EstadoMulta
{
    Pendiente,
    Pagada
}

public enum FiltroEstado
{
    Todas,
    Pendiente,
    Pagada
}

public enum TipoMulta
{
    Retraso,
    Danos
}

internal static class MultasApiValues
{
    public static string ToApiValue(this FiltroEstado estado) => estado switch
    {
        FiltroEstado.Todas => "TODAS",
        FiltroEstado.Pendiente => "PENDIENTE",
        FiltroEstado.Pagada => "PAGADA",
        _ => throw new ArgumentOutOfRangeException(nameof(estado), estado, null)
    };
}