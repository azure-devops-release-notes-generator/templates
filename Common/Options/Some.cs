using System;

namespace Common.Options
{
    public class Some<T> : Option<T>
    {
        private readonly T _value;
        public Some(T value) 
        {
            _value = value;
        }

        public override Option<TOther> Map<TOther>(Func<T, TOther> map) => map(_value);
        public override T Reduce(T whenNothing) => _value;
        public override T Reduce(Func<T> whenNothing) => _value;
    }
}
