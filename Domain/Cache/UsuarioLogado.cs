using Amazon.DynamoDBv2.DataModel;

namespace Domain.Autenticacao
{
    [DynamoDBTable("customers_cache")]
    public class UsuarioLogado
    {
        public UsuarioLogado(Guid usuarioId, string nome)
        {
            UsuarioId = usuarioId;
            Nome = nome;
            Ttl = DateTimeOffset.Now.AddDays(1).ToUnixTimeSeconds();
        }

        [DynamoDBHashKey("userId")]
        public Guid UsuarioId { get; set; }

        [DynamoDBProperty("nome")]
        public string Nome { get; set; }

        [DynamoDBProperty("ttl")]
        public long Ttl { get; set; }
    }
}