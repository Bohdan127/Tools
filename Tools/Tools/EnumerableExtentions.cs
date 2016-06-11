using System;
using System.Collections.Generic;
using System.Linq;

namespace ToolsPortable
{
    public static class EnumerableExtentions
    {
        public static TResult With<TInput, TResult>(this TInput input, Func<TInput, TResult> func)
            where TResult : class
            where TInput : class
                => input == null ? null : func(input);

        public static TResult Return<TInput, TResult>(this TInput input,
            Func<TInput, TResult> func, TResult failureValue)
            where TInput : class
                => input == null ? failureValue : func(input);

        public static TInput If<TInput>(this TInput input, Func<TInput, bool> func)
            where TInput : class
                => input == null ? null : (func(input) ? input : null);

        public static TInput Unless<TInput>(this TInput input, Func<TInput, bool> func)
            where TInput : class
                => input == null ? null : (func(input) ? null : input);

        public static TInput Do<TInput>(this TInput input, Action<TInput> action)
            where TInput : class
        {
            if (input != null)
                action(input);
            return input;
        }

        public static IEnumerable<TInput> ForEach<TInput>(this IEnumerable<TInput> enumeration, Action<TInput> action)
           where TInput : class
        {
            var enumerable = enumeration as IList<TInput> ?? enumeration.ToList();
            if (enumeration == null) return new TInput[0];
            else foreach (TInput item in enumerable) action(item);
            return enumerable;
        }

        public static IEnumerable<T> ToEnumerable<T>(this T item)
        {
            yield return item;
        }
    }
}
