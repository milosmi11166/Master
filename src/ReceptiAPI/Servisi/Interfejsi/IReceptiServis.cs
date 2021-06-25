using ReceptiAPI.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReceptiAPI.Servisi.Interfejsi
{
    public interface IReceptiServis
    {
        List<ReceptDTO> PronadjiSve();
        Task<ReceptDTO> PronadjiJedan(string id);
        Task<ReceptDTO> Kreiraj(ReceptDTO recept);
        Task<ReceptDTO> Azuriraj(string id, ReceptDTO recept);
        Task Obrisi(string id);
    }
}
