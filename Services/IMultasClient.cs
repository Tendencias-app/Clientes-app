namespace CarsATTM_CustomerApp.Services;

public interface IMultasClient
{
    Task<IReadOnlyList<MultaResponse>> ListarAsync(
        FiltroEstado estado = FiltroEstado.Todas,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MultaResponse>> ListarPorUsuarioAsync(
        int usuarioId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MultaResponse>> ListarPorOrdenAsync(
        int ordenId,
        CancellationToken cancellationToken = default);

    Task<MultaResponse> ObtenerAsync(
        int multaId,
        CancellationToken cancellationToken = default);

    Task<MultaResponse> PagarAsync(
        int multaId,
        CancellationToken cancellationToken = default);
}