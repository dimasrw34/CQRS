using System;
using System.Data;

namespace InTouch.Infrastructure.Data;

public sealed class DbConnectionFactory(Func<IDbConnection> connectionFactory) : IDbConnectionFactory
{
    private readonly Func<IDbConnection> _connectionFactoryFunc = 
        connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));

    public IDbConnection CreateOpenConnection() => _connectionFactoryFunc();
}