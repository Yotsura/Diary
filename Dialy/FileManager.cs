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
        public static SortedDictionary<DateTime, string> GetAllDiaries(string folderpath)
        {
            var diaries = new SortedDictionary<DateTime, string>();
            var files = Directory.EnumerateFiles(folderpath, "*.log", System.IO.SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var datetxt = new FileInfo(file).Name.Replace(".log", "").Replace("_", "/");
                if (!DateTime.TryParse(datetxt, out var date)) continue;
                diaries[date] = String.Join("\r\n", File.ReadAllLines(file));
            }
            return diaries;
        }

        public static void SaveFile(string topFolderpath, string year, string date, string txt)
        {
            var folderpath = $"{topFolderpath}\\{year}";
            if (!Directory.Exists(folderpath)) Directory.CreateDirectory(folderpath);
            var filepath = $"{folderpath}\\{date}.log";
            var alllines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            File.WriteAllLines(filepath, alllines);
        }

        public static void DeleteFile(string topFolderpath, string year, string date)
        {
            var filepath = $"{topFolderpath}\\{year}\\{date}.log";
            if (!File.Exists(filepath)) return;
            File.Delete(filepath);
        }

        public static void CreateBackUp(SortedDictionary<DateTime, string>alldiaries)
        {
            var folderpath = $"{System.IO.Directory.GetCurrentDirectory()}\\BackUp";
            if (!Directory.Exists(folderpath)) Directory.CreateDirectory(folderpath);


            var filename = $"{DateTime.Now.ToString("yyyyMMdd HHmmss")}.log";
            var filepath = $"{folderpath}\\{filename}";

            var alllines=new string[] {$"{DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")}作成バックアップ"};
            foreach (var diary in alldiaries)
            {
                var header = new string[] { Environment.NewLine,
                    "______________________________________________________________________"
                    ,diary.Key.ToString("yyyyMMdd")};
                alllines = alllines.Concat(header)
                    .Concat(diary.Value.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)).ToArray();
            }
            
            File.WriteAllLines(filepath, alllines);
        }
    }
}
