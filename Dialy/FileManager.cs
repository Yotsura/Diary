using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Dialy
{
    class FileManager
    {
        public static Dictionary<string, string> GetAllDialy(string folderpath)
        {
            var dialies = new Dictionary<string, string>();
            var files = Directory.EnumerateFiles(folderpath, "*.log", System.IO.SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var date = new FileInfo(file).Name.Replace(".log", "");
                var txt = File.ReadAllLines(file);
                dialies.Add(date, String.Join("\r\n", txt));
            }
            return dialies;
        }

        public static void SaveFile(string folderPath,string date, string txt)
        {
            var filepath = $"{folderPath}\\{date}.log";
            var alllines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            File.WriteAllLines(filepath, alllines);
        }
    }
}
