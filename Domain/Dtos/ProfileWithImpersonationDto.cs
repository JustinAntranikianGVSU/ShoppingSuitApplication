namespace Domain.Dtos
{
	public class ProfileWithImpersonationDto
	{
		public UserProfileDto LoggedInUserProfile { get; set; }

		public UserProfileDto? ImpersonationUserProfile { get; set; }

		public bool IsImpersonating { get => ImpersonationUserProfile != null; }

		public ProfileWithImpersonationDto(UserProfileDto loggedInUserProfile, UserProfileDto? impersonationUserProfile)
		{
			LoggedInUserProfile = loggedInUserProfile;
			ImpersonationUserProfile = impersonationUserProfile;
		}
	}
}
