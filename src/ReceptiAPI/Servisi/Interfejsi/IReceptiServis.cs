using ReceptiAPI.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReceptiAPI.Servisi.Interfejsi
{
    public interface IReceptiServis
    {
        Task<List<ReceptDTO>> PronadjiSve(int brojStrane, int velicinaStrane);
        Task<ReceptDTO> PronadjiJedan(string id);
        Task<ReceptDTO> Kreiraj(ReceptDTO recept);
        Task<ReceptDTO> Azuriraj(string id, ReceptDTO recept);
        Task Obrisi(string id);
    }
}
