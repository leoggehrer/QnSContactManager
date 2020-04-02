//@QnSCodeCopy
//MdStart
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using QnSContactManager.Adapters.Exceptions;
using QnSContactManager.Transfer.Persistence.Account;

namespace QnSContactManager.Adapters.Service
{
    partial class InvokeServiceAdapter : ServiceAdapterObject
    {
        static InvokeServiceAdapter()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        public InvokeServiceAdapter(string baseUri)
            : base(baseUri)
        {
            Constructing();
            Constructed();
        }
        public InvokeServiceAdapter(string baseUri, string sessionToken)
            : base(baseUri, sessionToken)
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        #region account methods
        public async Task<LoginSession> LogonAsync(string jsonWebToken)
        {
            using (var client = GetClient(BaseUri))
            {
                HttpResponseMessage response = await client.GetAsync($"Account/Logon/{jsonWebToken}").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                    return await JsonSerializer.DeserializeAsync<LoginSession>(contentData, DeserializerOptions).ConfigureAwait(false);
                }
                else
                {
                    string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                    System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                    throw new AdapterException((int)response.StatusCode, errorMessage);
                }
            }
        }
        public async Task<LoginSession> LogonAsync(string email, string password)
        {
            using (var client = GetClient(BaseUri))
            {
                HttpResponseMessage response = await client.GetAsync($"Account/Logon/{email}/{password}").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                    return await JsonSerializer.DeserializeAsync<LoginSession>(contentData, DeserializerOptions).ConfigureAwait(false);
                }
                else
                {
                    string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                    System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                    throw new AdapterException((int)response.StatusCode, errorMessage);
                }
            }
        }
        public async Task LogoutAsync(string sessionToken)
        {
            using (var client = GetClient(BaseUri))
            {
                HttpResponseMessage response = await client.GetAsync($"Account/Logout/{sessionToken}").ConfigureAwait(false);

                if (response.IsSuccessStatusCode == false)
                {
                    string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                    System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                    throw new AdapterException((int)response.StatusCode, errorMessage);
                }
            }
        }
        public async Task ChangePasswordAsync(string sessionToken, string oldPwd, string newPwd)
        {
            using (var client = GetClient(BaseUri))
            {
                HttpResponseMessage response = await client.GetAsync($"Account/ChangePassword/{sessionToken}/{oldPwd}/{newPwd}").ConfigureAwait(false);

                if (response.IsSuccessStatusCode == false)
                {
                    string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                    System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                    throw new AdapterException((int)response.StatusCode, errorMessage);
                }
            }
        }
        public async Task ChangePasswordForAsync(string sessionToken, string email, string newPwd)
        {
            using (var client = GetClient(BaseUri))
            {
                HttpResponseMessage response = await client.GetAsync($"Account/ChangePasswordFor/{sessionToken}/{email}/{newPwd}").ConfigureAwait(false);

                if (response.IsSuccessStatusCode == false)
                {
                    string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                    System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                    throw new AdapterException((int)response.StatusCode, errorMessage);
                }
            }
        }
        public async Task ResetForAsync(string sessionToken, string email)
        {
            using (var client = GetClient(BaseUri))
            {
                HttpResponseMessage response = await client.GetAsync($"Account/ResetFor/{sessionToken}/{email}").ConfigureAwait(false);

                if (response.IsSuccessStatusCode == false)
                {
                    string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                    System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                    throw new AdapterException((int)response.StatusCode, errorMessage);
                }
            }
        }
        public async Task<bool> HasRoleAsync(string sessionToken, string role)
        {
            using (var client = GetClient(BaseUri))
            {
                HttpResponseMessage response = await client.GetAsync($"Account/HasRole/{sessionToken}/{role}").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                    return await JsonSerializer.DeserializeAsync<bool>(contentData, DeserializerOptions).ConfigureAwait(false);
                }
                else
                {
                    string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                    System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                    throw new AdapterException((int)response.StatusCode, errorMessage);
                }
            }
        }
        public async Task<LoginSession> QueryLoginAsync(string sessionToken)
        {
            using (var client = GetClient(BaseUri))
            {
                HttpResponseMessage response = await client.GetAsync($"Account/QueryLogin/{sessionToken}").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                    return await JsonSerializer.DeserializeAsync<LoginSession>(contentData, DeserializerOptions).ConfigureAwait(false);
                }
                else
                {
                    string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                    System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                    throw new AdapterException((int)response.StatusCode, errorMessage);
                }
            }
        }
        #endregion account methods
    }
}
//MdEnd
