using System;

namespace Common.Options
{
    public abstract class Option<T>
    {
        public static implicit operator Option<T>(Nothing nothing) => new None<T>();
        
        public static implicit operator Option<T>(T something) => new Some<T>(something);


        public abstract Option<TOther> Map<TOther>(Func<T, TOther> map);
        public abstract T Reduce(T whenNothing);
        public abstract T Reduce(Func<T> whenNothing);

    }
}
