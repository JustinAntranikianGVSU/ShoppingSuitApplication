namespace Domain.Dtos
{
	public class TokenReponseDto
	{
		public string Token { get; set; }

		public TokenReponseDto(string token)
		{
			Token = token;
		}
	}
}
