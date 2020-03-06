﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShoppingSuitePlatform.Helpers
{
	public class JwtTokenHelper
	{
		private readonly IConfiguration _config;

		public JwtTokenHelper(IConfiguration config)
		{
			_config = config;
		}

		public string GenerateJSONWebToken(List<Claim> claims)
		{
			var key = _config["Jwt:Key"];
			var issuer = _config["Jwt:Issuer"];

			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(issuer, issuer, claims, expires: DateTime.Now.AddMinutes(120), signingCredentials: credentials);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}