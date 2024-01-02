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

        public async Task<bool> AddUsuarioLogado(string token)
        {
            try
            {
                var userLogin = new UsuarioLogado(token);
                await _dynamoDBContext.SaveAsync(userLogin);
                return true;
            }
            catch(Exception e)
            {
                var teste = e.Message;
                return false;

            }
        }
    }
}