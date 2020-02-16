using AutoMapper;
using Domain.Entities;

namespace Domain
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<User, UserEntity>()
				.ForMember(oo => oo.Roles, opt => opt.Ignore());

			CreateMap<UserEntity, User>()
				.ForMember(oo => oo.Roles, opt => opt.Ignore());
		}
	}
}
