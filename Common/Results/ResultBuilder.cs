using System;
using System.Collections.Generic;

namespace Common.Results
{
    public class ResultBuilder
    {
        private readonly IList<string> _errors = new List<string>();
        public void AddError(string error) 
        {
            if (string.IsNullOrEmpty(error))
                throw new ArgumentNullException(nameof(error));
            _errors.Add(error);
        }

        public Result Build() => _errors.Count == 0 ? (Result)new Success() : new Failure(_errors);
    }
}
