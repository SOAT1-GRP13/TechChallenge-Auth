namespace Domain.Autenticacao
{
    public interface IUsuarioLogadoRepository
    {
        Task<bool> AddUsuarioLogado(string usuarioId, string nome);
    }
}