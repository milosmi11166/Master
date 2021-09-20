using AutoMapper;
using ReceptiAPI.DTO;
using ReceptiAPI.Modeli;
using ReceptiAPI.PristupPodacima.Interfejsi;
using ReceptiAPI.Servisi.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReceptiAPI.Servisi
{
    public class ReceptiServis : IReceptiServis
    {
        private readonly IRepozitorijum<Recept> _receptiRepozitorijum;
        private readonly IRepozitorijum<KorakPripreme> _koraciPripremeRepozitorijum;
        private readonly IRepozitorijum<Sastojak> _sastojciRepozitorijum;
        private readonly IRepozitorijum<Namirnica> _namirniceRepozitorijum;
        private readonly IMapper _maper;

        public ReceptiServis(
            IRepozitorijum<Recept> receptiRepozitorijum,
            IMapper maper,
            IRepozitorijum<KorakPripreme> koraciPripremeRepozitorijum,
            IRepozitorijum<Sastojak> sastojciRepozitorijum,
            IRepozitorijum<Namirnica> namirniceRepozitorijum)
        {
            _receptiRepozitorijum = receptiRepozitorijum;
            _maper = maper;
            _koraciPripremeRepozitorijum = koraciPripremeRepozitorijum;
            _sastojciRepozitorijum = sastojciRepozitorijum;
            _namirniceRepozitorijum = namirniceRepozitorijum;
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

        public async Task<SastojakDTO> AzurirajSastojak(string idRecepta, string idNamirnice, SastojakDTO sastojakDTO)
        {
            Recept recept = await _receptiRepozitorijum.PronadjiJedan(idRecepta);
            List<Sastojak> sastojci = await _sastojciRepozitorijum.PronadjiSve("idNamirnice", idNamirnice, false, 1, 1);
            Sastojak sastojak = sastojci.FirstOrDefault();

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

        public async Task ObrisiSastojak(string idRecepta, string idNamirnice)
        {
            Recept recept = await _receptiRepozitorijum.PronadjiJedan(idRecepta);
            List<Sastojak> sastojci = await _sastojciRepozitorijum.PronadjiSve("idNamirnice", idNamirnice, false, 1, 1);
            Sastojak sastojak = sastojci.FirstOrDefault();

            await _sastojciRepozitorijum.Obrisi(sastojak.Id);
        }

        public async Task<ReceptDTO> PronadjiJedan(string id)
        {
            Recept recept = await _receptiRepozitorijum.PronadjiJedan(id);
            List<Sastojak> sastojci = await _sastojciRepozitorijum.PronadjiSve("idRecepta", recept.Id, false, 1, int.MaxValue);
            List<KorakPripreme> koraciPripreme = await _koraciPripremeRepozitorijum.PronadjiSve("idRecepta", recept.Id, false, 1, int.MaxValue);

            ReceptDTO receptDTO = _maper.Map<ReceptDTO>(recept);
            receptDTO.Sastojci = _maper.Map<List<SastojakDTO>>(sastojci);
            receptDTO.KoraciPripreme = _maper.Map<List<KorakPripremeDTO>>(koraciPripreme);

            receptDTO.NutritivneVrednosti = await IzracunajNutritivneVrednosti(sastojci);

            return receptDTO;
        }

        public async Task<ListaSaPaginacijomDTO<ReceptDTO>> PronadjiSve(string opis, int brojStrane, int velicinaStrane)
        {
            (List<Recept>, Paginacija) recepti = await _receptiRepozitorijum.PronadjiSveSaPaginacijom(!string.IsNullOrEmpty(opis) ? "opis" : null, opis, true, brojStrane, velicinaStrane);

            return new ListaSaPaginacijomDTO<ReceptDTO>
            {
                Podaci = _maper.Map<List<ReceptDTO>>(recepti.Item1),
                Paginacija = _maper.Map<PaginacijaDTO>(recepti.Item2)
            };
        }

        private async Task<NutritivneVrednostiDTO> IzracunajNutritivneVrednosti(List<Sastojak> sastojci)
        {
            if(sastojci.Count == 0)
            {
                return new NutritivneVrednostiDTO();
            }

            List<string> idNamirnica = sastojci.Select(x => x.IdNamirnice).ToList();

            List<Namirnica> namirnice = await _namirniceRepozitorijum.PronadjiSve("id", idNamirnica, 1, int.MaxValue);

            IEnumerable<NutritivneVrednostiDTO> nutritivneVrednostiPoNamirnicama = 
                                                                             from s in sastojci
                                                                             join n in namirnice on s.IdNamirnice equals n.Id
                                                                             select new NutritivneVrednostiDTO
                                                                             {
                                                                                 Kalorije = ((decimal)s.KolicinaUGramima / 100) * n.Kalorije,
                                                                                 Proteini = ((decimal)s.KolicinaUGramima / 100) * n.Proteini,
                                                                                 Seceri = ((decimal)s.KolicinaUGramima / 100) * n.Seceri,
                                                                                 Masti = ((decimal)s.KolicinaUGramima / 100) * n.Masti,
                                                                                 ZasiceneMasti = ((decimal)s.KolicinaUGramima / 100) * n.ZasiceneMasti,
                                                                                 Vlakna = ((decimal)s.KolicinaUGramima / 100) * n.Vlakna,
                                                                             };

            return new NutritivneVrednostiDTO
            {
                Kalorije = nutritivneVrednostiPoNamirnicama.Sum(x => x.Kalorije),
                Proteini = nutritivneVrednostiPoNamirnicama.Sum(x => x.Proteini),
                Seceri = nutritivneVrednostiPoNamirnicama.Sum(x => x.Seceri),
                Masti = nutritivneVrednostiPoNamirnicama.Sum(x => x.Masti),
                ZasiceneMasti = nutritivneVrednostiPoNamirnicama.Sum(x => x.ZasiceneMasti),
                Vlakna = nutritivneVrednostiPoNamirnicama.Sum(x => x.Vlakna)
            };
        }
    }
}
