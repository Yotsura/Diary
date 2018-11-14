﻿using System;
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

        public static void SaveFile(string folderPath, string year, string date, string txt)
        {
            var folderPath2 = $"{folderPath}\\{year}";
            if (!Directory.Exists(folderPath2)) Directory.CreateDirectory(folderPath2);
            var filepath = $"{folderPath2}\\{date}.log";
            var alllines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            File.WriteAllLines(filepath, alllines);
        }

        public static void DeleteFile(string folderPath, string year, string date)
        {
            var filepath = $"{folderPath}\\{year}\\{date}.log";
            if (!File.Exists(filepath)) return;
            File.Delete(filepath);
        }
    }
}
