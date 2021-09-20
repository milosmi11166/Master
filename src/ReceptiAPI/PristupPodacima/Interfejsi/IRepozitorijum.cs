using ReceptiAPI.Modeli;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReceptiAPI.PristupPodacima.Interfejsi
{
    public interface IRepozitorijum<T> where T : class
    {
        Task<List<T>> PronadjiSve(string poljeFiltera, string vrednostFiltera, bool filterirajDeoVrednosti, int brojStrane, int velicinaStrane);
        Task<List<T>> PronadjiSve(string poljeFiltera, List<string> vrednostFiltera, int brojStrane, int velicinaStrane);
        Task<(List<T>, Paginacija)> PronadjiSveSaPaginacijom(string poljeFiltera = null, string vrednostFiltera = null, bool filterirajDeoVrednosti = false, int brojStrane = 1, int velicinaStrane = 10);
        Task<T> PronadjiJedan(string id);
        Task<T> Kreiraj(T objekat);
        Task<T> Azuriraj(T objekat);
        Task Obrisi(string id);
    }
}
