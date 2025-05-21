using KitabhChautari.Shared.DTOs;
using Refit;

namespace KitabhChautari.Web.Apis
{
    public interface IAuthApi
    {
        [Post("/api/auth/login")]
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    }
}
