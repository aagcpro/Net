using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstructuraCondicionalIfElse
{
    class Program
    {
        static void Main(string[] args)
        {
            double media = 8;
            Console.WriteLine("¿Cual es la Calificacion que ha optenido el Alumno");

            media = Convert.ToDouble(Console.ReadLine());
            
            if (media > 7)

            {
                Console.WriteLine("El Alumno a Aprobado el Examen");
            }
             else if (media < 7 && media > 5 ) 
                {
                Console.WriteLine("El alumno tiene que recursar");
            }
            else
            {
                Console.WriteLine("El Alumno ha suspendido el Examen");
            }
            Console.ReadKey();
        }
    }
}
