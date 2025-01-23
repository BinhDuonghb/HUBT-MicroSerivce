using HUBT_Social_Core.Models.DTOs;
using HUBT_Social_Core.Models.DTOs.IdentityDTO;
using HUBT_Social_Core.Settings;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HUBT_Social_Core.Decode
{

    public static class TokenDecodeExtension
    {
        public static ClaimsPrincipal? DecodeToken(this string accessToken, JwtSetting jwtSettings, out SecurityToken? securityToken, bool refresh = false)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(refresh ? jwtSettings.RefreshSecretKey : jwtSettings.SecretKey);

                return tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience
                }, out securityToken);
            }
            catch
            {
                securityToken = null;
                return null;
            }
        }
        public static ClaimsPrincipal? DecodeToken(this string accessToken, JwtSetting jwtSettings,bool refresh = false)
        {
            return DecodeToken(accessToken, jwtSettings, out _,refresh);
        }
    }
}

