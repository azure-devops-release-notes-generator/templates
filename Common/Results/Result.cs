using System;

namespace Common.Results
{
    public abstract class Result
    {
        public abstract bool IsSuccess { get; }
    }

    public abstract class Result<T>
    {
        public abstract bool IsSuccess { get; }
        public abstract Result<TOther> Map<TOther>(Func<T, TOther> map);
    }
}
