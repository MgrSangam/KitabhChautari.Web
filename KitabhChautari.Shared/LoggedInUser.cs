using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KitabhChautari.Shared
{
    public  record class LoggedInUser(int Id, string Name, string Role, string token)
    {
        public string ToJson() => JsonSerializer.Serialize(this);

        public Claim[] ToClaims() =>
            [
            new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
            new Claim(ClaimTypes.Name, Name),
            new Claim(ClaimTypes.Role, Role),
            new Claim(nameof(token), token),


            ];

        public static LoggedInUser? LoadFrom(string json) =>
            !string.IsNullOrWhiteSpace(json)
            ? JsonSerializer.Deserialize<LoggedInUser>(json) : null;

    } 
    
}
