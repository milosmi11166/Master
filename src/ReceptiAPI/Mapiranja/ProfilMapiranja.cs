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
        }
    }
}
