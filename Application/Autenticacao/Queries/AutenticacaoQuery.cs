using Domain.Autenticacao;
using Application.Autenticacao.Dto.Cliente;

namespace Application.Autenticacao.Queries
{
    public class AutenticacaoQuery : IAutenticacaoQuery
    {
        private readonly IAutenticacaoRepository _autenticacaoRepository;

        public AutenticacaoQuery(IAutenticacaoRepository autenticacaoRepository)
        {
            _autenticacaoRepository = autenticacaoRepository;
        }

        public async Task CadastraCliente(CadastraClienteDto dto)
        {
            var usuario = new AcessoCliente(dto.CPF, dto.Senha, dto.Email, dto.Nome);

            _autenticacaoRepository.CadastraCliente(usuario);

            await _autenticacaoRepository.UnitOfWork.Commit();
        }

        public async Task AnonimizaCliente(string cpf)
        {
            _autenticacaoRepository.AnonimizaClient(cpf);

            await _autenticacaoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> ClienteJaExiste(CadastraClienteDto dto)
        {
            var usuario = new AcessoCliente(dto.CPF, dto.Senha, dto.Email, dto.Nome);

            return await _autenticacaoRepository.ClienteJaExiste(usuario);
        }
    }
}
