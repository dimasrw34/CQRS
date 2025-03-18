using System;
using System.Collections.Generic;
using System.Data;

namespace InTouch.Infrastructure.Data;

public static class DbTypeMapper
{
     private static readonly Dictionary<Type, DbType> TypeToDbType = new Dictionary<Type, DbType>
    {
        // Числовые типы
        { typeof(byte), DbType.Byte },
        { typeof(sbyte), DbType.SByte },
        { typeof(short), DbType.Int16 },
        { typeof(ushort), DbType.UInt16 },
        { typeof(int), DbType.Int32 },
        { typeof(uint), DbType.UInt32 },
        { typeof(long), DbType.Int64 },
        { typeof(ulong), DbType.UInt64 },
        { typeof(float), DbType.Single },
        { typeof(double), DbType.Double },
        { typeof(decimal), DbType.Decimal },
        { typeof(bool), DbType.Boolean },

        // Строковые типы
        { typeof(string), DbType.String },
        { typeof(char), DbType.StringFixedLength },

        // Даты и время
        { typeof(DateTime), DbType.DateTime },
        { typeof(DateTimeOffset), DbType.DateTimeOffset },

        // Бинарные типы
        { typeof(byte[]), DbType.Binary },
        { typeof(Guid), DbType.Guid },

        // Специальные типы
        { typeof(TimeSpan), DbType.Time },
        { typeof(object), DbType.Object }
    };

    public static DbType GetDbType(Type type)
    {
        if (type == null)
            throw new ArgumentNullException(nameof(type));

        if (type.IsNullable())
        {
            return GetDbType(type.GetUnderlyingType());
        }

        if (TypeToDbType.TryGetValue(type, out DbType dbType))
        {
            return dbType;
        }

        throw new ArgumentException($"Неизвестный тип: {type.Name}", nameof(type));
    }

    public static Type GetClrType(DbType dbType)
    {
        return TypeMapping.DbTypeToType[dbType];
    }

    private static class TypeMapping
    {
        public static readonly Dictionary<DbType, Type> DbTypeToType = new Dictionary<DbType, Type>
        {
            { DbType.Byte, typeof(byte) },
            { DbType.SByte, typeof(sbyte) },
            { DbType.Int16, typeof(short) },
            { DbType.UInt16, typeof(ushort) },
            { DbType.Int32, typeof(int) },
            { DbType.UInt32, typeof(uint) },
            { DbType.Int64, typeof(long) },
            { DbType.UInt64, typeof(ulong) },
            { DbType.Single, typeof(float) },
            { DbType.Double, typeof(double) },
            { DbType.Decimal, typeof(decimal) },
            { DbType.Boolean, typeof(bool) },
            { DbType.String, typeof(string) },
            { DbType.StringFixedLength, typeof(char) },
            { DbType.DateTime, typeof(DateTime) },
            { DbType.DateTimeOffset, typeof(DateTimeOffset) },
            { DbType.Binary, typeof(byte[]) },
            { DbType.Guid, typeof(Guid) },
            { DbType.Time, typeof(TimeSpan) },
            { DbType.Object, typeof(object) }
        };
    }
}

public static class TypeExtensions
{
    public static bool IsNullable(this Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    public static Type GetUnderlyingType(this Type type)
    {
        return type.IsNullable() ? 
            Nullable.GetUnderlyingType(type) : 
            type;
    }
}