using System.Collections.Generic;
using System.Threading.Tasks;
using ReceptiAPI.DTO;

namespace ReceptiAPI.Servisi.Interfejsi
{
    public interface INamirniceServis
    {
        Task<NamirnicaDTO> Kreiraj(NamirnicaDTO namirnicaDTO);
        Task<NamirnicaDTO> Azuriraj(string id, NamirnicaDTO namirnicaDTO);
        Task Obrisi(string id);
        Task<ListaSaPaginacijomDTO<NamirnicaDTO>> PronadjiSve(string naziv, int brojStrane, int velicinaStrane);
        Task<NamirnicaDTO> PronadjiJedan(string id);
    }
}
