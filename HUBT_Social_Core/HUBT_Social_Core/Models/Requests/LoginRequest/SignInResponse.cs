using HUBT_Social_Core.Models.DTOs.IdentityDTO;

namespace HUBT_Social_Core.Models.Requests.LoginRequest
{
    public class SignInResponse
    {
        public TokenResponseDTO? UserToken { get; set; } = null;

        public string MaskEmail { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool RequiresTwoFactor { get; set; }
    }
}
