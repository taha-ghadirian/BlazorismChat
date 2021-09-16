namespace BlazorismChat.ClientLibraries.Convertors;

public struct ApiLocations
{
    private const string _baseUrl = "api/v1/";

    public static readonly string Account = _baseUrl + "Account/";
}