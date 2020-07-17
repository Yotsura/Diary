using Dialy.Funcs;
using System.IO;

namespace Dialy
{
    public class TaskRecord
    {
        internal EncryptUtils encrypt;

        public string Filepath { get; set; }

        public string Txt { get; set; } = string.Empty;
        public TaskRecord(string folderPath,string pass)
        {
            Filepath = $"{folderPath}\\taskTxt.log";
            encrypt = new EncryptUtils(pass);
        }

        //public void CheckKey()
        //{
        //    if (!encrypt.CheckKey())
        //        encrypt.UpdateKey();
        //}

        public void SaveTaskFile()
        {
            File.WriteAllText(Filepath, encrypt.AesEncrypt(Txt));
            //Settings.Default.Data = encrypt.AesEncrypt(Txt);
            //Settings.Default.Save();
        }

        public void OpenTaskFile()
        {
            Txt = File.Exists(Filepath) ? encrypt.AesDecrypt(File.ReadAllText(Filepath)) : string.Empty;
            //var encrypted = Settings.Default.Data;
            //Txt = string.IsNullOrEmpty(encrypted) ? string.Empty : encrypt.AesDecrypt(encrypted);
        }

        public void UpdatePass(string pass)
        {
            encrypt.UpdatePass(pass);
            SaveTaskFile();
        }
    }
}
