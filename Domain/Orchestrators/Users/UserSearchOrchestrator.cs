using CoreLibrary;
using CoreLibrary.Orchestrators;
using CoreLibrary.ServiceResults;
using DataAccess;
using DataAccess.Repositories;
using Domain.Dtos;
using Domain.Entities;
using Domain.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Dtos.UserSearchRequestViewModel;

namespace Domain.Orchestrators.Users
{
	public interface IUserSearchOrchestrator
	{
		Task<ServiceResult<List<UserWithLocationsDto>>> GetSearchResults(UserSearchRequestViewModel userSearch);
	}

	public class UserSearchOrchestrator : OrchestratorBase, IUserSearchOrchestrator
	{
		private readonly UserWithLocationsMapper _userWithLocationsMapper;
		private readonly UsersWithLocationsRepository _usersWithLocationsRepository;

		public UserSearchOrchestrator(AppDbContext dbContext) : base(dbContext)
		{
			_userWithLocationsMapper = new UserWithLocationsMapper();
			_usersWithLocationsRepository = new UsersWithLocationsRepository(dbContext);
		}

		public async Task<ServiceResult<List<UserWithLocationsDto>>> GetSearchResults(UserSearchRequestViewModel userSearch)
		{
			var baseQuery = _usersWithLocationsRepository.GetReadOnlyQuery();

			if (userSearch.FirstName is string firstName)
			{
				baseQuery = baseQuery.Where(oo => oo.FirstName == firstName);
			}

			if (userSearch.LastName is string lastName)
			{
				baseQuery = baseQuery.Where(oo => oo.LastName == lastName);
			}

			if (userSearch.Email is string email)
			{
				baseQuery = baseQuery.Where(oo => oo.Email == email);
			}

			if (userSearch.AccessListCount is int accessListCount)
			{
				baseQuery = baseQuery.Where(oo => oo.AccessLists.Count >= accessListCount);
			}

			if (userSearch.AccessListName is string accessListName)
			{
				baseQuery = FilterByAccessListName(baseQuery, accessListName);
			}

			if (userSearch.LocationCount is int locationCount)
			{
				baseQuery = FilterByLocationCount(baseQuery, locationCount);
			}

			if (userSearch.LocationName is string locationName)
			{
				baseQuery = FilterByLocationName(baseQuery, locationName);
			}

			if (userSearch.RoleName is string roleName)
			{
				var role = RoleLookup.GetRole(roleName);
				baseQuery = role != null ? baseQuery.Where(oo => oo.Roles.Any(oo => oo.RoleGuid == role.Id)) : baseQuery;
			}

			if (userSearch.RoleCount is int roleCount)
			{
				baseQuery = baseQuery.Where(oo => oo.Roles.Count >= roleCount);
			}

			if (userSearch.RoleIdsMatchAny is List<Guid> roleIdsToMatchAny)
			{
				baseQuery = FilterByRoles(baseQuery, roleIdsToMatchAny);
			}

			if (userSearch.RoleIdsMatchAll is List<Guid> roleIdsToMatchAll)
			{
				baseQuery = FilterByRolesMatchAll(baseQuery, roleIdsToMatchAll);
			}

			if (userSearch.SortByField is SortField sortField)
			{
				baseQuery = ApplySort(baseQuery, sortField, userSearch.SortDescending);
			}

			if (userSearch.Skip is int skip)
			{
				baseQuery = baseQuery.Skip(skip);
			}

			if (userSearch.Take is int take)
			{
				baseQuery = baseQuery.Take(take);
			}

			var userEntities = await baseQuery.ToListAsync();
			var dtos = _userWithLocationsMapper.Map(userEntities);
			return GetProcessedResult(dtos);
		}

		private IQueryable<UserEntity> FilterByAccessListName(IQueryable<UserEntity> userEntities, string accessListName)
		{
			var query = userEntities.Where(oo => oo.AccessLists.Any(oo => oo.AccessList.Name == accessListName));
			return query;
		}

		private IQueryable<UserEntity> FilterByLocationCount(IQueryable<UserEntity> userEntities, int locationCount)
		{
			var query = userEntities
							.Where(oo => oo.AccessLists
							.SelectMany(oo => oo.AccessList.Locations)
							.Select(oo => oo.Location)
							.Count() >= locationCount);

			return query;
		}

		private IQueryable<UserEntity> FilterByLocationName(IQueryable<UserEntity> userEntities, string locationName)
		{
			var query = userEntities
							.Where(oo => oo.AccessLists
							.SelectMany(oo => oo.AccessList.Locations)
							.Select(oo => oo.Location)
							.Any(oo => oo.Name == locationName));

			return query;
		}

		private IQueryable<UserEntity> FilterByRoles(IQueryable<UserEntity> userEntities, List<Guid> roleIdentifiers)
		{
			var query = userEntities.Where(oo => oo.Roles.Any(oo => roleIdentifiers.Contains(oo.RoleGuid)));
			return query;
		}

		private IQueryable<UserEntity> FilterByRolesMatchAll(IQueryable<UserEntity> userEntities, List<Guid> roleIdentifiers)
		{
			var roleCount = roleIdentifiers.Count;
			var query = userEntities.Where(oo => oo.Roles.Count(oo => roleIdentifiers.Contains(oo.RoleGuid)) == roleCount);
			return query;
		}

		private IQueryable<UserEntity> ApplySort(IQueryable<UserEntity> userEntities, SortField sortField, bool sortDescending)
		{
			var query = (sortField, sortDescending) switch
			{
				(SortField.FirstName, false) => userEntities.OrderBy(oo => oo.FirstName),
				(SortField.FirstName, true) => userEntities.OrderByDescending(oo => oo.FirstName),
				(SortField.LastName, false) => userEntities.OrderBy(oo => oo.LastName),
				(SortField.LastName, true) => userEntities.OrderByDescending(oo => oo.LastName),
				(SortField.Email, false) => userEntities.OrderBy(oo => oo.Email),
				(SortField.Email, true) => userEntities.OrderByDescending(oo => oo.Email),
				(_, _) => userEntities
			};

			return query;
		}
	}
}
