using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstructuraCondicionalIfELse_Parte2
{
    class Program
    {
        static void Main(string[] args)
        {
            double media = 8;
            Console.WriteLine("¿Cual es la Calificacion que ha optenido el Alumno");

            media = Convert.ToDouble(Console.ReadLine());

            string resultado = "El alumno ha \t";
            resultado += media >= 7 ? "aprobado" : "suspenso.";
            Console.WriteLine(resultado);
            Console.ReadKey();
        }
    }
}
