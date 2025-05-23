﻿using KitabhChautari.Api.Services;
using KitabhChautari.Shared.DTOs;

namespace KitabhChautari.Api.Endpoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/auth/login", async (LoginDto dto, AuthService authService) =>
                Results.Ok(await authService.LoginAsync(dto)));
            return app;
        }
    }
}
