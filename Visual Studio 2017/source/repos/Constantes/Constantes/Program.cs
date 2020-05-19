using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constante

{
    class Program
    {
        static void Main(string[] args)
        {
            const double pi = 3.14159;
            double radio;

            Console.WriteLine("Introduce el valor del radio:");
            radio = Convert.ToDouble(Console.ReadLine());

            double areaCirculo = pi * radio * radio;

            Console.WriteLine("El Radio es: {0} y el Area del circulo es:{1} ", radio, areaCirculo);
            Console.ReadKey();
        }
    }
}
