using ReceptiAPI.Servisi.Interfejsi;

namespace ReceptiAPI.Servisi
{
    public class KonfiguracijaServis : IKonfiguracijaServis
    {
        public string CosmosDbNazivBaze => System.Environment.GetEnvironmentVariable("cosmosDbNazivBaze");

        public string CosmosDbUrl => System.Environment.GetEnvironmentVariable("cosmosDbUrl");

        public string CosmosDbAutKljuc => System.Environment.GetEnvironmentVariable("cosmosDbAutKljuc");
    }
}
