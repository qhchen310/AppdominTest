using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3.Models
{
    public class MarshaledResultSetter<T> : MarshalByRefObject
    {
        private TaskCompletionSource<T> m_tcs = new TaskCompletionSource<T>();
        public void SetResult(T result) => m_tcs.SetResult(result);
        public void SetException(Exception exception) => m_tcs.SetException(exception);
        public Task<T> Task { get { return m_tcs.Task; } }
    }
}
