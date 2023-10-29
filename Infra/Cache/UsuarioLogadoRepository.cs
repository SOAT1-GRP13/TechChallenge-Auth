using Amazon.DynamoDBv2.DataModel;
using Domain.Autenticacao;

namespace Infra.Autenticacao.Repository
{
    public class UsuarioLogadoRepository : IUsuarioLogadoRepository
    {
        private readonly IDynamoDBContext _dynamoDBContext;
        public UsuarioLogadoRepository(IDynamoDBContext dynamoDBContext)
        {
            _dynamoDBContext = dynamoDBContext;
        }

        public async Task<bool> AddUsuarioLogado(Guid usuarioId, string nome)
        {
            try
            {
                var userLogin = new UsuarioLogado(usuarioId, nome);
                await _dynamoDBContext.SaveAsync(userLogin);
                return true;
            }
            catch
            {
                return false;

            }
        }
    }
}