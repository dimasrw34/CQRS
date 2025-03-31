using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using InTouch.UserService.Core;

namespace InTouch.UserService.Infrastructure.Data;

public static class PropertiesUtils
{
    public static List<string> GetPropertiesName<IEntity>() =>
        typeof(IEntity)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(x => (!x.PropertyType.IsClass || x.PropertyType == typeof(string) || x.PropertyType == typeof(byte[]))
                    && !Attribute.IsDefined(x, typeof(NotMappedAttribute))
                    )
        .Select(x => x.Name)
        .ToList();
    
    public static List<string> GetPropertiesName<T>(Expression<Func<IEntity, object>> properties)
    {
        var propertyNameList = new List<string>();

        if (properties != null)
        {
            var newExp = properties.Body as NewExpression;

            foreach (var argExp in newExp.Arguments)
            {
                var memberExp = argExp as MemberExpression;
                propertyNameList.Add(memberExp.Member.Name);
            }
        }
        return propertyNameList;
    }
}
