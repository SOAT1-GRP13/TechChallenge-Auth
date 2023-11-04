namespace Domain.ValueObjects
{
    public class DatabaseSettings
    {
        public DatabaseSettings()
        {
            PostgresString = string.Empty;
            DynamoDbString = string.Empty;
        }

        public const string DatabaseConfiguration = "DatabaseSettings";
        public string PostgresString { get; set; }
        public string DynamoDbString { get; set; }
    }
}
