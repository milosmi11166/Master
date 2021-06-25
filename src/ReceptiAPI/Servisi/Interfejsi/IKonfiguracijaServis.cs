
namespace ReceptiAPI.Servisi.Interfejsi
{
    public interface IKonfiguracijaServis
    {
        string CosmosDbNazivBaze { get; }
        string CosmosDbUrl { get; }
        string CosmosDbAutKljuc { get; }
    }
}
