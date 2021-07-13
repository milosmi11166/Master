using System.Collections.Generic;
using System.Threading.Tasks;
using ReceptiAPI.DTO;
using ReceptiAPI.Servisi.Interfejsi;

namespace ReceptiAPI.Servisi
{
    public class NamirniceServis : INamirniceServis
    {
        public Task<NamirnicaDTO> Azuriraj(string id, NamirnicaDTO namirnicaDTO)
        {
            throw new System.NotImplementedException();
        }

        public Task<NamirnicaDTO> Kreiraj(NamirnicaDTO namirnicaDTO)
        {
            throw new System.NotImplementedException();
        }

        public Task Obrisi(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<NamirnicaDTO> PronadjiJedan(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<NamirnicaDTO>> PronadjiSve(int brojStrane, int velicinaStrane)
        {
            throw new System.NotImplementedException();
        }
    }
}
