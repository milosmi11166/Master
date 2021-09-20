using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ReceptiAPI.DTO;
using ReceptiAPI.Modeli;
using ReceptiAPI.PristupPodacima.Interfejsi;
using ReceptiAPI.Servisi.Interfejsi;

namespace ReceptiAPI.Servisi
{
    public class NamirniceServis : INamirniceServis
    {
        private readonly IRepozitorijum<Namirnica> _namirniceRepozitorijum;
        private readonly IMapper _maper;

        public NamirniceServis(
            IRepozitorijum<Namirnica> namirniceRepozitorijum,
            IMapper maper)
        {
            _namirniceRepozitorijum = namirniceRepozitorijum;
            _maper = maper;
        }

        public async Task<NamirnicaDTO> Azuriraj(string id, NamirnicaDTO namirnicaDTO)
        {
            Namirnica namirnica = await _namirniceRepozitorijum.PronadjiJedan(id);

            namirnica = _maper.Map<NamirnicaDTO, Namirnica>(namirnicaDTO, namirnica);

            namirnica = await _namirniceRepozitorijum.Azuriraj(namirnica);

            return _maper.Map<NamirnicaDTO>(namirnica);
        }

        public async Task<NamirnicaDTO> Kreiraj(NamirnicaDTO namirnicaDTO)
        {
            Namirnica namirnica = _maper.Map<Namirnica>(namirnicaDTO);
            namirnica.Id = Guid.NewGuid().ToString();

            namirnica = await _namirniceRepozitorijum.Kreiraj(namirnica);

            return _maper.Map<NamirnicaDTO>(namirnica);
        }

        public async Task Obrisi(string id)
        {
            Namirnica namirnica = await _namirniceRepozitorijum.PronadjiJedan(id);

            await _namirniceRepozitorijum.Obrisi(namirnica.Id);
        }

        public async Task<NamirnicaDTO> PronadjiJedan(string id)
        {
            Namirnica namirnica = await _namirniceRepozitorijum.PronadjiJedan(id);

            return _maper.Map<NamirnicaDTO>(namirnica);
        }

        public async Task<ListaSaPaginacijomDTO<NamirnicaDTO>> PronadjiSve(string naziv, int brojStrane, int velicinaStrane)
        {
            (List<Namirnica>, Paginacija) namirnice = await _namirniceRepozitorijum.PronadjiSveSaPaginacijom(!string.IsNullOrEmpty(naziv) ? "naziv" : null, naziv, true, brojStrane, velicinaStrane);

            return new ListaSaPaginacijomDTO<NamirnicaDTO>
            {
                Podaci = _maper.Map<List<NamirnicaDTO>>(namirnice.Item1),
                Paginacija = _maper.Map<PaginacijaDTO>(namirnice.Item2)
            };
        }
    }
}
