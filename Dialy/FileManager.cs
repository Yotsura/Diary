﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;

namespace Dialy
{
    class FileManager
    {
        public static SortedDictionary<DateTime, string> GetAllDiaries(string folderpath)
        {
            var diaries = new SortedDictionary<DateTime, string>(
                Directory.EnumerateFiles(folderpath, "*.log", System.IO.SearchOption.AllDirectories)
                .Where(file=>!file.EndsWith("taskTxt.log"))
                .ToDictionary(file => DateTime.Parse(new FileInfo(file).Name.Replace(".log", "").Replace("_", "/"))
                , file => File.ReadAllText(file)));
            //var test = Directory.EnumerateFiles(folderpath, "*.log", System.IO.SearchOption.AllDirectories)
            //    .AsParallel().Select(file => new DiaryRecord(file)).Where(x => x.DirectoryPath != null).OrderBy(x => x.Date).ToList();
            return diaries;
        }

        public static void SaveFile(string topFolderpath, DateTime day, string txt)
        {
            var folderpath = $"{topFolderpath}\\{day.ToString("yyyy")}";
            if (!Directory.Exists(folderpath)) Directory.CreateDirectory(folderpath);
            var filepath = $"{folderpath}\\{day.ToString("yyyy_MM_dd")}.log";
            File.WriteAllText(filepath, txt);
        }

        public static void DeleteFile(string topFolderpath, DateTime day)
        {
            var filepath = $"{topFolderpath}\\{day.ToString("yyyy")}\\{day.ToString("yyyy_MM_dd")}.log";
            if (!File.Exists(filepath)) return;
            File.Delete(filepath);
        }

        public static bool CreateBackUp(SortedDictionary<DateTime, string>alldiaries)
        {
            var folderpath = $"{Directory.GetCurrentDirectory()}\\BackUps";
            if (!Directory.Exists(folderpath)) Directory.CreateDirectory(folderpath);


            var filename = $"{DateTime.Now.ToString("yyyyMMdd HHmmss")}.log";
            var filepath = $"{folderpath}\\{filename}";

            var alllines=new string[] {$"{DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")}作成バックアップ"};
            foreach (var diary in alldiaries)
            {
                var header = new string[] { "______________________________________________________________________"
                    ,diary.Key.ToString("yyyyMMdd")};
                var detail = diary.Value.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                alllines = alllines.Concat(header)
                    .Concat(detail).ToArray();
            }

            File.WriteAllText(filepath, string.Join("\r\n", alllines));

            return true;
        }

        public static bool OpenBackUp(string filepath)
        {
            var text = File.ReadAllText(filepath);
            var records = new SortedDictionary<DateTime, string>();

            string[] del = { "\r\n______________________________________________________________________" };
            var dialies = text.Split(del, StringSplitOptions.None);
            foreach (var dialy in dialies)
            {
                var daystr = dialy.Substring(2, 8).Insert(4, "/").Insert(7, "/");
                if (!DateTime.TryParse(daystr, out var date)) continue;

                var dialyTxt = dialy.Remove(0, 12);
                records.Add(date, dialyTxt);
            }


            //Logの形に出力
            var filenum = filepath.Split('\\').Last().Replace(".log", "");
            var folderpath = $"{System.IO.Directory.GetCurrentDirectory()}\\BackUps\\{filenum}";
            if (Directory.Exists(folderpath)) return false;
            Directory.CreateDirectory(folderpath);
            folderpath = $"{folderpath}\\Logs";
            Directory.CreateDirectory(folderpath);
            foreach (var diary in records)
            {
                SaveFile(folderpath, diary.Key, diary.Value);
            }
            return true;
        }

        public static string FileDialog()
        {
            var dialog = new OpenFileDialog
            {
                InitialDirectory = System.IO.Directory.GetCurrentDirectory(),
                Filter = "すべてのファイル|*,*|log ファイル|*.log",
                FilterIndex = 2
            };
            return dialog.ShowDialog() == true ? dialog.FileName : string.Empty;
        }
    }
}
