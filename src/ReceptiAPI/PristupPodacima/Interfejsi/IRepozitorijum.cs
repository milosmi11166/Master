using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReceptiAPI.PristupPodacima.Interfejsi
{
    public interface IRepozitorijum<T> where T : class
    {
        Task<List<T>> PronadjiSve(string poljeFiltera, string vrednostFiltera, bool filterirajDeoVrednosti, int brojStrane, int velicinaStrane);
        Task<List<T>> PronadjiSve(string poljeFiltera, List<string> vrednostFiltera, int brojStrane, int velicinaStrane);
        Task<T> PronadjiJedan(string id);
        Task<T> Kreiraj(T objekat);
        Task<T> Azuriraj(T objekat);
        Task Obrisi(string id);
    }
}
