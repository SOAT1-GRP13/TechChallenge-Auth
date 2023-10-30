
using TechChallengeAuth.Setup.SecretsManager;

namespace TechChallengeAuth.Setup
{
        public static class SecretsManagerSetup
    {
        public static void AddAmazonSecretsManager(this IConfigurationBuilder configurationBuilder,
                                string region,
                                string secretName)
        {
            var configurationSource =
                    new AmazonSecretsManagerConfigurationSource(region, secretName);

            configurationBuilder.Add(configurationSource);
        }

        public static void LoadSecretsData(this IConfigurationBuilder configurationBuilder,
                        string region,
                        string secretName)
        {
            new AmazonSecretsManagerConfigurationSource(region, secretName).Load(configurationBuilder);


        }
    }
}
