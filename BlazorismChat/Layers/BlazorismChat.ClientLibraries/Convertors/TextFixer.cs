namespace BlazorismChat.ClientLibraries.Convertors;

public static class TextFixer
{
    public static string FixUserName(string username) => (username+"").Trim().ToUpper();

    public static string FixEmail(string email) => (email+"").Trim().ToUpper();
}
