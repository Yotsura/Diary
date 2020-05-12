using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialy
{
    public class DiaryRecord
    {
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public string DirectoryPath { get; set; }
        public string FileName { get; set; }
        public string FullPath { get => $"{DirectoryPath}\\{FileName}"; }

        public DiaryRecord(DateTime date,string text)
        {
            Date = date;
            Text = text;
            FileName = Date.ToString("yyyy_MM_dd.log");
        }
        public DiaryRecord(string filepath)
        {
            var file = new FileInfo(filepath);
            var datetxt =file.Name.Replace(".log", "").Replace("_", "/");
            if (!DateTime.TryParse(datetxt, out var date)) return;

            FileName = file.Name;
            DirectoryPath = file.DirectoryName;
            Date = date;
            Text = File.ReadAllText(filepath);
        }

        public void SaveFile()
        {
            File.WriteAllText(FullPath, Text);
        }

        public void DeleteFile()
        {
            if (File.Exists(FullPath)) File.Delete(FullPath);
        }
    }
}
