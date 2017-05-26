using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace ClasificareFoneme
{
    class Program
    {
        static void Main(string[] args)
        {
            var prototypeBase = new Prototype();
            var prototype = prototypeBase.ComputePrototype();

            Console.WriteLine("Write F1: ");
            var readF1 = Console.ReadLine();

            Console.WriteLine("Write F2: ");
            var readF2 = Console.ReadLine();

            try
            {
                if (!string.IsNullOrEmpty(readF1) || !string.IsNullOrEmpty(readF2) || prototype.Count > 0)
                {
                    var phone2Identify = new Tuple<int, int>(Int32.Parse(readF1), Int32.Parse(readF2));

                    int min = 100000;
                    var cluster = "";

                    foreach (var prototyp in prototype)
                    {                        
                        double dist =
                            Math.Sqrt(
                                Math.Abs(Math.Pow(phone2Identify.Item1 - prototyp.Item1, 2) +
                                         Math.Pow(phone2Identify.Item2 - prototyp.Item2, 2)));
                        int dist2Int = (int) Math.Round(dist);
                        if (dist2Int < min)
                        {
                            min = dist2Int;
                            cluster = prototyp.Item3;
                        }
                    }

                    Console.WriteLine($"Distanta : {min}, identificat ca vocala {cluster}");
                }
                else
                    Console.WriteLine("Wrong input!.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
