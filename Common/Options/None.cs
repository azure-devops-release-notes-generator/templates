using System;

namespace Common.Options
{
    public class None<T> : Option<T>
    {
        public override Option<TOther> Map<TOther>(Func<T, TOther> map) => Nothing.Value;
        public override T Reduce(T whenNothing) => whenNothing;
        public override T Reduce(Func<T> whenNothing) => whenNothing();
    }

    public class Nothing 
    {
        public static Nothing Value = new Nothing();
        private Nothing() { }
    }
}
