using System;
using System.Threading.Tasks;

namespace Common.Results
{
    public static class ResultExtensions
    {
        public static Result<T> Map<T>(this Result result, Func<T> map)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            return result switch
            {
                Success _ => new Success<T>(map()),
                Failure f => new Failure<T>(f.Errors),
                _ => throw new ArgumentOutOfRangeException($"{result.GetType()} is not supported")
            };
        }

        public static Task<Result> MapTask<T>(this Result<T> result, Func<T, Task<Result>> map)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            return result switch
            {
                Success<T> s => map(s.Value),
                Failure<T> f => Task.FromResult((Result)f),
                _ => throw new ArgumentOutOfRangeException($"{result.GetType()} is not supported")
            };
        }

        public static Task<Result<TOther>> MapTask<T,TOther>(this Result<T> result, Func<T, Task<Result<TOther>>> map)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            return result switch
            {
                Success<T> s => map(s.Value),
                Failure<T> f => Task.FromResult((Result<TOther>)new Failure<TOther>(f.Errors)),
                _ => throw new ArgumentOutOfRangeException($"{result.GetType()} is not supported")
            };
        }

        public static Task<Result> MapTask(this Result result, Func<Task<Result>> map) 
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            if (map == null)
                throw new ArgumentNullException(nameof(map));
            return result switch
            {
                Success _ => map(),
                Failure f => Task.FromResult((Result)f),
                _ => throw new ArgumentOutOfRangeException($"{result.GetType()} is not supported")
            };
        }
    }
}
