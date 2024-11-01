using System.Data;
using Dapper;

using InTouch.UserService.Domain;

namespace InTouch.Infrastructure;

public class EmailTypeHandler : SqlMapper.TypeHandler<Email>
{
    public override Email Parse(object value) =>
        new Email((string)value);

    public override void SetValue(IDbDataParameter parameter, Email? value) => 
        parameter.Value = value.Address;
}