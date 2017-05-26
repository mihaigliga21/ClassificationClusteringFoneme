using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace ClasificareFoneme
{
    public class Prototype
    {
        public List<Tuple<int, int, string>> ComputePrototype()
        {
            var prototype = new List<Tuple<int, int, string>>();

            var basePath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)));
            if (basePath != null)
            {
                var t = basePath.Replace("file:\\", "");
                //files
                var filesPath = t + @"\Resources\";

                var listOfFiles = new DirectoryInfo(filesPath).GetFiles();

                if (listOfFiles.Length > 0)
                {
                    foreach (FileInfo file in listOfFiles)
                    {
                        int f1Sum = 0;
                        int f2Sum = 0;
                        int n = 0;

                        var fileContent = File.ReadAllText(file.FullName);
                        if (!string.IsNullOrEmpty(fileContent))
                        {
                            // ReSharper disable once InconsistentNaming
                            var f1f2Line = fileContent.Split('\n');
                            foreach (var fileLine in f1f2Line)
                            {
                                var f1 = fileLine.Split('\t')[0].Trim();
                                var f2 = fileLine.Split('\t')[1].Trim();

                                f1Sum += Int32.Parse(f1);
                                f2Sum += Int32.Parse(f2);
                                n++;
                            }
                            var currentPrototype = new Tuple<int, int, string>(f1Sum / n, f2Sum / n, file.Name.Split('.')[0].Trim().ToUpper());
                            prototype.Add(currentPrototype);
                        }
                    }

                    return prototype;
                }

            }

            return prototype;
        }
    }
}
