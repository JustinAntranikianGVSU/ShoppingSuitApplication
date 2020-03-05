using AutoMapper;
using CoreLibrary;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Mappers
{
	public class UserMapper
	{
		private readonly IMapper _mapper;

		public UserMapper(IMapper mapper) => _mapper = mapper;

		public UserDto Map(UserEntity entity)
		{
			var user = _mapper.Map<UserDto>(entity);
			user.Roles = entity.Roles.Select(oo => RoleLookup.GetRole(oo.RoleGuid)).ToList();
			return user;
		}

		public List<UserDto> Map(List<UserEntity> entities) => entities.Select(Map).ToList();
	}
}
