﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KitabhChautari.Shared.DTOs
{
    
        public record class AuthResponseDto(LoggedInUser User, string? ErrorMessage = null)
        {
        [JsonIgnore]

        public bool HasError => ErrorMessage != null;
    }

    
}
