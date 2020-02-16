using AutoMapper;
using Domain.Entities;
using Domain.Security;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Mappers
{
	public class UserMapper
	{
		private readonly IMapper _mapper;

		public UserMapper(IMapper mapper) => _mapper = mapper;

		public User Map(UserEntity entity)
		{
			var user = _mapper.Map<User>(entity);
			user.Roles = entity.Roles.Select(oo => RoleLookup.GetRole(oo.RoleGuid)).ToList();
			return user;
		}

		public List<User> Map(List<UserEntity> entities) => entities.Select(Map).ToList();
	}
}
