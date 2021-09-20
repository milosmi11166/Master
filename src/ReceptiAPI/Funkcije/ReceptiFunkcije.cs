using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using ReceptiAPI.Servisi.Interfejsi;
using ReceptiAPI.DTO;
using ReceptiAPI.Izuzeci;
using ReceptiAPI.Konstante;

namespace ReceptiAPI
{
    public class ReceptiFunkcije
    {
        private readonly ILogger _dnevnik;
        private readonly IReceptiServis _receptiServis;

        public ReceptiFunkcije(ILoggerFactory dnevnikFabrika, IReceptiServis receptiServis)
        {
            _dnevnik = dnevnikFabrika.CreateLogger<ReceptiFunkcije>();
            _receptiServis = receptiServis;
        }

        [FunctionName("KreirajRecept")]
        public  async Task<JsonResult> KreirajRecept(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "v1/recepti")] [FromBody] ReceptDTO receptDTO)
        {
            _dnevnik.LogInformation("KreirajRecept funkcija je primila zahtev.");

            var odgovor = new JsonResult(null);
            ReceptDTO kreiraniReceptDTO = null;

            try
            {
                kreiraniReceptDTO = await _receptiServis.Kreiraj(receptDTO);

                odgovor.StatusCode = StatusCodes.Status201Created;
                odgovor.Value = kreiraniReceptDTO;
            }
            catch (ReceptiAPIIzuzetak rai)
            {
                odgovor.StatusCode = rai.HttpStatusKod;
                odgovor.Value = new GreskaDTO { PorukaGreske = rai.Poruka };
            }
            catch (Exception i)
            {
                _dnevnik.LogError("Neobradjen izuzetak u funkciji KreirajRecept.", i);

                odgovor.StatusCode = StatusCodes.Status500InternalServerError;
                odgovor.Value = new GreskaDTO { PorukaGreske = KonstantneVrednosti.GreskaNaServerskojStrani };
            }

            return odgovor;
        }

        [FunctionName("AzurirajRecept")]
        public async Task<JsonResult> AzurirajRecept(
            [HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "v1/recepti/{id}")] [FromBody] ReceptDTO receptDTO,
            [FromRoute] string id)
        {
            _dnevnik.LogInformation("AzurirajRecept funkcija je primila zahtev. Id = " + id);

            var odgovor = new JsonResult(null);
            ReceptDTO azuriraniReceptDTO = null;

            try
            {
                azuriraniReceptDTO = await _receptiServis.Azuriraj(id, receptDTO);

                odgovor.StatusCode = StatusCodes.Status200OK;
                odgovor.Value = azuriraniReceptDTO;
            }
            catch (ReceptiAPIIzuzetak rai)
            {
                odgovor.StatusCode = rai.HttpStatusKod;
                odgovor.Value = new GreskaDTO { PorukaGreske = rai.Poruka };
            }
            catch (Exception i)
            {
                _dnevnik.LogError("Neobradjen izuzetak u funkciji AzurirajRecept.", i);

                odgovor.StatusCode = StatusCodes.Status500InternalServerError;
                odgovor.Value = new GreskaDTO { PorukaGreske = KonstantneVrednosti.GreskaNaServerskojStrani };
            }

            return odgovor;
        }

        [FunctionName("ObrisiRecept")]
        public async Task<JsonResult> ObrisiRecept(
            [HttpTrigger(AuthorizationLevel.Function, "DELETE", Route = "v1/recepti/{id}")] HttpRequest zahtev,
            [FromRoute] string id)
        {
            _dnevnik.LogInformation("ObrisiRecept funkcija je primila zahtev. Id = " + id);

            var odgovor = new JsonResult(null);

            try
            {
                await _receptiServis.Obrisi(id);

                odgovor.StatusCode = StatusCodes.Status204NoContent;
                odgovor.Value = string.Empty;
            }
            catch (ReceptiAPIIzuzetak rai)
            {
                odgovor.StatusCode = rai.HttpStatusKod;
                odgovor.Value = new GreskaDTO { PorukaGreske = rai.Poruka };
            }
            catch (Exception i)
            {
                _dnevnik.LogError("Neobradjen izuzetak u funkciji ObrisiRecept.", i);

                odgovor.StatusCode = StatusCodes.Status500InternalServerError;
                odgovor.Value = new GreskaDTO { PorukaGreske = KonstantneVrednosti.GreskaNaServerskojStrani };
            }

            return odgovor;
        }

        [FunctionName("PronadjiSveRecepte")]
        public  async Task<JsonResult> PronadjiSveRecepte(
           [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "v1/recepti")] HttpRequest zahtev)
        {
            _dnevnik.LogInformation("PronadjiSveRecepte funkcija je primila zahtev.");

            string opis = zahtev.Query["opis"];
            int brojStrane = Int32.TryParse(zahtev.Query["brojStrane"], out brojStrane) ? brojStrane : 1;
            int velicinaStrane = Int32.TryParse(zahtev.Query["velicinaStrane"], out velicinaStrane) ? velicinaStrane : 10;

            var odgovor = new JsonResult(null);
            var recepti = new ListaSaPaginacijomDTO<ReceptDTO>();

            try
            {
                recepti = await _receptiServis.PronadjiSve(opis, brojStrane, velicinaStrane);

                odgovor.Value = recepti;
                odgovor.StatusCode = StatusCodes.Status200OK;
            }
            catch(ReceptiAPIIzuzetak rai)
            {
                odgovor.Value = new GreskaDTO { PorukaGreske = rai.Poruka };
                odgovor.StatusCode = rai.HttpStatusKod;
            }
            catch(Exception i)
            {
                _dnevnik.LogError("Neobradjen izuzetak u funkciji ObrisiRecept.", i);

                odgovor.Value = new GreskaDTO { PorukaGreske = KonstantneVrednosti.GreskaNaServerskojStrani };
                odgovor.StatusCode = StatusCodes.Status500InternalServerError;
            }

            return odgovor;
        }

        [FunctionName("PronadjiJedanRecept")]
        public async Task<JsonResult> PronadjiJedanRecept(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "v1/recepti/{id}")] HttpRequest zahtev,
            [FromRoute] string id)
        {
            _dnevnik.LogInformation("PronadjiJedanRecept funkcija je primila zahtev. Id = " + id);

            var odgovor = new JsonResult(null);
            ReceptDTO recept = null;

            try
            {
                recept = await _receptiServis.PronadjiJedan(id);

                odgovor.StatusCode = StatusCodes.Status200OK;
                odgovor.Value = recept;
            }
            catch (ReceptiAPIIzuzetak rai)
            {
                odgovor.StatusCode = rai.HttpStatusKod;
                odgovor.Value = new GreskaDTO { PorukaGreske = rai.Poruka };
            }
            catch (Exception i)
            {
                _dnevnik.LogError("Neobradjen izuzetak u funkciji PronadjiJedanRecept.", i);

                odgovor.StatusCode = StatusCodes.Status500InternalServerError;
                odgovor.Value = new GreskaDTO { PorukaGreske = KonstantneVrednosti.GreskaNaServerskojStrani };
            }

            return odgovor;
        }

        [FunctionName("KreirajKorakPripreme")]
        public async Task<JsonResult> KreirajKorakPripreme(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "v1/recepti/{idRecepta}/koraci-pripreme")] [FromBody] KorakPripremeDTO korakPripremeDTO,
            [FromRoute] string idRecepta)
        {
            _dnevnik.LogInformation("KreirajKorakPripreme funkcija je primila zahtev. Id: " + idRecepta);

            var odgovor = new JsonResult(null);
            KorakPripremeDTO kreiraniKorakPripremeDTO = null;

            try
            {
                kreiraniKorakPripremeDTO = await _receptiServis.KreirajKorakPripreme(idRecepta, korakPripremeDTO);

                odgovor.StatusCode = StatusCodes.Status201Created;
                odgovor.Value = kreiraniKorakPripremeDTO;
            }
            catch (ReceptiAPIIzuzetak rai)
            {
                odgovor.StatusCode = rai.HttpStatusKod;
                odgovor.Value = new GreskaDTO { PorukaGreske = rai.Poruka };
            }
            catch (Exception i)
            {
                _dnevnik.LogError("Neobradjen izuzetak u funkciji KreirajKorakPripreme.", i);

                odgovor.StatusCode = StatusCodes.Status500InternalServerError;
                odgovor.Value = new GreskaDTO { PorukaGreske = KonstantneVrednosti.GreskaNaServerskojStrani };
            }

            return odgovor;
        }

        [FunctionName("AzurirajKorakPripreme")]
        public async Task<JsonResult> AzurirajKorakPripreme(
            [HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "v1/recepti/{idRecepta}/koraci-pripreme/{idKorakaPripreme}")] [FromBody] KorakPripremeDTO korakPripremeDTO,
            [FromRoute] string idRecepta,
            [FromRoute] string idKorakaPripreme)
        {
            _dnevnik.LogInformation("AzurirajKorakPripreme funkcija je primila zahtev. IdRecepta = " + idRecepta + ", idKorakaPripreme = " + idKorakaPripreme);

            var odgovor = new JsonResult(null);
            KorakPripremeDTO azuriraniKorakPripremeDTO = null;

            try
            {
                azuriraniKorakPripremeDTO = await _receptiServis.AzurirajKorakPripreme(idRecepta, idKorakaPripreme, korakPripremeDTO);

                odgovor.StatusCode = StatusCodes.Status200OK;
                odgovor.Value = azuriraniKorakPripremeDTO;
            }
            catch (ReceptiAPIIzuzetak rai)
            {
                odgovor.StatusCode = rai.HttpStatusKod;
                odgovor.Value = new GreskaDTO { PorukaGreske = rai.Poruka };
            }
            catch (Exception i)
            {
                _dnevnik.LogError("Neobradjen izuzetak u funkciji AzurirajKorakPripreme.", i);

                odgovor.StatusCode = StatusCodes.Status500InternalServerError;
                odgovor.Value = new GreskaDTO { PorukaGreske = KonstantneVrednosti.GreskaNaServerskojStrani };
            }

            return odgovor;
        }

        [FunctionName("ObrisiKorakPripreme")]
        public async Task<JsonResult> ObrisiKorakPripreme(
            [HttpTrigger(AuthorizationLevel.Function, "DELETE", Route = "v1/recepti/{idRecepta}/koraci-pripreme/{idKorakaPripreme}")] HttpRequest zahtev,
            [FromRoute] string idRecepta,
            [FromRoute] string idKorakaPripreme)
        {
            _dnevnik.LogInformation("ObrisiKorakPripreme funkcija je primila zahtev. IdRecepta = " + idRecepta + ", idKorakaPripreme = " + idKorakaPripreme);

            var odgovor = new JsonResult(null);

            try
            {
                await _receptiServis.ObrisiKorakPripreme(idRecepta, idKorakaPripreme);

                odgovor.StatusCode = StatusCodes.Status204NoContent;
                odgovor.Value = string.Empty;
            }
            catch (ReceptiAPIIzuzetak rai)
            {
                odgovor.StatusCode = rai.HttpStatusKod;
                odgovor.Value = new GreskaDTO { PorukaGreske = rai.Poruka };
            }
            catch (Exception i)
            {
                _dnevnik.LogError("Neobradjen izuzetak u funkciji ObrisiKorakPripreme.", i);

                odgovor.StatusCode = StatusCodes.Status500InternalServerError;
                odgovor.Value = new GreskaDTO { PorukaGreske = KonstantneVrednosti.GreskaNaServerskojStrani };
            }

            return odgovor;
        }

        [FunctionName("KreirajSastojak")]
        public async Task<JsonResult> KreirajSastojak(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "v1/recepti/{idRecepta}/sastojci")] [FromBody] SastojakDTO sastojakDTO,
            [FromRoute] string idRecepta)
        {
            _dnevnik.LogInformation("KreirajSastojak funkcija je primila zahtev. Id: " + idRecepta);

            var odgovor = new JsonResult(null);
            SastojakDTO kreiraniSastojakDTO = null;

            try
            {
                kreiraniSastojakDTO = await _receptiServis.KreirajSastojak(idRecepta, sastojakDTO);

                odgovor.StatusCode = StatusCodes.Status201Created;
                odgovor.Value = kreiraniSastojakDTO;
            }
            catch (ReceptiAPIIzuzetak rai)
            {
                odgovor.StatusCode = rai.HttpStatusKod;
                odgovor.Value = new GreskaDTO { PorukaGreske = rai.Poruka };
            }
            catch (Exception i)
            {
                _dnevnik.LogError("Neobradjen izuzetak u funkciji KreirajSastojak.", i);

                odgovor.StatusCode = StatusCodes.Status500InternalServerError;
                odgovor.Value = new GreskaDTO { PorukaGreske = KonstantneVrednosti.GreskaNaServerskojStrani };
            }

            return odgovor;
        }

        [FunctionName("AzurirajSastojak")]
        public async Task<JsonResult> AzurirajSastojak(
            [HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "v1/recepti/{idRecepta}/sastojci/{idNamirnice}")] [FromBody] SastojakDTO sastojakDTO,
            [FromRoute] string idRecepta,
            [FromRoute] string idNamirnice)
        {
            _dnevnik.LogInformation("AzurirajSastojak funkcija je primila zahtev. IdRecepta = " + idRecepta + ", idNamirnice = " + idNamirnice);

            var odgovor = new JsonResult(null);
            SastojakDTO azuriraniSastojakDTO = null;

            try
            {
                azuriraniSastojakDTO = await _receptiServis.AzurirajSastojak(idRecepta, idNamirnice, sastojakDTO);

                odgovor.StatusCode = StatusCodes.Status200OK;
                odgovor.Value = azuriraniSastojakDTO;
            }
            catch (ReceptiAPIIzuzetak rai)
            {
                odgovor.StatusCode = rai.HttpStatusKod;
                odgovor.Value = new GreskaDTO { PorukaGreske = rai.Poruka };
            }
            catch (Exception i)
            {
                _dnevnik.LogError("Neobradjen izuzetak u funkciji AzurirajSastojak.", i);

                odgovor.StatusCode = StatusCodes.Status500InternalServerError;
                odgovor.Value = new GreskaDTO { PorukaGreske = KonstantneVrednosti.GreskaNaServerskojStrani };
            }

            return odgovor;
        }

        [FunctionName("ObrisiSastojak")]
        public async Task<JsonResult> ObrisiSastojak(
            [HttpTrigger(AuthorizationLevel.Function, "DELETE", Route = "v1/recepti/{idRecepta}/sastojci/{idNamirnice}")] HttpRequest zahtev,
            [FromRoute] string idRecepta,
            [FromRoute] string idNamirnice)
        {
            _dnevnik.LogInformation("ObrisiSastojak funkcija je primila zahtev. IdRecepta = " + idRecepta + ", idNamirnice = " + idNamirnice);

            var odgovor = new JsonResult(null);

            try
            {
                await _receptiServis.ObrisiSastojak(idRecepta, idNamirnice);

                odgovor.StatusCode = StatusCodes.Status204NoContent;
                odgovor.Value = string.Empty;
            }
            catch (ReceptiAPIIzuzetak rai)
            {
                odgovor.StatusCode = rai.HttpStatusKod;
                odgovor.Value = new GreskaDTO { PorukaGreske = rai.Poruka };
            }
            catch (Exception i)
            {
                _dnevnik.LogError("Neobradjen izuzetak u funkciji ObrisiSastojak.", i);

                odgovor.StatusCode = StatusCodes.Status500InternalServerError;
                odgovor.Value = new GreskaDTO { PorukaGreske = KonstantneVrednosti.GreskaNaServerskojStrani };
            }

            return odgovor;
        }
    }
}
