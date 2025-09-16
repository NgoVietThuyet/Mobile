using System.Linq.Expressions;
using System.Reflection;

namespace Common
{
    public static class QueryExtensions
    {
        public static IQueryable<T> SortByColumn<T>(this IQueryable<T> source, string columnName, bool? isDescending)
        {
            if (string.IsNullOrEmpty(columnName)) return source;

            var propertyInfo = typeof(T).GetProperty(columnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null) return source;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyInfo);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = isDescending == true ? "OrderByDescending" : "OrderBy";
            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), propertyInfo.PropertyType);

            return (IQueryable<T>)method.Invoke(null, [source, lambda]);
        }

        public static IQueryable<T> SelectFields<T>(this IQueryable<T> query, List<string> fieldNames)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            var bindings = fieldNames.Select(fieldName =>
            {
                var propertyInfo = typeof(T).GetProperty(fieldName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new ArgumentException($"Property '{fieldName}' not found on type '{typeof(T).Name}'.");
                var property = Expression.Property(parameter, propertyInfo);
                var binding = Expression.Bind(propertyInfo, property);
                return binding;
            });

            var body = Expression.MemberInit(Expression.New(typeof(T)), bindings);
            var selector = Expression.Lambda<Func<T, T>>(body, parameter);

            return query.Select(selector);
        }
    }
}
