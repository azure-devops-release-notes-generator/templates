using System;

namespace Common.Results
{
    public class Success: Result
    {
        public override bool IsSuccess => true;
    }

    public class Success<T> : Result<T> 
    {
        public override bool IsSuccess => true;
        public T Value { get; }
        public Success(T value) 
        {
            Value = value;
        }
        public override Result<TOther> Map<TOther>(Func<T, TOther> map) => new Success<TOther>(map(Value));
    }
}
