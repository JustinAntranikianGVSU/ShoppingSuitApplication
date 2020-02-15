using AutoMapper;
using Domain.Entities;

namespace Domain
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<User, UserEntity>();
			CreateMap<UserEntity, User>();
		}
	}
}
