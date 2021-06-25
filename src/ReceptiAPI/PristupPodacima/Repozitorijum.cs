using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using ReceptiAPI.Izuzeci;
using ReceptiAPI.Konstante;
using ReceptiAPI.PristupPodacima.Interfejsi;

namespace ReceptiAPI.PristupPodacima
{
    public class Repozitorijum<T> : IRepozitorijum<T> where T : class
    {
        private readonly ILogger _dnevnik;
        private CosmosStoreSettings _cosmosStorePodesavanja;
        private CosmosStore<T> _cosmosStore;

        public Repozitorijum(ILoggerFactory dnevnikFabrika)
        {
            _dnevnik = dnevnikFabrika.CreateLogger<Repozitorijum<T>>();
        }

        public void PostaviParametreBaze(string cosmosDbNazivBaze, string cosmosDbUrl, string cosmosDbAutKljuc)
        {
            _cosmosStorePodesavanja = new CosmosStoreSettings(cosmosDbNazivBaze, cosmosDbUrl, cosmosDbAutKljuc);
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

        public List<T> PronadjiSve()
        {
            List<T> rezultat = new List<T>();

            try
            {
                rezultat = _cosmosStore.Query(new FeedOptions { EnableCrossPartitionQuery = true }).ToList<T>();
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
