using System;
using System.Collections.Generic;

namespace Functional.Solutions._01Enumerable
{
  public static class Basics
  {
    public static IEnumerable<T> Empty<T>()
    {
      yield break;
    }

    public static IEnumerable<T> Unit<T>(this T obj)
    {
      yield return obj;
    }

    // SelectMany
    public static IEnumerable<TResult> Bind<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> f)
    {
      foreach (var item in source)
      {
        foreach (var result in f(item))
        {
          yield return result;
        }
      }
    }

    // Select
    public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> f) =>
      source.Bind(item => f(item).Unit());

    // Where
    public static IEnumerable<TSource> Filter<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> f) =>
      source.Bind(item => f(item) ? item.Unit() : Empty<TSource>());

    // Aggregate
    public static TState Fold<TSource, TState>(this IEnumerable<TSource> source, TState initial, Func<TState, TSource, TState> f)
    {
      var state = initial;
      foreach (var item in source)
      {
        state = f(state, item);
      }

      return state;
    }

    public static TSource Last<TSource>(this IEnumerable<TSource> source) => source.Fold(default(TSource), (agg, current) => current);

    // First
    public static TSource Head<TSource>(this IEnumerable<TSource> source) =>
      source.Fold(new { Found = false, Item = default(TSource) }, (agg, current) => !agg.Found ? new { Found = true, Item = current } : agg).Item;
  }
}
