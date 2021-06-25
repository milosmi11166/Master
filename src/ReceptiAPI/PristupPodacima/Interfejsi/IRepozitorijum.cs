using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReceptiAPI.PristupPodacima.Interfejsi
{
    public interface IRepozitorijum<T> where T : class
    {
        void PostaviParametreBaze(string cosmosDbNazivBaze, string cosmosDbUrl, string cosmosDbAutKljuc);
        List<T> PronadjiSve();
        Task<T> PronadjiJedan(string id);
        Task<T> Kreiraj(T objekat);
        Task<T> Azuriraj(T objekat);
        Task Obrisi(string id);
    }
}
