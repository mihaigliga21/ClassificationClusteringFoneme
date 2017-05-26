using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ClusteringKMeans
{
    public class FileReader
    {
        public List<FileInfo> GetFilesFromDir(string path, bool isDefault)
        {
            var listOfFiles = new List<FileInfo>();

            try
            {
                var dirPath = "";
                if (isDefault)
                {
                    var basePath =
                        Path.GetDirectoryName(
                            Path.GetDirectoryName(
                                Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)));
                    if (basePath != null)
                    {
                        var t = basePath.Replace("file:\\", "");
                        dirPath = t + @"\Resources\";
                    }
                }
                else
                    dirPath = path;

                if (dirPath != "")                
                    listOfFiles = new DirectoryInfo(dirPath).GetFiles().ToList();                

                return listOfFiles;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Tuple<double, double>> ReadFiles(List<FileInfo> listOfFiles)
        {
            var list2Return = new List<Tuple<double, double>>();

            try
            {
                foreach (FileInfo file in listOfFiles)
                {
                    var fileContent = File.ReadAllText(file.FullName);
                    if (!string.IsNullOrEmpty(fileContent))
                    {
                        var f1f2Line = fileContent.Split('\n');
                        foreach (var fileLine in f1f2Line)
                        {
                            var f1 = fileLine.Split('\t')[0].Trim();
                            var f2 = fileLine.Split('\t')[1].Trim();

                            list2Return.Add(new Tuple<double, double>(Convert.ToDouble(f1), Convert.ToDouble(f2)));
                        }
                    }
                }

                return list2Return;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
