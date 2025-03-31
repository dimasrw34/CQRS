using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace InTouch.UserService.Infrastructure.Data;

internal sealed class NpgConnectionFactory (Func<NpgsqlDataSource> dataSourceFactory) : IDbConnectionFactory
{
    private readonly Func<NpgsqlDataSource> _dataSourceFactory = dataSourceFactory;
    private readonly int _maxRetries = 3;
    private readonly TimeSpan _retryDelay = TimeSpan.FromSeconds(1);
    
    public async Task<NpgsqlConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        for (int attempt = 1; attempt <= _maxRetries; attempt++)
        {
            try
            {
                var dataSource = _dataSourceFactory();
                
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();

                GetConnection = await dataSource.OpenConnectionAsync(cancellationToken);
                await VerifyConnectionAsync((NpgsqlConnection)GetConnection, cancellationToken);
                
                return (NpgsqlConnection)GetConnection;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (NpgsqlException ex) when (attempt < _maxRetries)
            {
                await Task.Delay(_retryDelay * attempt, cancellationToken);
                continue;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Ошибка при создании подключения к базе данных (попыток {_maxRetries}).",
                    ex);
            }
        }

        throw new InvalidOperationException(
            $"Не удалось создать подключение после {_maxRetries} попыток.");
    }

    public  NpgsqlConnection GetConnection { get; private set; } 

    private async Task VerifyConnectionAsync(NpgsqlConnection connection, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT 1";
            command.CommandTimeout = 5;
        
            await command.ExecuteScalarAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Ошибка при проверке подключения к базе данных: {ex.Message}",
                ex);
        }
    }
}