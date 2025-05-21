using KitabhChautari.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace KitabhChautari.Web.Auth
{
    public class KitabhChautariAuthStateProvider : AuthenticationStateProvider
    {
        private const string AuthType = " kitabhChautari-auth";
        private const string UserDataKey = " udata";
        private  Task<AuthenticationState> _authStateTask;
        private readonly IJSRuntime _jsRuntime;

        public KitabhChautariAuthStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            SetAuthStateTask();


        }
        public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
            _authStateTask;
        public LoggedInUser User { get; private set; }
        public bool IsLoggedIn => User?.Id > 0;

        public async Task SetLoginAsync(LoggedInUser user)
        {
            User = user;

            await _jsRuntime.InvokeVoidAsync("localStorage, setItem", UserDataKey, user.ToJson());

            SetAuthStateTask();

            NotifyAuthenticationStateChanged(_authStateTask);
        }
        
        public async Task SetLoggedAsync()
        {
            User = null;
            SetAuthStateTask();

            NotifyAuthenticationStateChanged(_authStateTask);

            await _jsRuntime.InvokeVoidAsync("localStorage, removeItem", UserDataKey);


        }
        private bool Initializing { get;  private set; } = true;                                          

        public async Task InitializeAsync()
        {
            try
            {
                var udata = await _jsRuntime.InvokeAsync<string?>("localStorage, getItem", UserDataKey);
                if (!string.IsNullOrWhiteSpace(udata))
                {
                    //user data/state is not there in the browser storage
                    return;
                }

                var user = LoggedInUser.LoadFrom(udata);
                if (user == null || user.Id == 0)
                {
                    //user data/state is not valid
                    return;

                }
                await SetLoginAsync(user);

            }
            finally
            {
                Initializing = false;

            }
        }

        private void SetAuthStateTask()
        {
            if(IsLoggedIn)
            {
                var identity = new ClaimsIdentity(User.ToClaims(), AuthType);
                var principal = new ClaimsPrincipal(identity);
                var authState = new AuthenticationState(principal);

                _authStateTask = Task.FromResult(authState);

            }
            else
            {
                var identity = new ClaimsIdentity();
                var principal = new ClaimsPrincipal(identity);
                var authState = new AuthenticationState(principal);
                _authStateTask = Task.FromResult(authState);
            }
        }
    }
}
