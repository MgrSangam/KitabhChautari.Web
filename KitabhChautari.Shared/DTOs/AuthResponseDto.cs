using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KitabhChautari.Shared.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public string? ErrorMessage { get; set; }

        public AuthResponseDto(string token, string? errorMessage = null)
        {
            Token = token;
            ErrorMessage = errorMessage;
        }

        [JsonIgnore]
        public bool HasError => ErrorMessage != null;
    }
}
