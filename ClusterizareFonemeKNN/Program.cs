using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace ClasificareFonemeKNN
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Write F1: ");
            var readF1 = Console.ReadLine();

            Console.WriteLine("Write F2: ");
            var readF2 = Console.ReadLine();

            if (string.IsNullOrEmpty(readF1) || string.IsNullOrEmpty(readF2))
            {
                Console.WriteLine("Wrong input for F1 && F2");
                Console.ReadLine();
                return;
            }

            var customPath = ConfigurationManager.AppSettings["FileReader:DirPathToFiles"];
            var fileReader = new FileReader();
            List<FileInfo> allFilesFromDir = null;
            if (String.IsNullOrEmpty(customPath))
                allFilesFromDir = fileReader.GetFilesFromDir("", true);
            else
                allFilesFromDir = fileReader.GetFilesFromDir(customPath, false);

            if (allFilesFromDir == null)
            {
                var basePath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)));
                if (string.IsNullOrEmpty(customPath))
                    Console.WriteLine($"No files were found in {basePath + "\\Resources"}");
                else
                    Console.WriteLine($"No files were found in {customPath}");

                Console.ReadLine();

                return;
            }
            else
            {
                var allFilesFromDirContent = fileReader.ReadFiles(allFilesFromDir);
                if (allFilesFromDirContent.Count == 0)
                {
                    Console.WriteLine("There was an error reading files content.");
                    Console.ReadLine();
                    return;
                }
                else
                {
                    var knn = new KNN();
                    var distAndCluster = new List<Tuple<string, double>>();

                    //1 step - calculate distance
                    foreach (Tuple<string, string> tuple in allFilesFromDirContent)
                    {
                        if (tuple.Item2 != "")
                        {
                            var f1f2Line = tuple.Item2.Split('\n');
                            foreach (var fileLine in f1f2Line)
                            {
                                var f1 = fileLine.Split('\t')[0].Trim();
                                var f2 = fileLine.Split('\t')[1].Trim();

                                var instance2Calculate = new Tuple<double, double>(Convert.ToDouble(readF1), Convert.ToDouble(readF2));
                                var instanceTrain = new Tuple<double, double>(Convert.ToDouble(f1), Convert.ToDouble(f2));

                                var dist = knn.CalculateEuclidianDistance(instance2Calculate, instanceTrain);

                                distAndCluster.Add(new Tuple<string, double>(tuple.Item1.Split('.')[0].Trim().ToUpper(), dist));
                            }
                        }
                    }
                    //2 step - sort distance
                    if (distAndCluster.Count == 0)
                    {
                        Console.WriteLine("Distance were not calculated");
                        Console.ReadLine();
                        return;
                    }
                    else
                    {
                        //3 step - Counting the Classes
                        var sortedDist = knn.SortingDistance(distAndCluster);
                        if (sortedDist.Count > 0)
                        {
                            var countedClasses = knn.CountingClasses(sortedDist);

                            if (countedClasses.Count > 0)
                            {
                                Console.WriteLine(@"------------------------------------------------");
                                int i = 0;

                                //check if result have 3 classes
                                if (countedClasses.Count == 3)
                                {
                                    //get min from classes dist
                                    Tuple<string, List<double>, int> minClass = null;
                                    foreach (Tuple<string, List<double>, int> tuple in countedClasses)
                                    {
                                        if (minClass == null)
                                            minClass = tuple;
                                        else
                                        {
                                            if (tuple.Item2[0] < minClass.Item2[0])
                                                minClass = tuple;
                                        }
                                    }
                                    if (minClass != null)
                                        Console.WriteLine($"Class with min distance: {minClass.Item1} which have {minClass.Item3} neighbors and distances {minClass.Item2[0]}");
                                }
                                else
                                {
                                    foreach (Tuple<string, List<double>, int> tuple in countedClasses)
                                    {
                                        var dist = "";
                                        foreach (var d in tuple.Item2)
                                            dist += d + "; ";

                                        if (i == 0)
                                            Console.WriteLine(
                                                $"Most popular class: {tuple.Item1} which have {tuple.Item3} neighbors and distances {dist}");
                                        else
                                            Console.WriteLine(
                                                $"Less segnificant class: {tuple.Item1} which have {tuple.Item3} neighbors and distances {dist}");

                                        Console.WriteLine(@"------------------------------------------------");

                                        i++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
