using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloWorldVS
{
    class Program
    {
        private static object Result;

        static void Main(string[] args)
        {
            string name = args[0];
            Int32 x = 2;
            
            int x = 5;
            int y = 10;
            if (x != y)

            {
                Int32 result = 4 / x;
                x = x + 5;
                x++;
            }
                        


           Console.WriteLine(Result.GetType().AssemblyQualifiedName);
           Console.WriteLine("Hello,"+ name + Result);
           Console.ReadLine();      

         }
    }
}
