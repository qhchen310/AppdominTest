using ConsoleApp3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        static event EventHandler<AIEventArgs> RaiseCustomEvent;
        static void Main(string[] args)
        {
            var flag = true;
            RaiseCustomEvent += HandleCustomEvent;
            var ad = AppDomain.CreateDomain("ai domin");
            var aiTask = Task.Run(async () =>
            {
                try
                {
                    return await StartOnChildDomin(ad, RaiseCustomEvent);
                }
                catch (Exception ex)
                {
                    RaiseCustomEvent.Invoke(null, new AIEventArgs(ex.Message));
                }
                return 0;
            });
            aiTask.ContinueWith((result) =>
            {
                Console.WriteLine($"result: ${result.Result}");
                Console.WriteLine($"domin: {AppDomain.CurrentDomain.FriendlyName}");
                flag = false;
            });
            var count = 1;
            while (flag)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"do other tasks {count++}");
            }
            Console.ReadLine();
        }

        static Task<int> StartOnChildDomin(AppDomain appDomain, EventHandler<AIEventArgs> eventHandler)
        {
            var ch = new MarshaledResultSetter<int>();
            try
            {
                var handler = (AIHandler)appDomain.CreateInstanceAndUnwrap(
                    typeof(AIHandler).Assembly.FullName, "ConsoleApp3.Models.AIHandler");
                handler.Execute(ch);
                if (ch.Task.Exception != null)
                {
                    throw new Exception(ch.Task.Exception.Message);
                }
                return ch.Task;
            }
            catch (Exception ex)
            {
                eventHandler.Invoke(null, new AIEventArgs(ex.Message));
            }
            return Task.FromResult(-1);
        }

        static void HandleCustomEvent(object sender, AIEventArgs e)
        {
            Console.WriteLine("received this message: {0}", e.Message);
        }
    }
}
