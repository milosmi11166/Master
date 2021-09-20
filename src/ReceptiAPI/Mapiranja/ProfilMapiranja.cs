using AutoMapper;
using ReceptiAPI.DTO;
using ReceptiAPI.Modeli;

namespace ReceptiAPI.Mapiranja
{
    public class ProfilMapiranja : Profile
    {
        public ProfilMapiranja()
        {
            CreateMap<Recept, ReceptDTO>();
            CreateMap<ReceptDTO, Recept>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.DatumKreiranja, opt => opt.Ignore())
                .ForMember(x => x.DatumAzuriranja, opt => opt.Ignore());

            CreateMap<Sastojak, SastojakDTO>();
            CreateMap<SastojakDTO, Sastojak>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.DatumKreiranja, opt => opt.Ignore())
                .ForMember(x => x.DatumAzuriranja, opt => opt.Ignore());
            CreateMap<KorakPripreme, KorakPripremeDTO>();
            CreateMap<KorakPripremeDTO, KorakPripreme>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.DatumKreiranja, opt => opt.Ignore())
                .ForMember(x => x.DatumAzuriranja, opt => opt.Ignore());
            CreateMap<Namirnica, NamirnicaDTO>();
            CreateMap<NamirnicaDTO, Namirnica>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<Paginacija, PaginacijaDTO>();
            CreateMap<PaginacijaDTO, Paginacija>();
        }
    }
}
