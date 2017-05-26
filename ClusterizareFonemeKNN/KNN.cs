using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace ClasificareFonemeKNN
{
    public class KNN
    {
        //1 step - calculate distance
        public double CalculateEuclidianDistance(Tuple<double, double> instance2Calc, Tuple<double, double> instanceTrain)
        {
            var dist = Math.Sqrt(Math.Abs(Math.Pow(instance2Calc.Item1 - instanceTrain.Item1, 2) + Math.Pow(instance2Calc.Item2 - instanceTrain.Item2, 2)));
            return dist;
        }

        //2 step - sort distance
        public List<Tuple<string, double>> SortingDistance(List<Tuple<string, double>> listOfDistance, int k = 3)
        {
            return listOfDistance.OrderBy(x => x.Item2).Take(k).ToList();
        }

        //3 step - Counting the Classes
        public List<Tuple<string, List<double>, int>> CountingClasses(List<Tuple<string, double>> listOfDistence)
        {
            var countedList = new List<Tuple<string, List<double>, int>>();

            foreach (Tuple<string, double> tuple in listOfDistence)
            {
                var checkItem = countedList.FirstOrDefault(x => x.Item1 == tuple.Item1);

                if (checkItem == null)
                {
                    var distList = new List<double>();
                    distList.Add(tuple.Item2);
                    countedList.Add(new Tuple<string, List<double>, int>(tuple.Item1, distList, 1));
                }
                else
                {
                    var curentCount = checkItem.Item3 + 1;
                    var newDistList = checkItem.Item2;
                    newDistList.Add(tuple.Item2);

                    var newCheckItem = new Tuple<string, List<double>, int>(checkItem.Item1, newDistList, curentCount);

                    countedList.Remove(checkItem);
                    countedList.Add(newCheckItem);
                }
            }

            return countedList.OrderByDescending(x=>x.Item3).ToList();
        }
    }
}
