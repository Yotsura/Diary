using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialy
{
    public class TaskRecord
    {
        public TaskRecord(string folderPath)
        {
            Filepath = $"{folderPath}\\taskTxt.log";
        }

        public string Filepath { get; set; }

        public string Txt { get; set; } = string.Empty;

        public void SaveTaskFile()
        {
            File.WriteAllText(Filepath, Funcs.EncryptUtils.AesEncrypt(Txt));
        }

        public void OpenTaskFile()
        {
            Txt = File.Exists(Filepath) ? Funcs.EncryptUtils.AesDecrypt(File.ReadAllText(Filepath)) : string.Empty;
        }
    }
}
