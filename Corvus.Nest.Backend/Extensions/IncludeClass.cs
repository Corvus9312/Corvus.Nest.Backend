using Corvus.Nest.Backend.Interfaces.IRepositories;
using Corvus.Nest.Backend.Models.DAL.Corvus;
using System.Collections;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Corvus.Nest.Backend.Extensions;

public static class IncludeClass
{
    public static void Include<T, TProperty>(this T item, Expression<Func<T, TProperty>> selector) where T : class
    {
        Type tType = typeof(T);
        PropertyDescriptorCollection tProps = TypeDescriptor.GetProperties(tType);

        MemberExpression member = (MemberExpression)selector.Body;
        PropertyInfo selectProp = (PropertyInfo)member.Member;
        Type propType = selectProp.PropertyType;

        string? includeProperty = selectProp.Name;
        if (string.IsNullOrWhiteSpace(includeProperty)) return;

        var select = selector.Compile()(item);
        if (select is IEnumerable)
            propType = propType.GetGenericArguments().First();

        var relational = Context.Relationals
            .Where(x => x.Primary.Name.Equals(tType.Name) || x.Foreign.Name.Equals(tType.Name))
            .Where(x => x.Primary.Name.Equals(propType.Name) || x.Foreign.Name.Equals(propType.Name))
            .SingleOrDefault();

        if (relational is null) return;

        var key = relational.Primary.Name.Equals(tType.Name) ? relational.Primary.Key : relational.Foreign.Key;
        var value = Guid.Parse($"{tType.GetProperty(key)?.GetValue(item)}");

        var methodName = select is IEnumerable ? $"Get{includeProperty}": $"Get{includeProperty.Replace("Navigation", "")}";
        var repoMehtod = typeof(IAppRepository).GetMethod(methodName, new Type[] { relational.Type }) ?? throw new Exception($"Method:「{methodName}」 is not found");
        var taskResult = (Task<TProperty>?)repoMehtod.Invoke(Program.AppRepository, [value]);

        if (taskResult is null) return;

        taskResult.Wait();

        var result = taskResult.Result;

        tType.GetProperty(includeProperty)?.SetValue(item, result);
    }

    public static void Include<T, TProperty>(this IEnumerable<T> items, Expression<Func<T, TProperty>> selector) where T : class
    {
        Parallel.ForEach(items, (item, token) => Include(item, selector));
    }
}