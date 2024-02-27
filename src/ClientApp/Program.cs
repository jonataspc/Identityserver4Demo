using IdentityModel.Client;

const string identityServer4BaseAddress = "https://localhost:7195";
const string protectedApiEndpoint = "https://localhost:7281/identity";

var handler = new SocketsHttpHandler
{
    PooledConnectionLifetime = TimeSpan.FromMinutes(15) // Recreate every 15 minutes
};

// Singleton HttpClients
var accessTokenClient = new HttpClient(handler);
var protectedWebApiClient = new HttpClient(handler);


// Wait APIs to start up
await Task.Delay(TimeSpan.FromSeconds(30));

string? token = await AuthenticateAsync();
await RequestProtectedApiAsync(token);

async Task<string?> AuthenticateAsync()
{
    var disco = await accessTokenClient.GetDiscoveryDocumentAsync(identityServer4BaseAddress);

    ClientCredentialsTokenRequest passwordTokenRequest = new()
    {
        Address = disco.TokenEndpoint,
        ClientId = "client",
        ClientSecret = "secret",
        Scope = "api1_read",
    };

    var tokenResponse = await accessTokenClient.RequestClientCredentialsTokenAsync(passwordTokenRequest);

    if (tokenResponse.IsError)
    {
        Console.WriteLine($"Fail: \n{tokenResponse.Error}");
        return null;
    }

    Console.WriteLine($"Success on getting JWT: \n{tokenResponse.Json}");

    return tokenResponse.AccessToken;
}

async Task RequestProtectedApiAsync(string? token)
{
    if (token == null)
    {
        return;
    }

    protectedWebApiClient!.SetBearerToken(token);

    var response = await protectedWebApiClient!.GetAsync(protectedApiEndpoint);

    if (!response.IsSuccessStatusCode)
    {
        Console.WriteLine(response.StatusCode);
    }
    else
    {
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Success on requesting a protected API: {content}");
    }

    Console.Read();
}