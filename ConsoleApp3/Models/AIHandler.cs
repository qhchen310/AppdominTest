using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3.Models
{
    public class AIHandler : MarshalByRefObject
    {
        public AIHandler()
        {

        }

        public async void Execute(MarshaledResultSetter<int> callback)
        {
            try
            {
                Console.WriteLine($"domin: {AppDomain.CurrentDomain.FriendlyName}");
                await Task.Delay(10000);
                var s = "test";
                Convert.ToInt32(s);
                callback.SetResult(1);
            }
            catch (Exception ex)
            {
                //callback.SetResult(-1);
                callback.SetException(ex);
            }
        }
    }
}
