using Application.Autenticacao.Dto.Cliente;

namespace Application.Autenticacao.Queries
{
    public interface IAutenticacaoQuery
    {
        Task<bool> ClienteJaExiste(CadastraClienteDto dto);
        Task CadastraCliente(CadastraClienteDto dto);
        Task AnonimizaCliente(string cpf);
    }
}
