using AutoMapper;
using Domain.Entities;

namespace Domain
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<UserDto, UserEntity>()
				.ForMember(oo => oo.Roles, opt => opt.Ignore());

			CreateMap<UserEntity, UserDto>()
				.ForMember(oo => oo.Roles, opt => opt.Ignore());
		}
	}
}
