using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

namespace ToolsPortable
{
    public static class EnumerableExtentions
    {
        [DebuggerStepThrough]
        [Pure]
        public static TResult With<TInput, TResult>(
            this TInput input, Func<TInput, TResult> func)
            where TResult : class
            where TInput : class
                => input == null ? null : func(input);

        [DebuggerStepThrough]
        [Pure]
        public static TResult Return<TInput, TResult>(this TInput input,
            Func<TInput, TResult> func, TResult failureValue)
            where TInput : class
                => input == null ? failureValue : func(input);

        [DebuggerStepThrough]
        [Pure]
        public static TInput If<TInput>(this TInput input, Func<TInput, bool> func)
            where TInput : class
                => input == null ? null : (func(input) ? input : null);

        [DebuggerStepThrough]
        [Pure]
        public static TInput Unless<TInput>(this TInput input, Func<TInput, bool> func)
            where TInput : class
                => input == null ? null : (func(input) ? null : input);

        [DebuggerStepThrough]
        [Pure]
        public static TInput Do<TInput>(this TInput input, Action<TInput> action)
            where TInput : class
        {
            if (input != null)
                action(input);
            return input;
        }

        [DebuggerStepThrough]
        [Pure]
        public static IEnumerable<TInput> ForEach<TInput>(
            this IEnumerable<TInput> enumeration, Action<TInput> action)
            where TInput : class
        {
            var enumerable = enumeration as IList<TInput> ?? enumeration.ToList();
            if (enumeration == null) return new TInput[0];
            else foreach (TInput item in enumerable) action(item);
            return enumerable;
        }

        [DebuggerStepThrough]
        [Pure]
        public static IEnumerable<T> ToEnumerable<T>(this T item)
        {
            yield return item;
        }

        [DebuggerStepThrough]
        [Pure]
        public static TResult PipeTo<TSource, TResult>(
            this TSource source, Func<TSource, TResult> func)
            => func(source);

        [DebuggerStepThrough]
        [Pure]
        public static TOutput Either<TInput, TOutput>(
            this TInput input, Func<TInput, bool> condition,
            Func<TInput, TOutput> ifTrue, Func<TInput, TOutput> ifFalse)
            => condition(input) ? ifTrue(input) : ifFalse(input);

        [DebuggerStepThrough]
        [Pure]
        public static TOutput Either<TInput, TOutput>(
            this TInput input, Func<TInput, TOutput> ifTrue,
            Func<TInput, TOutput> ifFalse)
            => input.Either(x => x != null, ifTrue, ifFalse);

        [DebuggerStepThrough]
        [Pure]
        public static TEntity ById<TKey, TEntity>(this IQueryable<TEntity> queryable, TKey id)
            where TEntity : class, IHasId
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
            => queryable.SingleOrDefault(x => x.Id.Equals(id));

        [DebuggerStepThrough]
        [Pure]
        public static TProjection ById<TKey, TEntity, TProjection>(
            this IQueryable<TEntity> queryable, TKey id,
            Expression<Func<TEntity, TProjection>> projectionExpression)
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
            where TEntity : class, IHasId
            where TProjection : class, IHasId
            => queryable.Select(projectionExpression).SingleOrDefault(x => x.Id.Equals(id));
    }
}
