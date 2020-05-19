using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContinueWhile
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 1;
            while (i < 11)
            {
                i++;
                if (i %2 == 0)
                {
                    continue;
                }
                Console.WriteLine(i);
            }
            Console.ReadKey();

        }

        
    }

}
