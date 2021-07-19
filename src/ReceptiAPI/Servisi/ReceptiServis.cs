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
        private readonly IRepozitorijum<KorakPripreme> _koraciPripremeRepozitorijum;
        private readonly IRepozitorijum<Sastojak> _sastojciRepozitorijum;
        private readonly IMapper _maper;

        public ReceptiServis(
            IRepozitorijum<Recept> receptiRepozitorijum,
            IMapper maper,
            IRepozitorijum<KorakPripreme> koraciPripremeRepozitorijum,
            IRepozitorijum<Sastojak> sastojciRepozitorijum)
        {
            _receptiRepozitorijum = receptiRepozitorijum;
            _maper = maper;
            _koraciPripremeRepozitorijum = koraciPripremeRepozitorijum;
            _sastojciRepozitorijum = sastojciRepozitorijum;
        }

        public async Task<ReceptDTO> Azuriraj(string id, ReceptDTO receptDTO)
        {
            Recept recept = await _receptiRepozitorijum.PronadjiJedan(id);

            recept = _maper.Map<ReceptDTO, Recept>(receptDTO, recept);
            recept.DatumAzuriranja = DateTime.UtcNow;

            recept = await _receptiRepozitorijum.Azuriraj(recept);

            return _maper.Map<ReceptDTO>(recept);
        }

        public async Task<KorakPripremeDTO> AzurirajKorakPripreme(string idRecepta, string idKorakaPripreme, KorakPripremeDTO korakPripremeDTO)
        {
            Recept recept = await _receptiRepozitorijum.PronadjiJedan(idRecepta);
            KorakPripreme korakPripreme = await _koraciPripremeRepozitorijum.PronadjiJedan(idKorakaPripreme);

            korakPripreme = _maper.Map<KorakPripremeDTO, KorakPripreme>(korakPripremeDTO, korakPripreme);
            korakPripreme.DatumAzuriranja = DateTime.UtcNow;

            korakPripreme = await _koraciPripremeRepozitorijum.Azuriraj(korakPripreme);

            return _maper.Map<KorakPripremeDTO>(korakPripreme);
        }

        public async Task<SastojakDTO> AzurirajSastojak(string idRecepta, string idSastojka, SastojakDTO sastojakDTO)
        {
            Recept recept = await _receptiRepozitorijum.PronadjiJedan(idRecepta);
            Sastojak sastojak = await _sastojciRepozitorijum.PronadjiJedan(idSastojka);

            sastojak = _maper.Map<SastojakDTO, Sastojak>(sastojakDTO, sastojak);
            sastojak.DatumAzuriranja = DateTime.UtcNow;

            sastojak = await _sastojciRepozitorijum.Azuriraj(sastojak);

            return _maper.Map<SastojakDTO>(sastojak);
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

        public async Task<KorakPripremeDTO> KreirajKorakPripreme(string idRecepta, KorakPripremeDTO korakPripremeDTO)
        {
            Recept recept = await _receptiRepozitorijum.PronadjiJedan(idRecepta);

            KorakPripreme korakPripreme = _maper.Map<KorakPripreme>(korakPripremeDTO);

            korakPripreme.Id = Guid.NewGuid().ToString();
            korakPripreme.DatumKreiranja = DateTime.UtcNow;
            korakPripreme.DatumAzuriranja = DateTime.UtcNow;
            korakPripreme.IdRecepta = idRecepta;

            korakPripreme = await _koraciPripremeRepozitorijum.Kreiraj(korakPripreme);

            return _maper.Map<KorakPripremeDTO>(korakPripreme);
        }

        public async Task<SastojakDTO> KreirajSastojak(string idRecepta, SastojakDTO sastojakDTO)
        {
            Recept recept = await _receptiRepozitorijum.PronadjiJedan(idRecepta);

            Sastojak sastojak = _maper.Map<Sastojak>(sastojakDTO);

            sastojak.Id = Guid.NewGuid().ToString();
            sastojak.DatumKreiranja = DateTime.UtcNow;
            sastojak.DatumAzuriranja = DateTime.UtcNow;
            sastojak.IdRecepta = idRecepta;

            sastojak = await _sastojciRepozitorijum.Kreiraj(sastojak);

            return _maper.Map<SastojakDTO>(sastojak);
        }

        public async Task Obrisi(string id)
        {
            Recept recept = await _receptiRepozitorijum.PronadjiJedan(id);

            await _receptiRepozitorijum.Obrisi(recept.Id);
        }

        public async Task ObrisiKorakPripreme(string idRecepta, string idKorakaPripreme)
        {
            Recept recept = await _receptiRepozitorijum.PronadjiJedan(idRecepta);
            KorakPripreme korakPripreme = await _koraciPripremeRepozitorijum.PronadjiJedan(idKorakaPripreme);

            await _koraciPripremeRepozitorijum.Obrisi(korakPripreme.Id);
        }

        public async Task ObrisiSastojak(string idRecepta, string idSastojka)
        {
            Recept recept = await _receptiRepozitorijum.PronadjiJedan(idRecepta);
            Sastojak sastojak = await _sastojciRepozitorijum.PronadjiJedan(idSastojka);

            await _sastojciRepozitorijum.Obrisi(sastojak.Id);
        }

        public async Task<ReceptDTO> PronadjiJedan(string id)
        {
            Recept recept = await _receptiRepozitorijum.PronadjiJedan(id);
            List<Sastojak> sastojci = await _sastojciRepozitorijum.PronadjiSve("idRecepta", recept.Id, false, 1, Int32.MaxValue);
            List<KorakPripreme> koraciPripreme = await _koraciPripremeRepozitorijum.PronadjiSve("idRecepta", recept.Id, false, 1, Int32.MaxValue);

            ReceptDTO receptDTO = _maper.Map<ReceptDTO>(recept);
            receptDTO.Sastojci = _maper.Map<List<SastojakDTO>>(sastojci);
            receptDTO.KoraciPripreme = _maper.Map<List<KorakPripremeDTO>>(koraciPripreme);

            return receptDTO;
        }

        public async Task<List<ReceptDTO>> PronadjiSve(int brojStrane, int velicinaStrane)
        {
            List<Recept> recepti = await _receptiRepozitorijum.PronadjiSve(null, null, false, brojStrane, velicinaStrane);

            return _maper.Map<List<ReceptDTO>>(recepti);
        }
    }
}
