using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace ClasificareFonemeKNN
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

        public List<Tuple<string, string>> ReadFiles(List<FileInfo> listOfFiles)
        {
            var list2Return = new List<Tuple<string, string>>();

            try
            {
                foreach (FileInfo file in listOfFiles)
                {
                    var fileContent = File.ReadAllText(file.FullName);
                    if (!string.IsNullOrEmpty(fileContent))                    
                        list2Return.Add(new Tuple<string, string>(file.Name, fileContent));                    
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
