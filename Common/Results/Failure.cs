using System;
using System.Collections.Generic;

namespace Common.Results
{
    public class Failure: Result
    {
        private readonly IList<string> _errors;
        public override bool IsSuccess => false;
        public IEnumerable<string> Errors => _errors;
        public Failure(IEnumerable<string> errors)
        {
            if (errors == null)
                throw new ArgumentNullException(nameof(errors));

            _errors = new List<string>(errors);
        }
        public Failure(string error) 
        {
            if (string.IsNullOrEmpty(error))
                throw new ArgumentNullException(nameof(error));

            _errors = new List<string>() { error };
        }
    }

    public class Failure<T> : Result<T> 
    {
        private readonly IList<string> _errors;
        public override bool IsSuccess => false;
        public IEnumerable<string> Errors => _errors;
        public Failure(IEnumerable<string> errors)
        {
            if (errors == null)
                throw new ArgumentNullException(nameof(errors));

            _errors = new List<string>(errors);
        }
        public Failure(string error)
        {
            if (string.IsNullOrEmpty(error))
                throw new ArgumentNullException(nameof(error));

            _errors = new List<string>() { error };
        }

        public override Result<TOther> Map<TOther>(Func<T, TOther> map) => new Failure<TOther>(Errors);

        public static explicit operator Failure(Failure<T> failure) => new Failure(failure.Errors);
    }
}
