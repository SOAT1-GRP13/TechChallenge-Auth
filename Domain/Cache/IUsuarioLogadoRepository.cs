namespace Domain.Autenticacao
{
    public interface IUsuarioLogadoRepository
    {
        Task<bool> AddUsuarioLogado(Guid usuarioId, string nome);
    }
}