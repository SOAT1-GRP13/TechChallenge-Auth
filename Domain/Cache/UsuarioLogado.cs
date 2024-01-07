using Amazon.DynamoDBv2.DataModel;

namespace Domain.Autenticacao
{
    [DynamoDBTable("usuariosLogados")]
    public class UsuarioLogado
    {
        public UsuarioLogado(string token)
        {
            Token = "Bearer " + token;
            Ttl = DateTimeOffset.Now.AddHours(2).ToUnixTimeSeconds();
        }

        public UsuarioLogado()
        {
            Token = string.Empty;
            Ttl = 0;
        }

        [DynamoDBHashKey("token")]
        public string Token { get; set; }

        [DynamoDBProperty("ttl")]
        public long Ttl { get; set; }
    }
}