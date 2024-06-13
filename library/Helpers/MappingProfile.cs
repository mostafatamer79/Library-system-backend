using AutoMapper;
namespace library.Helpers
{
   

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, User_Dto>(); 
            CreateMap<User_Dto, User>(); 
        }
    }

}
