using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace InTouch.UserService.Infrastructure.Data;

public static class SqlQueriesBuilder
{
    public static string BuildInsertQuery<T>()
    {
        string tableName = typeof(T).Name;
        var columnList = PropertiesUtils.GetPropertiesName<T>();

        var columnListWithSquareBracket = columnList.Select(col => $"[{col}]").ToList();
            string columnString = string.Join(",", columnListWithSquareBracket);

        var paramList = columnList.Select(s => $"@{s}").ToList();
            string paramString = string.Join(",", paramList);

        return $"DECLARE @t TABLE([Id] UNIQUEIDENTIFIER); INSERT INTO [{tableName}] ({columnString}) OUTPUT INSERTED.[Id] INTO @t VALUES ({paramString}); SELECT [Id] FROM @t ";
    }

    public static string BuildSelectAllQuery<T>()
    {
        string tableName = typeof(T).Name;
        List<string> columnList = PropertiesUtils.GetPropertiesName<T>();
        columnList = columnList.Select(colName => $"[{colName}]").ToList();
        string columnString = string.Join(",", columnList);
        string query = $@"SELECT {columnString} FROM [{tableName}] WHERE 1=1 ";
        return query;
    }

    public static string BuildSelectQuery<T>(Expression<Func<T, object>> properties, Expression<Func<T, object>> parameters = null)
    {
        string tableName = typeof(T).Name;
        var columnList = PropertiesUtils.GetPropertiesName(properties).Select(name => $"[{name}]");
        var paramList = PropertiesUtils.GetPropertiesName(parameters)?.Select(name => $"[{name}]=@{name}");
        var sqlString = new StringBuilder();

        // Construct and append column string
        string columnString = string.Join(",", columnList);
        sqlString.Append($@"SELECT {columnString} FROM [{tableName}] WHERE 1=1 ");

        // Construct and append parameter string
        if (paramList != null && paramList.Any())
        {
            sqlString.Append(" AND ");
            string paramString = string.Join(" AND ", paramList);
            sqlString.Append(paramString);
        }
            return sqlString.ToString();
    }

    public static string BuildUpdateQuery<T>(Expression<Func<T, object>> propertiesToUpdate = null)
    {
        string tableName = typeof(T).Name;

        List<string> columnList = (propertiesToUpdate != null) 
        ? PropertiesUtils.GetPropertiesName(propertiesToUpdate)
        : PropertiesUtils.GetPropertiesName<T>();

        List<string> paramList = columnList.Select(name => $"[{name}]=@{name}").ToList();
        string paramString = string.Join(",", paramList);

        return $"UPDATE [{tableName}] SET {paramString} WHERE 1=1 ";
    }
}
