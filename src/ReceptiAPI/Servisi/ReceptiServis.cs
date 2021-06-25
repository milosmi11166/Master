using AutoMapper;
using ReceptiAPI.DTO;
using ReceptiAPI.Modeli;
using ReceptiAPI.PristupPodacima.Interfejsi;
using ReceptiAPI.Servisi.Interfejsi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReceptiAPI.Servisi
{
    public class ReceptiServis : IReceptiServis
    {
        private readonly IRepozitorijum<Recept> _receptiRepozitorijum;
        private readonly IMapper _maper;
        private readonly IKonfiguracijaServis _konfiguracijaServis;

        public ReceptiServis(IRepozitorijum<Recept> receptiRepozitorijum, IMapper maper, IKonfiguracijaServis konfiguracijaServis)
        {
            _receptiRepozitorijum = receptiRepozitorijum;
            _maper = maper;
            _konfiguracijaServis = konfiguracijaServis;

            _receptiRepozitorijum.PostaviParametreBaze(
                    _konfiguracijaServis.CosmosDbNazivBaze,
                    _konfiguracijaServis.CosmosDbUrl,
                    _konfiguracijaServis.CosmosDbAutKljuc);
        }

        public async Task<ReceptDTO> Azuriraj(string id, ReceptDTO receptDTO)
        {
            Recept recept = await _receptiRepozitorijum.PronadjiJedan(id);

            recept = _maper.Map<ReceptDTO, Recept>(receptDTO, recept);
            recept.DatumAzuriranja = DateTime.UtcNow;

            recept = await _receptiRepozitorijum.Azuriraj(recept);

            return _maper.Map<ReceptDTO>(recept);
        }

        public async Task<ReceptDTO> Kreiraj(ReceptDTO receptDTO)
        {
            Recept recept = _maper.Map<Recept>(receptDTO);

            recept.Id = Guid.NewGuid().ToString();
            recept.DatumKreiranja = DateTime.UtcNow;
            recept.DatumAzuriranja = DateTime.UtcNow;

            recept = await _receptiRepozitorijum.Kreiraj(recept);

            return _maper.Map<ReceptDTO>(recept);
        }

        public async Task Obrisi(string id)
        {
            await _receptiRepozitorijum.Obrisi(id);

            return;
        }

        public async Task<ReceptDTO> PronadjiJedan(string id)
        {
            Recept recept = await _receptiRepozitorijum.PronadjiJedan(id);

            return _maper.Map<ReceptDTO>(recept);
        }

        public List<ReceptDTO> PronadjiSve()
        {
            List<Recept> recepti = _receptiRepozitorijum.PronadjiSve();

            return _maper.Map<List<ReceptDTO>>(recepti);
        }
    }
}
