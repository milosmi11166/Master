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
    public class NamirniceFunkcije
    {
        private readonly ILogger _dnevnik;
        private readonly INamirniceServis _namirniceServis;

        public NamirniceFunkcije(ILoggerFactory dnevnikFabrika, INamirniceServis namirniceServis)
        {
            _dnevnik = dnevnikFabrika.CreateLogger<NamirniceFunkcije>();
            _namirniceServis = namirniceServis;
        }


        [FunctionName("KreirajNamirnicu")]
        public  async Task<JsonResult> KreirajNamirnicu(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "v1/namirnice")] [FromBody] NamirnicaDTO namirnicaDTO)
        {
            _dnevnik.LogInformation("KreirajNamirnicu funkcija je primila zahtev.");

            var odgovor = new JsonResult(null);
            NamirnicaDTO kreiranaNamirnica = null;

            try
            {
                kreiranaNamirnica = await _namirniceServis.Kreiraj(namirnicaDTO);

                odgovor.StatusCode = StatusCodes.Status201Created;
                odgovor.Value = kreiranaNamirnica;
            }
            catch (ReceptiAPIIzuzetak rai)
            {
                odgovor.StatusCode = rai.HttpStatusKod;
                odgovor.Value = new GreskaDTO { PorukaGreske = rai.Poruka };
            }
            catch (Exception i)
            {
                _dnevnik.LogError("Neobradjen izuzetak u funkciji KreirajNamirnicu.", i);

                odgovor.StatusCode = StatusCodes.Status500InternalServerError;
                odgovor.Value = new GreskaDTO { PorukaGreske = KonstantneVrednosti.GreskaNaServerskojStrani };
            }

            return odgovor;
        }

        [FunctionName("AzurirajNamirnicu")]
        public async Task<JsonResult> AzurirajNamirnicu(
            [HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "v1/namirnice/{id}")] [FromBody] NamirnicaDTO namirnicaDTO,
            [FromRoute] string id)
        {
            _dnevnik.LogInformation("AzurirajNamirnicu funkcija je primila zahtev. Id = " + id);

            var odgovor = new JsonResult(null);
            NamirnicaDTO azuriranaNamirnicaDTO = null;

            try
            {
                azuriranaNamirnicaDTO = await _namirniceServis.Azuriraj(id, namirnicaDTO);

                odgovor.StatusCode = StatusCodes.Status200OK;
                odgovor.Value = azuriranaNamirnicaDTO;
            }
            catch (ReceptiAPIIzuzetak rai)
            {
                odgovor.StatusCode = rai.HttpStatusKod;
                odgovor.Value = new GreskaDTO { PorukaGreske = rai.Poruka };
            }
            catch (Exception i)
            {
                _dnevnik.LogError("Neobradjen izuzetak u funkciji AzurirajNamirnicu.", i);

                odgovor.StatusCode = StatusCodes.Status500InternalServerError;
                odgovor.Value = new GreskaDTO { PorukaGreske = KonstantneVrednosti.GreskaNaServerskojStrani };
            }

            return odgovor;
        }

        [FunctionName("ObrisiNamirnicu")]
        public async Task<JsonResult> ObrisiNamirnicu(
            [HttpTrigger(AuthorizationLevel.Function, "DELETE", Route = "v1/namirnice/{id}")] HttpRequest zahtev,
            [FromRoute] string id)
        {
            _dnevnik.LogInformation("ObrisiNamirnicu funkcija je primila zahtev. Id = " + id);

            var odgovor = new JsonResult(null);

            try
            {
                await _namirniceServis.Obrisi(id);

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
                _dnevnik.LogError("Neobradjen izuzetak u funkciji ObrisiNamirnicu.", i);

                odgovor.StatusCode = StatusCodes.Status500InternalServerError;
                odgovor.Value = new GreskaDTO { PorukaGreske = KonstantneVrednosti.GreskaNaServerskojStrani };
            }

            return odgovor;
        }

        [FunctionName("PronadjiSveNamirnice")]
        public  async Task<JsonResult> PronadjiSveNamirnice(
           [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "v1/namirnice")] HttpRequest zahtev)
        {
            _dnevnik.LogInformation("PronadjiSveNamirnice funkcija je primila zahtev.");

            string naziv = zahtev.Query["naziv"];
            int brojStrane = Int32.TryParse(zahtev.Query["brojStrane"], out brojStrane) ? brojStrane : 1;
            int velicinaStrane = Int32.TryParse(zahtev.Query["velicinaStrane"], out velicinaStrane) ? velicinaStrane : 10;

            var odgovor = new JsonResult(null);
            var recepti = new ListaSaPaginacijomDTO<NamirnicaDTO>();

            try
            {
                recepti = await _namirniceServis.PronadjiSve(naziv, brojStrane, velicinaStrane);

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
                _dnevnik.LogError("Neobradjen izuzetak u funkciji PronadjiSveNamirnice.", i);

                odgovor.Value = new GreskaDTO { PorukaGreske = KonstantneVrednosti.GreskaNaServerskojStrani };
                odgovor.StatusCode = StatusCodes.Status500InternalServerError;
            }

            return odgovor;
        }

        [FunctionName("PronadjiJednuNamirnicu")]
        public async Task<JsonResult> PronadjiJednuNamirnicu(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "v1/namirnice/{id}")] HttpRequest zahtev,
            [FromRoute] string id)
        {
            _dnevnik.LogInformation("PronadjiJednuNamirnicu funkcija je primila zahtev. Id = " + id);

            var odgovor = new JsonResult(null);
            NamirnicaDTO recept = null;

            try
            {
                recept = await _namirniceServis.PronadjiJedan(id);

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
                _dnevnik.LogError("Neobradjen izuzetak u funkciji PronadjiJednuNamirnicu.", i);

                odgovor.StatusCode = StatusCodes.Status500InternalServerError;
                odgovor.Value = new GreskaDTO { PorukaGreske = KonstantneVrednosti.GreskaNaServerskojStrani };
            }

            return odgovor;
        }
    }
}
