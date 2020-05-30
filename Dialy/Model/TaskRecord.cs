using System.IO;

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
            Funcs.EncryptUtils.UpdateKey();//復号直後に鍵を更新する。
        }
    }
}
