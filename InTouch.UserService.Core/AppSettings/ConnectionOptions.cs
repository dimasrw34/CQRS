using System;

namespace InTouch.UserService.Core;

public sealed class ConnectionOptions : IAppOptions
{
    static string IAppOptions.ConfigSectionPath => "ConnectionString";
    
    public string SqlConnection { get; private init; }
    public string NoSqlConnection { get; private init; }
    public string CacheConnection { get; private init; }

    public bool CacheConnectionInMemory() =>
        CacheConnection.Equals("InMemory", StringComparison.CurrentCultureIgnoreCase);
}