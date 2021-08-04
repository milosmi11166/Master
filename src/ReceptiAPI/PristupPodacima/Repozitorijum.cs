using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using ReceptiAPI.Izuzeci;
using ReceptiAPI.Konstante;
using ReceptiAPI.PristupPodacima.Interfejsi;
using ReceptiAPI.Servisi.Interfejsi;

namespace ReceptiAPI.PristupPodacima
{
    public class Repozitorijum<T> : IRepozitorijum<T> where T : class
    {
        private readonly ILogger _dnevnik;
        private CosmosStoreSettings _cosmosStorePodesavanja;
        private CosmosStore<T> _cosmosStore;
        private readonly IKonfiguracijaServis _konfiguracijaServis;

        public Repozitorijum(ILoggerFactory dnevnikFabrika, IKonfiguracijaServis konfiguracijaServis)
        {
            _dnevnik = dnevnikFabrika.CreateLogger<Repozitorijum<T>>();
            _konfiguracijaServis = konfiguracijaServis;

            _cosmosStorePodesavanja = new CosmosStoreSettings(
                    _konfiguracijaServis.CosmosDbNazivBaze,
                    _konfiguracijaServis.CosmosDbUrl,
                    _konfiguracijaServis.CosmosDbAutKljuc);

            _cosmosStore = new CosmosStore<T>(_cosmosStorePodesavanja);
        }

        public async Task<T> Kreiraj(T objekat)
        {
            T rezultat = null;

            try
            {
                rezultat = await _cosmosStore.AddAsync(objekat);
            }
            catch (Exception i)
            {
                _dnevnik.LogError("Izuzetak prilikom pristupa bazi podataka.", i);

                throw new ReceptiAPIIzuzetak(500, KonstantneVrednosti.GreskaPrilikomPristupaBaziPodataka);
            }

            return rezultat;
        }

        public async Task<T> PronadjiJedan(string id)
        {
            T rezultat = null;

            try
            {
                rezultat = await _cosmosStore.FindAsync(id, id);
            }
            catch (Exception i)
            {
                _dnevnik.LogError("Izuzetak prilikom pristupa bazi podataka.", i);

                throw new ReceptiAPIIzuzetak(500, KonstantneVrednosti.GreskaPrilikomPristupaBaziPodataka);
            }

            if (rezultat == null)
            {
                throw new ReceptiAPIIzuzetak(404, KonstantneVrednosti.EntitetNijePronadjen);
            }

            return rezultat;
        }

        public async Task<List<T>> PronadjiSve(string poljeFiltera = null, string vrednostFiltera = null, bool filterirajDeoVrednosti = false, int brojStrane = 1, int velicinaStrane = 10)
        {
            List<T> rezultat = null;
            string cosmosDbUpit = "select * from c";

            if(!string.IsNullOrEmpty(poljeFiltera))
            {
                cosmosDbUpit += filterirajDeoVrednosti ?
                    " where contains(c." + poljeFiltera + ", '" + vrednostFiltera + "')" :
                    " where c." + poljeFiltera + " = '" + vrednostFiltera + "'";
            }
                
            try
            {
                rezultat = await _cosmosStore.Query(cosmosDbUpit, null, new FeedOptions { EnableCrossPartitionQuery = true })
                    .WithPagination(brojStrane, velicinaStrane)
                    .ToListAsync();
            }
            catch (Exception i)
            {
                _dnevnik.LogError("Izuzetak prilikom pristupa bazi podataka.", i);

                throw new ReceptiAPIIzuzetak(500, KonstantneVrednosti.GreskaPrilikomPristupaBaziPodataka);
            }

            return rezultat;
        }

        public async Task<List<T>> PronadjiSve(string poljeFiltera = null, List<string> vrednostiFiltera = null, int brojStrane = 1, int velicinaStrane = 10)
        {
            List<T> rezultat = null;
            string cosmosDbUpit = "select * from c";

            if (!string.IsNullOrEmpty(poljeFiltera))
            {
                cosmosDbUpit += " where c." + poljeFiltera + " in (";
                for(int i = 0; i < vrednostiFiltera.Count - 1; i++)
                {
                    cosmosDbUpit += "'" + vrednostiFiltera[i] + "', ";
                }
                cosmosDbUpit += "'" + vrednostiFiltera.Last() + "')";
            }

            try
            {
                rezultat = await _cosmosStore.Query(cosmosDbUpit, null, new FeedOptions { EnableCrossPartitionQuery = true })
                    .WithPagination(brojStrane, velicinaStrane)
                    .ToListAsync();
            }
            catch (Exception i)
            {
                _dnevnik.LogError("Izuzetak prilikom pristupa bazi podataka.", i);

                throw new ReceptiAPIIzuzetak(500, KonstantneVrednosti.GreskaPrilikomPristupaBaziPodataka);
            }

            return rezultat;
        }

        public async Task<T> Azuriraj(T objekat)
        {
            T rezultat = null;

            try
            {
                rezultat = await _cosmosStore.UpdateAsync(objekat);
            }
            catch (Exception i)
            {
                _dnevnik.LogError("Izuzetak prilikom pristupa bazi podataka.", i);

                throw new ReceptiAPIIzuzetak(500, KonstantneVrednosti.GreskaPrilikomPristupaBaziPodataka);
            }

            return rezultat;
        }

        public async Task Obrisi(string id)
        {
            try
            {
                var rezultat = await _cosmosStore.RemoveByIdAsync(
                    id, 
                    new RequestOptions { PartitionKey = new PartitionKey(id) });
            }
            catch (Exception i)
            {
                _dnevnik.LogError("Izuzetak prilikom pristupa bazi podataka.", i);

                throw new ReceptiAPIIzuzetak(500, KonstantneVrednosti.GreskaPrilikomPristupaBaziPodataka);
            }

            return;
        }
    }
}
