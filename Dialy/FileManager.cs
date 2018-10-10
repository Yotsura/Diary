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
        public static Dictionary<DateTime, string> GetAllDialy(string folderpath)
        {
            var dialies = new Dictionary<DateTime, string>();
            var files = Directory.EnumerateFiles(folderpath, "*.log", System.IO.SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var datetxt = new FileInfo(file).Name.Replace(".log", "").Replace("_","/");
                DateTime.TryParse(datetxt, out var date);

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
