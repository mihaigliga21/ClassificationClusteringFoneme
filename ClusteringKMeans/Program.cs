using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClusteringKMeans
{
    class Program
    {
        static void Main(string[] args)
        {
            var customPath = ConfigurationManager.AppSettings["FileReader:DirPathToFiles"];
            var fileReader = new FileReader();
            List<FileInfo> allFilesFromDir = null;
            allFilesFromDir = String.IsNullOrEmpty(customPath) ? fileReader.GetFilesFromDir("", true) : fileReader.GetFilesFromDir(customPath, false);

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
                if (allFilesFromDirContent == null)
                {
                    Console.WriteLine("There was an error reading files content.");
                    Console.ReadLine();
                    return;
                }
                else
                {
                    var countData = allFilesFromDirContent.Count;
                    double[][] data = new double[countData][];
                    for (int i = 0; i < countData; i++)
                        data[i] = new[] { allFilesFromDirContent[i].Item1, allFilesFromDirContent[i].Item2 };

                    int clusters = allFilesFromDir.Count;

                    KMeans.ShowData(data, 1, true, true);
                    int[] clustering = KMeans.Cluster(data, clusters);                  

                    KMeans.ShowClustered(data, clustering, clusters, 1);
                }
            }
            Console.ReadLine();
        }
    }
}
