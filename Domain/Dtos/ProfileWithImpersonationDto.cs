namespace Domain.Dtos
{
	public class ProfileWithImpersonationDto
	{
		public UserWithLocationsDto LoggedInUserProfile { get; set; }

		public UserWithLocationsDto? ImpersonationUserProfile { get; set; }

		public bool IsImpersonating { get => ImpersonationUserProfile != null; }

		public ProfileWithImpersonationDto(UserWithLocationsDto loggedInUserProfile, UserWithLocationsDto? impersonationUserProfile)
		{
			LoggedInUserProfile = loggedInUserProfile;
			ImpersonationUserProfile = impersonationUserProfile;
		}
	}
}
