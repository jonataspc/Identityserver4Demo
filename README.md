IdentityServer4 demo application, based on the quick-start guide (https://identityserver4.readthedocs.io/en/aspnetcore1/quickstarts/1_client_credentials.html) and ported to .NET 8

Projects of the solution:

- IdentityServer: IS4 web application

- ProtectedApi: web API used to :
	- validate the incoming token to make sure it is coming from a trusted issuer
	- validate that the token is valid to be used with this api (aka scope)

- ClientApp: client app used to authenticate and make a request to the ProtectedApi


