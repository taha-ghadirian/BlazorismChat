using System;

namespace BlazorismChat.ClientLibraries.Convertors;

public static class NameGenerator
{
    public static string GenerateUniqueCode() => Guid.NewGuid().ToString().Replace("-", "");
}