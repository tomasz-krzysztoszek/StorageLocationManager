using AutoMapper;
using DAO.DataModels;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Helpers
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<AritSlmMiejscaSklView, MscSkl>()
                .ForMember(m => m.IsBlocked, c => c.MapFrom(s => s.BLOKADA == 0 ? false : true))
                .ReverseMap();
            CreateMap<AritSlmRackView, Rack>().ReverseMap();
            CreateMap<AritSlmMagazynView, Wearehouse>().ReverseMap();
            CreateMap<AritSlmDostawaView, Delivery>().ReverseMap();
            CreateMap<AritSlmUzytkownik, User>().ReverseMap();
        }
    }
}
