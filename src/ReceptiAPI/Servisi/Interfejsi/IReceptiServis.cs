using ReceptiAPI.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReceptiAPI.Servisi.Interfejsi
{
    public interface IReceptiServis
    {
        Task<List<ReceptDTO>> PronadjiSve(string opis, int brojStrane, int velicinaStrane);
        Task<ReceptDTO> PronadjiJedan(string id);
        Task<ReceptDTO> Kreiraj(ReceptDTO recept);
        Task<ReceptDTO> Azuriraj(string id, ReceptDTO recept);
        Task Obrisi(string id);
        Task<KorakPripremeDTO> KreirajKorakPripreme(string idRecepta, KorakPripremeDTO korakPripremeDTO);
        Task<KorakPripremeDTO> AzurirajKorakPripreme(string idRecepta, string idKorakaPripreme, KorakPripremeDTO korakPripremeDTO);
        Task ObrisiKorakPripreme(string idRecepta, string idKorakaPripreme);
        Task<SastojakDTO> KreirajSastojak(string idRecepta, SastojakDTO sastojakDTO);
        Task<SastojakDTO> AzurirajSastojak(string idRecepta, string idNamirnice, SastojakDTO sastojakDTO);
        Task ObrisiSastojak(string idRecepta, string idNamirnice);
    }
}
