using System.Collections.Generic;

namespace Hahn.ApplicationProcess.December2020.Domain.Models
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public IEnumerable<KeyValue> Errors { get; set; }

    }
}
