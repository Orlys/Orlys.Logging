namespace Orlys.Logging.Dev
{
    using Orlys.Flatten;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    internal class Program
    {
        static Program()
        {
            Log.Setup();
        }

        static void Main(string[] args)
        {
            TryCatch
                .Try(() => throw new ArgumentNullException("a"))
                .Catch<ArgumentNullException>(e => Console.WriteLine("A: " + e))
                .Catch(x => Console.WriteLine("B: " + x))
                .Go();

            var k = TryCatch
                .Try(() => throw new ArgumentException(), 12)
                .Catch()
                .Go();

            Console.WriteLine(k);
        }
    }

}
